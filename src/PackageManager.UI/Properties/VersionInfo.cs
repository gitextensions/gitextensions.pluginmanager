using System.Reflection;

namespace PackageManager
{
    public static class VersionInfo
    {
        public static string Version => typeof(VersionInfo).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
