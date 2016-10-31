using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class BillAcceptor : TerminalDevice<BillAcceptorCommand, BillAcceptorResponse, BillAcceptorEvent>
    {
        public BillAcceptor() 
            : base("BillAcceptor")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new AcceptorIdProperty(this),
                    new StatusProperty(this),
                    new BankNoteStatusProperty(this),
                    new SafeboxStatusProperty(this),
                    new OpenedProperty(this),
                    new CurrentNoteProperty(this),
                    new CapabilitiesProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new OpenMethod(this),
                    new CloseMethod(this),
                    new CollectMethod(this),
                    new EjectMethod(this),
                    new NoteStateConfirmMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new StatusChangedEvent(this),
                    new OpenChangedEvent(this),
                    new AcceptorStatusChangedEvent(this),
                    new BankNoteStatusChangedEvent(this),
                    new StatusBytesChangedEvent(this),
                    new SafeboxStatusChangedEvent(this),
                    new StackerStatusChangedEvent(this),
                });
        }

        #region Device Properties

        [ValueProperty("AcceptorId")]
        public class AcceptorIdProperty :
            TerminalDeviceProperty<string, GetAcceptorIdCommand, GetAcceptorIdResponse>
        {
            public AcceptorIdProperty(ITerminalDevice device)
                : base(device, "AcceptorId")
            {
                GetCommand = new TerminalDeviceCommand<GetAcceptorIdCommand, GetAcceptorIdResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("State")]
        public class StatusProperty :
            TerminalDeviceProperty<Status, GetStatusCommand, GetStatusResponse>
        {
            public StatusProperty(ITerminalDevice device)
                : base(device, "Status")
            {
                GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("BankNoteState")]
        public class BankNoteStatusProperty :
            TerminalDeviceProperty<BankNoteStatus, GetBankNoteStatusCommand, GetBankNoteStatusResponse>
        {
            public BankNoteStatusProperty(ITerminalDevice device)
                : base(device, "BankNoteStatus")
            {
                GetCommand = new TerminalDeviceCommand<GetBankNoteStatusCommand, GetBankNoteStatusResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("SafeboxState")]
        public class SafeboxStatusProperty :
            TerminalDeviceProperty<SafeboxStatus, GetSafeboxStatusCommand, GetSafeboxStatusResponse>
        {
            public SafeboxStatusProperty(ITerminalDevice device)
                : base(device, "SafeboxStatus")
            {
                GetCommand = new TerminalDeviceCommand<GetSafeboxStatusCommand, GetSafeboxStatusResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("Open")]
        public class OpenedProperty : TerminalDeviceProperty<bool, GetOpenedCommand, GetOpenedResponse>
        {
            public OpenedProperty(ITerminalDevice device)
                : base(device, "Opened")
            {
                GetCommand = new TerminalDeviceCommand<GetOpenedCommand, GetOpenedResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("BankNoteValue")]
        public class CurrentNoteProperty : TerminalDeviceProperty<BankNoteValue, GetCurrentNoteCommand, GetCurrentNoteResponse>
        {
            public CurrentNoteProperty(ITerminalDevice device)
                : base(device, "CurrentNote")
            {
                GetCommand = new TerminalDeviceCommand<GetCurrentNoteCommand, GetCurrentNoteResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("CapSafebox")]
        public class CapabilitiesProperty : 
            TerminalDeviceProperty<bool, GetBillAcceptorCapabilitiesCommand, GetBillAcceptorCapabilitiesResponse>
        {
            public CapabilitiesProperty(ITerminalDevice device)
                : base(device, "Capabilities")
            {
                GetCommand = new TerminalDeviceCommand
                    <GetBillAcceptorCapabilitiesCommand, GetBillAcceptorCapabilitiesResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        #endregion

        #region Device Methods

        public class OpenMethod :
            TerminalDeviceMethod<OpenBillAcceptorCommand, OpenBillAcceptorResponse>
        {
            public OpenMethod(ITerminalDevice device)
                : base(device, "Open")
            {
                InvokeCommand = new TerminalDeviceCommand<OpenBillAcceptorCommand, OpenBillAcceptorResponse>(
                    this,
                    Name
                    );
            }
        }

        public class CloseMethod :
            TerminalDeviceMethod<CloseBillAcceptorCommand, CloseBillAcceptorResponse>
        {
            public CloseMethod(ITerminalDevice device)
                : base(device, "Close")
            {
                InvokeCommand = new TerminalDeviceCommand<CloseBillAcceptorCommand, CloseBillAcceptorResponse>(
                    this,
                    Name
                    );
            }
        }

        public class CollectMethod :
            TerminalDeviceMethod<CollectCommand, CollectResponse>
        {
            public CollectMethod(ITerminalDevice device)
                : base(device, "Collect")
            {
                InvokeCommand = new TerminalDeviceCommand<CollectCommand, CollectResponse>(
                    this,
                    Name
                    );
            }
        }

        public class EjectMethod :
            TerminalDeviceMethod<EjectCommand, EjectResponse>
        {
            public EjectMethod(ITerminalDevice device)
                : base(device, "Eject")
            {
                InvokeCommand = new TerminalDeviceCommand<EjectCommand, EjectResponse>(
                    this,
                    Name
                    );
            }
        }

        public class NoteStateConfirmMethod :
            TerminalDeviceMethod<NoteStateConfirmCommand, NoteStateConfirmResponse>
        {
            public NoteStateConfirmMethod(ITerminalDevice device)
                : base(device, "NoteStateConfirm")
            {
                InvokeCommand = new TerminalDeviceCommand<NoteStateConfirmCommand, NoteStateConfirmResponse>(
                    this,
                    Name
                    );
            }
        }

        #endregion

        #region Device Events

        public class StatusChangedEvent : TerminalDeviceEvent<StatusChanged>
        {
            public StatusChangedEvent(ITerminalDevice device)
                : base(device, "StatusChanged")
            {
            }
        }

        public class OpenChangedEvent : TerminalDeviceEvent<OpenChanged>
        {
            public OpenChangedEvent(ITerminalDevice device)
                : base(device, "OpenChanged")
            {
            }
        }

        public class AcceptorStatusChangedEvent : TerminalDeviceEvent<AcceptorStatusChanged>
        {
            public AcceptorStatusChangedEvent(ITerminalDevice device)
                : base(device, "AcceptorStatusChanged")
            {
            }
        }

        public class BankNoteStatusChangedEvent : TerminalDeviceEvent<BankNoteStatusChanged>
        {
            public BankNoteStatusChangedEvent(ITerminalDevice device)
                : base(device, "BankNoteStatusChanged")
            {
            }
        }

        public class StatusBytesChangedEvent : TerminalDeviceEvent<BillAcceptorStatusBytesChanged>
        {
            public StatusBytesChangedEvent(ITerminalDevice device)
                : base(device, "StatusBytesChanged")
            {
            }
        }

        public class SafeboxStatusChangedEvent : TerminalDeviceEvent<SafeboxStatusChanged>
        {
            public SafeboxStatusChangedEvent(ITerminalDevice device)
                : base(device, "SafeboxStatusChanged")
            {
            }
        }

        public class StackerStatusChangedEvent : TerminalDeviceEvent<StackerStatusChanged>
        {
            public StackerStatusChangedEvent(ITerminalDevice device)
                : base(device, "StackerStatusChanged")
            {
            }
        }

        #endregion
    }
}
