@echo off
cd /d D:\work\roberto\tools\FFPatch
dotnet build FFPatch.csproj -c Release -v q
bin\Release\net48\FFPatch.exe roundtrip-test "D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll"
