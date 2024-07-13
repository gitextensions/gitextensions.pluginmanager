using PackageManager.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PackageManager.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<IPackage>> SearchAsync(IEnumerable<IPackageSource> packageSources, string searchText, SearchOptions options = default, CancellationToken cancellationToken = default);

        Task<IPackage> FindLatestVersionAsync(IEnumerable<IPackageSource> packageSources, IPackage package, bool isPrereleaseIncluded = false, CancellationToken cancellationToken = default);
    }
}
