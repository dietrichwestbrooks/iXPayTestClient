using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class SamReader
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<SAMReaderCommand, SAMReaderResponse, SAMReaderEvent>(
                    "SAMReader", new TerminalRequestHandlerByName("Terminal"), typeof(SamReader));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(SamReader));

        public static readonly TerminalDeviceProperty AvailableSlotsProperty =
            TerminalDeviceProperty.Register<SAMSlot, GetAvailableSlotsCommand, GetAvailableSlotsResponse>("AvailableSlots",
                "SAMSlot", typeof(SamReader));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod RefreshAvailableSlotsMethod =
            TerminalDeviceMethod.Register<RefreshAvailableSlotsCommand, RefreshAvailableSlotsResponse>("RefreshAvailableSlots",
                typeof(SamReader));

        public static readonly TerminalDeviceMethod SelectSamSlotMethod =
            TerminalDeviceMethod.Register<SelectSAMSlotCommand, SelectSAMSlotResponse>("SelectSAMSlot",
                typeof(SamReader), () => new SelectSAMSlotCommand { SlotID = 4 });

        public static readonly TerminalDeviceMethod ActivateSamMethod =
            TerminalDeviceMethod.Register<ActivateSAMCommand, ActivateSAMResponse>("ActivateSAM",
                typeof(SamReader));

        public static readonly TerminalDeviceMethod DeactivateSamMethod =
            TerminalDeviceMethod.Register<DeactivateSAMCommand, DeactivateSAMResponse>("DeactivateSAM",
                typeof(SamReader));

        public static readonly TerminalDeviceMethod SoftResetSamMethod =
            TerminalDeviceMethod.Register<SoftResetSAMCommand, SoftResetSAMResponse>("SoftResetSAM",
                typeof(SamReader));

        public static readonly TerminalDeviceMethod SamProcessApduMethod =
            TerminalDeviceMethod.Register<SAMProcessAPDUCommand, SAMProcessAPDUResponse>("SAMProcessAPDU",
                typeof(SamReader));

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(SamReader)); 

        #endregion
    }
}
