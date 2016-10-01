using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(SecurityModuleCommand), typeof(SecurityModuleResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.SecurityModule, typeof(SecurityModule))]
    public class SecurityModule : TerminalDevice
    {
        //public override string Name { get; } = "Security Module";
    }
}
