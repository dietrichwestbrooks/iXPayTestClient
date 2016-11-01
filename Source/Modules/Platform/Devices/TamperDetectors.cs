using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class TamperDetectors
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<TamperDetectorsCommand, TamperDetectorsResponse, TamperDetectorsEvent>(
                    "TamperDetectors", new TerminalRequestHandlerByName("Terminal"), typeof(TamperDetectors));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty AvailableDetectorsProperty =
            TerminalDeviceProperty.Register<TamperDetectors[], GetAvailableDetectorsCommand, GetAvailableDetectorsResponse>(
                "AvailableDetectors", "TamperDetector", typeof(TamperDetectors));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod ConfirmDetectorOpenMethod =
        TerminalDeviceMethod.Register<ConfirmDetectorOpenCommand, ConfirmDetectorOpenResponse>("ConfirmDetectorOpen",
            typeof(TamperDetectors));

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent StatusChangedEvent =
          TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(TamperDetectors));

        #endregion
    }
}
