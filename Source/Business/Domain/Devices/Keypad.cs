using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class Keypad : Device
    {
        public Keypad()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetOpenedCommand", typeof (GetOpenedCommand)),
                    new DeviceCommand("DisableKeypadCommand", typeof (DisableKeypadCommand)),
                    new DeviceCommand("EnableKeypadFunctionKeysCommand", typeof (EnableKeypadFunctionKeysCommand)),
                    new DeviceCommand("ValidatePINBlockCommand", typeof (ValidatePINBlockCommand)),
                };
        }

        public override string Name { get; } = "Keypad";

        public override Type CommandType { get; } = typeof(KeypadCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
