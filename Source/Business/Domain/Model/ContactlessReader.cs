using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(ContactlessReaderCommand), typeof(ContactlessReaderResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.ContactlessReader, typeof(ContactlessReader))]
    public class ContactlessReader : TerminalDevice
    {
        //public override string Name { get; } = "Contactless Reader";
    }
}
