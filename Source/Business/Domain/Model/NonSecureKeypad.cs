using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    //[TerminalRequestHandler(typeof(NonSecureKeypadCommand), typeof(NonSecureKeypadResponse), Successor = typeof(Terminal))]
    [Attributes.TerminalDevice(Constants.DeviceNames.NonSecureKeypad, typeof(NonSecureKeypad))]
    public class NonSecureKeypad : TerminalDevice
    {
        //public override string Name { get; } = "Non-Secure Keypad";
    }
}
