function Get-TextBytes($path) {
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
            $slice = New-Object byte[] $raw
            [Array]::Copy($b, $ptr, $slice, 0, $raw)
            return $slice
        }
    }
}

$client = Get-TextBytes 'D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll'
$rt = Get-TextBytes 'D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll.roundtrip.dll'
Write-Host "client text len $($client.Length) rt len $($rt.Length)"
$min = [Math]::Min($client.Length, $rt.Length)
$diff = 0
for ($i = 0; $i -lt $min; $i++) { if ($client[$i] -ne $rt[$i]) { $diff++ } }
Write-Host "diffs in first $min bytes: $diff"
if ($rt.Length -gt $client.Length) {
    $extra = $rt[$client.Length..($rt.Length-1)]
    $nz = ($extra | Where-Object { $_ -ne 0 }).Count
    Write-Host "extra tail bytes $($extra.Length) nonZero=$nz"
}
