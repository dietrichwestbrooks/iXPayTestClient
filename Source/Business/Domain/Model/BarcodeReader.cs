using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[Attributes.TerminalRequestHandler(typeof(BarcodeReaderCommand), typeof(BarcodeReaderResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.BarcodeReader, typeof(BarcodeReader))]
    public class BarcodeReader : TerminalDevice
    {
        //public override string Name { get; } = "Barcode Reader";
     }
}
