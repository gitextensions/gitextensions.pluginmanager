using PackageManager.Models;
using System;

namespace PackageManager.Views.DesignData
{
    public class MockPackageSource : IPackageSource
    {
        public string Name { get; }
        public Uri Uri { get; }

        public MockPackageSource(string name, Uri uri)
        {
            Name = name;
            Uri = uri;
        }
    }
}
