using System;
using System.Globalization;
using System.Windows.Data;

namespace PackageManager.Views.Converters
{
    public class DropNewLineConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? target = value?.ToString();
            if (string.IsNullOrEmpty(target))
                return null;

            target = target.Replace(Environment.NewLine, string.Empty);
            target = target.Replace("\n", string.Empty);
            return target;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
