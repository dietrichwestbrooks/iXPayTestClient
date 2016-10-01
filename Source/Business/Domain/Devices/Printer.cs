using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class Printer : Device
    {
        public Printer()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetPaperStatusCommand", typeof (GetPaperStatusCommand)),
                    new DeviceCommand("GetCapabilitiesCommand", typeof (GetCapabilitiesCommand)),
                    new DeviceCommand("GetWidthCommand", typeof (GetWidthCommand)),
                    new DeviceCommand("GetFontsCommand", typeof (GetFontsCommand)),
                    new DeviceCommand("GetTypeFacesCommand", typeof (GetTypeFacesCommand)),
                    new DeviceCommand("GetSupportedImagesCommand", typeof (GetSupportedImagesCommand)),
                    new DeviceCommand("GetSupportedBarCodeSymbologiesCommand", typeof (GetSupportedBarCodeSymbologiesCommand)),
                    new DeviceCommand("PrintCommand", typeof (PrintCommand)),
                    new DeviceCommand("SetMemoryImageCommand", typeof (SetMemoryImageCommand)),
                    new DeviceCommand("TurnLightOnCommand", typeof (TurnLightOnCommand)),
                    new DeviceCommand("FlashLightCommand", typeof (FlashLightCommand)),
                    new DeviceCommand("TurnLightOffCommand", typeof (TurnLightOffCommand)),
                };
        }

        public override string Name { get; } = "Printer";

        public override Type CommandType { get; } = typeof(PrinterCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
