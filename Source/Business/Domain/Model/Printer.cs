using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(PrintCommand), typeof(PrintResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.Printer, typeof(Printer))]
    public class Printer : TerminalDevice
    {
        //public override string Name { get; } = "Printer";
    }
}
