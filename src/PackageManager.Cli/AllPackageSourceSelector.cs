using Neptuo;
using PackageManager.Models;
using PackageManager.ViewModels;
using System.Collections.Generic;

namespace PackageManager
{
    public class AllPackageSourceSelector : IPackageSourceSelector
    {
        private readonly IPackageSourceProvider service;

        public IEnumerable<IPackageSource> Sources => service.All;

        public AllPackageSourceSelector(IPackageSourceProvider service)
        {
            Ensure.NotNull(service, "service");
            this.service = service;
        }
    }
}
