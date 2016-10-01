using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(DallasKeyCommand), typeof(DallasKeyResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.DallasKey, typeof(DallasKey))]
    public class DallasKey : TerminalDevice
    {
        //public override string Name { get; } = "Dallas Key";
    }
}
