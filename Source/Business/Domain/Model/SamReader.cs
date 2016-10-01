using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(SAMReaderCommand), typeof(SAMReaderResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.SamReader, typeof(SamReader))]
    public class SamReader : TerminalDevice
    {
        //public override string Name { get; } = "SAM Reader";
    }
}
