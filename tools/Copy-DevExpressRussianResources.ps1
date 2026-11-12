[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$BuildDirectory,

    [Parameter(Mandatory = $true)]
    [string]$ResourcesDirectory
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $BuildDirectory -PathType Container)) {
    throw "Build output directory does not exist: '$BuildDirectory'. Build the desktop project before copying DevExpress resources."
}
if (-not (Test-Path -LiteralPath $ResourcesDirectory -PathType Container)) {
    throw "DevExpress Russian resources directory does not exist: '$ResourcesDirectory'. Set DevExpress24RussianResourcesDir to the Framework directory of DevExpressLocalizedResources_2024.2_ru."
}

$required = @(
    'DevExpress.Data.v24.2.resources.dll'
    'DevExpress.Dialogs.v24.2.Core.resources.dll'
    'DevExpress.Utils.v24.2.resources.dll'
    'DevExpress.XtraBars.v24.2.resources.dll'
    'DevExpress.XtraEditors.v24.2.resources.dll'
    'DevExpress.XtraGrid.v24.2.resources.dll'
)
$publicKeyToken = 'b88d1754d700e49a'
$resources = @{}
foreach ($mainFile in Get-ChildItem -LiteralPath $BuildDirectory -File -Filter 'DevExpress.*.dll') {
    $resourceFileName = $mainFile.BaseName + '.resources.dll'
    $resourcePath = Join-Path $ResourcesDirectory $resourceFileName
    if (-not (Test-Path -LiteralPath $resourcePath -PathType Leaf)) {
        continue
    }

    try {
        $main = [Reflection.AssemblyName]::GetAssemblyName($mainFile.FullName)
        $resource = [Reflection.AssemblyName]::GetAssemblyName($resourcePath)
    } catch {
        throw "Cannot read the DevExpress assembly identities for '$($mainFile.FullName)' and '$resourcePath'. $($_.Exception.Message)"
    }

    $mainToken = -join $(foreach ($byte in $main.GetPublicKeyToken()) { $byte.ToString('x2') })
    $resourceToken = -join $(foreach ($byte in $resource.GetPublicKeyToken()) { $byte.ToString('x2') })
    # DevExpress 24.2.5 uses satellites versioned 24.2.0.0.
    if ($resource.Name -ne "$($main.Name).resources" -or $resource.CultureName -ne 'ru' -or
        $main.Version.Major -ne 24 -or $main.Version.Minor -ne 2 -or
        $resource.Version -ne [version]'24.2.0.0' -or
        $mainToken -ne $publicKeyToken -or $resourceToken -ne $publicKeyToken) {
        throw "DevExpress Russian resource '$resourcePath' has identity '$($resource.FullName)' and is incompatible with '$($main.FullName)'. Use the Russian DevExpress 24.2 Framework resource pack."
    }

    $resources[$resourceFileName] = $resourcePath
}

$missing = @($required | Where-Object { -not $resources.ContainsKey($_) })
if ($missing.Count -gt 0) {
    throw "Missing required Russian DevExpress resources or their matching build assemblies: $($missing -join ', '). Check '$ResourcesDirectory' and '$BuildDirectory'."
}

$russianDirectory = Join-Path $BuildDirectory 'ru'
New-Item -ItemType Directory -Path $russianDirectory -Force | Out-Null
Copy-Item -LiteralPath @($resources.Values) -Destination $russianDirectory -Force
Write-Host "Copied $($resources.Count) Russian DevExpress resource assemblies to '$russianDirectory'."
