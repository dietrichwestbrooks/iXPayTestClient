using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Converters
{
    public class LogicalOrConverter : ConverterBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Cast<bool>().Any(v => v);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
