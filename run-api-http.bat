@echo off
set "ASPNETCORE_URLS=http://localhost:5183"
set "ASPNETCORE_ENVIRONMENT=Production"
echo Iniciando API em %ASPNETCORE_URLS% (ENV=%ASPNETCORE_ENVIRONMENT%)
"%~dp0SeniorAPITeste.exe"
pause
