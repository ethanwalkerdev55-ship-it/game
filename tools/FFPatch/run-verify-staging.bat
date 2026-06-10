@echo off
setlocal

set "ROOT=D:\work\roberto"
set "CLIENT_DLL=%ROOT%\_inspect_bundle\client\Assembly - CSharp.dll"
set "STAGED=%ROOT%\tools\FFPatch\staging\Assembly-CSharp.patched.dll"
set "FFPATCH=%ROOT%\tools\FFPatch\bin\Release\net48\FFPatch.exe"

if not exist "%STAGED%" (
    echo Run apply-mission-patch-staging.bat first.
    exit /b 1
)

dotnet build "%ROOT%\tools\FFPatch\FFPatch.csproj" -c Release -v q

"%FFPATCH%" verify-il "%STAGED%" --lite
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%
"%FFPATCH%" verify-load-safe "%STAGED%" "%CLIENT_DLL%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

"%FFPATCH%" verify-no-donor-refs "%STAGED%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

"%FFPATCH%" verify-bootstrap "%STAGED%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

"%FFPATCH%" verify-safe-plus "%STAGED%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

exit /b 0
