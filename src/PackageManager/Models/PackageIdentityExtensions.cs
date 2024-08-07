﻿using Neptuo;

namespace PackageManager.Models
{
    public static class PackageIdentityExtensions
    {
        /// <summary>
        /// Gets package identity in form "{id}-v{version}" for <paramref name="package"/>.
        /// </summary>
        /// <param name="package">A package to identity for.</param>
        /// <returns>A package identity string.</returns>
        public static string ToIdentityString(this IPackageIdentity package)
        {
            Ensure.NotNull(package, "package");
            return $"{package.Id}-v{package.Version}";
        }
    }
}
