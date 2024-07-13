using Neptuo;
using NuGet.Configuration;
using System;

namespace PackageManager.Models
{
    public class NuGetPackageSource : IPackageSource
    {
        internal PackageSource Original { get; }

        public string Name => Original.Name;
        public Uri Uri => Original.SourceUri;

        public NuGetPackageSource(PackageSource source)
        {
            Ensure.NotNull(source, "source");
            Original = source;
        }
    }
}
