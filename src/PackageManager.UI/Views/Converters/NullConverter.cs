﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace PackageManager.Views.Converters
{
    public class NullConverter : IValueConverter
    {
        public object? TrueValue { get; set; }
        public object? FalseValue { get; set; }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return TrueValue;

            if (value is string)
            {
                string? stringValue = value as string;
                if (stringValue == string.Empty)
                    return TrueValue;
            }

            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
