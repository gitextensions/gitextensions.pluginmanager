Push-Location $PSScriptRoot;

$targetPath = '..\';

$isAppveyor = $True -eq $env:APPVEYOR;

If (!$isAppveyor)
{
    Write-Host "Running a local build";

    $targetPath = Join-Path $targetPath 'artifacts';
}

dotnet restore ..\GitExtensions.PluginManager.sln

msbuild ..\GitExtensions.PluginManager.sln /p:Configuration=Release -verbosity:minimal
if (!($LastExitCode -eq 0))
{
    Write-Error -Message "MSBuild failed with $LastExitCode" -ErrorAction Stop
}

$packPath = Join-Path ".." $targetPath;
dotnet pack ..\src\GitExtensions.PluginManager -c Release -o $packPath --no-build

Copy-Item ..\src\GitExtensions.PluginManager\bin\Release\GitExtensions.PluginManager.*.zip $targetPath
Copy-Item ..\src\GitExtensions.PluginManager\bin\Release\GitExtensions.PluginManager.*.nupkg $targetPath

Pop-Location;