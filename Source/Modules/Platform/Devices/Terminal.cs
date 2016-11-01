using System;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class Terminal
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<TerminalCommand, TerminalResponse, TerminalEvent>(
                    "Terminal", typeof(Terminal));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty TerminalIdProperty =
            TerminalDeviceProperty.Register<int, GetTerminalIdCommand, GetTerminalIdResponse>("TerminalId",
                "TerminalId", typeof(Terminal));

        public static readonly TerminalDeviceProperty SupportedTerminalIdsProperty =
            TerminalDeviceProperty.Register<SupportedTerminal[],
            GetSupportedTerminalIdsCommand, GetSupportedTerminalIdsResponse>("SupportedTerminalIds",
                "SupportedTerminal", typeof(Terminal));

        public static readonly TerminalDeviceProperty PackageVersionProperty =
            TerminalDeviceProperty.Register<string, GetPackageVersionCommand, GetPackageVersionResponse>("PackageVersion",
                "PackageVersion", typeof(Terminal));

        public static readonly TerminalDeviceProperty OperationalStateProperty =
            TerminalDeviceProperty.Register<OperationalStatus,
            GetOperationalStateCommand, GetOperationalStateResponse>("OperationalState",
                "OperationalState", typeof(Terminal));

        public static readonly TerminalDeviceProperty TerminalStateProperty =
            TerminalDeviceProperty.Register<TerminalStatus, GetTerminalStateCommand, GetTerminalStateResponse>("TerminalState",
                "TerminalState", typeof(Terminal));

        public static readonly TerminalDeviceProperty BatteryStateProperty =
            TerminalDeviceProperty.Register<BatteryStatus, GetBatteryStateCommand, GetBatteryStateResponse>("BatteryState",
                "BatteryState", typeof(Terminal));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod OpenMethod =
            TerminalDeviceMethod.Register<DisplaySecurePromptCommand, DisplaySecurePromptResponse>(
                "DisplaySecurePrompt", 
                typeof (Terminal), () => new DisplaySecurePromptCommand
                    {
                        Language = "en",
                        SecurityLevel = SecurityLevels.Unencrypted,
                        EntryType = EntryType.AlphaNumeric,
                    });

        public static readonly TerminalDeviceMethod ResetMethod =
            TerminalDeviceMethod.Register<ResetCommand, ResetResponse>("Reset",
                typeof(Terminal));

        public static readonly TerminalDeviceMethod SetDateTimeMethod =
            TerminalDeviceMethod.Register<SetDateTimeCommand, SetDateTimeResponse>("SetDateTime",
                typeof(Terminal), () => new SetDateTimeCommand
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
                });

        public static readonly TerminalDeviceMethod SetToIdleMethod =
         TerminalDeviceMethod.Register<SetToIdleCommand, SetToIdleResponse>("SetToIdle",
             typeof(Terminal));

        public static readonly TerminalDeviceMethod EnableFunctionKeysMethod =
         TerminalDeviceMethod.Register<EnableFunctionKeysCommand, EnableFunctionKeysResponse>("EnableFunctionKeys",
             typeof(Terminal), () => new EnableFunctionKeysCommand
             {
                 FunctionKey = new[]
                                {
                                    new FunctionKey {Function = 1, Location = 1, LocationSpecified = true, Text = "Credit"},
                                    new FunctionKey {Function = 2, Location = 5, LocationSpecified = true, Text = "Debit"},
                                    new FunctionKey {Function = 3, Location = 4, LocationSpecified = true, Text = "Cancel"},
                                }
             });

        public static readonly TerminalDeviceMethod DisableFunctionKeysMethod =
            TerminalDeviceMethod.Register<DisableFunctionKeysCommand, DisableFunctionKeysResponse>(
                "DisableFunctionKeys", typeof(Terminal));

        public static readonly TerminalDeviceMethod EnableCancelKeyMethod =
            TerminalDeviceMethod.Register<EnableCancelKeyCommand, EnableCancelKeyResponse>(
                "EnableCancelKey", typeof(Terminal));

        public static readonly TerminalDeviceMethod DisableCancelKeyMethod =
            TerminalDeviceMethod.Register<DisableCancelKeyCommand, DisableCancelKeyResponse>(
                "DisableCancelKey", typeof(Terminal));

        public static readonly TerminalDeviceMethod PassThroughMethod =
            TerminalDeviceMethod.Register<PassThroughCommand, PassThroughResponse>(
                "PassThrough", typeof (Terminal),
                () => new PassThroughCommand {Value = new byte[] {0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0}});

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent CancelKeyPressedEvent =
            TerminalDeviceEvent.Register<CancelKeyPressed>("CancelKeyPressed", typeof(Terminal));

        public static readonly TerminalDeviceEvent FunctionKeyPressedEvent =
            TerminalDeviceEvent.Register<FunctionKeyPressed>("FunctionKeyPressed", typeof(Terminal));

        public static readonly TerminalDeviceEvent OperationalStatusChangedEvent =
            TerminalDeviceEvent.Register<OperationalStatusChanged>("OperationalStatusChanged", typeof(Terminal));

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<TerminalStatusChanged>("StatusChanged", typeof(Terminal));

        public static readonly TerminalDeviceEvent BatteryStatusChangedEvent =
            TerminalDeviceEvent.Register<TerminalStatusChanged>("BatteryStatusChanged", typeof(Terminal));

        public static readonly TerminalDeviceEvent FunctionKeyEntryTimedOutEvent =
            TerminalDeviceEvent.Register<TerminalStatusChanged>("FunctionKeyEntryTimedOut", typeof(Terminal));

        #endregion
    }
}
