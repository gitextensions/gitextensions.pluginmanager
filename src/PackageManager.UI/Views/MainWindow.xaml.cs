using Neptuo;
using PackageManager.Services;
using PackageManager.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace PackageManager.Views
{
    public partial class MainWindow : Window
    {
        private readonly ProcessService processes;
        private readonly Navigator navigator;
        private readonly bool closeWithConfirmation;

        public MainViewModel ViewModel
            => (MainViewModel)DataContext;

        internal MainWindow(MainViewModel viewModel, ProcessService processes, Navigator navigator, bool closeWithConfirmation)
        {
            Ensure.NotNull(viewModel, "viewModel");
            Ensure.NotNull(processes, "processes");
            Ensure.NotNull(navigator, "navigator");
            DataContext = viewModel;
            this.processes = processes;
            this.navigator = navigator;
            this.closeWithConfirmation = closeWithConfirmation;
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
                if (this.closeWithConfirmation)
                {
                    bool result = navigator.Confirm(
                        "Plugin Manager",
                        "Plugin Manager is going to write to files that are holded by other executables. " + Environment.NewLine +
                        "Do you want to kill all instances of these applications?"
                    );

                    if (result)
                    {
                        context.Execute();
                    }
                    else
                    {
                        return Task.FromResult(false);
                    }
                }
                else
                {
                    context.Execute();
                }
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
