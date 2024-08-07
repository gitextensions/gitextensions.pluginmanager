﻿using Neptuo;

namespace PackageManager.Services
{
    partial class SelfUpdateService
    {
        public interface IArgs : ICloneable<IArgs>
        {
            string Path { get; }

            bool IsSelfUpdate { get; set; }
            string SelfOriginalPath { get; set; }
            string SelfUpdateVersion { get; set; }
        }
    }
}
