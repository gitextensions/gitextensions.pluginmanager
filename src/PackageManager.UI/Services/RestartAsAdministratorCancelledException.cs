using System;

namespace PackageManager.Services
{
    /// <summary>
    /// An exception raised when attempt to restart the application as administrator was cancelled by the user.
    /// </summary>
    public class RestartAsAdministratorCancelledException : Exception
    {
        /// <summary>
        /// Creates new instance with the <paramref name="inner"/> exception.
        /// </summary>
        /// <param name="inner">The inner cause of the exceptional state.</param>
        public RestartAsAdministratorCancelledException(Exception inner)
            : base("Attempt to restart the application as administrator was cancelled by the user.", inner)
        { }
    }
}
