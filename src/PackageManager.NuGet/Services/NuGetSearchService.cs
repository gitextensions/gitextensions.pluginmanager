using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Neptuo;
using Neptuo.Activators;
using Neptuo.Logging;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using PackageManager.Logging;
using PackageManager.Models;

namespace PackageManager.Services
{
    public partial class NuGetSearchService : ISearchService
    {
        public const int PageCountToProbe = 10;

        private readonly IFactory<SourceRepository, IPackageSource> repositoryFactory;
        private readonly ILog log;
        private readonly NuGetPackageContentService contentService;
        private readonly ILogger nuGetLog;
        private readonly NuGetPackageVersionService versionService;
        private readonly INuGetPackageFilter filter;
        private readonly INuGetSearchTermTransformer queryTransformer;

        public NuGetSearchService(IFactory<SourceRepository, IPackageSource> repositoryFactory, ILog log, NuGetPackageContentService contentService, NuGetPackageVersionService versionService, INuGetPackageFilter filter = null, INuGetSearchTermTransformer termTransformer = null)
        {
            Ensure.NotNull(repositoryFactory, "repositoryFactory");
            Ensure.NotNull(log, "log");
            Ensure.NotNull(contentService, "contentService");
            Ensure.NotNull(versionService, "versionService");

            if (filter == null)
                filter = OkNuGetPackageFilter.Instance;

            this.repositoryFactory = repositoryFactory;
            this.log = log;
            this.contentService = contentService;
            this.nuGetLog = new NuGetLogger(log);
            this.versionService = versionService;
            this.filter = filter ?? OkNuGetPackageFilter.Instance;
            this.queryTransformer = termTransformer ?? EmptyNuGetSearchTermTransformer.Instance;
        }

        private SearchOptions EnsureOptions(SearchOptions options)
        {
            if (options == null)
            {
                options = new SearchOptions()
                {
                    PageIndex = 0,
                    PageSize = 10
                };
            }

            return options;
        }

        public async Task<IEnumerable<IPackage>> SearchAsync(IEnumerable<IPackageSource> packageSources, string searchText, SearchOptions options = default, CancellationToken cancellationToken = default)
        {
            var (feedTerm, localTerm) = PrepareSearchTerms(searchText);

            log.Debug($"Searching - user text:'{searchText}'; feed query:'{feedTerm}'.");

            options = EnsureOptions(options);

            List<IPackage> result = new List<IPackage>();
            List<IPackageSource> sources = new List<IPackageSource>(packageSources);
            List<IPackageSource> sourcesToSkip = new List<IPackageSource>();

            // Try to find N results passing filter (until zero results is returned).
            while (result.Count < options.PageSize && options.PageIndex < PageCountToProbe)
            {
                log.Debug($"Loading page '{options.PageIndex}'.");

                foreach (IPackageSource packageSource in sources)
                {
                    log.Debug($"Searching in '{packageSource.Uri}'.");

                    SourceRepository repository = repositoryFactory.Create(packageSource);
                    PackageSearchResource search = await repository.GetResourceAsync<PackageSearchResource>(cancellationToken);
                    if (search == null)
                    {
                        log.Debug($"Source skipped, because it doesn't provide '{nameof(PackageSearchResource)}'.");

                        sourcesToSkip.Add(packageSource);
                        continue;
                    }

                    if (!await ApplyLocalResourceSearchAsync(result, repository, search, feedTerm, localTerm, options, cancellationToken))
                        sourcesToSkip.Add(packageSource);
                }

                options = new SearchOptions()
                {
                    PageIndex = options.PageIndex + 1,
                    PageSize = options.PageSize
                };

                foreach (IPackageSource source in sourcesToSkip)
                    sources.Remove(source);

                if (sources.Count == 0)
                    break;
            }

            log.Debug($"Search completed. Found '{result.Count}' items.");
            return result;
        }

        /// <summary>
        /// Prepares instance of terms for filtering in feed and in-app.
        /// </summary>
        /// <remarks>
        /// localTerm should always have all search terms.
        /// </remarks>
        private (NuGetSearchTerm feedTerm, NuGetSearchTerm localTerm) PrepareSearchTerms(string searchText)
        {
            NuGetSearchTerm feedTerm = new NuGetSearchTerm();
            feedTerm.Id.Add(searchText);

            queryTransformer.Transform(feedTerm);
            feedTerm.Id.Remove(searchText);

            NuGetSearchTerm localTerm = null;
            if (feedTerm.IsEmpty())
            {
                feedTerm.Id.Add(searchText);
            }
            else
            {
                localTerm = feedTerm.Clone();
                localTerm.Id.Add(searchText);
            }

            return (feedTerm, localTerm);
        }

        /// <summary>
        /// Tries to apply special conditions for looking in local folder feed.
        /// </summary>
        /// <returns><c>true</c> if search reached the end of the feed; <c>false</c> otherwise.</returns>
        private Task<bool> ApplyLocalResourceSearchAsync(List<IPackage> result, SourceRepository repository, PackageSearchResource search, NuGetSearchTerm feedTerm, NuGetSearchTerm localTerm, SearchOptions options, CancellationToken cancellationToken)
        {
            if (search is LocalPackageSearchResource)
            {
                // Searching a feed from folder. Look for all packages and then filter in-app.
                if (localTerm == null)
                    localTerm = feedTerm;

                feedTerm = new NuGetSearchTerm();
            }

            return ApplySearchAsync(result, repository, search, feedTerm, localTerm, options, cancellationToken);
        }

        /// <summary>
        /// Execute search on <paramref name="search"/>.
        /// </summary>
        /// <returns><c>true</c> if search reached the end of the feed; <c>false</c> otherwise.</returns>
        private async Task<bool> ApplySearchAsync(List<IPackage> result, SourceRepository repository, PackageSearchResource search, NuGetSearchTerm feedTerm, NuGetSearchTerm localTerm, SearchOptions options, CancellationToken cancellationToken)
        {
            if (localTerm != null && options.PageSize == 1)
                options.PageSize = 10;

            int sourceSearchPackageCount = 0;
            foreach (IPackageSearchMetadata package in await SearchAsync(search, feedTerm.ToString(), options, cancellationToken))
            {
                sourceSearchPackageCount++;

                log.Debug($"Found '{package.Identity}'.");

                if (result.Count >= options.PageSize)
                    break;

                if (localTerm != null && !localTerm.IsMatched(package))
                {
                    log.Debug($"Package skipped by late search term '{localTerm}'.");
                    continue;
                }

                await AddPackageAsync(result, repository, package, options.IsPrereleaseIncluded, cancellationToken);
            }

            // If package source reached end, skip it from next probing.
            if (sourceSearchPackageCount < options.PageSize)
                return false;

            return true;
        }

        private Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(PackageSearchResource search, string searchText, SearchOptions options, CancellationToken cancellationToken)
            => search.SearchAsync(searchText, new SearchFilter(options.IsPrereleaseIncluded), options.PageIndex * options.PageSize, options.PageSize, nuGetLog, cancellationToken);

        private async Task AddPackageAsync(List<IPackage> result, SourceRepository repository, IPackageSearchMetadata package, bool isPrereleaseIncluded, CancellationToken cancellationToken)
        {
            NuGetPackageFilterResult filterResult = await filter.FilterAsync(repository, package, cancellationToken);
            switch (filterResult)
            {
                case NuGetPackageFilterResult.Ok:
                    log.Debug("Package added.");
                    result.Add(new NuGetPackage(package, repository, contentService, versionService));
                    break;

                case NuGetPackageFilterResult.NotCompatibleVersion:
                    log.Debug("Loading order versions.");
                    result.AddRange(
                        await versionService.GetListAsync(
                            1,
                            package,
                            repository,
                            (source, target) => source.Identity.Version != target.Identity.Version,
                            isPrereleaseIncluded,
                            cancellationToken
                        )
                    );
                    break;

                default:
                    log.Debug("Package skipped.");
                    break;
            }
        }

        public async Task<IPackage> FindLatestVersionAsync(IEnumerable<IPackageSource> packageSources, IPackage package, bool isPrereleaseIncluded, CancellationToken cancellationToken = default)
        {
            Ensure.NotNull(package, "package");

            log.Debug($"Finding latest version of '{package.Id}'.");

            IEnumerable<IPackage> packages = await SearchAsync(packageSources, package.Id, new SearchOptions() { PageSize = 1, IsPrereleaseIncluded = isPrereleaseIncluded }, cancellationToken);
            IPackage latest = packages.FirstOrDefault();
            if (latest != null && string.Equals(latest.Id, package.Id, StringComparison.InvariantCultureIgnoreCase))
            {
                log.Debug($"Found version '{latest.Version}'.");
                return latest;
            }

            return null;
        }
    }
}
