﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PackageManager.Views.Converters
{
    public class FirstNotNullMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != null && values[i] != DependencyProperty.UnsetValue)
                    return values[i];
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
