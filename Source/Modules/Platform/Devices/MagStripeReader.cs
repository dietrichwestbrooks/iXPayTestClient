using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class MagStripeReader
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<MagStripeReaderCommand, MagStripeReaderResponse, MagStripeReaderEvent>(
                    "MagStripeReader", new TerminalRequestHandlerByName("Terminal"), typeof(MagStripeReader));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(MagStripeReader));

        public static readonly TerminalDeviceProperty OpenedProperty =
            TerminalDeviceProperty.Register<bool, GetOpenedCommand, GetOpenedResponse>("Opened",
                "Open", typeof(MagStripeReader));

        public static readonly TerminalDeviceProperty CardPositionProperty =
            TerminalDeviceProperty.Register<CardPosition, GetCardPositionCommand, GetCardPositionResponse>("CardPosition",
                "CardPosition", typeof(MagStripeReader));

        public static readonly TerminalDeviceProperty ReaderTypeProperty =
            TerminalDeviceProperty.Register<CardReader, GetReaderTypeCommand, GetReaderTypeResponse>("ReaderType",
                "ReaderType", typeof(MagStripeReader));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod OpenMethod =
         TerminalDeviceMethod.Register<OpenMagStripeReaderCommand, OpenMagStripeReaderResponse>("Open",
             typeof(MagStripeReader), () => new OpenMagStripeReaderCommand
             {
                 Timeout = 30,
                 TimeoutSpecified = true,
             });

        public static readonly TerminalDeviceMethod CloseMethod =
         TerminalDeviceMethod.Register<CloseMagStripeReaderCommand, CloseMagStripeReaderResponse>("Close",
             typeof(MagStripeReader));

        public static readonly TerminalDeviceMethod TurnLightOnMethod =
         TerminalDeviceMethod.Register<TurnLightOnCommand, TurnLightOnResponse>("TurnLightOn",
             typeof(MagStripeReader));

        public static readonly TerminalDeviceMethod FlashLightMethod =
         TerminalDeviceMethod.Register<FlashLightCommand, FlashLightResponse>("FlashLight",
             typeof(MagStripeReader));

        public static readonly TerminalDeviceMethod TurnLightOffMethod =
         TerminalDeviceMethod.Register<TurnLightOffCommand, TurnLightOffResponse>("TurnLightOff",
             typeof(MagStripeReader));

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent OpenChangedEvent =
            TerminalDeviceEvent.Register<OpenChanged>("OpenChanged", typeof(MagStripeReader));

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(MagStripeReader));

        public static readonly TerminalDeviceEvent DataReadEvent =
            TerminalDeviceEvent.Register<MagStripeData>("DataRead", typeof(MagStripeReader));

        public static readonly TerminalDeviceEvent InvalidDataReadEvent =
            TerminalDeviceEvent.Register<MagStripeInvalidData>("InvalidDataRead", typeof(MagStripeReader));

        public static readonly TerminalDeviceEvent CardPositionChangedEvent =
            TerminalDeviceEvent.Register<CardPositionChanged>("CardPositionChanged", typeof(MagStripeReader));

        public static readonly TerminalDeviceEvent TimedOutEvent =
            TerminalDeviceEvent.Register<MagStripeReaderTimedOut>("TimedOut", typeof(MagStripeReader));

        #endregion
    }
}
