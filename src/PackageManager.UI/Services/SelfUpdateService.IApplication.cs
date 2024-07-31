namespace PackageManager.Services
{
    partial class SelfUpdateService
    {
        public interface IApplication
        {
            IArgs Args { get; }
            void Shutdown();
        }
    }
}
