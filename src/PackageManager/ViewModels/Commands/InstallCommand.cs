﻿using Neptuo;
using Neptuo.Observables.Commands;
using PackageManager.Models;
using PackageManager.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PackageManager.ViewModels.Commands
{
    public class InstallCommand : AsyncCommand<IPackage>
    {
        private readonly IInstallService service;

        public event Func<Task<bool>> Executing;
        public event Action Completed;

        public InstallCommand(IInstallService service)
        {
            Ensure.NotNull(service, "service");
            this.service = service;
        }

        protected override bool CanExecuteOverride(IPackage package)
            => package != null && !service.IsInstalled(package.Id);

        protected override async Task ExecuteAsync(IPackage package, CancellationToken cancellationToken)
        {
            bool execute = true;

            if (Executing != null)
                execute = await Executing();

            if (execute)
            {
                IPackageContent packageContent = await package.GetContentAsync(cancellationToken);
                string pluginPath = Path.Combine(service.Path, package.Id);
                Directory.CreateDirectory(pluginPath);
                await packageContent.ExtractToAsync(pluginPath, cancellationToken);

                service.Install(package);
            }

            Completed?.Invoke();
        }

        public new void RaiseCanExecuteChanged()
            => base.RaiseCanExecuteChanged();
    }
}
