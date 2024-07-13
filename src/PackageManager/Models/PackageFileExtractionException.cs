using System;

namespace PackageManager.Models
{
    /// <summary>
    /// An exception raised when there was an extracting file from package content.
    /// </summary>
    public class PackageFileExtractionException : Exception
    {
        /// <summary>
        /// Gets a real file path where extraction problem occured.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Creates a new instance where <paramref name="filePath"/> caused the problem and <paramref name="inner"/> exception.
        /// </summary>
        /// <param name="filePath">A real file path where extraction problem occured.</param>
        /// <param name="inner">An inner cause of the exceptional state.</param>
        public PackageFileExtractionException(string filePath, Exception inner)
            : base($"Error deleting file from '{filePath}'.", inner)
        {
            FilePath = filePath;
        }
    }
}
