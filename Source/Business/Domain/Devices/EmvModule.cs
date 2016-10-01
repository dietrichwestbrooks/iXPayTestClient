using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class EmvModule : Device
    {
        public EmvModule()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetEMVModuleStatusCommand", typeof (GetEMVModuleStatusCommand)),
                    new DeviceCommand("GetApplicationExclusionListCommand", typeof (GetApplicationExclusionListCommand)),
                    new DeviceCommand("InitiateTransactionCommand", typeof (InitiateTransactionCommand)),
                    new DeviceCommand("SetAmountsCommand", typeof (SetAmountsCommand)),
                    new DeviceCommand("ContinueTransactionCommand", typeof (ContinueTransactionCommand)),
                    new DeviceCommand("CompleteOnlineTransactionCommand", typeof (CompleteOnlineTransactionCommand)),
                    new DeviceCommand("ReadCardDataCommand", typeof (ReadCardDataCommand)),
                    new DeviceCommand("AbortTransactionCommand", typeof (AbortTransactionCommand)),
                    new DeviceCommand("DownloadApplicationCommand", typeof (DownloadApplicationCommand)),
                    new DeviceCommand("DeleteApplicationCommand", typeof (DeleteApplicationCommand)),
                    new DeviceCommand("DownloadApplicationPublicKeysCommand", typeof (DownloadApplicationPublicKeysCommand)),
                    new DeviceCommand("DeleteApplicationPublicKeysCommand", typeof (DeleteApplicationPublicKeysCommand)),
                    new DeviceCommand("SetApplicationExclusionListCommand", typeof (SetApplicationExclusionListCommand)),
                    new DeviceCommand("ClearAllApplicationsAndKeysCommand", typeof (ClearAllApplicationsAndKeysCommand)),
                    new DeviceCommand("ReenterOnlinePINCommand", typeof (ReenterOnlinePINCommand)),
                    new DeviceCommand("SetTimeoutCommand", typeof (SetTimeoutCommand)),
                };
        }

        public override string Name { get; } = "EMV Module";

        public override Type CommandType { get; } = typeof(EMVModuleCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
