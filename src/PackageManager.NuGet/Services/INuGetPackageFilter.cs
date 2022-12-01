using NuGet.Protocol.Core.Types;
using System.Threading;
using System.Threading.Tasks;

namespace PackageManager.Services
{
    public interface INuGetPackageFilter
    {
        Task<NuGetPackageFilterResult> FilterAsync(SourceRepository repository, IPackageSearchMetadata package, CancellationToken cancellationToken);
    }
}
