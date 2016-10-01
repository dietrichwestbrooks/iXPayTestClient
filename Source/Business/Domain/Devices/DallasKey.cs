using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class DallasKey : Device
    {
        public DallasKey()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetOpenedCommand", typeof (GetOpenedCommand)),
                    new DeviceCommand("GetPositionCommand", typeof (GetPositionCommand)),
                    new DeviceCommand("OpenDallasKeyReaderCommand", typeof (OpenDallasKeyReaderCommand)),
                    new DeviceCommand("CloseDallasKeyReaderCommand", typeof (CloseDallasKeyReaderCommand)),
                    new DeviceCommand("ReadDallasKeyDataCommand", typeof (ReadDallasKeyDataCommand)),
                    new DeviceCommand("WriteDallasKeyDataCommand", typeof (WriteDallasKeyDataCommand)),
                    new DeviceCommand("TurnLightOnCommand", typeof (TurnLightOnCommand)),
                    new DeviceCommand("FlashLightCommand", typeof (FlashLightCommand)),
                    new DeviceCommand("TurnLightOffCommand", typeof (TurnLightOffCommand)),
                };
        }

        public override string Name { get; } = "Dallas Key";

        public override Type CommandType { get; } = typeof(DallasKeyCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
