using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Local3DModelRepository.ValueConverters
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool valueAsBool)
            {
                throw new ArgumentException($"Expected value to be of type bool, but instead it was {targetType}");
            }

            return valueAsBool
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}