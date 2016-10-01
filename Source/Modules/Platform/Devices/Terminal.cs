using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class Terminal : TerminalDeviceT<TerminalCommand, TerminalResponse>
    {
        #region Convenience Methods and Properties
        // Add methods and properties that make it more convienent to send commands
        // to this device from scripting environment

        #endregion
    }
}
