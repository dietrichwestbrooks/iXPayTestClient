using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class TamperDetectors : Device
    {
        public TamperDetectors()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetAvailableDetectorsCommand", typeof (GetAvailableDetectorsCommand)),
                    new DeviceCommand("ConfirmDetectorOpenCommand", typeof (ConfirmDetectorOpenCommand)),
                };
        }

        public override string Name { get; } = "Tamper Detectors";

        public override Type CommandType { get; } = typeof(TamperDetectorsCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
