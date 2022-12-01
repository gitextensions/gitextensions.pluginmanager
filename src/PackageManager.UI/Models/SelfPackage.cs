using Neptuo;
using System;

namespace PackageManager.Models
{
    internal class SelfPackage : IPackageIdentity
    {
        public string Id { get; }
        public string Version { get; }

        public SelfPackage(string id)
        {
            Ensure.NotNull(id, "id");
            Id = id;
            Version = VersionInfo.Version;

            int indexOfPlus = Version.IndexOf('+');
            if (indexOfPlus > 0)
                Version = Version.Substring(0, indexOfPlus);
        }

        public bool Equals(IPackageIdentity? other)
        {
            if (other == null)
                return false;

            return string.Equals(Id, other.Id, StringComparison.CurrentCultureIgnoreCase) && string.Equals(Version, other.Version, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
