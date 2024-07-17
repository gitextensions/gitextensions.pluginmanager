using PackageManager.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PackageManager.Views
{
    public partial class PackageDetail : UserControl
    {
        public IPackage Model
        {
            get { return (IPackage)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "Model", 
            typeof(IPackage), 
            typeof(PackageDetail), 
            new PropertyMetadata(null, OnModelChanged)
        );

        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PackageDetail view = (PackageDetail)d;
            view.DataContext = view.Model;
        }

        public PackageDetail()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });
            e.Handled = true;
        }
    }
}
