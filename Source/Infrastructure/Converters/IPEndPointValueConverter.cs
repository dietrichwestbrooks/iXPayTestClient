using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows.Data;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Converters
{
    public class IPEndPointValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() != 2 || string.IsNullOrWhiteSpace(values[0]?.ToString()))
                return null;

            IPAddress address;
            int port;

            if (!IPAddress.TryParse(values[0].ToString(), out address))
                return null;

            if (!int.TryParse(values[1]?.ToString(), out port))
                port = 0;

            return new IPEndPoint(address, port);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var endPoint = value as IPEndPoint;

            return endPoint == null ? null : new object[] {endPoint.Address, endPoint.Port};
        }
    }
}
