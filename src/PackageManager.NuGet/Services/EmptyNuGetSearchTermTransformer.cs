using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageManager.Services
{
    public class EmptyNuGetSearchTermTransformer : INuGetSearchTermTransformer
    {
        public void Transform(NuGetSearchTerm searchTerm)
        {
        }

        public readonly static EmptyNuGetSearchTermTransformer Instance = new EmptyNuGetSearchTermTransformer();
    }
}
