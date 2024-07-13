using PackageManager.Models;

namespace PackageManager.Services
{
    public interface ISelfUpdateService
    {
        bool IsSelfUpdate { get; }

        void Update(IPackage latest);

        void RunNewInstance(IPackage package);
    }
}
