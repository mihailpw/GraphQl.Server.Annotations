@echo off 

set buildoutdir=out
set nugetoutdir=nuget-out
	
rmdir %buildoutdir% /Q /S nonemptydir
mkdir %buildoutdir%

dotnet publish "../src/GraphQl.Server.Annotations/GraphQl.Server.Annotations.csproj" -o "%buildoutdir%" -c Release --nologo

rmdir %nugetoutdir% /Q /S nonemptydir
mkdir %nugetoutdir%

nuget.exe pack "GraphQl.Server.Annotations.nuspec" -OutputDirectory %nugetoutdir%