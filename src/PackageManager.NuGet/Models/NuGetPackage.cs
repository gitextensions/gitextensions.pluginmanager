using Neptuo;
using NuGet.Protocol.Core.Types;
using PackageManager.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PackageManager.Models
{
    public class NuGetPackage : NuGetPackageIdentity, IPackage
    {
        private readonly IPackageSearchMetadata source;
        private readonly SourceRepository repository;
        private readonly NuGetPackageContentService contentService;
        private readonly NuGetPackageVersionService versionService;

        public string Description => source.Description;

        public string Authors => source.Authors;
        public DateTime? Published => source.Published?.DateTime;
        public string Tags => source.Tags;

        public Uri IconUrl => source.IconUrl;
        public Uri ProjectUrl => source.ProjectUrl;
        public Uri LicenseUrl => source.LicenseUrl;

        public NuGetPackage(IPackageSearchMetadata source, SourceRepository repository, NuGetPackageContentService contentService, NuGetPackageVersionService versionService)
            : base(source.Identity)
        {
            Ensure.NotNull(source, "source");
            Ensure.NotNull(repository, "repository");
            Ensure.NotNull(contentService, "contentService");
            Ensure.NotNull(versionService, "versionService");
            this.source = source;
            this.repository = repository;
            this.contentService = contentService;
            this.versionService = versionService;
        }

        public async Task<IPackageContent> GetContentAsync(CancellationToken cancellationToken)
            => await contentService.DownloadAsync(repository, source, cancellationToken);

        public async Task<IEnumerable<IPackage>> GetVersionsAsync(bool isPrereleaseIncluded, CancellationToken cancellationToken)
            => await versionService.GetListAsync(int.MaxValue, source, repository, isPrereleaseIncluded: isPrereleaseIncluded, cancellationToken: cancellationToken);

        public bool Equals(IPackage? other)
            => Equals(other as IPackageIdentity);

        public override bool Equals(object? obj)
        {
            if (obj is IPackage other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17 * 2;
            hash += 5 * Id.GetHashCode();
            hash += 5 * Version.GetHashCode();
            return hash;
        }
    }
}
