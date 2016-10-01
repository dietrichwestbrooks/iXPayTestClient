using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(SoftphoneCommand), typeof(SoftphoneResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.Softphone, typeof(Softphone))]
    public class Softphone : TerminalDevice
    {
        //public override string Name { get; } = "Softphone";
    }
}
