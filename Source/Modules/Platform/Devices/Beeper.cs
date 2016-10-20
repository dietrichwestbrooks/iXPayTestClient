using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class Beeper : TerminalDevice<BeepCommand, BeeperResponse>
    {
        public Beeper() 
            : base("Beeper")
        {
            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new BeepMethod(this),
                });
        }
    }

    public class BeepMethod : TerminalDeviceMethod<BeepCommand, BeeperResponse>
    {
        public BeepMethod(ITerminalDevice device) 
            : base(device, "Beep")
        {
            InvokeCommand = new TerminalDeviceCommand<BeepCommand, BeeperResponse>(
                this,
                Name,
                new SortedList<string, object>
                    {
                        {"onTime", 1000},
                        {"onTimeSpecified", true},
                        {"offTime", 100},
                        {"offTimeSpecified", true},
                    }
                );
        }
    }
}
