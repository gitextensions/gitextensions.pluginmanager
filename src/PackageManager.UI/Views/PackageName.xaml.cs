using PackageManager.Models;
using System.Windows;
using System.Windows.Controls;

namespace PackageManager.Views
{
    public partial class PackageName : UserControl
    {
        public IPackage Model
        {
            get { return (IPackage)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "Model", 
            typeof(IPackage), 
            typeof(PackageName), 
            new PropertyMetadata(null, OnModelChanged)
        );

        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PackageName view = (PackageName)d;
            view.DataContext = view.Model;
        }

        public PackageName()
        {
            InitializeComponent();
        }
    }
}
