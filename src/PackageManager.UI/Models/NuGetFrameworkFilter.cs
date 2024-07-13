using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging;

namespace PackageManager.Models
{
    internal class NuGetFrameworkFilter : NuGetPackageContent.IFrameworkFilter
    {
        private readonly IReadOnlyCollection<NuGetFramework> collection;

        public NuGetFrameworkFilter(IReadOnlyCollection<NuGetFramework> collection)
            => this.collection = collection;

        public bool IsPassed(FrameworkSpecificGroup group)
            => collection.Contains(group.TargetFramework);
    }
}
