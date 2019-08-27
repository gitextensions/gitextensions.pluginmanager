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
        string Transform(string searchTerm);
    }
}
