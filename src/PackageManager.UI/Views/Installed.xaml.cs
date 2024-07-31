using PackageManager.Models;
using PackageManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PackageManager.Views
{
    public partial class Installed : UserControl, IAutoFocus
    {
        public InstalledViewModel ViewModel
        {
            get { return (InstalledViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof(InstalledViewModel),
            typeof(Installed),
            new PropertyMetadata(null, OnViewModelChanged)
        );

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Installed view = (Installed)d;
            view.OnViewModelChanged((InstalledViewModel)e.OldValue, (InstalledViewModel)e.NewValue);
        }


        internal IInstalledPackage SelectedPackage
        {
            get { return (IInstalledPackage)GetValue(SelectedPackageProperty); }
            set { SetValue(SelectedPackageProperty, value); }
        }

        internal static readonly DependencyProperty SelectedPackageProperty = DependencyProperty.Register(
            "SelectedPackage",
            typeof(IInstalledPackage),
            typeof(Installed),
            new PropertyMetadata(null)
        );

        public Installed()
        {
            InitializeComponent();
        }

        private void OnViewModelChanged(InstalledViewModel oldValue, InstalledViewModel newValue)
        {
            if (oldValue != null)
                oldValue.Uninstall.Completed += OnRefresh;

            MainPanel.DataContext = newValue;

            if (newValue != null)
                newValue.Uninstall.Completed += OnRefresh;
        }

        private void OnRefresh()
            => ViewModel.Refresh.Execute();

        void IAutoFocus.Focus()
        {
            ViewModel.Refresh.Execute();
            lvwPackages.Focus();
        }

        private void lvwPackages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Uninstall.RaiseCanExecuteChanged();
            ViewModel.Reinstall.RaiseCanExecuteChanged();
        }
    }
}
