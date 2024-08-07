﻿using Neptuo;
using Neptuo.Observables.Commands;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PackageManager.ViewModels.Commands
{
    public partial class UpdateAllCommand : AsyncCommand
    {
        private readonly IViewModel viewModel;

        public UpdateAllCommand(IViewModel viewModel)
        {
            Ensure.NotNull(viewModel, "viewModel");
            this.viewModel = viewModel;

            viewModel.Packages.CollectionChanged += OnPackagesChanged;
        }

        private void OnPackagesChanged(object sender, NotifyCollectionChangedEventArgs e)
            => RaiseCanExecuteChanged();

        protected override bool CanExecuteOverride()
            => viewModel.Packages.Count > 0;

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            PackageUpdateViewModel selfUpdate = null;

            foreach (PackageUpdateViewModel package in viewModel.Packages.ToList())
            {
                if (package.IsSelf)
                    selfUpdate = package;
                else
                    await UpdateAsync(package);
            }

            if (selfUpdate != null)
                await UpdateAsync(selfUpdate);
        }

        private async Task UpdateAsync(PackageUpdateViewModel package)
        {
            if (viewModel.Update.CanExecute(package))
                await viewModel.Update.ExecuteAsync(package);
        }
    }
}
