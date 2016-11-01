using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class BarcodeReader
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<SAMReaderCommand, SAMReaderResponse, SAMReaderEvent>(
                    "BarcodeReader", new TerminalRequestHandlerByName("Terminal"), typeof(BarcodeReader));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(BarcodeReader));

        public static readonly TerminalDeviceProperty OpenedProperty =
            TerminalDeviceProperty.Register<bool, GetOpenedCommand, GetOpenedResponse>("Opened",
                "Open", typeof(BarcodeReader));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod OpenMethod =
         TerminalDeviceMethod.Register<OpenBarcodeReaderCommand, OpenBarcodeReaderResponse>("Open",
             typeof(BarcodeReader));

        public static readonly TerminalDeviceMethod CloseMethod =
         TerminalDeviceMethod.Register<CloseBarCodeReaderCommand, CloseBarCodeReaderResponse>("Close",
             typeof(BarcodeReader));

        public static readonly TerminalDeviceMethod TurnLightOnMethod =
         TerminalDeviceMethod.Register<TurnLightOnCommand, TurnLightOnResponse>("TurnLightOn",
             typeof(BarcodeReader));

        public static readonly TerminalDeviceMethod FlashLightMethod =
         TerminalDeviceMethod.Register<FlashLightCommand, FlashLightResponse>("FlashLight",
             typeof(BarcodeReader));

        public static readonly TerminalDeviceMethod TurnLightOffMethod =
         TerminalDeviceMethod.Register<TurnLightOffCommand, TurnLightOffResponse>("TurnLightOff",
             typeof(BarcodeReader));

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent OpenChangedEvent =
            TerminalDeviceEvent.Register<OpenChanged>("OpenChanged", typeof(BarcodeReader));

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(BarcodeReader));

        public static readonly TerminalDeviceEvent DataReadEvent =
            TerminalDeviceEvent.Register<BarcodeData>("DataRead", typeof(BarcodeReader));

        public static readonly TerminalDeviceEvent InvalidDataReadEvent =
            TerminalDeviceEvent.Register<BarcodeInvalidData>("InvalidDataRead", typeof(BarcodeReader));

        #endregion
    }
}
