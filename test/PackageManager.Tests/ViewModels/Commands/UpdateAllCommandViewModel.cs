using Moq;
using Neptuo.Observables.Collections;
using System.Collections.Generic;

namespace PackageManager.ViewModels.Commands
{
    public class UpdateAllCommandViewModel
    {
        public CallCounter CommandCalled { get; } = new CallCounter();
        public CallCounter PackagesCalled { get; } = new CallCounter();

        public UpdateAllCommand.IViewModel Object { get; }

        public UpdateAllCommandViewModel(UpdateCommand command, ICollection<PackageUpdateViewModel> packages)
        {
            Mock<UpdateAllCommand.IViewModel> mock = new Mock<UpdateAllCommand.IViewModel>();
            mock
                .SetupGet(vm => vm.Update)
                .Callback(() => CommandCalled.Increment())
                .Returns(command);

            mock
                .SetupGet(vm => vm.Packages)
                .Callback(() => PackagesCalled.Increment())
                .Returns(new ObservableCollection<PackageUpdateViewModel>(packages));

            Object = mock.Object;
        }
    }
}
