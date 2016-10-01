using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class NonSecureKeypad : Device
    {
        public NonSecureKeypad()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetOpenedCommand", typeof (GetOpenedCommand)),
                    new DeviceCommand("DisableKeypadCommand", typeof (DisableKeypadCommand)),
                    new DeviceCommand("EnableKeypadCommand", typeof (EnableKeypadCommand)),
                    new DeviceCommand("EnableKeypadForPINCommand", typeof (EnableKeypadForPINCommand)),
                };
        }

        public override string Name { get; } = "Non-Secure Keypad";

        public override Type CommandType { get; } = typeof(NonSecureKeypadCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
