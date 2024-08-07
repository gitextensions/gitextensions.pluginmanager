﻿using Neptuo.Observables;
using PackageManager.Models;
using PackageManager.Services;
using PackageManager.ViewModels.Commands;
using System;
using System.Collections.Generic;

namespace PackageManager.ViewModels
{
    public class MainViewModel : ObservableModel, IDisposable
    {
        public PackageSourceSelectorViewModel SourceSelector { get; }

        public BrowserViewModel Browser { get; }
        public InstalledViewModel Installed { get; }
        public UpdatesViewModel Updates { get; }

        public CancelCommand Cancel { get; }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MainViewModel(IPackageSourceCollection sources, ISearchService search, IInstallService install, SelfPackageConfiguration selfPackageConfiguration, ISelfUpdateService selfUpdate, IComparer<IPackageIdentity> packageVersionComparer)
        {
            SourceSelector = new PackageSourceSelectorViewModel(sources);

            Browser = new BrowserViewModel(SourceSelector, search, install);
            Installed = new InstalledViewModel(SourceSelector, install, selfPackageConfiguration);
            Updates = new UpdatesViewModel(SourceSelector, install, search, selfPackageConfiguration, selfUpdate, packageVersionComparer);

            Cancel = new CancelCommand(
                Browser.Search, 
                Browser.Install, 
                Installed.Uninstall,
                Installed.UninstallAll,
                Installed.Reinstall,
                Installed.Refresh,
                Updates.Update,
                Updates.UpdateAll,
                Updates.Refresh
            );
            Cancel.CanExecuteChanged += OnCancelCanExecuteChanged;

            Installed.Uninstall.Completed += OnInstalledChanged;
            Updates.Update.Completed += OnInstalledChanged;
        }

        private void OnCancelCanExecuteChanged(object sender, EventArgs e)
            => IsLoading = Cancel.CanExecute();

        private void OnInstalledChanged()
            => Browser.Packages.Clear();

        public void Dispose()
        {
            SourceSelector.Dispose();
        }
    }
}
