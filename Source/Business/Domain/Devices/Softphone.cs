using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class Softphone : Device
    {
        public Softphone()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetCallStatusCommand", typeof (GetCallStatusCommand)),
                    new DeviceCommand("GetConnectStatusCommand", typeof (GetConnectStatusCommand)),
                    new DeviceCommand("SetVoipAccountCommand", typeof (SetVoipAccountCommand)),
                    new DeviceCommand("GetVoipAccountCommand", typeof (GetVoipAccountCommand)),
                    new DeviceCommand("GetVoipProtocolCommand", typeof (GetVoipProtocolCommand)),
                    new DeviceCommand("GetPhonenumberCommand", typeof (GetPhonenumberCommand)),
                    new DeviceCommand("SetPhonenumberCommand", typeof (SetPhonenumberCommand)),
                    new DeviceCommand("LiftReceiverCommand", typeof (LiftReceiverCommand)),
                    new DeviceCommand("HangupReceiverCommand", typeof (HangupReceiverCommand)),
                };
        }

        public override string Name { get; } = "Softphone";

        public override Type CommandType { get; } = typeof(SoftphoneCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
