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
