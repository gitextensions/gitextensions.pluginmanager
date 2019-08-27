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
        public Task<NuGetPackageFilterResult> IsPassedAsync(SourceRepository repository, IPackageSearchMetadata package, CancellationToken cancellationToken)
            => Task.FromResult(NuGetPackageFilterResult.Ok);

        #region Singleton

        private static OkNuGetPackageFilter instance;
        private static object instanceLock = new object();

        public static OkNuGetPackageFilter Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                            instance = new OkNuGetPackageFilter();
                    }
                }

                return instance;
            }
        }

        #endregion
    }
}
