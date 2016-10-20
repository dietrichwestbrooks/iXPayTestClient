using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string FormattedXml(this string source)
        {
            if (source == null)
                return null;

            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(source);

            var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true,
                    NewLineOnAttributes = true
                };

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        public static string ToTitleCase(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return source;

            if (source.Length == 1)
                return source.ToLower();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            return textInfo.ToTitleCase(source);
        }
    }
}
