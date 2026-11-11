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
    Invoke-Checked $msbuild @('ProjectDocumentation.sln', '/t:Rebuild', '/p:Configuration=Release', "/p:OutDir=$desktop/", '/m', '/nr:false', '/v:minimal')

    $schemaData = Join-Path $repo 'schema_engine\schemas'
    $portalData = Join-Path $repo 'portal\dist'
    # PyInstaller's nested lxml resources need room under the Windows path limit.
    $tempRoot = (Resolve-Path -LiteralPath ([IO.Path]::GetTempPath())).Path.TrimEnd('\')
    $bundleWork = (New-Item -ItemType Directory -Path (Join-Path $tempRoot ('PD-' + [guid]::NewGuid().ToString('N').Substring(0, 10)))).FullName
    try {
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
        Get-ChildItem -LiteralPath $engine | Copy-Item -Destination $desktop -Recurse
    } finally {
        $resolved = (Resolve-Path -LiteralPath $bundleWork).Path
        if ($resolved -eq $bundleWork -and (Split-Path -Parent $resolved) -eq $tempRoot) {
            Remove-Item -LiteralPath $resolved -Recurse -Force
        }
    }
    $requiredFiles = @('ProjectDocumentation.NET.exe', 'ProjectDocumentation.Schema.exe')
    if (-not $DesktopOnly) {
        Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'Start-Portal.cmd') -Destination $desktop
        $requiredFiles += @('Start-Portal.cmd', '_internal\portal\dist\index.html')
    }
    foreach ($required in $requiredFiles) {
        if (-not (Test-Path -LiteralPath (Join-Path $desktop $required) -PathType Leaf)) {
            throw "Отсутствует файл комплекта $required."
        }
    }

    Write-Output "Desktop: $desktop"
    if (-not $DesktopOnly) { Write-Output "Portal entry point: $(Join-Path $desktop 'Start-Portal.cmd')" }
} finally {
    Pop-Location
}
