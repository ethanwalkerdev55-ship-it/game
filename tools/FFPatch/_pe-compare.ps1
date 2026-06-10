function Get-PeInfo($path) {
    $b = [IO.File]::ReadAllBytes($path)
    $peOff = [BitConverter]::ToInt32($b, 0x3C)
    $numSec = [BitConverter]::ToInt16($b, $peOff + 6)
    $optSize = [BitConverter]::ToInt16($b, $peOff + 20)
    $secStart = $peOff + 24 + $optSize
    $secs = @()
    for ($i = 0; $i -lt $numSec; $i++) {
        $o = $secStart + ($i * 40)
        $name = [Text.Encoding]::ASCII.GetString($b, $o, 8).Trim([char]0)
        $vsize = [BitConverter]::ToInt32($b, $o + 8)
        $vaddr = [BitConverter]::ToInt32($b, $o + 12)
        $raw = [BitConverter]::ToInt32($b, $o + 16)
        $rawptr = [BitConverter]::ToInt32($b, $o + 20)
        $secs += [PSCustomObject]@{ Name=$name; VirtSize=$vsize; RawSize=$raw; RawPtr=$rawptr }
    }
    [PSCustomObject]@{ Path=$path; Length=$b.Length; Sections=$secs }
}

$c = Get-PeInfo 'D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll'
Write-Host "CLIENT $($c.Length)"
$c.Sections | Format-Table -AutoSize
