﻿using PackageManager.Models;
using PackageManager.Services;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PackageManager.Views.Converters
{
    public class SelfPackageConverter : IValueConverter
    {
        public static SelfPackageConfiguration Configuration { get; set; }

        public object TrueValue { get; set; }
        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsSelfPackage(value))
                return TrueValue;

            return FalseValue;
        }

        private static bool IsSelfPackage(object value)
        {
            if (Configuration == null)
                return false;

            string packageId = value as string;
            if (packageId == null)
            {
                var package = value as IPackageIdentity;
                if (package == null)
                    return false;

                packageId = package.Id;
            }

            return Configuration.Equals(packageId);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
