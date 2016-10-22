using System;
using System.Globalization;
using System.Windows.Data;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class NullToBoolValueConverter : ConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value == null;
            return parameter != null ? !result : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
