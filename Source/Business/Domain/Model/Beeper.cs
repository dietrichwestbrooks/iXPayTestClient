using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(BeeperCommand), typeof(BeeperResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.Beeper, typeof(Beeper))]
    public class Beeper : TerminalDevice
    {
        //public override string Name { get; } = "Beeper";
    }
}
