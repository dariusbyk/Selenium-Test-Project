@echo off
cd /d "%~dp0"
if exist Reports (
    rmdir /s /q Reports
)
mkdir Reports
dotnet test SeleniumHomework.csproj --logger "html;LogFileName=report.html" --results-directory Reports
set TEST_EXIT_CODE=%ERRORLEVEL%
echo.
echo Test process exit code: %TEST_EXIT_CODE%
if exist Reports\report.html (
    echo Report: Reports\report.html
) else (
    echo No report created. Check build and test startup errors above.
)
pause
exit /b %TEST_EXIT_CODE%
