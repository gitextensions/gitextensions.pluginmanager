using Neptuo.Observables.Collections;
using PackageManager.Models;

namespace PackageManager.ViewModels.Commands
{
    partial class UninstallAllCommand
    {
        public interface IViewModel
        {
            ObservableCollection<IInstalledPackage> Packages { get; }
            UninstallCommand Uninstall { get; }
        }
    }
}
