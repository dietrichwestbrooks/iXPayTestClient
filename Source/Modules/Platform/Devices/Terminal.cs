using System;
using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class Terminal : TerminalDevice<TerminalCommand, TerminalResponse, TerminalEvent>
    {
        public Terminal() 
            : base("Terminal")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new TerminalIdProperty(this),
                    new SupportedTerminalIdsProperty(this),
                    new PackageVersionProperty(this),
                    new OperationalStateProperty(this),
                    new TerminalStateProperty(this),
                    new BatteryStateProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new ResetMethod(this),
                    new SetDateTimeMethod(this),
                    new SetToIdleMethod(this),
                    new DisplaySecurePromptMethod(this),
                    new EnableFunctionKeysMethod(this),
                    new DisableFunctionKeysMethod(this),
                    new EnableCancelKeyMethod(this),
                    new DisableCancelKeyMethod(this),
                    new PassThroughMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new CancelKeyPressedEvent(this),
                    new FunctionKeyPressedEvent(this),
                    new OperationalStatusChangedEvent(this),
                    new StatusChangedEvent(this),
                    new BatteryStatusChangedEvent(this),
                    new FunctionKeyEntryTimedOutEvent(this),
                });
        }

        #region Device Properties

        [ValueProperty("TerminalId")]
        public class TerminalIdProperty : TerminalDeviceProperty<int,
            GetTerminalIdCommand, GetTerminalIdResponse>
        {
            public TerminalIdProperty(ITerminalDevice device)
                : base(device, "TerminalId")
            {
                GetCommand = new TerminalDeviceCommand<GetTerminalIdCommand, GetTerminalIdResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("SupportedTerminal")]
        public class SupportedTerminalIdsProperty : TerminalDeviceProperty<SupportedTerminal[],
            GetSupportedTerminalIdsCommand, GetSupportedTerminalIdsResponse>
        {
            public SupportedTerminalIdsProperty(ITerminalDevice device)
                : base(device, "SupportedTerminalIds")
            {
                GetCommand = new TerminalDeviceCommand<GetSupportedTerminalIdsCommand, GetSupportedTerminalIdsResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("PackageVersion")]
        public class PackageVersionProperty : TerminalDeviceProperty<string,
            GetPackageVersionCommand, GetPackageVersionResponse>
        {
            public PackageVersionProperty(ITerminalDevice device)
                : base(device, "PackageVersion")
            {
                GetCommand = new TerminalDeviceCommand<GetPackageVersionCommand, GetPackageVersionResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("OperationalState")]
        public class OperationalStateProperty : TerminalDeviceProperty<OperationalStatus,
            GetOperationalStateCommand, GetOperationalStateResponse>
        {
            public OperationalStateProperty(ITerminalDevice device)
                : base(device, "OperationalState")
            {
                GetCommand = new TerminalDeviceCommand<GetOperationalStateCommand, GetOperationalStateResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("TerminalState")]
        public class TerminalStateProperty : TerminalDeviceProperty<TerminalStatus,
            GetTerminalStateCommand, GetTerminalStateResponse>
        {
            public TerminalStateProperty(ITerminalDevice device)
                : base(device, "TerminalState")
            {
                GetCommand = new TerminalDeviceCommand<GetTerminalStateCommand, GetTerminalStateResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("BatteryState")]
        public class BatteryStateProperty : TerminalDeviceProperty<BatteryStatus,
            GetBatteryStateCommand, GetBatteryStateResponse>
        {
            public BatteryStateProperty(ITerminalDevice device)
                : base(device, "BatteryState")
            {
                GetCommand = new TerminalDeviceCommand<GetBatteryStateCommand, GetBatteryStateResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        #endregion

        #region Device Methods

        public class DisplaySecurePromptMethod : TerminalDeviceMethod<DisplaySecurePromptCommand, DisplaySecurePromptResponse>
        {
            public DisplaySecurePromptMethod(ITerminalDevice device)
                : base(device, "DisplaySecurePrompt")
            {
                InvokeCommand = new TerminalDeviceCommand<DisplaySecurePromptCommand, DisplaySecurePromptResponse>(
                    this,
                    Name,
                    () => new DisplaySecurePromptCommand
                        {
                            Language = "en",
                            SecurityLevel = SecurityLevels.Unencrypted,
                            EntryType = EntryType.AlphaNumeric,
                        }
                    );
            }
        }

        public class ResetMethod : TerminalDeviceMethod<ResetCommand, ResetResponse>
        {
            public ResetMethod(ITerminalDevice device)
                : base(device, "Reset")
            {
                InvokeCommand = new TerminalDeviceCommand<ResetCommand, ResetResponse>(
                    this,
                    Name
                    );
            }
        }

        public class SetDateTimeMethod : TerminalDeviceMethod<SetDateTimeCommand, SetDateTimeResponse>
        {
            public SetDateTimeMethod(ITerminalDevice device)
                : base(device, "SetDateTime")
            {
                InvokeCommand = new TerminalDeviceCommand<SetDateTimeCommand, ResetResponse>(
                    this,
                    Name,
                    () => new SetDateTimeCommand
                    {
                        SystemClock = new SystemClock
                        {
                            dateTime = DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss"),
                            TimeZone = new SystemClockTimeZone
                            {
                                utcOffsetMinutes = (int)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes,
                                standardName = TimeZone.CurrentTimeZone.StandardName,
                                daylightName = TimeZone.CurrentTimeZone.DaylightName,
                            }
                        },
                    }
                    );
            }
        }

        public class SetToIdleMethod : TerminalDeviceMethod<SetToIdleCommand, SetToIdleResponse>
        {
            public SetToIdleMethod(ITerminalDevice device)
                : base(device, "SetToIdle")
            {
                InvokeCommand = new TerminalDeviceCommand<SetToIdleCommand, SetToIdleResponse>(
                    this,
                    Name
                    );
            }
        }

        public class EnableFunctionKeysMethod : TerminalDeviceMethod<EnableFunctionKeysCommand, EnableFunctionKeysResponse>
        {
            public EnableFunctionKeysMethod(ITerminalDevice device)
                : base(device, "EnableFunctionKeys")
            {
                InvokeCommand = new TerminalDeviceCommand<EnableFunctionKeysCommand, EnableFunctionKeysResponse>(
                    this,
                    Name,
                    () => new EnableFunctionKeysCommand
                        {
                            FunctionKey = new []
                                {
                                    new FunctionKey {Function = 1, Location = 1, LocationSpecified = true, Text = "Credit"},
                                    new FunctionKey {Function = 2, Location = 5, LocationSpecified = true, Text = "Debit"},
                                    new FunctionKey {Function = 3, Location = 4, LocationSpecified = true, Text = "Cancel"},
                                }
                    }
                    );
            }
        }

        public class DisableFunctionKeysMethod : TerminalDeviceMethod<DisableFunctionKeysCommand, DisableFunctionKeysResponse>
        {
            public DisableFunctionKeysMethod(ITerminalDevice device)
                : base(device, "DisableFunctionKeys")
            {
                InvokeCommand = new TerminalDeviceCommand<DisableFunctionKeysCommand, DisableFunctionKeysResponse>(
                    this,
                    Name
                    );
            }
        }

        public class EnableCancelKeyMethod : TerminalDeviceMethod<EnableCancelKeyCommand, EnableCancelKeyResponse>
        {
            public EnableCancelKeyMethod(ITerminalDevice device)
                : base(device, "EnableCancelKey")
            {
                InvokeCommand = new TerminalDeviceCommand<EnableCancelKeyCommand, EnableCancelKeyResponse>(
                    this,
                    Name
                    );
            }
        }

        public class DisableCancelKeyMethod : TerminalDeviceMethod<DisableCancelKeyCommand, DisableCancelKeyResponse>
        {
            public DisableCancelKeyMethod(ITerminalDevice device)
                : base(device, "DisableCancelKey")
            {
                InvokeCommand = new TerminalDeviceCommand<DisableCancelKeyCommand, DisableCancelKeyResponse>(
                    this,
                    Name
                    );
            }
        }

        public class PassThroughMethod : TerminalDeviceMethod<PassThroughCommand, PassThroughResponse>
        {
            public PassThroughMethod(ITerminalDevice device)
                : base(device, "PassThrough")
            {
                InvokeCommand = new TerminalDeviceCommand<PassThroughCommand, PassThroughResponse>(
                    this,
                    Name,
                    () => new PassThroughCommand {Value = new byte[] {0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0}}
                    );
            }
        }

        #endregion

        #region Device Events

        public class CancelKeyPressedEvent : TerminalDeviceEvent<CancelKeyPressed>
        {
            public CancelKeyPressedEvent(ITerminalDevice device)
                : base(device, "CancelKeyPressed")
            {
            }
        }

        public class FunctionKeyPressedEvent : TerminalDeviceEvent<FunctionKeyPressed>
        {
            public FunctionKeyPressedEvent(ITerminalDevice device)
                : base(device, "FunctionKeyPressed")
            {
            }
        }

        public class OperationalStatusChangedEvent : TerminalDeviceEvent<OperationalStatusChanged>
        {
            public OperationalStatusChangedEvent(ITerminalDevice device)
                : base(device, "OperationalStatusChanged")
            {
            }
        }

        public class StatusChangedEvent : TerminalDeviceEvent<TerminalStatusChanged>
        {
            public StatusChangedEvent(ITerminalDevice device)
                : base(device, "StatusChanged")
            {
            }
        }

        public class BatteryStatusChangedEvent : TerminalDeviceEvent<TerminalStatusChanged>
        {
            public BatteryStatusChangedEvent(ITerminalDevice device)
                : base(device, "BatteryStatusChanged")
            {
            }
        }

        public class FunctionKeyEntryTimedOutEvent : TerminalDeviceEvent<TerminalStatusChanged>
        {
            public FunctionKeyEntryTimedOutEvent(ITerminalDevice device)
                : base(device, "FunctionKeyEntryTimedOut")
            {
            }
        }

        #endregion
    }
}
