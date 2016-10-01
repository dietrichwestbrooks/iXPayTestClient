using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(BillAcceptorCommand), typeof(BillAcceptorResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.BillAcceptor, typeof(BillAcceptor))]
    public class BillAcceptor : TerminalDevice
    {
        //public override string Name { get; } = "Bill Acceptor";
    }
}
