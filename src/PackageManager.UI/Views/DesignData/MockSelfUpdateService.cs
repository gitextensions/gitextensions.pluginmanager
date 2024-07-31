using PackageManager.Models;
using PackageManager.Services;

namespace PackageManager.Views.DesignData
{
    public class MockSelfUpdateService : ISelfUpdateService
    {
        public bool IsSelfUpdate { get; set; }

        public void RunNewInstance(IPackage package)
        { }

        public void Update(IPackage latest)
        { }
    }
}
