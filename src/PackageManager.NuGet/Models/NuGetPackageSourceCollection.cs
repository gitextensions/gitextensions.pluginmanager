﻿using Neptuo;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using INuGetPackageSourceProvider = NuGet.Configuration.IPackageSourceProvider;

namespace PackageManager.Models
{
    public class NuGetPackageSourceCollection : DisposableBase, IPackageSourceCollection
    {
        internal INuGetPackageSourceProvider Provider { get; }
        internal List<NuGetPackageSource> Sources { get; }

        public event Action Changed;

        public IPackageSource Primary => Sources.FirstOrDefault(s => string.Equals(s.Name, Provider.ActivePackageSourceName, StringComparison.CurrentCultureIgnoreCase));
        public IReadOnlyCollection<IPackageSource> All => Sources;

        public NuGetPackageSourceCollection(INuGetPackageSourceProvider provider)
        {
            Ensure.NotNull(provider, "provider");
            this.Provider = provider;

            provider.PackageSourcesChanged += OnProviderChanged;
            Sources = provider.LoadPackageSources().Select(s => new NuGetPackageSource(s)).ToList();
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            Provider.PackageSourcesChanged -= OnProviderChanged;
        }

        private void OnProviderChanged(object sender, EventArgs e)
            => Changed?.Invoke();

        private NuGetPackageSource EnsureType(IPackageSource source, string argumentName = null)
        {
            Ensure.NotNull(source, argumentName ?? "source");
            if (source is NuGetPackageSource target)
                return target;

            throw new InvalidPackageSourceImplementationException();
        }

        private PackageSource UnWrap(IPackageSource source, string argumentName = null)
            => EnsureType(source, argumentName).Original;

        public IPackageSourceBuilder Add()
            => new NuGetPackageSourceBuilder(this);

        public IPackageSourceBuilder Edit(IPackageSource source)
            => new NuGetPackageSourceBuilder(this, EnsureType(source));

        public void Remove(IPackageSource source)
        {
            NuGetPackageSource target = EnsureType(source);
            if (Sources.Remove(target))
                SavePackageSources();
        }

        public void MarkAsPrimary(IPackageSource source)
        {
            if (string.Equals(Provider.ActivePackageSourceName, source?.Name, StringComparison.CurrentCultureIgnoreCase))
                return;

            if (source == null)
                Provider.SaveActivePackageSource(null);
            else
                Provider.SaveActivePackageSource(UnWrap(source));
        }

        internal void SavePackageSources(bool isOrderChanged = false)
        {
            // This is a workaround for change/bug in the underlaying package source provider,
            // which ignores changed order. So we save an empty list and than the actual.
            if (isOrderChanged)
                Provider.SavePackageSources(Enumerable.Empty<PackageSource>());

            Provider.SavePackageSources(Sources.Select(s => s.Original));
        }

        public int MoveUp(IPackageSource source)
        {
            NuGetPackageSource target = EnsureType(source);
            int index = Sources.IndexOf(target);
            if (index > 0)
            {
                Sources.RemoveAt(index);
                Sources.Insert(--index, target);
                SavePackageSources(true);
            }

            return index;
        }

        public int MoveDown(IPackageSource source)
        {
            NuGetPackageSource target = EnsureType(source);
            int index = Sources.IndexOf(target);
            if (index < Sources.Count - 1)
            {
                Sources.RemoveAt(index);
                Sources.Insert(++index, target);
                SavePackageSources(true);
            }

            return index;
        }
    }
}
