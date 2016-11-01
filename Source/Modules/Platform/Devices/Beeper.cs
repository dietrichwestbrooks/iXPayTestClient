using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class Beeper
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register <BeeperCommand, BeeperResponse > (
                    "Beeper", new TerminalRequestHandlerByName("Terminal"), typeof(Beeper));
        }

        public static readonly TerminalDeviceMethod BeepMethod =
         TerminalDeviceMethod.Register<BeepCommand, BeeperResponse>("Beep",
             typeof(Beeper), () => new BeepCommand
             {
                 OnTime = 50,
                 OnTimeSpecified = true,
                 OffTime = 100,
                 OffTimeSpecified = true,
                 NumBeeps = 3,
                 NumBeepsSpecified = true,
             });
    }
}
