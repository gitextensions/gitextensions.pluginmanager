using Neptuo;
using NuGet.Packaging.Core;
using System;

namespace PackageManager.Models
{
    public class NuGetPackageIdentity : IPackageIdentity
    {
        private readonly PackageIdentity identity;

        public string Id => identity.Id;
        public string Version => identity.Version.ToFullString();

        public NuGetPackageIdentity(PackageIdentity identity)
        {
            Ensure.NotNull(identity, "identity");
            this.identity = identity;
        }

        public bool Equals(IPackageIdentity other)
        {
            if (other == null)
                return false;

            return string.Equals(Id, other.Id, StringComparison.CurrentCultureIgnoreCase) && string.Equals(Version, other.Version, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
