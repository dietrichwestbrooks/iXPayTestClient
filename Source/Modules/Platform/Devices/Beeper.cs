using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class Beeper : TerminalDevice<BeeperCommand, BeeperResponse>
    {
        public Beeper() 
            : base("Beeper")
        {
            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new BeeperBeepMethod(this),
                });
        }
    }

    public class BeeperBeepMethod : TerminalDeviceMethod<BeepCommand, BeeperResponse>
    {
        public BeeperBeepMethod(ITerminalDevice device) 
            : base(device, "Beep")
        {
            InvokeCommand = new TerminalDeviceCommand<BeepCommand, BeeperResponse>(
                this,
                Name,
                () => new BeepCommand
                    {
                        OnTime = 50,
                        OnTimeSpecified = true,
                        OffTime = 100,
                        OffTimeSpecified = true,
                        NumBeeps = 3,
                        NumBeepsSpecified = true,
                    }
                );
        }
    }
}
