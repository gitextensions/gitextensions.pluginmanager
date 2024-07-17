using PackageManager.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PackageManager.Views
{
    public partial class Browser : UserControl, IAutoFocus
    {
        public BrowserViewModel ViewModel
        {
            get { return (BrowserViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof(BrowserViewModel),
            typeof(Browser),
            new PropertyMetadata(null, OnViewModelChanged)
        );

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Browser view = (Browser)d;
            view.OnViewModelChanged((BrowserViewModel)e.OldValue, (BrowserViewModel)e.NewValue);
        }


        internal PackageViewModel SelectedPackage
        {
            get { return (PackageViewModel)GetValue(SelectedPackageProperty); }
            set { SetValue(SelectedPackageProperty, value); }
        }

        internal static readonly DependencyProperty SelectedPackageProperty = DependencyProperty.Register(
            "SelectedPackage", 
            typeof(PackageViewModel), 
            typeof(Browser), 
            new PropertyMetadata(null)
        );


        internal PackageViewModel SelectedVersion
        {
            get { return (PackageViewModel)GetValue(SelectedVersionProperty); }
            set { SetValue(SelectedVersionProperty, value); }
        }

        internal static readonly DependencyProperty SelectedVersionProperty = DependencyProperty.Register(
            "SelectedVersion", 
            typeof(PackageViewModel), 
            typeof(Browser), 
            new PropertyMetadata(null, OnSelectedVersionChanged)
        );

        private static void OnSelectedVersionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public Browser()
        {
            InitializeComponent();
            UpdateInitialMessage(true);
        }

        private void OnViewModelChanged(BrowserViewModel oldValue, BrowserViewModel newValue)
        {
            if (oldValue != null)
                oldValue.Install.Completed -= RaiseCanExecuteChangedOnCommands;

            MainPanel.DataContext = newValue;

            if (newValue != null)
            {
                newValue.Install.Completed += RaiseCanExecuteChangedOnCommands;
                newValue.Search.Completed += OnSearchCompleted;
            }
        }

        private void tbxSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ViewModel.Search.CanExecute())
                    ViewModel.Search.Execute();
            }
        }

        private void OnSearchCompleted()
        {
            lvwPackages.Focus();
            lvwPackages.SelectedIndex = 0;
            UpdateInitialMessage(false);
        }

        private void lvwPackages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedVersion = (PackageViewModel)lvwPackages.SelectedItem;
            RaiseCanExecuteChangedOnCommands();
        }

        private void cbxVersions_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => RaiseCanExecuteChangedOnCommands();

        private void UpdateInitialMessage(bool isInitial)
        {
            if (isInitial)
            {
                stpNothing.Visibility = Visibility.Collapsed;
                stpInitial.Visibility = Visibility.Visible;
            }
            else
            {
                stpNothing.Visibility = Visibility.Visible;
                stpInitial.Visibility = Visibility.Collapsed;
            }
        }

        private void RaiseCanExecuteChangedOnCommands()
            => ViewModel.Install.RaiseCanExecuteChanged();

        void IAutoFocus.Focus()
            => tbxSearch.Focus();
    }
}
