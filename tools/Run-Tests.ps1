Push-Location $PSScriptRoot;

$targetPath = '..\';

$isAppveyor = $True -eq $env:APPVEYOR;

If (!$isAppveyor)
{
    Write-Host "Running locally";

    $targetPath = Join-Path $targetPath 'artifacts';
}

dotnet test ..\test\PackageManager.NuGet.Tests\PackageManager.NuGet.Tests.csproj -c Release --test-adapter-path:.. --logger:Appveyor /property:Platform=AnyCPU -bl:$targetPath\build-PackageManager.NuGet.Tests.binlog
dotnet test ..\test\PackageManager.Tests\PackageManager.Tests.csproj -c Release --test-adapter-path:.. --logger:Appveyor /property:Platform=AnyCPU -bl:$targetPath\build-PackageManager.Tests.binlog

Pop-Location;