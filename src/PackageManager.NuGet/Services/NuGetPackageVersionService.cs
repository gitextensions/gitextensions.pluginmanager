﻿using Neptuo;
using Neptuo.Logging;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using PackageManager.Logging;
using PackageManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PackageManager.Services
{
    public class NuGetPackageVersionService
    {
        private readonly INuGetPackageFilter filter;
        private readonly NuGetPackageContent.IFrameworkFilter? frameworkFilter;
        private readonly NuGetPackageContentService contentService;
        private readonly ILog log;
        private readonly ILogger nuGetLog;

        public NuGetPackageVersionService(NuGetPackageContentService contentService, ILog log, INuGetPackageFilter? filter = null, NuGetPackageContent.IFrameworkFilter? frameworkFilter = null)
        {
            Ensure.NotNull(contentService, "contentService");
            Ensure.NotNull(log, "log");
            this.contentService = contentService;
            this.log = log;
            this.nuGetLog = new NuGetLogger(log);

            if (filter == null)
                filter = OkNuGetPackageFilter.Instance;

            this.filter = filter;
            this.frameworkFilter = frameworkFilter;
        }

        public async Task<IReadOnlyList<IPackage>> GetListAsync(int resultCount, IPackageSearchMetadata package, SourceRepository repository, Func<IPackageSearchMetadata, IPackageSearchMetadata, bool>? versionFilter = null, bool isPrereleaseIncluded = false, CancellationToken cancellationToken = default)
        {
            if (versionFilter == null)
                versionFilter = (source, target) => true;

            try
            {
                List<IPackage> result = new List<IPackage>();
                if (await SearchOlderVersionsDirectlyAsync(result, resultCount, package, repository, versionFilter, cancellationToken))
                    return result;

                if (await SearchOlderVersionsUsingMetadataResourceAsync(result, resultCount, package, repository, versionFilter, isPrereleaseIncluded, cancellationToken))
                    return result;

                return new List<IPackage>();
            }
            catch (FatalProtocolException e) when (e.InnerException is TaskCanceledException)
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw e;
            }
        }

        private async Task<bool> SearchOlderVersionsDirectlyAsync(List<IPackage> result, int resultCount, IPackageSearchMetadata package, SourceRepository repository, Func<IPackageSearchMetadata, IPackageSearchMetadata, bool> versionFilter, CancellationToken cancellationToken)
        {
            bool isSuccess = false;
            IEnumerable<VersionInfo>? versions = null;

            try
            {
                versions = await package.GetVersionsAsync();
            }
            catch (NullReferenceException)
            {
            }

            if (versions == null)
                return false;

            foreach (VersionInfo version in versions)
            {
                // TODO: Filter prelease on V2 feed.
                if (version.PackageSearchMetadata != null && versionFilter(package, version.PackageSearchMetadata))
                {
                    IPackage? item = await ProcessOlderVersionAsync(repository, version.PackageSearchMetadata, cancellationToken);
                    if (item != null)
                    {
                        result.Add(item);
                        if (result.Count == resultCount)
                            return true;

                        isSuccess = true;
                    }
                }
            }

            return isSuccess;
        }

        private async Task<bool> SearchOlderVersionsUsingMetadataResourceAsync(List<IPackage> result, int resultCount, IPackageSearchMetadata package, SourceRepository repository, Func<IPackageSearchMetadata, IPackageSearchMetadata, bool> versionFilter, bool isPrereleaseIncluded, CancellationToken cancellationToken)
        {
            PackageMetadataResource metadataResource = await repository.GetResourceAsync<PackageMetadataResource>(cancellationToken);
            if (metadataResource == null)
                return false;

            using (var sourceCacheContext = new SourceCacheContext())
            {
                IEnumerable<IPackageSearchMetadata> versions = await metadataResource.GetMetadataAsync(
                    package.Identity.Id,
                    isPrereleaseIncluded,
                    false,
                    sourceCacheContext,
                    nuGetLog,
                    cancellationToken
                );

                versions = versions.OrderByDescending(p => p.Identity.Version, VersionComparer.Default);
                foreach (IPackageSearchMetadata version in versions)
                {
                    if (versionFilter(package, version))
                    {
                        IPackage? item = await ProcessOlderVersionAsync(repository, version, cancellationToken);
                        if (item != null)
                        {
                            result.Add(item);
                            if (result.Count == resultCount)
                                return true;
                        }
                    }
                }
            }

            return true;
        }

        private async Task<IPackage?> ProcessOlderVersionAsync(SourceRepository repository, IPackageSearchMetadata version, CancellationToken cancellationToken)
        {
            log.Debug($"Found '{version.Identity}'.");

            NuGetPackageFilterResult filterResult = await filter.FilterAsync(repository, version, cancellationToken);
            switch (filterResult)
            {
                case NuGetPackageFilterResult.Ok:
                    log.Debug("Package added.");
                    return new NuGetPackage(version, repository, contentService, this);

                default:
                    log.Debug("Package skipped.");
                    break;
            }

            return null;
        }
    }
}
