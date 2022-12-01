using Neptuo.Observables.Collections;

namespace PackageManager.ViewModels.Commands
{
    partial class UpdateAllCommand
    {
        public interface IViewModel
        {
            ObservableCollection<PackageUpdateViewModel> Packages { get; }
            UpdateCommand Update { get; }
        }
    }
}
