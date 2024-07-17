using System;
using System.Globalization;
using System.Windows.Data;

namespace PackageManager.Views.Converters
{
    public class ValidUrlToTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string url = (string)value;
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uri);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
