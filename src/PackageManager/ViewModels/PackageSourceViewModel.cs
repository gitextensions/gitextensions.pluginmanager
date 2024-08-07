﻿using Neptuo;
using Neptuo.Observables;
using Neptuo.Observables.Collections;
using Neptuo.Observables.Commands;
using PackageManager.Models;
using PackageManager.ViewModels.Commands;
using MoveCommand = PackageManager.ViewModels.Commands.MoveCommand<PackageManager.Models.IPackageSource>;

namespace PackageManager.ViewModels
{
    public class PackageSourceViewModel : ObservableModel
    {
        private readonly IPackageSourceCollection service;

        public ObservableCollection<IPackageSource> Sources { get; }
        public Command Add { get; }
        public DelegateCommand<IPackageSource> Edit { get; }
        public RemoveSourceCommand Remove { get; }

        public MoveCommand MoveUp { get; }
        public MoveCommand MoveDown { get; }

        public SaveSourceCommand Save { get; }
        public Command Cancel { get; }

        private bool isEditActive;
        public bool IsEditActive
        {
            get { return isEditActive; }
            set
            {
                if (isEditActive != value)
                {
                    isEditActive = value;
                    RaisePropertyChanged();
                }
            }
        }

        public PackageSourceViewModel(IPackageSourceCollection service)
        {
            Ensure.NotNull(service, "service");
            this.service = service;

            Sources = new ObservableCollection<IPackageSource>(service.All);

            Add = new DelegateCommand(OnAdd);
            Edit = new DelegateCommand<IPackageSource>(OnEdit, CanEdit);
            Remove = new RemoveSourceCommand(Sources, service);

            MoveUp = new MoveCommand(Sources, source => service.MoveUp(source), source => Sources.IndexOf(source) > 0);
            MoveDown = new MoveCommand(Sources, source => service.MoveDown(source), source => Sources.IndexOf(source) < Sources.Count - 1);

            Save = new SaveSourceCommand(Sources, service);
            Save.Executed += () => IsEditActive = false;
            Cancel = new DelegateCommand(() => IsEditActive = false);
        }

        private void OnAdd()
        {
            Save.New();
            IsEditActive = true;
        }

        private bool CanEdit(IPackageSource source)
            => source != null;

        private void OnEdit(IPackageSource source)
        {
            if (CanEdit(source))
            {
                Save.Edit(source);
                IsEditActive = true;
            }
        }
    }
}
