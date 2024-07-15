using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace PackageManager.Services
{
    public class OkNuGetPackageFilter : INuGetPackageFilter
    {
        public Task<NuGetPackageFilterResult> FilterAsync(SourceRepository repository, IPackageSearchMetadata package, CancellationToken cancellationToken)
            => Task.FromResult(NuGetPackageFilterResult.Ok);

        public readonly static OkNuGetPackageFilter Instance = new OkNuGetPackageFilter();
    }
}
