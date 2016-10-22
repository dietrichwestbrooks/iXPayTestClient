using System;
using System.Windows.Markup;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Converters
{
    public abstract class ConverterBase : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
