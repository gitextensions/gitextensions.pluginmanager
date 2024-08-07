﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace PackageManager.Views.Converters
{
    public class FormatConverter : IValueConverter
    {
        public string Format { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IFormattable formattable = value as IFormattable;
            if (formattable != null)
                return formattable.ToString(Format, culture);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
