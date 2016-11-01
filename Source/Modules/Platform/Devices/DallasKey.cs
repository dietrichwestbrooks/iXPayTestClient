using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class DallasKey
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<DallasKeyCommand, DallasKeyResponse, DallasKeyEvent>(
                    "DallasKey", new TerminalRequestHandlerByName("Terminal"), typeof(DallasKey));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(DallasKey));

        public static readonly TerminalDeviceProperty OpenedProperty =
            TerminalDeviceProperty.Register<bool, GetOpenedCommand, GetOpenedResponse>("Opened",
                "Open", typeof(DallasKey));

        public static readonly TerminalDeviceProperty PositionProperty =
            TerminalDeviceProperty.Register<DallasKeyPosition, GetPositionCommand, GetPositionResponse>("Position",
                "Position", typeof(DallasKey));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod OpenMethod =
         TerminalDeviceMethod.Register<OpenDallasKeyReaderCommand, OpenDallasKeyReaderResponse>("Open",
             typeof(DallasKey), () => new OpenDallasKeyReaderCommand
             {
                 MaxReadSize = 96,
                 MaxReadSizeSpecified = true,
             });

        public static readonly TerminalDeviceMethod CloseMethod =
         TerminalDeviceMethod.Register<CloseDallasKeyReaderCommand, CloseDallasKeyReaderResponse>("Close",
             typeof(DallasKey));

        public static readonly TerminalDeviceMethod TurnLightOnMethod =
         TerminalDeviceMethod.Register<TurnLightOnCommand, TurnLightOnResponse>("TurnLightOn",
             typeof(DallasKey));

        public static readonly TerminalDeviceMethod FlashLightMethod =
         TerminalDeviceMethod.Register<FlashLightCommand, FlashLightResponse>("FlashLight",
             typeof(DallasKey));

        public static readonly TerminalDeviceMethod TurnLightOffMethod =
         TerminalDeviceMethod.Register<TurnLightOffCommand, TurnLightOffResponse>("TurnLightOff",
             typeof(DallasKey));

        public static readonly TerminalDeviceMethod ReadDataMethod =
         TerminalDeviceMethod.Register<ReadDallasKeyDataCommand, ReadDallasKeyDataResponse>("ReadData",
             typeof(DallasKey), () => new ReadDallasKeyDataCommand
             {
                 Offset = 96,
                 Length = 16,
             });

        public static readonly TerminalDeviceMethod WriteDataMethod =
         TerminalDeviceMethod.Register<WriteDallasKeyDataCommand, WriteDallasKeyDataResponse>("TurnLightOff",
             typeof(DallasKey), () => new WriteDallasKeyDataCommand
             {
                 ROMData = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0x12, 0x34, 0x56 },
                 DallasKeyWriteData = new[]
                                {
                                    new DallasKeyWriteData
                                        {
                                            Offset = 96,
                                            Data = new byte[] {0xab, 0xcd, 0xef},
                                        },
                                    new DallasKeyWriteData
                                        {
                                            Offset = 96,
                                            Data = new byte[] {0xfe, 0xdc, 0xab},
                                        },
                                }
             });

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent OpenChangedEvent =
            TerminalDeviceEvent.Register<OpenChanged>("OpenChanged", typeof(DallasKey));

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(DallasKey));

        public static readonly TerminalDeviceEvent DataReadEvent =
            TerminalDeviceEvent.Register<DallasKeyData>("DataRead", typeof(DallasKey));

        public static readonly TerminalDeviceEvent InvalidDataReadEvent =
            TerminalDeviceEvent.Register<DallasKeyInvalidData>("InvalidDataRead", typeof(DallasKey));

        public static readonly TerminalDeviceEvent PositionChangedEvent =
            TerminalDeviceEvent.Register<PositionChanged>("PositionChanged", typeof(DallasKey));

        #endregion
    }
}
