using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Local3DModelRepository.ValueConverters
{
    public sealed class InvserseBooleanToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool valueAsBoolean)
            {
                return valueAsBoolean
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
