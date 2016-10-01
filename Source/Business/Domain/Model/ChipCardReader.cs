using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(ChipCardReaderCommand), typeof(ChipCardReaderResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.ChipCardReader, typeof(ChipCardReader))]
    public class ChipCardReader : TerminalDevice
    {
        //public override string Name { get; } = "Chip Card Reader";
    }
}
