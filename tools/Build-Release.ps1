param(
    [string]$OutputRoot = '',
    [string]$Python = 'python.exe',
    [string]$SchemaPython = '',
    [string]$WinFormsOutputDirectory = '',
    [switch]$SkipNuGetRestore,
    [switch]$DesktopOnly
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path -Parent $PSScriptRoot
if (-not $OutputRoot) { $OutputRoot = Join-Path $repo 'artifacts' }
$output = [IO.Path]::GetFullPath($OutputRoot)
$build = Join-Path $output ('release-' + (Get-Date -Format 'yyyyMMdd-HHmmss') + '-' + [guid]::NewGuid().ToString('N').Substring(0, 6))
$stagedRelease = Join-Path $build 'desktop.tmp'
$publishedRelease = Join-Path $build 'desktop'

function Invoke-Checked([string]$Executable, [string[]]$Arguments) {
    & $Executable @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$Executable returned exit code $LASTEXITCODE." }
}

function Read-LicenseNotice([string]$Component, [string]$Path) {
    $license = Get-Content -LiteralPath $Path -Raw -Encoding utf8
    "$Component`r`n`r`n$license"
}

if ($WinFormsOutputDirectory) {
    $WinFormsOutputDirectory = (Resolve-Path -LiteralPath $WinFormsOutputDirectory).Path
    if (-not (Test-Path -LiteralPath (Join-Path $WinFormsOutputDirectory 'ProjectDocumentation.NET.exe') -PathType Leaf)) {
        throw "В каталоге $WinFormsOutputDirectory нет собранного ProjectDocumentation.NET.exe."
    }
} else {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    $msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' |
        Select-Object -First 1
    if (-not $msbuild) { throw 'Не найден MSBuild из Visual Studio.' }
}

# PyInstaller's nested lxml resources need room under the Windows path limit.
$tempRoot = (Resolve-Path -LiteralPath ([IO.Path]::GetTempPath())).Path.TrimEnd('\')
$bundleWork = (New-Item -ItemType Directory -Path (Join-Path $tempRoot ('PD-' + [guid]::NewGuid().ToString('N').Substring(0, 10)))).FullName

Push-Location -LiteralPath $repo
try {
    if (-not $SchemaPython) {
        $venv = Join-Path $bundleWork 'python'
        Invoke-Checked $Python @('-m', 'venv', $venv)
        $SchemaPython = Join-Path $venv 'Scripts\python.exe'
    }
    Invoke-Checked $SchemaPython @('-m', 'pip', 'install', '--disable-pip-version-check', '-r', 'schema_engine/requirements-build.txt')
    Invoke-Checked $SchemaPython @('-X', 'utf8', '-m', 'unittest', 'discover', '-s', 'schema_engine/tests', '-v')

    $portalBuild = Join-Path $bundleWork 'portal'
    if (-not $DesktopOnly) {
        New-Item -ItemType Directory -Path $portalBuild | Out-Null
        $portalSources = Get-ChildItem -LiteralPath (Join-Path $repo 'portal') | Where-Object {
            $_.Name -notin @('node_modules', 'dist')
        }
        Copy-Item -LiteralPath $portalSources.FullName -Destination $portalBuild -Recurse
        Push-Location -LiteralPath $portalBuild
        try {
            Invoke-Checked 'npm.cmd' @('ci')
            Invoke-Checked 'npm.cmd' @('run', 'build')
        } finally {
            Pop-Location
        }
    }

    if (-not $WinFormsOutputDirectory -and -not $SkipNuGetRestore) {
        $config = Join-Path $bundleWork 'NuGet.Config'
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
            $env:NUGET_PACKAGES = Join-Path $bundleWork 'nuget-packages'
            $env:NUGET_HTTP_CACHE_PATH = Join-Path $bundleWork 'nuget-http'
            Invoke-Checked $msbuild @('ProjectDocumentation.sln', '/t:Restore', '/p:RestorePackagesConfig=true', "/p:RestoreConfigFile=$config", '/p:Configuration=Release', '/nr:false', '/v:minimal')
        } finally {
            $env:NUGET_PACKAGES = $oldPackages
            $env:NUGET_HTTP_CACHE_PATH = $oldHttpCache
        }
    }
    $licenseNotices = [System.Collections.Generic.List[string]]::new()
    $licenseNotices.Add((Read-LicenseNotice 'ProjectDocumentation' (Join-Path $repo 'LICENSE.txt')))
    $schemaData = Join-Path $repo 'schema_engine\schemas'
    $portalData = Join-Path $portalBuild 'dist'
    $winforms = $WinFormsOutputDirectory
    if (-not $winforms) {
        $winforms = Join-Path $bundleWork 'winforms'
        Invoke-Checked $msbuild @('ProjectDocumentation.sln', '/t:Rebuild', '/p:Configuration=Release', "/p:OutDir=$winforms/", '/m', '/nr:false', '/v:minimal')
    }

    $desktop = (New-Item -ItemType Directory -Path (Join-Path $bundleWork 'desktop')).FullName
    $dll = (New-Item -ItemType Directory -Path (Join-Path $desktop 'dll')).FullName
    $launchFiles = 'ProjectDocumentation.NET.exe', 'ProjectDocumentation.NET.exe.config' |
        ForEach-Object { Join-Path $winforms $_ }
    Copy-Item -LiteralPath $launchFiles -Destination $desktop
    $libraries = Get-ChildItem -LiteralPath $winforms -File | Where-Object {
        $_.Extension -eq '.dll' -or $_.Name.EndsWith('.dll.config', [StringComparison]::OrdinalIgnoreCase)
    }
    Copy-Item -LiteralPath $libraries.FullName -Destination $dll
    $dependencyFolders = Get-ChildItem -LiteralPath $winforms -Directory | Where-Object {
        $_.Name -in @('ru', 'x86', 'x64', 'runtimes') -or $_.Name -like 'ru-*'
    }
    foreach ($folder in $dependencyFolders) {
        Copy-Item -LiteralPath $folder.FullName -Destination $dll -Recurse
    }
    $webViewLicenses = Join-Path $winforms 'licenses\WebView2'
    foreach ($file in @('LICENSE.txt', 'NOTICE.txt')) {
        $licenseNotices.Add((Read-LicenseNotice "WebView2 / $file" (Join-Path $webViewLicenses $file)))
    }

    $bundles = Join-Path $bundleWork 'dist'
    $bundleArguments = @(
        '-m', 'PyInstaller', '--noconfirm', '--onedir', '--name', 'ProjectDocumentation.Schema',
        '--distpath', $bundles, '--workpath', (Join-Path $bundleWork 'build'), '--specpath', $bundleWork,
        '--paths', $repo, '--collect-data', 'xmlschema', '--collect-data', 'elementpath',
        '--add-data', "${schemaData}\*.xsd:schema_engine/schemas",
        '--add-data', "${schemaData}\*.xsl:schema_engine/schemas"
    )
    if (-not $DesktopOnly) { $bundleArguments += @('--add-data', "${portalData}:portal/dist") }
    Invoke-Checked $SchemaPython ($bundleArguments + 'schema_engine/__main__.py')
    $engine = Join-Path $bundles 'ProjectDocumentation.Schema'
    $locale = Join-Path $engine '_internal\xmlschema\locale'
    if (Test-Path -LiteralPath $locale -PathType Container) {
        $locale = (Resolve-Path -LiteralPath $locale).Path
        Get-ChildItem -LiteralPath $locale -Directory | ForEach-Object {
            $culture = (Resolve-Path -LiteralPath $_.FullName).Path
            if ($_.Name -notin @('en', 'ru') -and (Split-Path -Parent $culture) -eq $locale) {
                Remove-Item -LiteralPath $culture -Recurse
            }
        }
        Get-ChildItem -LiteralPath $locale -Recurse -File -Filter '*.po' | Remove-Item
    }
    Copy-Item -LiteralPath $engine -Destination (Join-Path $desktop 'engine') -Recurse
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
        foreach ($file in $files) {
            $licenseNotices.Add((Read-LicenseNotice "$package / $([IO.Path]::GetFileName($file))" $file))
        }
    }

    $requiredFiles = @(
        'ProjectDocumentation.NET.exe', 'ProjectDocumentation.NET.exe.config', 'dll\Core.NET.dll',
        'dll\ThirdPartyNotices.txt', 'dll\ru\AutoUpdater.NET.resources.dll',
        'dll\System.Data.SQLite.dll', 'dll\x86\SQLite.Interop.dll', 'dll\x64\SQLite.Interop.dll',
        'dll\Microsoft.Web.WebView2.Core.dll', 'dll\Microsoft.Web.WebView2.WinForms.dll',
        'dll\runtimes\win-x86\native\WebView2Loader.dll', 'dll\runtimes\win-x64\native\WebView2Loader.dll',
        'dll\runtimes\win-arm64\native\WebView2Loader.dll', 'engine\ProjectDocumentation.Schema.exe'
    )
    if (-not $DesktopOnly) {
        $requiredFiles += 'engine\_internal\portal\dist\index.html'
        foreach ($package in @('react', 'react-dom')) {
            $licenseNotices.Add((Read-LicenseNotice $package (Join-Path $portalBuild "node_modules\$package\LICENSE")))
        }
    }
    $licenseNotices -join "`r`n`r`n" | Set-Content -LiteralPath (Join-Path $dll 'ThirdPartyNotices.txt') -Encoding utf8
    foreach ($required in $requiredFiles) {
        if (-not (Test-Path -LiteralPath (Join-Path $desktop $required) -PathType Leaf)) {
            throw "Отсутствует файл комплекта $required."
        }
    }

    New-Item -ItemType Directory -Path $build -Force | Out-Null
    Copy-Item -LiteralPath $desktop -Destination $stagedRelease -Recurse
    Move-Item -LiteralPath $stagedRelease -Destination $publishedRelease
    Write-Output "Desktop: $publishedRelease"
} finally {
    Pop-Location
    $resolved = (Resolve-Path -LiteralPath $bundleWork).Path
    if ($resolved -eq $bundleWork -and (Split-Path -Parent $resolved) -eq $tempRoot) {
        Remove-Item -LiteralPath $resolved -Recurse -Force
    }
    if (Test-Path -LiteralPath $stagedRelease -PathType Container) {
        $resolvedStage = (Resolve-Path -LiteralPath $stagedRelease).Path
        if ($resolvedStage -eq $stagedRelease -and (Split-Path -Parent $resolvedStage) -eq $build) {
            Remove-Item -LiteralPath $resolvedStage -Recurse -Force
        }
    }
}
