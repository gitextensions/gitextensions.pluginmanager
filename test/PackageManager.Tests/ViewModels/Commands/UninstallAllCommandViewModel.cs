using Moq;
using Neptuo.Observables.Collections;
using PackageManager.Models;
using System.Collections.Generic;

namespace PackageManager.ViewModels.Commands
{
    public class UninstallAllCommandViewModel
    {
        public CallCounter CommandCalled { get; } = new CallCounter();
        public CallCounter PackagesCalled { get; } = new CallCounter();

        public UninstallAllCommand.IViewModel Object { get; }

        public UninstallAllCommandViewModel(UninstallCommand command, ICollection<IInstalledPackage> packages)
        {
            Mock<UninstallAllCommand.IViewModel> mock = new Mock<UninstallAllCommand.IViewModel>();
            mock
                .SetupGet(vm => vm.Uninstall)
                .Callback(() => CommandCalled.Increment())
                .Returns(command);

            mock
                .SetupGet(vm => vm.Packages)
                .Callback(() => PackagesCalled.Increment())
                .Returns(new ObservableCollection<IInstalledPackage>(packages));

            Object = mock.Object;
        }
    }
}
