using System;
using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class Terminal : Device
    {
        public Terminal()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetTerminalIdCommand", typeof (GetTerminalIdCommand)),
                    new DeviceCommand("GetPackageVersionCommand", typeof (GetPackageVersionCommand)),
                    new DeviceCommand("GetOperationalStateCommand", typeof (GetOperationalStateCommand)),
                    new DeviceCommand("GetTerminalStateCommand", typeof (GetTerminalStateCommand)),
                    new DeviceCommand("GetBatteryStateCommand", typeof (GetBatteryStateCommand)),
                    new DeviceCommand("ResetCommand", typeof (ResetCommand)),
                    new DeviceCommand("SetDateTimeCommand", typeof (SetDateTimeCommand)),
                    new DeviceCommand("SetToIdleCommand", typeof (SetToIdleCommand)),
                    new DeviceCommand("DisplaySecurePromptCommand", typeof (DisplaySecurePromptCommand)),
                    new DeviceCommand("EnableFunctionKeysCommand", typeof (EnableFunctionKeysCommand)),
                    new DeviceCommand("DisableFunctionKeysCommand", typeof (DisableFunctionKeysCommand)),
                    new DeviceCommand("EnableCancelKeyCommand", typeof (EnableCancelKeyCommand)),
                    new DeviceCommand("DisableCancelKeyCommand", typeof (DisableCancelKeyCommand)),
                    new DeviceCommand("GetSupportedTerminalIdsCommand", typeof (GetSupportedTerminalIdsCommand)),
                    new DeviceCommand("PassThroughCommand", typeof (PassThroughCommand)),
                };
        }

        public override string Name { get; } = "Terminal";

        public override Type CommandType { get; } = typeof(TerminalCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
