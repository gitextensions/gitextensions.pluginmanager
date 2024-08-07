﻿using Neptuo;
using PackageManager.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageManager
{
    public partial class Args : SelfUpdateService.IArgs, ICloneable<Args>, ICloneable<SelfUpdateService.IArgs>
    {
        public string Path { get; set; }
        public string SelfPackageId { get; set; }

        public bool IsUpdateCount { get; set; }

        public bool IsUpdatePackage { get; set; }
        public string PackageId { get; set; }

        public bool IsSelfUpdate { get; set; }
        public string SelfOriginalPath { get; set; }
        public string SelfUpdateVersion { get; set; }

        private Args()
        {
        }

        public Args(string[] args)
        {
            ParseParameters(args);
        }

        #region Parse

        private bool ParseParameters(string[] args)
        {
            List<string> items = args.ToList();
            foreach (string arg in items.ToList())
            {
                if (arg == "--selfupdate")
                {
                    IsSelfUpdate = true;
                    items.Remove(arg);
                }
            }

            args = items.ToArray();

            int skipped = 0;
            if (args[0] == "update")
            {
                skipped++;
                if (args[1] == "--count")
                {
                    IsUpdateCount = true;
                    skipped++;
                }
                else if (args[1] == "--package")
                {
                    IsUpdatePackage = true;
                    PackageId = args[2];
                    skipped += 2;
                }
            }

            if ((args.Length - skipped) % 2 == 0)
            {
                for (int i = skipped; i < args.Length; i += 2)
                {
                    string name = args[i];
                    string value = args[i + 1];
                    ParseParameter(name, value);
                }
            }

            return true;
        }

        private bool ParseParameter(string name, string value)
        {
            switch (name)
            {
                case "--path":
                    Path = value;
                    return true;
                case "--selfpackageid":
                    SelfPackageId = value;
                    return true;
                case "--selforiginalpath":
                    SelfOriginalPath = value;
                    return true;
                case "--selfupdateversion":
                    SelfUpdateVersion = value;
                    return true;
                default:
                    return false;
            }
        }

        #endregion

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(Path))
                result.Append($"--path \"{Path}\"");

            if (!string.IsNullOrEmpty(SelfPackageId))
                result.Append($" --selfpackageid {SelfPackageId}");

            if (IsSelfUpdate)
                result.Append(" --selfupdate");

            if (!string.IsNullOrEmpty(SelfOriginalPath))
                result.Append($" --selforiginalpath \"{SelfOriginalPath}\"");

            return result.ToString();
        }

        public Args Clone()
        {
            return new Args()
            {
                Path = Path,
                SelfPackageId = SelfPackageId,
                IsUpdateCount = IsUpdateCount,
                IsUpdatePackage = IsUpdatePackage,
                PackageId = PackageId,
                IsSelfUpdate = IsSelfUpdate,
                SelfOriginalPath = SelfOriginalPath,
                SelfUpdateVersion = SelfUpdateVersion
            };
        }

        SelfUpdateService.IArgs ICloneable<SelfUpdateService.IArgs>.Clone() 
            => Clone();
    }
}
