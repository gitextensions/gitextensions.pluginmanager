using PackageManager.Models;
using System.Collections.Generic;

namespace PackageManager.ViewModels
{
    /// <summary>
    /// A provider of current package sources.
    /// </summary>
    public interface IPackageSourceSelector
    {
        /// <summary>
        /// Gets an enumeration of current package sources.
        /// </summary>
        IEnumerable<IPackageSource> Sources { get; }
    }
}
