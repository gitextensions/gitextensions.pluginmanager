using NuGet.Versioning;
using System.Collections.Generic;

namespace PackageManager.Models
{
    public class NuGetPackageVersionComparer : IComparer<IPackageIdentity>
    {
        public static readonly NuGetPackageVersionComparer Instance = new NuGetPackageVersionComparer();

        public int Compare(IPackageIdentity? x, IPackageIdentity? y)
        {
            if (x == null && y == null)
                return 0;
            else if (x == null)
                return -1;
            else if (y == null)
                return 1;

            NuGetVersion xVersion = new NuGetVersion(x.Version);
            NuGetVersion yVersion = new NuGetVersion(y.Version);
            return xVersion.CompareTo(yVersion);
        }
    }
}
