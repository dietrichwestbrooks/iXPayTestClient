using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class SecurityModule : Device
    {
        public SecurityModule()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetSerialNumberCommand", typeof (GetSerialNumberCommand)),
                    new DeviceCommand("GetBankSerialNumberCommand", typeof (GetBankSerialNumberCommand)),
                    new DeviceCommand("GetSupportedKeyHierarchiesCommand", typeof (GetSupportedKeyHierarchiesCommand)),
                    new DeviceCommand("GetHierarchyInUseCommand", typeof (GetHierarchyInUseCommand)),
                    new DeviceCommand("GetSupportedWorkingKeysCommand", typeof (GetSupportedWorkingKeysCommand)),
                    new DeviceCommand("GetSecurityModuleCapabilitiesCommand", typeof (GetSecurityModuleCapabilitiesCommand)),
                    new DeviceCommand("SwitchKeyHierarchyCommand", typeof (SwitchKeyHierarchyCommand)),
                    new DeviceCommand("CalculateMACCommand", typeof (CalculateMACCommand)),
                    new DeviceCommand("VerifyMACCommand", typeof (VerifyMACCommand)),
                    new DeviceCommand("EncryptMessageCommand", typeof (EncryptMessageCommand)),
                    new DeviceCommand("SetWorkingKeyCommand", typeof (SetWorkingKeyCommand)),
                    new DeviceCommand("GetWorkingKeyStatisticsCommand", typeof (GetWorkingKeyStatisticsCommand)),
                    new DeviceCommand("SetHierarchyInUseCommand", typeof (SetHierarchyInUseCommand)),
                    new DeviceCommand("IncrementKSNCommand", typeof (IncrementKSNCommand)),
                };
        }

        public override string Name { get; } = "Security Module";

        public override Type CommandType { get; } = typeof(SecurityModuleCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
