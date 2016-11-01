using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class BillAcceptor
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<BillAcceptorCommand, BillAcceptorResponse, BillAcceptorEvent>(
                    "BillAcceptor", new TerminalRequestHandlerByName("Terminal"), typeof(BillAcceptor));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty AcceptorIdProperty =
            TerminalDeviceProperty.Register<string, GetAcceptorIdCommand, GetAcceptorIdResponse>("AcceptorId",
                "AcceptorId", typeof(BillAcceptor));

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(BillAcceptor));

        public static readonly TerminalDeviceProperty BankNoteStatusProperty =
            TerminalDeviceProperty.Register<BankNoteStatus, GetBankNoteStatusCommand, GetBankNoteStatusResponse>("BankNoteStatus",
                "BankNoteState", typeof(BillAcceptor));

        public static readonly TerminalDeviceProperty SafeboxStatusProperty =
            TerminalDeviceProperty.Register<SafeboxStatus, GetSafeboxStatusCommand, GetSafeboxStatusResponse>("SafeboxStatus",
                "SafeboxState", typeof(BillAcceptor));

        public static readonly TerminalDeviceProperty OpenedProperty =
            TerminalDeviceProperty.Register<bool, GetOpenedCommand, GetOpenedResponse>("Opened",
                "Open", typeof(BillAcceptor));

        public static readonly TerminalDeviceProperty CurrentNoteProperty =
            TerminalDeviceProperty.Register<BankNoteValue, GetCurrentNoteCommand, GetCurrentNoteResponse>("CurrentNote",
                "BankNoteValue", typeof(BillAcceptor));

        public static readonly TerminalDeviceProperty CapabilitiesProperty =
            TerminalDeviceProperty.Register<bool, GetBillAcceptorCapabilitiesCommand, GetBillAcceptorCapabilitiesResponse>("Capabilities",
                "CapSafebox", typeof(BillAcceptor));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod OpenMethod =
         TerminalDeviceMethod.Register<OpenBillAcceptorCommand, OpenBillAcceptorResponse>("Open",
             typeof(BillAcceptor));

        public static readonly TerminalDeviceMethod CloseMethod =
         TerminalDeviceMethod.Register<CloseBillAcceptorCommand, CloseBillAcceptorResponse>("Close",
             typeof(BillAcceptor));

        public static readonly TerminalDeviceMethod CollectMethod =
         TerminalDeviceMethod.Register<CollectCommand, CollectResponse>("Collect",
             typeof(BillAcceptor));

        public static readonly TerminalDeviceMethod EjectMethod =
         TerminalDeviceMethod.Register<EjectCommand, EjectResponse>("Eject",
             typeof(BillAcceptor));

        public static readonly TerminalDeviceMethod NoteStateConfirmMethod =
         TerminalDeviceMethod.Register<NoteStateConfirmCommand, NoteStateConfirmResponse>("NoteStateConfirm",
             typeof(BillAcceptor));

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent StatusChangedEvent =
         TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(BillAcceptor));

        public static readonly TerminalDeviceEvent OpenChangedEvent =
         TerminalDeviceEvent.Register<OpenChanged>("OpenChanged", typeof(BillAcceptor));

        public static readonly TerminalDeviceEvent AcceptorStatusChangedEvent =
         TerminalDeviceEvent.Register<AcceptorStatusChanged>("AcceptorStatusChanged", typeof(BillAcceptor));

        public static readonly TerminalDeviceEvent BankNoteStatusChangedEvent =
         TerminalDeviceEvent.Register<BankNoteStatusChanged>("BankNoteStatusChanged", typeof(BillAcceptor));

        public static readonly TerminalDeviceEvent StatusBytesChangedEvent =
         TerminalDeviceEvent.Register<BillAcceptorStatusBytesChanged>("StatusBytesChanged", typeof(BillAcceptor));

        public static readonly TerminalDeviceEvent SafeboxStatusChangedEvent =
         TerminalDeviceEvent.Register<SafeboxStatusChanged>("SafeboxStatusChanged", typeof(BillAcceptor));

        public static readonly TerminalDeviceEvent StackerStatusChangedEvent =
         TerminalDeviceEvent.Register<StackerStatusChanged>("StackerStatusChanged", typeof(BillAcceptor));

        #endregion
    }
}
