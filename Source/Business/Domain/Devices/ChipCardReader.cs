using System;
using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class ChipCardReader : Device
    {
        public ChipCardReader()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetOpenedCommand", typeof (GetOpenedCommand)),
                    new DeviceCommand("GetCardPositionCommand", typeof (GetCardPositionCommand)),
                    new DeviceCommand("GetReaderTypeCommand", typeof (GetReaderTypeCommand)),
                    new DeviceCommand("OpenChipCardReaderCommand", typeof (OpenChipCardReaderCommand)),
                    new DeviceCommand("CloseChipCardReaderCommand", typeof (CloseChipCardReaderCommand)),
                    new DeviceCommand("TurnLightOnCommand", typeof (TurnLightOnCommand)),
                    new DeviceCommand("FlashLightCommand", typeof (FlashLightCommand)),
                    new DeviceCommand("TurnLightOffCommand", typeof (TurnLightOffCommand)),
                };
        }

        public override string Name { get; } = "Chip Card Reader";

        public override Type CommandType { get; } = typeof(ChipCardReaderCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
