using PackageManager.Models;
using PackageManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
