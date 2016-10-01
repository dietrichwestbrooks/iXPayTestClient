using System;
using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class BarcodeReader : Device
    {
        public BarcodeReader()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetOpenedCommand", typeof (GetOpenedCommand)),
                    new DeviceCommand("OpenBarcodeReaderCommand", typeof (OpenBarcodeReaderCommand)),
                    new DeviceCommand("CloseBarCodeReaderCommand", typeof (CloseBarCodeReaderCommand)),
                    new DeviceCommand("TurnLightOnCommand", typeof (TurnLightOnCommand)),
                    new DeviceCommand("FlashLightCommand", typeof (FlashLightCommand)),
                    new DeviceCommand("TurnLightOffCommand", typeof (TurnLightOffCommand)),
                };
        }

        public override string Name { get; } = "Barcode Reader";

        public override Type CommandType { get; } = typeof (BarcodeReaderCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
