using Moq;

namespace PackageManager.ViewModels.Commands
{
    public class PackageOptions
    {
        public IPackageOptions Object { get; }

        public PackageOptions()
        {
            Mock<IPackageOptions> mock = new Mock<IPackageOptions>();
            mock
                .Setup(p => p.IsPrereleaseIncluded)
                .Returns(() => false);

            Object = mock.Object;
        }
    }
}
