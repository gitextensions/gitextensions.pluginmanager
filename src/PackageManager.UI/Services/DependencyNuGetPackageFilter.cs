using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Neptuo;
using Neptuo.Logging;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using PackageManager.Logging;

namespace PackageManager.Services
{
    public class DependencyNuGetPackageFilter : INuGetPackageFilter
    {
        private readonly ILog log;
        private readonly NuGetLogger nuGetLogger;
        private readonly IReadOnlyCollection<Args.Dependency> dependencies;
        private readonly IReadOnlyCollection<NuGetFramework> frameworks;

        public DependencyNuGetPackageFilter(ILog log, IReadOnlyCollection<Args.Dependency> dependencies, IReadOnlyCollection<NuGetFramework> frameworks)
        {
            Ensure.NotNull(log, "log");
            Ensure.NotNull(dependencies, "dependencies");
            Ensure.NotNull(frameworks, "frameworks");
            this.log = log;
            this.dependencies = dependencies;
            this.frameworks = frameworks;

            nuGetLogger = new NuGetLogger(log);
        }

        public async Task<NuGetPackageFilterResult> IsPassedAsync(SourceRepository repository, IPackageSearchMetadata package, CancellationToken cancellationToken)
        {
            if (!dependencies.Any())
                return NuGetPackageFilterResult.Ok;

            DependencyInfoResource resource = await repository.GetResourceAsync<DependencyInfoResource>();
            if (resource == null)
                return NuGetPackageFilterResult.NotCompatible;

            NuGetPackageFilterResult result = NuGetPackageFilterResult.Ok;
            foreach (NuGetFramework framework in frameworks)
            {
                SourcePackageDependencyInfo dependencyInfo = await resource.ResolvePackage(package.Identity, framework, new SourceCacheContext(), nuGetLogger, cancellationToken);
                if (dependencyInfo != null)
                {
                    // Dependency filtering:
                    // - When incompatible dependency version is found there is a chance that previous version has the right one.
                    // - When all dependencies are missing, don't even try previous versions.
                    foreach (var dependency in dependencies)
                    {
                        PackageDependency packageDependency = dependencyInfo.Dependencies.FirstOrDefault(p => p.Id == dependency.Id);
                        if (packageDependency == null)
                        {
                            log.Info($"Package '{package.Identity}' skipped: missing dependency '{dependency.Id}'.");
                            result = NuGetPackageFilterResult.NotCompatible;
                        }

                        if (dependency.Version != null && !packageDependency.VersionRange.Satisfies(new NuGetVersion(dependency.Version)))
                        {
                            log.Info($"Package '{package.Identity}' skipped: not compatible version '{dependency.Version}' on dependency '{dependency.Id}'.");
                            return NuGetPackageFilterResult.NotCompatibleVersion;
                        }
                    }

                    return result;
                }

            }

            return NuGetPackageFilterResult.NotCompatible;
        }
    }
}
