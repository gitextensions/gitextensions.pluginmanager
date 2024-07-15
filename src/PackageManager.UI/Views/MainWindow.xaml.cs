using Neptuo;
using PackageManager.Services;
using PackageManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PackageManager.Views
{
    public partial class MainWindow : Window
    {
        private readonly ProcessService processes;
        private readonly Navigator navigator;

        public MainViewModel ViewModel
            => (MainViewModel)DataContext;

        internal MainWindow(MainViewModel viewModel, ProcessService processes, Navigator navigator)
        {
            Ensure.NotNull(viewModel, "viewModel");
            Ensure.NotNull(processes, "processes");
            Ensure.NotNull(navigator, "navigator");
            DataContext = viewModel;
            this.processes = processes;
            this.navigator = navigator;
            InitializeViewModel();

            InitializeComponent();
        }

        private void InitializeViewModel()
        {
            ViewModel.Browser.Install.Executing += OnBeforeChange;
            ViewModel.Installed.Uninstall.Executing += OnBeforeChange;
            ViewModel.Updates.Update.Executing += OnBeforeChange;
        }

        private Task<bool> OnBeforeChange()
        {
            var context = processes.PrepareContextForProcessesKillBeforeChange();
            if (context.IsExecutable)
            {
                var processNames = string.Join(" and ", context.ProcessNames);
                if (context.ProcessCount > 1 || context.ProcessNames.Count > 1)
                    processNames = $"all instances of {processNames}";

                bool result = navigator.Confirm(
                    "Plugin Manager",
                    $"Plugin Manager will be writing to files that are currently in use.\r\n\r\nDo you want to stop {processNames}?"
                );

                if (result)
                    context.Execute();
            }

            return Task.FromResult(true);
        }

        public void SelectUpdatesTab()
            => Tabs.SelectedIndex = 2;

        private void PackageSourceSettings_Click(object sender, RoutedEventArgs e) 
            => navigator.OpenPackageSources();

        private void ShowLog_Click(object sender, RoutedEventArgs e)
            => navigator.OpenLog();
    }
}
