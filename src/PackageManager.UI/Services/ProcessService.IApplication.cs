namespace PackageManager.Services
{
    partial class ProcessService
    {
        public interface IApplication
        {
            object Args { get; }

            void Shutdown();
        }
    }
}
