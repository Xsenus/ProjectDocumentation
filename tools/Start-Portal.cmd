@echo off
"%~dp0ProjectDocumentation.Schema.exe" --port 8765 --open-browser
if errorlevel 1 pause
