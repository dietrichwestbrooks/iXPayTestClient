using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Scriptable;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [Export(typeof(ITerminalRequestHandler))]
    [Export(typeof(IDynamicTerminalDevice))]
    [Export("Terminal")]
    public class Terminal : DeviceObjectT<TerminalCommand, TerminalResponse>
    {
    }
}
