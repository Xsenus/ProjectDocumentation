param(
    [string]$OutputRoot = '',
    [string]$Python = 'python.exe',
    [string]$SchemaPython = '',
    [switch]$SkipNuGetRestore,
    [switch]$DesktopOnly
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path -Parent $PSScriptRoot
if (-not $OutputRoot) { $OutputRoot = Join-Path $repo 'artifacts' }
$output = [IO.Path]::GetFullPath($OutputRoot)
$build = Join-Path $output ('release-' + (Get-Date -Format 'yyyyMMdd-HHmmss') + '-' + [guid]::NewGuid().ToString('N').Substring(0, 6))
$desktop = Join-Path $build 'desktop'
New-Item -ItemType Directory -Path $desktop -Force | Out-Null

function Invoke-Checked([string]$Executable, [string[]]$Arguments) {
    & $Executable @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$Executable returned exit code $LASTEXITCODE." }
}

$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' |
    Select-Object -First 1
if (-not $msbuild) { throw 'Не найден MSBuild из Visual Studio.' }

Push-Location -LiteralPath $repo
try {
    if (-not $SchemaPython) {
        $venv = Join-Path $build 'python'
        Invoke-Checked $Python @('-m', 'venv', $venv)
        $SchemaPython = Join-Path $venv 'Scripts\python.exe'
    }
    Invoke-Checked $SchemaPython @('-m', 'pip', 'install', '--disable-pip-version-check', '-r', 'schema_engine/requirements-build.txt')
    Invoke-Checked $SchemaPython @('-X', 'utf8', '-m', 'unittest', 'discover', '-s', 'schema_engine/tests', '-v')

    if (-not $DesktopOnly) {
        Push-Location -LiteralPath (Join-Path $repo 'portal')
        try {
            Invoke-Checked 'npm.cmd' @('ci')
            Invoke-Checked 'npm.cmd' @('run', 'build')
        } finally {
            Pop-Location
        }
    }

    if (-not $SkipNuGetRestore) {
        $config = Join-Path $build 'NuGet.Config'
        @'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
'@ | Set-Content -LiteralPath $config -Encoding utf8
        $oldPackages = $env:NUGET_PACKAGES
        $oldHttpCache = $env:NUGET_HTTP_CACHE_PATH
        try {
            $env:NUGET_PACKAGES = Join-Path $build 'nuget-packages'
            $env:NUGET_HTTP_CACHE_PATH = Join-Path $build 'nuget-http'
            Invoke-Checked $msbuild @('ProjectDocumentation.sln', '/t:Restore', '/p:RestorePackagesConfig=true', "/p:RestoreConfigFile=$config", '/p:Configuration=Release', '/nr:false', '/v:minimal')
        } finally {
            $env:NUGET_PACKAGES = $oldPackages
            $env:NUGET_HTTP_CACHE_PATH = $oldHttpCache
        }
    }
    $schemaData = Join-Path $repo 'schema_engine\schemas'
    $portalData = Join-Path $repo 'portal\dist'
    # PyInstaller's nested lxml resources need room under the Windows path limit.
    $tempRoot = (Resolve-Path -LiteralPath ([IO.Path]::GetTempPath())).Path.TrimEnd('\')
    $bundleWork = (New-Item -ItemType Directory -Path (Join-Path $tempRoot ('PD-' + [guid]::NewGuid().ToString('N').Substring(0, 10)))).FullName
    try {
        $winforms = Join-Path $bundleWork 'winforms'
        Invoke-Checked $msbuild @('ProjectDocumentation.sln', '/t:Rebuild', '/p:Configuration=Release', "/p:OutDir=$winforms/", '/m', '/nr:false', '/v:minimal')

        $dll = (New-Item -ItemType Directory -Path (Join-Path $desktop 'dll')).FullName
        $launchFiles = 'ProjectDocumentation.NET.exe', 'ProjectDocumentation.NET.exe.config' |
            ForEach-Object { Join-Path $winforms $_ }
        Copy-Item -LiteralPath $launchFiles -Destination $desktop
        $libraries = Get-ChildItem -LiteralPath $winforms -File | Where-Object {
            $_.Extension -eq '.dll' -or $_.Name.EndsWith('.dll.config', [StringComparison]::OrdinalIgnoreCase)
        }
        Copy-Item -LiteralPath $libraries.FullName -Destination $dll
        foreach ($folder in @('ru', 'x86', 'x64', 'runtimes')) {
            $dependency = Join-Path $winforms $folder
            if (Test-Path -LiteralPath $dependency -PathType Container) {
                Copy-Item -LiteralPath $dependency -Destination $dll -Recurse
            }
        }
        Copy-Item -LiteralPath (Join-Path $winforms 'licenses') -Destination $desktop -Recurse

        $bundles = Join-Path $bundleWork 'dist'
        $bundleArguments = @(
            '-m', 'PyInstaller', '--noconfirm', '--onedir', '--name', 'ProjectDocumentation.Schema',
            '--distpath', $bundles, '--workpath', (Join-Path $bundleWork 'build'), '--specpath', $bundleWork,
            '--paths', $repo, '--collect-data', 'xmlschema', '--collect-data', 'elementpath',
            '--add-data', "${schemaData}:schema_engine/schemas"
        )
        if (-not $DesktopOnly) { $bundleArguments += @('--add-data', "${portalData}:portal/dist") }
        Invoke-Checked $SchemaPython ($bundleArguments + 'schema_engine/__main__.py')
        $engine = Join-Path $bundles 'ProjectDocumentation.Schema'
        Copy-Item -LiteralPath $engine -Destination (Join-Path $desktop 'engine') -Recurse
    } finally {
        $resolved = (Resolve-Path -LiteralPath $bundleWork).Path
        if ($resolved -eq $bundleWork -and (Split-Path -Parent $resolved) -eq $tempRoot) {
            Remove-Item -LiteralPath $resolved -Recurse -Force
        }
    }
    Copy-Item -LiteralPath (Join-Path $repo 'LICENSE.txt') -Destination $desktop
    $licenses = Join-Path $desktop 'licenses'
    $findPythonLicenses = @'
from importlib.metadata import distribution
from sys import argv

package = distribution(argv[1])
for path in package.files or ():
    if any(part.endswith('.dist-info') for part in path.parts) and path.name.lower().startswith(('license', 'copying')):
        print(package.locate_file(path))
'@
    foreach ($package in @('xmlschema', 'elementpath', 'lxml')) {
        $files = @(Invoke-Checked $SchemaPython @('-c', $findPythonLicenses, $package))
        if (-not $files) { throw "Не найдена лицензия $package." }
        $destination = (New-Item -ItemType Directory -Path (Join-Path $licenses $package)).FullName
        Copy-Item -LiteralPath $files -Destination $destination
    }

    $requiredFiles = @(
        'ProjectDocumentation.NET.exe', 'ProjectDocumentation.NET.exe.config', 'dll\Core.NET.dll',
        'dll\System.Data.SQLite.dll', 'dll\x86\SQLite.Interop.dll', 'dll\x64\SQLite.Interop.dll',
        'dll\Microsoft.Web.WebView2.Core.dll', 'dll\Microsoft.Web.WebView2.WinForms.dll',
        'dll\runtimes\win-x86\native\WebView2Loader.dll', 'dll\runtimes\win-x64\native\WebView2Loader.dll',
        'dll\runtimes\win-arm64\native\WebView2Loader.dll', 'engine\ProjectDocumentation.Schema.exe'
    )
    if (-not $DesktopOnly) {
        $requiredFiles += 'engine\_internal\portal\dist\index.html'
        foreach ($package in @('react', 'react-dom')) {
            Copy-Item -LiteralPath (Join-Path $repo "portal\node_modules\$package\LICENSE") -Destination (Join-Path $licenses "$package.txt")
        }
    }
    foreach ($required in $requiredFiles) {
        if (-not (Test-Path -LiteralPath (Join-Path $desktop $required) -PathType Leaf)) {
            throw "Отсутствует файл комплекта $required."
        }
    }

    Write-Output "Desktop: $desktop"
} finally {
    Pop-Location
}
