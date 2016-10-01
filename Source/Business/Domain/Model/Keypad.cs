using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(KeypadCommand), typeof(KeypadResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.Keypad, typeof(Keypad))]
    public class Keypad : TerminalDevice
    {
        //public override string Name { get; } = "Keypad";
    }
}
