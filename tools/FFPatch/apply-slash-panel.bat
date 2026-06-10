@echo off
setlocal EnableDelayedExpansion

set "ROOT=D:\work\roberto"
set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "BUNDLE=%ROOT%\%UUID%\main.unity3d"
set "MAIN=%ROOT%\%UUID%\main"
set "TARGET=%MAIN%\Assembly - CSharp.dll"
set "CLIENT_BUNDLE=%ROOT%\_inspect_bundle\client\main.unity3d"
set "FFPATCH=%ROOT%\tools\FFPatch\bin\Release\net48\FFPatch.exe"
set "DISUNITY=%ROOT%\tools\DisunityFF\disunity.bat"

echo === Apply slash-key panel (ClientMod) ===

if not exist "%TARGET%" (
    echo Missing DLL. Run ingest-client-bundle.bat first.
    exit /b 1
)

dotnet build "%ROOT%\tools\FFPatch\FFPatch.csproj" -c Release -v q
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

"%FFPATCH%" apply-slash-panel "%TARGET%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

for %%F in ("%TARGET%") do set "DLL_SIZE=%%~zF"
if %DLL_SIZE% LSS 1000000 (
    echo FAIL: patched DLL too small (%DLL_SIZE% bytes^). Aborting inject.
    exit /b 1
)

del /Q "%MAIN%\*.slash-panel.tmp" 2>nul
del /Q "%MAIN%\Assembly - CSharp.dll.*.tmp" 2>nul

for %%F in ("%BUNDLE%") do set "BASE_SIZE=%%~zF"

call "%DISUNITY%" bundle-inject "%BUNDLE%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

del /Q "%MAIN%\*.slash-panel.tmp" 2>nul
del /Q "%MAIN%\Assembly - CSharp.dll.*.tmp" 2>nul

for %%F in ("%BUNDLE%") do set "PATCHED_SIZE=%%~zF"
set /a "MIN_SIZE=%BASE_SIZE%*98/100"
if %PATCHED_SIZE% LSS %MIN_SIZE% (
    echo FAIL: bundle-inject shrank main.unity3d too far (%PATCHED_SIZE% ^< %MIN_SIZE% bytes^).
    echo Restoring from client backup...
    if exist "%CLIENT_BUNDLE%" copy /Y "%CLIENT_BUNDLE%" "%BUNDLE%" >nul
    exit /b 1
)
if %PATCHED_SIZE% LSS %BASE_SIZE% (
    echo NOTE: bundle slightly smaller after inject (%PATCHED_SIZE% vs %BASE_SIZE%^).
)

call "%~dp0fix-launcher-manifest.bat"
call "%~dp0launch-patched-client.bat"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo.
echo Slash panel deployed. In game press / or F9 to toggle the settings-style window.
echo UDP log should show: [ClientMod] Slash panel open
exit /b 0
