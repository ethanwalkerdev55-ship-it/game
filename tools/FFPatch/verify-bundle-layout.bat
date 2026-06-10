@echo off
setlocal

set "ROOT=D:\work\roberto"
set "CLIENT_BUNDLE=%ROOT%\_inspect_bundle\client\main.unity3d"
set "BUNDLE=%ROOT%\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main.unity3d"
set "REBUILD=%ROOT%\tools\FFPatch\_disunity_rebuild"
set "CP=%ROOT%\tools\DisunityFF\disunity.jar;%ROOT%\tools\DisunityFF\lib\*;%REBUILD%"

if "%~1" neq "" set "BUNDLE=%~1"

if not exist "%BUNDLE%" (
    echo FAIL: bundle missing: %BUNDLE%
    exit /b 1
)

java -cp "%CP%" BundleHeaderDump "%CLIENT_BUNDLE%" > "%TEMP%\client_hdr.txt"
java -cp "%CP%" BundleHeaderDump "%BUNDLE%" > "%TEMP%\patch_hdr.txt"

for /f "tokens=2 delims==" %%U in ('findstr /C:"lastU=" "%TEMP%\client_hdr.txt"') do set "CLIENT_U=%%U"
for /f "tokens=2 delims==" %%U in ('findstr /C:"lastU=" "%TEMP%\patch_hdr.txt"') do set "PATCH_U=%%U"

echo client u=%CLIENT_U%
echo patched u=%PATCH_U%

if not "%CLIENT_U%"=="%PATCH_U%" (
    echo FAIL: uncompressed layout mismatch — preserve-inject required
    exit /b 1
)

echo verify-bundle-layout: OK (u=%PATCH_U%)
exit /b 0
