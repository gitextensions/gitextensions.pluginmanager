using Neptuo;

namespace PackageManager.Models
{
    public class NuGetInstalledPackage : IInstalledPackage
    {
        public IPackage Definition { get; }
        public bool IsCompatible { get; }

        public NuGetInstalledPackage(IPackage definition, bool isCompatible)
        {
            Ensure.NotNull(definition, "definition");
            Definition = definition;
            IsCompatible = isCompatible;
        }
    }
}
