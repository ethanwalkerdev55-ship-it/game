@echo off
setlocal

set "ROOT=D:\work\roberto"
set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "CACHE=%ROOT%\%UUID%"

echo === Cleanup workspace artifacts ===
echo Keeps: tools, mods, ClientFile inbox, game cache, _inspect_bundle\client, _inspect_udp_listener

if exist "%ROOT%\_inspect_dll_diff" (
    echo Removing _inspect_dll_diff...
    rmdir /S /Q "%ROOT%\_inspect_dll_diff"
)
if exist "%ROOT%\_inspect_dll_diff2" (
    echo Removing _inspect_dll_diff2...
    rmdir /S /Q "%ROOT%\_inspect_dll_diff2"
)
if exist "%ROOT%\_inspect_dnspy_style" (
    echo Removing _inspect_dnspy_style...
    rmdir /S /Q "%ROOT%\_inspect_dnspy_style"
)
if exist "%ROOT%\_inspect_bundle\reference-donor" (
    echo Removing _inspect_bundle\reference-donor...
    rmdir /S /Q "%ROOT%\_inspect_bundle\reference-donor"
)
if exist "%ROOT%\_inspect_bundle\client\extracted" (
    echo Removing _inspect_bundle\client\extracted...
    rmdir /S /Q "%ROOT%\_inspect_bundle\client\extracted"
)
if exist "%ROOT%\disunityff" (
    echo Removing duplicate disunityff...
    rmdir /S /Q "%ROOT%\disunityff"
)
if exist "%ROOT%\FFSpy-7.0" (
    echo Removing duplicate FFSpy-7.0...
    rmdir /S /Q "%ROOT%\FFSpy-7.0"
)
if exist "%ROOT%\dlls" (
    echo Removing dlls...
    rmdir /S /Q "%ROOT%\dlls"
)
if exist "%ROOT%\dnSpy-net-win64" (
    echo Removing duplicate dnSpy-net-win64 ^(using tools\dnSpyEx^)...
    rmdir /S /Q "%ROOT%\dnSpy-net-win64"
)
if exist "%ROOT%\commad.txt" del /Q "%ROOT%\commad.txt"
if exist "%ROOT%\udp_listener (1).zip" del /Q "%ROOT%\udp_listener (1).zip"

if exist "%ROOT%\ClientFile\main" (
    echo Removing ClientFile\main extract folder...
    rmdir /S /Q "%ROOT%\ClientFile\main"
)
if exist "%ROOT%\ClientFile\_bundle.work.unity3d" del /Q "%ROOT%\ClientFile\_bundle.work.unity3d"

if exist "%CACHE%\patch-test" rmdir /S /Q "%CACHE%\patch-test"
if exist "%CACHE%\patch-test.unity3d" del /Q "%CACHE%\patch-test.unity3d"
if exist "%CACHE%\main.unity3d.test" del /Q "%CACHE%\main.unity3d.test"
if exist "%CACHE%\main.unity3d.bak" del /Q "%CACHE%\main.unity3d.bak"
if exist "%CACHE%\main.unity3d_" rmdir /S /Q "%CACHE%\main.unity3d_"

if not exist "%ROOT%\ClientFile\_archive" mkdir "%ROOT%\ClientFile\_archive"
if exist "%ROOT%\ClientFile\main.unity3d" (
    echo Archiving ClientFile\main.unity3d...
    move /Y "%ROOT%\ClientFile\main.unity3d" "%ROOT%\ClientFile\_archive\main.unity3d" >nul
)
if exist "%ROOT%\ClientFile\Assembly - CSharp.dll" (
    move /Y "%ROOT%\ClientFile\Assembly - CSharp.dll" "%ROOT%\ClientFile\_archive\" >nul
)
if exist "%ROOT%\ClientFile\Assembly - CSharp - first pass.dll" (
    move /Y "%ROOT%\ClientFile\Assembly - CSharp - first pass.dll" "%ROOT%\ClientFile\_archive\" >nul
)

echo.
echo Cleanup done.
exit /b 0
