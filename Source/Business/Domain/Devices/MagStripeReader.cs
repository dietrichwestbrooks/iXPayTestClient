using System;
using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class MagStripeReader : Device
    {
        public MagStripeReader()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetOpenedCommand", typeof (GetOpenedCommand)),
                    new DeviceCommand("GetCardPositionCommand", typeof (GetCardPositionCommand)),
                    new DeviceCommand("GetReaderTypeCommand", typeof (GetReaderTypeCommand)),
                    new DeviceCommand("OpenMagStripeReaderCommand", typeof (OpenMagStripeReaderCommand)),
                    new DeviceCommand("CloseMagStripeReaderCommand", typeof (CloseMagStripeReaderCommand)),
                    new DeviceCommand("TurnLightOnCommand", typeof (TurnLightOnCommand)),
                    new DeviceCommand("FlashLightCommand", typeof (FlashLightCommand)),
                    new DeviceCommand("TurnLightOffCommand", typeof (TurnLightOffCommand)),
                };
        }

        public override string Name { get; } = "Magnetic Stripe Reader";

        public override Type CommandType { get; } = typeof(MagStripeReaderCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
