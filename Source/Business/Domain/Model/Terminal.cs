using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(TerminalCommand), typeof(TerminalResponse))]
    [Attributes.TerminalDevice(Constants.DeviceNames.Terminal, typeof(Terminal))]
    public class Terminal : TerminalDevice
    {
        //public override string Name { get; } = "Terminal";
    }
}
