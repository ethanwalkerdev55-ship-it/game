@echo off
setlocal

set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "SRC=%LOCALAPPDATA%\OpenFusionLauncher\ffcache\%UUID%"
set "DST=D:\work\roberto\%UUID%"

if not exist "%SRC%" (
    echo ffcache not found: %SRC%
    echo Launch game once with default cache path to populate ffcache.
    exit /b 1
)

echo Syncing decompressed ffcache dirs into offline cache...
robocopy "%SRC%" "%DST%" /E /XO /NFL /NDL /NJH /NJS /nc /ns /np
echo Exit code: %ERRORLEVEL% (0-7 = success)
exit /b 0
