using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class Printer
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<PrinterCommand, PrinterResponse, PrinterEvent>(
                    "Printer", new TerminalRequestHandlerByName("Terminal"), typeof(Printer));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(Printer));

        public static readonly TerminalDeviceProperty PaperStatusProperty =
            TerminalDeviceProperty.Register<PrinterPaperStatus, GetPaperStatusCommand, GetPaperStatusResponse>("PaperStatus",
                "PrinterPaperStatus", typeof(Printer));

        public static readonly TerminalDeviceProperty CapabilitiesProperty =
            TerminalDeviceProperty.Register<bool, GetCapabilitiesCommand, GetCapabilitiesResponse>("Capabilities",
                "BarCode", typeof(Printer));

        public static readonly TerminalDeviceProperty WidthProperty =
            TerminalDeviceProperty.Register<int, GetWidthCommand, GetWidthResponse>("Width",
                "Width", typeof(Printer));

        public static readonly TerminalDeviceProperty FontsProperty =
            TerminalDeviceProperty.Register<int, GetFontsCommand, GetFontsResponse>("Fonts",
                "Font", typeof(Printer));

        public static readonly TerminalDeviceProperty TypeFacesProperty =
            TerminalDeviceProperty.Register<int, GetTypeFacesCommand, GetTypeFacesResponse>("TypeFaces",
                "TypeFaces", typeof(Printer));

        public static readonly TerminalDeviceProperty SupportedImageTypesProperty =
            TerminalDeviceProperty.Register<int, GetSupportedImagesCommand, GetSupportedImagesResponse>("SupportedImageTypes",
                "ImageType", typeof(Printer));

        public static readonly TerminalDeviceProperty SupportedBarCodeSymbologiesProperty =
            TerminalDeviceProperty.Register<int, GetSupportedBarCodeSymbologiesCommand, GetSupportedBarCodeSymbologiesResponse>(
                    "SupportedBarCodeSymbologies", "Symbology", typeof (Printer));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod PrintMethod =
         TerminalDeviceMethod.Register<PrintCommand, PrintResponse>("Print",
             typeof(Printer));

        public static readonly TerminalDeviceMethod SetMemoryImageMethod =
         TerminalDeviceMethod.Register<SetMemoryImageCommand, SetMemoryImageResponse>("SetMemoryImage",
             typeof(Printer));

        public static readonly TerminalDeviceMethod TurnLightOnMethod =
         TerminalDeviceMethod.Register<TurnLightOnCommand, TurnLightOnResponse>("TurnLightOn",
             typeof(Printer));

        public static readonly TerminalDeviceMethod FlashLightMethod =
         TerminalDeviceMethod.Register<FlashLightCommand, FlashLightResponse>("FlashLight",
             typeof(Printer), () => new FlashLightCommand
             {
                 OnTime = 300,
                 OffTime = 100,
             });

        public static readonly TerminalDeviceMethod TurnLightOffMethod =
         TerminalDeviceMethod.Register<TurnLightOffCommand, TurnLightOffResponse>("TurnLightOff",
             typeof(Printer));

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(Printer));

        public static readonly TerminalDeviceEvent PaperStatusChangedEvent =
            TerminalDeviceEvent.Register<PrinterPaperStatusChanged>("PaperStatusChanged", typeof(Printer));

        #endregion
    }
}
