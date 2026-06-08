param(

    [Parameter(Mandatory = $true)][string]$Bundle,

    [Parameter(Mandatory = $true)][string]$Manifest,

    [switch]$Pin,

    [switch]$Unpin

)



if ($Unpin) {

    if (Test-Path $Manifest) {

        attrib -R $Manifest 2>$null

        Write-Host "Manifest unpinned (writable): $Manifest"

    }

    exit 0

}



if (-not (Test-Path $Bundle)) {

    Write-Error "Bundle not found: $Bundle"

    exit 1

}

if (-not (Test-Path $Manifest)) {

    Write-Error "Manifest not found: $Manifest"

    exit 1

}



attrib -R $Manifest 2>$null



$hash = (Get-FileHash $Bundle -Algorithm SHA256).Hash.ToLower()

$size = (Get-Item $Bundle).Length

$j = Get-Content $Manifest -Raw | ConvertFrom-Json



Write-Host "BEFORE url=$($j.main_file_url)"

Write-Host "BEFORE hash=$($j.main_file_info.hash) size=$($j.main_file_info.size)"



$j.main_file_info.hash = $hash

$j.main_file_info.size = $size

$j.main_file_url = "file:///D:/work/roberto/6877b37c-e9cd-4826-b82c-5e8d3d5db744/main.unity3d"



$j | ConvertTo-Json -Depth 100 | Set-Content $Manifest -Encoding UTF8



Write-Host "AFTER url=$($j.main_file_url)"

Write-Host "AFTER hash=$hash size=$size"

$offlineCacheDir = Split-Path $Bundle -Parent
$bootstrapScript = Join-Path $PSScriptRoot "ensure-offline-bootstrap.ps1"
if (Test-Path $bootstrapScript) {
    & $bootstrapScript -OfflineCacheDir $offlineCacheDir
}

if ($Pin) {

    attrib +R $Manifest

    Write-Host "Manifest pinned (read-only). Launcher page navigation will not revert to CDN."

}


