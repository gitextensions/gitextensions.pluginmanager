namespace PackageManager.Services
{
    public interface INuGetSearchTermTransformer
    {
        void Transform(NuGetSearchTerm searchTerm);
    }
}
