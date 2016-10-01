using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class Beeper : Device
    {
        public Beeper()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("BeepCommand", typeof (BeepCommand))
                };
        }

        public override string Name { get; } = "Beeper";

        public override Type CommandType { get; } = typeof(BeeperCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
