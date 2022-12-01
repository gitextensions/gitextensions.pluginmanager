using NuGet.Packaging;

namespace PackageManager.Models
{
    partial class NuGetPackageContent
    {
        public interface IFrameworkFilter
        {
            bool IsPassed(FrameworkSpecificGroup group);
        }
    }
}
