using PackageManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PackageManager.Views
{
    public partial class Updates : UserControl, IAutoFocus
    {
        public UpdatesViewModel ViewModel
        {
            get { return (UpdatesViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof(UpdatesViewModel),
            typeof(Updates),
            new PropertyMetadata(null, OnViewModelChanged)
        );

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Updates view = (Updates)d;
            view.OnViewModelChanged((UpdatesViewModel)e.OldValue, (UpdatesViewModel)e.NewValue);
        }


        internal PackageUpdateViewModel SelectedPackage
        {
            get { return (PackageUpdateViewModel)GetValue(SelectedPackageProperty); }
            set { SetValue(SelectedPackageProperty, value); }
        }

        internal static readonly DependencyProperty SelectedPackageProperty = DependencyProperty.Register(
            "SelectedPackage", 
            typeof(PackageUpdateViewModel), 
            typeof(Updates), 
            new PropertyMetadata(null)
        );


        public Updates()
        {
            InitializeComponent();
        }

        private void OnViewModelChanged(UpdatesViewModel oldValue, UpdatesViewModel newValue)
        {
            if (oldValue != null)
                oldValue.Update.Completed += OnRefresh;

            MainPanel.DataContext = newValue;

            if (newValue != null)
                newValue.Update.Completed += OnRefresh;
        }

        private void OnRefresh()
            => ViewModel.Refresh.Execute();

        async void IAutoFocus.Focus()
        { 
            lvwPackages.Focus();
            await ViewModel.Refresh.ExecuteAsync();
        }

        private void lvwPackages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Update.RaiseCanExecuteChanged();
        }
    }
}
