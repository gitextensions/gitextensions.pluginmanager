using System;
using System.Globalization;
using System.Windows.Data;

namespace PackageManager.Views.Converters
{
    public class NullValueConverter : IValueConverter
    {
        public object? DefaultValue { get; set; }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DefaultValue;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
