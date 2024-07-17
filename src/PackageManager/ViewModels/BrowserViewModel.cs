using Neptuo.Observables;
using Neptuo.Observables.Collections;
using PackageManager.Services;
using PackageManager.ViewModels.Commands;

namespace PackageManager.ViewModels
{
    public class BrowserViewModel : ObservableModel, IPackageOptions
    {
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool isPrereleaseIncluded;
        public bool IsPrereleaseIncluded
        {
            get { return isPrereleaseIncluded; }
            set
            {
                if (isPrereleaseIncluded != value)
                {
                    isPrereleaseIncluded = value;
                    RaisePropertyChanged();
                }
            }
        }

        public PagingViewModel Paging { get; }
        public ObservableCollection<PackageViewModel> Packages { get; }
        public SearchCommand Search { get; }
        public InstallCommand Install { get; }

        public BrowserViewModel(IPackageSourceSelector packageSource, ISearchService search, IInstallService install)
        {
            Packages = new ObservableCollection<PackageViewModel>();
            Search = new SearchCommand(this, packageSource, search);
            Paging = new PagingViewModel(Search);
            Install = new InstallCommand(install);
        }
    }
}
