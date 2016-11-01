using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class ChipCardReader
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<ChipCardReaderCommand, ChipCardReaderResponse, ChipCardReaderEvent>(
                    "ChipCardReader", new TerminalRequestHandlerByName("Terminal"), typeof(ChipCardReader));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(ChipCardReader));

        public static readonly TerminalDeviceProperty OpenedProperty =
            TerminalDeviceProperty.Register<bool, GetOpenedCommand, GetOpenedResponse>("Opened",
                "Open", typeof(ChipCardReader));

        public static readonly TerminalDeviceProperty CardPositionProperty =
            TerminalDeviceProperty.Register<CardPosition, GetCardPositionCommand, GetCardPositionResponse>("CardPosition",
                "CardPosition", typeof(ChipCardReader));

        public static readonly TerminalDeviceProperty ReaderTypeProperty =
            TerminalDeviceProperty.Register<CardReader, GetReaderTypeCommand, GetReaderTypeResponse>("ReaderType",
                "ReaderType", typeof(ChipCardReader));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod OpenMethod =
         TerminalDeviceMethod.Register<OpenChipCardReaderCommand, OpenChipCardReaderResponse>("Open",
             typeof(ChipCardReader));

        public static readonly TerminalDeviceMethod CloseMethod =
         TerminalDeviceMethod.Register<CloseChipCardReaderCommand, CloseChipCardReaderResponse>("Close",
             typeof(ChipCardReader));

        public static readonly TerminalDeviceMethod TurnLightOnMethod =
         TerminalDeviceMethod.Register<TurnLightOnCommand, TurnLightOnResponse>("TurnLightOn",
             typeof(ChipCardReader));

        public static readonly TerminalDeviceMethod FlashLightMethod =
         TerminalDeviceMethod.Register<FlashLightCommand, FlashLightResponse>("FlashLight",
             typeof(ChipCardReader));

        public static readonly TerminalDeviceMethod TurnLightOffMethod =
         TerminalDeviceMethod.Register<TurnLightOffCommand, TurnLightOffResponse>("FlashLight",
             typeof(ChipCardReader));

        public static readonly TerminalDeviceMethod DeactivateMethod =
         TerminalDeviceMethod.Register<DeactivateChipCardCommand, DeactivateChipCardResponse>("Deactivate",
             typeof(ChipCardReader));

        public static readonly TerminalDeviceMethod SoftResetMethod =
         TerminalDeviceMethod.Register<SoftResetChipCardCommand, SoftResetChipCardResponse>("SoftReset",
             typeof(ChipCardReader));

        public static readonly TerminalDeviceMethod ProcessApduMethod =
         TerminalDeviceMethod.Register<ChipCardProcessAPDUCommand, ChipCardProcessAPDUResponse>("ProcessAPDU",
             typeof(ChipCardReader), () => new ChipCardProcessAPDUCommand
             {
                 CAPDU = new byte[] { 0x00, 0xA4, 0x04, 0x00, 0x07, 0xA0, 0x0, 0x0, 0x0, 0x04, 0x10, 0x10 },
             });

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent OpenChangedEvent =
            TerminalDeviceEvent.Register<OpenChanged>("OpenChanged", typeof(ChipCardReader));

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(ChipCardReader));

        public static readonly TerminalDeviceEvent CardPositionChangedEvent =
            TerminalDeviceEvent.Register<CardPositionChanged>("CardPositionChanged", typeof(ChipCardReader));

        public static readonly TerminalDeviceEvent DataReadEvent =
            TerminalDeviceEvent.Register<ChipCardData>("DataRead", typeof(ChipCardReader));

        public static readonly TerminalDeviceEvent InvalidDataReadEvent =
            TerminalDeviceEvent.Register<ChipCardInvalidData>("InvalidDataRead", typeof(ChipCardReader));

        public static readonly TerminalDeviceEvent ChipCardReaderTimedOutEvent =
            TerminalDeviceEvent.Register<ChipCardReaderTimedOut>("TimedOut", typeof(ChipCardReader));

        #endregion
    }
}
