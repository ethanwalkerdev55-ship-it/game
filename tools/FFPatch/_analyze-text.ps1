function Get-TextSlice($path) {
    $b = [IO.File]::ReadAllBytes($path)
    $peOff = [BitConverter]::ToInt32($b, 0x3C)
    $optSize = [BitConverter]::ToInt16($b, $peOff + 20)
    $secStart = $peOff + 24 + $optSize
    for ($i = 0; $i -lt [BitConverter]::ToUInt16($b, $peOff + 6); $i++) {
        $o = $secStart + ($i * 40)
        $name = [Text.Encoding]::ASCII.GetString($b, $o, 8).Trim([char]0)
        if ($name -eq '.text') {
            $raw = [BitConverter]::ToInt32($b, $o + 16)
            $ptr = [BitConverter]::ToInt32($b, $o + 20)
            return ,@($raw, $ptr, $b[$ptr..($ptr + $raw - 1)])
        }
    }
}

$clientPath = 'D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll'
# use last failed inject output from staging work - copy client and manually find
$paths = @(
    'D:\work\roberto\tools\FFPatch\staging\Assembly-CSharp.patched.dll.work.tmp',
    'D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll.roundtrip.dll'
)
foreach ($p in $paths) {
    if (-not (Test-Path $p)) { continue }
    $t = Get-TextSlice $p
    $raw = $t[0]; $slice = $t[2]
    $tail = 0
    for ($i = $slice.Length - 1; $i -ge 0; $i--) { if ($slice[$i] -eq 0) { $tail++ } else { break } }
    $nonZeroTail = $slice.Length - $tail
    Write-Host "$p textRaw=$raw tailZeros=$tail lastNonZero=$nonZeroTail fileLen=$((Get-Item $p).Length)"
}
