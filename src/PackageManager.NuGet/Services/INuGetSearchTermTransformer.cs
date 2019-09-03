using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageManager.Services
{
    public interface INuGetSearchTermTransformer
    {
        void Transform(NuGetSearchTerm searchTerm);
    }
}
