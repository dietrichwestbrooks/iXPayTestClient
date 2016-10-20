using System;
using System.Windows.Data;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class ObjectToTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.GetType();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}
