using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageManager.Services
{
    public class NuGetSearchTerm
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

            if (values.Any(s => !String.IsNullOrEmpty(s)))
                return true;

            return false;
        }
    }
}
