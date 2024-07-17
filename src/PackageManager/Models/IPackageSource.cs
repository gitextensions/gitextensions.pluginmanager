using System;

namespace PackageManager.Models
{
    /// <summary>
    /// A package source endpoint.
    /// </summary>
    public interface IPackageSource
    {
        /// <summary>
        /// Gets a user defined name of package source.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a URI of package source.
        /// </summary>
        Uri Uri { get; }
    }
}
