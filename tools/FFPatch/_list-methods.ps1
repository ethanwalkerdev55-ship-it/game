Add-Type -Path 'D:\work\roberto\tools\FFPatch\bin\Release\net48\Mono.Cecil.dll' -ErrorAction SilentlyContinue
$cecil = 'C:\Users\root\.nuget\packages\mono.cecil\0.11.5\lib\net40\Mono.Cecil.dll'
Add-Type -Path $cecil
$path = 'D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll'
$asm = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($path)
$type = $asm.MainModule.Types | Where-Object { $_.Name -eq 'cnMissionManager' } | Select-Object -First 1
$methods = $type.Methods | Where-Object { $_.HasBody } | ForEach-Object {
    [PSCustomObject]@{
        Name = $_.Name
        IL = $_.Body.Instructions.Count
        IsPrivate = $_.IsPrivate
    }
} | Sort-Object IL -Descending
$methods | Select-Object -First 25 | Format-Table -AutoSize
Write-Host "total methods with body:" ($methods.Count)
