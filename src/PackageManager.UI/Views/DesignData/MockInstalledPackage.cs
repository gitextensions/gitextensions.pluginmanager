using PackageManager.Models;

namespace PackageManager.Views.DesignData
{
    internal class MockInstalledPackage : IInstalledPackage
    {
        public IPackage Definition { get; }
        public bool IsCompatible { get; }

        public MockInstalledPackage(IPackage definition, bool isCompatible)
        {
            Definition = definition;
            IsCompatible = isCompatible;
        }
    }
}
