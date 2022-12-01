﻿using PackageManager.Models;
using PackageManager.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PackageManager.Views.DesignData
{
    internal class MockInstallService : IInstallService
    {
        public string Path => @"C:\Temp";
        public IPackageIdentity? Installed { get; set; }

        public bool IsInstalled(string packageId)
            => false;

        public bool IsInstalled(IPackageIdentity package)
            => string.Equals(Installed?.Id, package?.Id, StringComparison.CurrentCultureIgnoreCase) && string.Equals(Installed?.Version, package?.Version, StringComparison.CurrentCultureIgnoreCase);

        public void Install(IPackageIdentity package)
        { }

        public void Uninstall(IPackageIdentity package)
        { }

        public Task<IReadOnlyCollection<IInstalledPackage>> GetInstalledAsync(IEnumerable<IPackageSource> packageSources, CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyCollection<IInstalledPackage>>(
                new List<IInstalledPackage>()
                {
                    ViewModelLocator.IncompatiblePackage,
                    ViewModelLocator.CompatiblePackage
                }
            );
        }

        public Task<IPackageIdentity?> FindInstalledAsync(string packageId, CancellationToken cancellationToken)
        {
            return Task.FromResult<IPackageIdentity?>(null);
        }
    }
}
