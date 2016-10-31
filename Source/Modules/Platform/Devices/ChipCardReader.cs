using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class ChipCardReader : TerminalDevice<ChipCardReaderCommand, ChipCardReaderResponse, ChipCardReaderEvent>
    {
        public ChipCardReader()
            : base("ChipCardReader")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new StatusProperty(this),
                    new OpenedProperty(this),
                    new CardPositionProperty(this),
                    new ReaderTypeProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new OpenMethod(this),
                    new CloseMethod(this),
                    new TurnLightOnMethod(this),
                    new FlashLightMethod(this),
                    new TurnLightOffMethod(this),
                    new DeactivateMethod(this),
                    new SoftResetMethod(this),
                    new ProcessApduMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new OpenChangedEvent(this),
                    new StatusChangedEvent(this),
                    new CardPositionChangedEvent(this),
                    new DataReadEvent(this),
                    new InvalidDataReadEvent(this),
                    new ChipCardReaderTimedOutEvent(this),
                });
        }

        #region Device Properties

        [ValueProperty("State")]
        public class StatusProperty : TerminalDeviceProperty<Status,
            GetStatusCommand, GetStatusResponse>
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

        [ValueProperty("Open")]
        public class OpenedProperty : TerminalDeviceProperty<bool,
            GetOpenedCommand, GetOpenedResponse>
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

        [ValueProperty("CardPosition")]
        public class CardPositionProperty : TerminalDeviceProperty<CardPosition,
            GetCardPositionCommand, GetCardPositionResponse>
        {
            public CardPositionProperty(ITerminalDevice device)
                : base(device, "CardPosition")
            {
                GetCommand = new TerminalDeviceCommand<GetCardPositionCommand, GetCardPositionResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("ReaderType")]
        public class ReaderTypeProperty : TerminalDeviceProperty<CardReader,
            GetReaderTypeCommand, GetReaderTypeResponse>
        {
            public ReaderTypeProperty(ITerminalDevice device)
                : base(device, "ReaderType")
            {
                GetCommand = new TerminalDeviceCommand<GetReaderTypeCommand, GetReaderTypeResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        #endregion

        #region Device Methods

        public class OpenMethod : TerminalDeviceMethod<OpenChipCardReaderCommand, OpenChipCardReaderResponse>
        {
            public OpenMethod(ITerminalDevice device)
                : base(device, "Open")
            {
                InvokeCommand = new TerminalDeviceCommand<OpenChipCardReaderCommand, OpenChipCardReaderResponse>(
                    this,
                    Name
                    );
            }
        }

        public class CloseMethod :
            TerminalDeviceMethod<CloseChipCardReaderCommand, CloseChipCardReaderResponse>
        {
            public CloseMethod(ITerminalDevice device)
                : base(device, "Close")
            {
                InvokeCommand = new TerminalDeviceCommand<CloseChipCardReaderCommand, CloseChipCardReaderResponse>(
                    this,
                    Name
                    );
            }
        }


        public class TurnLightOnMethod :
            TerminalDeviceMethod<TurnLightOnCommand, TurnLightOnResponse>
        {
            public TurnLightOnMethod(ITerminalDevice device)
                : base(device, "TurnLightOn")
            {
                InvokeCommand = new TerminalDeviceCommand<TurnLightOnCommand, TurnLightOnResponse>(
                    this,
                    Name
                    );
            }
        }

        public class FlashLightMethod :
            TerminalDeviceMethod<FlashLightCommand, FlashLightResponse>
        {
            public FlashLightMethod(ITerminalDevice device)
                : base(device, "FlashLight")
            {
                InvokeCommand = new TerminalDeviceCommand<FlashLightCommand, FlashLightResponse>(
                    this,
                    Name
                    );
            }
        }

        public class TurnLightOffMethod :
            TerminalDeviceMethod<TurnLightOffCommand, TurnLightOffResponse>
        {
            public TurnLightOffMethod(ITerminalDevice device)
                : base(device, "TurnLightOff")
            {
                InvokeCommand = new TerminalDeviceCommand<TurnLightOffCommand, TurnLightOffResponse>(
                    this,
                    Name
                    );
            }
        }

        public class DeactivateMethod : TerminalDeviceMethod<DeactivateChipCardCommand, DeactivateChipCardResponse>
        {
            public DeactivateMethod(ITerminalDevice device)
                : base(device, "Deactivate")
            {
                InvokeCommand = new TerminalDeviceCommand<DeactivateChipCardCommand, DeactivateChipCardResponse>(
                    this,
                    Name,
                    () => new DeactivateChipCardCommand
                    {
                        ReleaseCard = true,
                    }
                    );
            }
        }

        public class SoftResetMethod : TerminalDeviceMethod<SoftResetChipCardCommand, SoftResetChipCardResponse>
        {
            public SoftResetMethod(ITerminalDevice device)
                : base(device, "SoftReset")
            {
                InvokeCommand = new TerminalDeviceCommand<SoftResetChipCardCommand, SoftResetChipCardResponse>(
                    this,
                    Name
                    );
            }
        }

        public class ProcessApduMethod : TerminalDeviceMethod<ChipCardProcessAPDUCommand, ChipCardProcessAPDUResponse>
        {
            public ProcessApduMethod(ITerminalDevice device)
                : base(device, "ProcessAPDU")
            {
                InvokeCommand = new TerminalDeviceCommand<ChipCardProcessAPDUCommand, ChipCardProcessAPDUResponse>(
                    this,
                    Name,
                    () => new ChipCardProcessAPDUCommand
                    {
                        CAPDU = new byte[] { 0x00, 0xA4, 0x04, 0x00, 0x07, 0xA0, 0x0, 0x0, 0x0, 0x04, 0x10, 0x10 },
                    }
                    );
            }
        }

        #endregion

        #region Device Events

        public class OpenChangedEvent : TerminalDeviceEvent<OpenChanged>
        {
            public OpenChangedEvent(ITerminalDevice device)
                : base(device, "OpenChanged")
            {
            }
        }

        public class StatusChangedEvent : TerminalDeviceEvent<StatusChanged>
        {
            public StatusChangedEvent(ITerminalDevice device)
                : base(device, "StatusChanged")
            {
            }
        }

        public class CardPositionChangedEvent : TerminalDeviceEvent<CardPositionChanged>
        {
            public CardPositionChangedEvent(ITerminalDevice device)
                : base(device, "CardPositionChanged")
            {
            }
        }

        public class DataReadEvent : TerminalDeviceEvent<ChipCardData>
        {
            public DataReadEvent(ITerminalDevice device)
                : base(device, "DataRead")
            {
            }
        }

        public class InvalidDataReadEvent : TerminalDeviceEvent<ChipCardInvalidData>
        {
            public InvalidDataReadEvent(ITerminalDevice device)
                : base(device, "InvalidDataRead")
            {
            }
        }

        public class ChipCardReaderTimedOutEvent : TerminalDeviceEvent<ChipCardReaderTimedOut>
        {
            public ChipCardReaderTimedOutEvent(ITerminalDevice device)
                : base(device, "TimedOut")
            {
            }
        }

        #endregion
    }
}
