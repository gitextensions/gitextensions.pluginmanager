﻿using PackageManager.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PackageManager.Services
{
    public interface IInstallService
    {
        string Path { get; }

        bool IsInstalled(string packageId);
        bool IsInstalled(IPackageIdentity package);

        void Install(IPackageIdentity package);
        void Uninstall(IPackageIdentity package);

        Task<IReadOnlyCollection<IInstalledPackage>> GetInstalledAsync(IEnumerable<IPackageSource> packageSources, CancellationToken cancellationToken);
        Task<IPackageIdentity> FindInstalledAsync(string packageId, CancellationToken cancellationToken);
    }
}
