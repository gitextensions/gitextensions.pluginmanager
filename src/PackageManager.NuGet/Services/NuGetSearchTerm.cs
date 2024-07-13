using Neptuo;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageManager.Services
{
    public class NuGetSearchTerm : ICloneable<NuGetSearchTerm>
    {
        public List<string> Id { get; } = new List<string>();
        public List<string> Version { get; } = new List<string>();
        public List<string> Title { get; } = new List<string>();
        public List<string> Tags { get; } = new List<string>();
        public List<string> Description { get; } = new List<string>();
        public List<string> Summary { get; } = new List<string>();
        public List<string> Owner { get; } = new List<string>();

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            Append(result, "id", Id);
            Append(result, "version", Version);
            Append(result, "title", Title);
            Append(result, "tags", Tags);
            Append(result, "description", Description);
            Append(result, "summary", Summary);
            Append(result, "owner", Owner);
            return result.ToString();
        }

        private void Append(StringBuilder result, string key, List<string> values)
        {
            foreach (var value in values)
                result.Append(" ").Append(key).Append(":").Append(value);
        }

        public bool IsEmpty()
            => IsEmpty(Id) 
            && IsEmpty(Version) 
            && IsEmpty(Title) 
            && IsEmpty(Tags) 
            && IsEmpty(Description) 
            && IsEmpty(Summary) 
            && IsEmpty(Owner);

        private bool IsEmpty(List<string> values)
        {
            if (values.Count == 0)
                return false;

            if (values.Any(s => !string.IsNullOrEmpty(s)))
                return true;

            return false;
        }

        public bool IsMatched(IPackageSearchMetadata package)
            => IsMatched(package.Identity.Id, Id)
            && IsMatched(package.Identity.Version.OriginalVersion, Version)
            && IsMatched(package.Title, Title)
            && IsMatched(package.Tags, Tags)
            && IsMatched(package.Description, Description)
            && IsMatched(package.Summary, Summary)
            && IsMatched(package.Owners, Owner);

        private bool IsMatched(string packageValue, List<string> values)
        {
            bool result = true;
            foreach (var value in values)
            {
                if (string.IsNullOrEmpty(value))
                    continue;

                if (packageValue.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1)
                    return true;

                result = false;
            }

            return result;
        }

        public NuGetSearchTerm Clone()
        {
            var clone = new NuGetSearchTerm();
            clone.Id.AddRange(Id);
            clone.Version.AddRange(Version);
            clone.Title.AddRange(Title);
            clone.Tags.AddRange(Tags);
            clone.Description.AddRange(Description);
            clone.Summary.AddRange(Summary);
            clone.Owner.AddRange(Owner);
            return clone;
        }
    }
}
