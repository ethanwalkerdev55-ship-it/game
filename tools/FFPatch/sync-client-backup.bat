@echo off
setlocal

set "ROOT=D:\work\roberto"
set "DST=%ROOT%\_inspect_bundle\client"
set "SRC_BUNDLE=%ROOT%\ClientFile\main 2.unity3d"
if not exist "%SRC_BUNDLE%" set "SRC_BUNDLE=%ROOT%\ClientFile\main.unity3d"

if not exist "%SRC_BUNDLE%" (
    echo ClientFile\main 2.unity3d not found.
    exit /b 1
)

if not exist "%DST%" mkdir "%DST%"

echo Syncing ClientFile -^> _inspect_bundle\client ...
copy /Y "%SRC_BUNDLE%" "%DST%\main.unity3d" >nul
echo Run ingest-client-bundle.bat to extract DLLs from the bundle.

for %%F in (main.unity3d "Assembly - CSharp.dll" "Assembly - CSharp - first pass.dll") do (
    if exist "%DST%\%%~F" echo   %%~F — OK
)

echo Client backup ready: %DST%
exit /b 0
