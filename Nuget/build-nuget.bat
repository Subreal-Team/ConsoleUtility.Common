@echo off
set path=.
set packageName=SubRealTeam.ConsoleUtility.Common
set packageDirName=Common
if not [%1]==[] set path=%1

IF NOT EXIST Packages MKDIR Packages

echo Creating %packageName% Package
 %path%\..\.nuget\nuget pack %path%\..\%packageDirName%\%packageName%.csproj -IncludeReferencedProjects -OutputDirectory %path%\..\Nuget\Packages -Properties Configuration=Release

if errorlevel 1 echo Error creating %packageName% Package

pause;