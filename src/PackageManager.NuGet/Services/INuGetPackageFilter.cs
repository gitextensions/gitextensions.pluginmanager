using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PackageManager.Services
{
    public interface INuGetPackageFilter
    {
        Task<NuGetPackageFilterResult> FilterAsync(SourceRepository repository, IPackageSearchMetadata package, CancellationToken cancellationToken);
    }
}
