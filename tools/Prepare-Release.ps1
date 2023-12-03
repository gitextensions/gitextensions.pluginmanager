Push-Location $PSScriptRoot;

$targetPath = '..\';

$isAppveyor = $True -eq $env:APPVEYOR;

If (!$isAppveyor)
{
    Write-Host "Running a local build";

    $targetPath = Join-Path $targetPath 'artifacts';
}

function EnsureLastCommandSucceeded()
{
    if (!($LastExitCode -eq 0))
    {
        Pop-Location;
        Write-Error -Message "MSBuild failed with $LastExitCode" -ErrorAction Stop
    }
}

# Install the required .NET SDK
Invoke-WebRequest "https://dot.net/v1/dotnet-install.ps1" -OutFile "./dotnet-install.ps1"
./dotnet-install.ps1 -Channel LTS -InstallDir 'C:\Program Files\dotnet'
# Remove the script so it doesn't "pollute" the build
Remove-Item -Path .\dotnet-install.ps1

Write-Host "Restore solution"
dotnet restore ..\GitExtensions.PluginManager.sln -p:RuntimeIdentifier=win-x86
EnsureLastCommandSucceeded

Write-Host "Publish PackageManager.UI"
dotnet publish ..\src\PackageManager.UI\PackageManager.UI.csproj -c Release -p:PublishDir=bin\Release\net8.0-windows\publish\ -bl:$targetPath\build-PackageManager.UI.binlog
EnsureLastCommandSucceeded

Write-Host "Publish GitExtensions.PluginManager"
dotnet publish ..\src\GitExtensions.PluginManager\GitExtensions.PluginManager.csproj --configuration Release -verbosity:minimal -bl:$targetPath\build-GitExtensions.PluginManager.binlog
EnsureLastCommandSucceeded

if (!(Test-Path $targetPath))
{
      New-Item -ItemType Directory -Force -Path $targetPath
}

Write-Host "Copy artifacts"
Copy-Item ..\src\GitExtensions.PluginManager\bin\Release\GitExtensions.PluginManager.*.zip $targetPath
Copy-Item ..\src\GitExtensions.PluginManager\bin\Release\GitExtensions.PluginManager.*.nupkg $targetPath

Pop-Location;