using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(EMVModuleCommand), typeof(EMVModuleResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.EmvModule, typeof(EmvModule))]
    public class EmvModule : TerminalDevice
    {
        //public override string Name { get; } = "EMV Module";
    }
}
