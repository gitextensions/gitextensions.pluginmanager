using Neptuo;
using PackageManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PackageManager.Views
{
    public partial class PackageSourceWindow : Window
    {
        public PackageSourceViewModel ViewModel => (PackageSourceViewModel)DataContext;

        internal PackageSourceWindow(PackageSourceViewModel viewModel)
        {
            Ensure.NotNull(viewModel, "viewModel");
            DataContext = viewModel;

            InitializeComponent();
        }

        private void lvwSources_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            ViewModel.Edit.RaiseCanExecuteChanged();
            ViewModel.Remove.RaiseCanExecuteChanged();
            ViewModel.MoveUp.RaiseCanExecuteChanged();
            ViewModel.MoveDown.RaiseCanExecuteChanged();
        }
    }
}
