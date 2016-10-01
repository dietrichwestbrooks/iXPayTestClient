using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class SamReader : Device
    {
        public SamReader()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetAvailableSlotsCommand", typeof (GetAvailableSlotsCommand)),
                    new DeviceCommand("RefreshAvailableSlotsCommand", typeof (RefreshAvailableSlotsCommand)),
                    new DeviceCommand("SelectSAMSlotCommand", typeof (SelectSAMSlotCommand)),
                    new DeviceCommand("ActivateSAMCommand", typeof (ActivateSAMCommand)),
                    new DeviceCommand("DeactivateSAMCommand", typeof (DeactivateSAMCommand)),
                    new DeviceCommand("SoftResetSAMCommand", typeof (SoftResetSAMCommand)),
                    new DeviceCommand("SAMProcessAPDUCommand", typeof (GetStatusCommand)),
                };
        }

        public override string Name { get; } = "SAM Reader";

        public override Type CommandType { get; } = typeof(SAMReaderCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
