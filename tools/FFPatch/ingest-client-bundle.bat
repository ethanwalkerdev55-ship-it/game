@echo off
setlocal EnableDelayedExpansion

set "ROOT=D:\work\roberto"
set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "CACHE=%ROOT%\%UUID%"
set "BUNDLE=%CACHE%\main.unity3d"
set "MAIN=%CACHE%\main"
set "CLIENT_IN=%ROOT%\ClientFile"
set "SRC=%CLIENT_IN%\main 2.unity3d"
set "BACKUP=%ROOT%\_inspect_bundle\client"
set "DISUNITY=%ROOT%\tools\DisunityFF\disunity.bat"
set "FFPATCH=%ROOT%\tools\FFPatch\bin\Release\net48\FFPatch.exe"

if not "%~1"=="" set "SRC=%~1"

echo === Ingest client bundle ===
echo Source: %SRC%
echo Game dir: %BUNDLE%

if not exist "%SRC%" (
    echo Missing source bundle. Place client file at:
    echo   %CLIENT_IN%\main 2.unity3d
    exit /b 1
)

dotnet build "%ROOT%\tools\FFPatch\FFPatch.csproj" -c Release -v q
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

if not exist "%BACKUP%" mkdir "%BACKUP%"

echo [1/6] Deploy main.unity3d to game offline cache...
copy /Y "%SRC%" "%BUNDLE%" >nul
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

if exist "%MAIN%" rmdir /S /Q "%MAIN%"
if exist "%CACHE%\main.unity3d_" rmdir /S /Q "%CACHE%\main.unity3d_"
if exist "%CACHE%\patch-test" rmdir /S /Q "%CACHE%\patch-test"
if exist "%CACHE%\patch-test.unity3d" del /Q "%CACHE%\patch-test.unity3d"
if exist "%CACHE%\main.unity3d.test" del /Q "%CACHE%\main.unity3d.test"
if exist "%CACHE%\main.unity3d.bak" del /Q "%CACHE%\main.unity3d.bak"

echo [2/6] Extract bundle (DisunityFF)...
call "%DISUNITY%" bundle-extract "%BUNDLE%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo [3/6] Sync DLLs to _inspect_bundle\client...
copy /Y "%BUNDLE%" "%BACKUP%\main.unity3d" >nul
copy /Y "%MAIN%\Assembly - CSharp.dll" "%BACKUP%\Assembly - CSharp.dll" >nul
if exist "%MAIN%\Assembly - CSharp - first pass.dll" (
    copy /Y "%MAIN%\Assembly - CSharp - first pass.dll" "%BACKUP%\Assembly - CSharp - first pass.dll" >nul
)

echo [4/6] Verify patch markers...
"%FFPATCH%" verify "%MAIN%\Assembly - CSharp.dll"
set "VERIFY=%ERRORLEVEL%"

powershell -NoProfile -Command ^
  "$b='%BUNDLE%'; $h=(Get-FileHash $b -Algorithm SHA256).Hash.ToLower(); $s=(Get-Item $b).Length;" ^
  "Write-Host ('Bundle size='+$s+' hash='+$h)"

echo [5/6] Update launcher manifest...
call "%~dp0fix-launcher-manifest.bat"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo [6/6] Decompile to mods\decompiled (not ClientFile)...
call "%ROOT%\tools\FFSpy\export-assembly-csharp-dnspy.bat"
if %ERRORLEVEL% neq 0 (
    echo Decompile failed — bundle is deployed; fix dnSpy and re-run export-assembly-csharp-dnspy.bat
    exit /b %ERRORLEVEL%
)

echo.
if %VERIFY% equ 0 (
    echo Ingest OK — client DLL contains patch markers.
) else (
    echo Ingest OK — bundle deployed; DLL has NO ForceComplete markers yet ^(vanilla or partial client build^).
)
echo Next: launch-patched-client.bat then test in game.
exit /b 0
