using Neptuo;

namespace PackageManager.Services
{
    public class TagsNuGetSearchTermTransformer : INuGetSearchTermTransformer
    {
        private readonly string tags;

        public TagsNuGetSearchTermTransformer(string tags)
        {
            Ensure.NotNull(tags, "tags");
            this.tags = tags;
        }

        public void Transform(NuGetSearchTerm searchTerm)
        {
            if (!string.IsNullOrEmpty(tags))
                searchTerm.Tags.Add(tags);
        }
    }
}
