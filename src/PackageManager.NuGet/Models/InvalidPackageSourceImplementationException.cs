using System;

namespace PackageManager.Models
{
    /// <summary>
    /// Raised when passed implementation of <see cref="IPackageSource"/> to not compatible with <see cref="NuGetPackageSourceCollection"/>,
    /// </summary>
    public class InvalidPackageSourceImplementationException : Exception
    {
        /// <summary>
        /// Creates a new empty instance.
        /// </summary>
        public InvalidPackageSourceImplementationException()
        { }
    }
}
