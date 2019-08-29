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

        #region Singleton

        private static EmptyNuGetSearchTermTransformer instance;
        private static object instanceLock = new object();

        public static EmptyNuGetSearchTermTransformer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                            instance = new EmptyNuGetSearchTermTransformer();
                    }
                }

                return instance;
            }
        }

        #endregion
    }
}
