using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(TamperDetectorsCommand), typeof(TamperDetectorsResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.TamperDetectors, typeof(TamperDetectors))]
    public class TamperDetectors : TerminalDevice
    {
        //public override string Name { get; } = "Tamper Detectors";
    }
}
