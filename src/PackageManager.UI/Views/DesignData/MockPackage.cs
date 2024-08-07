﻿using PackageManager.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PackageManager.Views.DesignData
{
    internal class MockPackage : IPackage
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }

        public string Authors { get; set; }
        public DateTime? Published { get; set; }
        public string Tags { get; set; }

        public Uri IconUrl { get; set; }
        public Uri ProjectUrl { get; set; }
        public Uri LicenseUrl { get; set; }

        public bool Equals(IPackageIdentity other)
        {
            if (other == null)
                return false;

            return string.Equals(Id, other.Id, StringComparison.CurrentCultureIgnoreCase) && string.Equals(Version, other.Version, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool Equals(IPackage other)
            => Equals((IPackageIdentity)other);

        public Task<IPackageContent> GetContentAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IPackage>> GetVersionsAsync(bool isPrereleaseIncluded, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
