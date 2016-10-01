using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(MagStripeReaderCommand), typeof(MagStripeReaderResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.MagStripeReader, typeof(MagStripeReader))]
    public class MagStripeReader : TerminalDevice
    {
        //public override string Name { get; } = "Magnetic Stripe Reader";
    }
}
