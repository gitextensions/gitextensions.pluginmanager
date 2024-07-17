using PackageManager.Models;
using PackageManager.ViewModels;
using System;
using System.Collections.Generic;

namespace PackageManager.Views.DesignData
{
    public class MockPackageSourceProvider : IPackageSourceSelector
    {
        public IEnumerable<IPackageSource> Sources => new List<IPackageSource>(1) { new MockPackageSource("NuGet.org", new Uri("https://api.nuget.org/v3/index.json", UriKind.Absolute)) };
    }
}
