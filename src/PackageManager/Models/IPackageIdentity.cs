using System;

namespace PackageManager.Models
{
    /// <summary>
    /// A package identification (unique name and version).
    /// </summary>
    public interface IPackageIdentity : IEquatable<IPackageIdentity>
    {
        /// <summary>
        /// Gets an unique package identifier.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets a package version.
        /// </summary>
        string Version { get; }
    }
}
