using Neptuo;
using PackageManager.Logging.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace PackageManager.Views
{
    public partial class LogWindow : Window
    {
        private readonly MemoryLogSerializer log;

        public LogWindow(MemoryLogSerializer log)
        {
            Ensure.NotNull(log, "log");
            this.log = log;

            InitializeComponent();
            RefreshContent();
        }

        private async void RefreshContent()
        {
            TextContent.Text = log.GetContent();
            if (string.IsNullOrEmpty(TextContent.Text))
                TextContent.Text = "No entries.";

            await Task.Delay(50);
            TextContent.ScrollToEnd();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            log.Clear();
            RefreshContent();
        }

        private void GoToBottom_Click(object sender, RoutedEventArgs e)
            => TextContent.ScrollToEnd();
    }
}
