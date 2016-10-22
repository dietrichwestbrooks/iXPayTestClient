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
                    new TerminalSupportedIdsProperty(this),
                    new TerminalPackageVersionProperty(this),
                    new TerminalOperationalStateProperty(this),
                    new TerminalStateProperty(this),
                    new TerminalBatteryStateProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new TerminalResetMethod(this),
                    new TerminalSetDateTimeMethod(this),
                    new TerminalSetToIdleMethod(this),
                    new TerminalDisplaySecurePromptMethod(this),
                    new TerminalEnableFunctionKeysMethod(this),
                    new TerminalDisableFunctionKeysMethod(this),
                    new TerminalEnableCancelKeyMethod(this),
                    new TerminalDisableCancelKeyMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new TerminalCancelKeyPressedEvent(this),
                    new TerminalFunctionKeyPressedEvent(this),
                    new TerminalOperationalStatusChangedEvent(this),
                    new TerminalStatusChangedEvent(this),
                    new TerminalBatteryStatusChangedEvent(this),
                    new TerminalFunctionKeyEntryTimedOutEvent(this),
                });
        }
    }

    #region Properties

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
    public class TerminalSupportedIdsProperty : TerminalDeviceProperty<SupportedTerminal[],
        GetSupportedTerminalIdsCommand, GetSupportedTerminalIdsResponse>
    {
        public TerminalSupportedIdsProperty(ITerminalDevice device)
            : base(device, "SupportedTerminalIds")
        {
            GetCommand = new TerminalDeviceCommand<GetSupportedTerminalIdsCommand, GetSupportedTerminalIdsResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("PackageVersion")]
    public class TerminalPackageVersionProperty : TerminalDeviceProperty<string,
        GetPackageVersionCommand, GetPackageVersionResponse>
    {
        public TerminalPackageVersionProperty(ITerminalDevice device)
            : base(device, "PackageVersion")
        {
            GetCommand = new TerminalDeviceCommand<GetPackageVersionCommand, GetPackageVersionResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("OperationalState")]
    public class TerminalOperationalStateProperty : TerminalDeviceProperty<OperationalStatus,
        GetOperationalStateCommand, GetOperationalStateResponse>
    {
        public TerminalOperationalStateProperty(ITerminalDevice device)
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
    public class TerminalBatteryStateProperty : TerminalDeviceProperty<BatteryStatus,
        GetBatteryStateCommand, GetBatteryStateResponse>
    {
        public TerminalBatteryStateProperty(ITerminalDevice device)
            : base(device, "BatteryState")
        {
            GetCommand = new TerminalDeviceCommand<GetBatteryStateCommand, GetBatteryStateResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    #endregion

    #region Methods

    public class TerminalDisplaySecurePromptMethod : TerminalDeviceMethod<DisplaySecurePromptCommand, DisplaySecurePromptResponse>
    {
        public TerminalDisplaySecurePromptMethod(ITerminalDevice device)
            : base(device, "DisplaySecurePrompt")
        {
            InvokeCommand = new TerminalDeviceCommand<DisplaySecurePromptCommand, DisplaySecurePromptResponse>(
                this,
                Name,
                () => new DisplaySecurePromptCommand
                    {
                        PromptId = 1,
                        Language = "en",
                        SecurityLevel = SecurityLevels.Encrypted,
                    }
                );
        }
    }

    public class TerminalResetMethod : TerminalDeviceMethod<ResetCommand, ResetResponse>
    {
        public TerminalResetMethod(ITerminalDevice device)
            : base(device, "Reset")
        {
            InvokeCommand = new TerminalDeviceCommand<ResetCommand, ResetResponse>(
                this,
                Name
                );
        }
    }

    public class TerminalSetDateTimeMethod : TerminalDeviceMethod<SetDateTimeCommand, SetDateTimeResponse>
    {
        public TerminalSetDateTimeMethod(ITerminalDevice device)
            : base(device, "SetDateTime")
        {
            InvokeCommand = new TerminalDeviceCommand<SetDateTimeCommand, ResetResponse>(
                this,
                Name,
                () => new SetDateTimeCommand
                    {
                        SystemClock = new SystemClock
                            {
                                dateTime = "2003-04-18 13:24:45",
                                TimeZone = new SystemClockTimeZone
                                    {
                                        utcOffsetMinutes = -360,
                                        standardName = "Central Standard Time",
                                        daylightName = "Central Daylight Time",
                                    }
                            },
                    }
                );
        }
    }

    public class TerminalSetToIdleMethod : TerminalDeviceMethod<SetToIdleCommand, SetToIdleResponse>
    {
        public TerminalSetToIdleMethod(ITerminalDevice device)
            : base(device, "SetToIdle")
        {
            InvokeCommand = new TerminalDeviceCommand<SetToIdleCommand, SetToIdleResponse>(
                this,
                Name
                );
        }
    }

    public class TerminalEnableFunctionKeysMethod : TerminalDeviceMethod<EnableFunctionKeysCommand, EnableFunctionKeysResponse>
    {
        public TerminalEnableFunctionKeysMethod(ITerminalDevice device)
            : base(device, "EnableFunctionKeys")
        {
            InvokeCommand = new TerminalDeviceCommand<EnableFunctionKeysCommand, EnableFunctionKeysResponse>(
                this,
                Name
                );
        }
    }

    public class TerminalDisableFunctionKeysMethod : TerminalDeviceMethod<DisableFunctionKeysCommand, DisableFunctionKeysResponse>
    {
        public TerminalDisableFunctionKeysMethod(ITerminalDevice device)
            : base(device, "DisableFunctionKeys")
        {
            InvokeCommand = new TerminalDeviceCommand<DisableFunctionKeysCommand, DisableFunctionKeysResponse>(
                this,
                Name
                );
        }
    }

    public class TerminalEnableCancelKeyMethod : TerminalDeviceMethod<EnableCancelKeyCommand, EnableCancelKeyResponse>
    {
        public TerminalEnableCancelKeyMethod(ITerminalDevice device)
            : base(device, "EnableCancelKey")
        {
            InvokeCommand = new TerminalDeviceCommand<EnableCancelKeyCommand, EnableCancelKeyResponse>(
                this,
                Name
                );
        }
    }

    public class TerminalDisableCancelKeyMethod : TerminalDeviceMethod<DisableCancelKeyCommand, DisableCancelKeyResponse>
    {
        public TerminalDisableCancelKeyMethod(ITerminalDevice device)
            : base(device, "DisableCancelKey")
        {
            InvokeCommand = new TerminalDeviceCommand<DisableCancelKeyCommand, DisableCancelKeyResponse>(
                this,
                Name
                );
        }
    }

    #endregion

    #region Events

    public class TerminalCancelKeyPressedEvent : TerminalDeviceEvent<CancelKeyPressed>
    {
        public TerminalCancelKeyPressedEvent(ITerminalDevice device)
            : base(device, "CancelKeyPressed")
        {
        }
    }

    public class TerminalFunctionKeyPressedEvent : TerminalDeviceEvent<FunctionKeyPressed>
    {
        public TerminalFunctionKeyPressedEvent(ITerminalDevice device)
            : base(device, "FunctionKeyPressed")
        {
        }
    }

    public class TerminalOperationalStatusChangedEvent : TerminalDeviceEvent<OperationalStatusChanged>
    {
        public TerminalOperationalStatusChangedEvent(ITerminalDevice device)
            : base(device, "OperationalStatusChanged")
        {
        }
    }

    public class TerminalStatusChangedEvent : TerminalDeviceEvent<TerminalStatusChanged>
    {
        public TerminalStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class TerminalBatteryStatusChangedEvent : TerminalDeviceEvent<TerminalStatusChanged>
    {
        public TerminalBatteryStatusChangedEvent(ITerminalDevice device)
            : base(device, "BatteryStatusChanged")
        {
        }
    }

    public class TerminalFunctionKeyEntryTimedOutEvent : TerminalDeviceEvent<TerminalStatusChanged>
    {
        public TerminalFunctionKeyEntryTimedOutEvent(ITerminalDevice device)
            : base(device, "FunctionKeyEntryTimedOut")
        {
        }
    }

    #endregion
}
