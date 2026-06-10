@echo off
setlocal
set "CLIENT=D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll"
set "FF=D:\work\roberto\tools\FFPatch\bin\Release\net48\FFPatch.exe"
cd /d D:\work\roberto\tools\FFPatch
dotnet build FFPatch.csproj -c Release -v q

"%FF%" roundtrip-test "%CLIENT%"
if errorlevel 1 exit /b 1

copy /y "%CLIENT%.roundtrip.dll" "staging\rt.dll" >nul
"%FF%" pe-shell-fit "staging\rt.dll" "%CLIENT%"
if errorlevel 1 exit /b 1

"%FF%" verify-load-safe "staging\rt.dll" "%CLIENT%"
exit /b %ERRORLEVEL%
