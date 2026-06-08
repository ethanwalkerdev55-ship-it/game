@echo off
setlocal

set "ROOT=D:\work\roberto"
set "CLIENT_DLL=%ROOT%\_inspect_bundle\client\Assembly - CSharp.dll"
set "STAGED=%ROOT%\tools\FFPatch\staging\Assembly-CSharp.patched.dll"
set "FFPATCH=%ROOT%\tools\FFPatch\bin\Release\net48\FFPatch.exe"

if not exist "%CLIENT_DLL%" (
    echo Missing client base DLL. Run sync-client-backup.bat
    exit /b 1
)

if not exist "%ROOT%\tools\FFPatch\staging" mkdir "%ROOT%\tools\FFPatch\staging"

dotnet build "%ROOT%\tools\FFPatch\FFPatch.csproj" -c Release -v q
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo Building staging DLL (SAFE)...
"%FFPATCH%" apply-client-safe "%STAGED%" "%CLIENT_DLL%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo.
echo Staging OK: %STAGED%
echo Next: run-verify-staging.bat then apply-mission-patch-client-deploy.bat
exit /b 0
