using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class MagStripeReader : TerminalDevice<MagStripeReaderCommand, MagStripeReaderResponse, MagStripeReaderEvent>
    {
        public MagStripeReader()
            : base("MagStripeReader")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new StatusProperty(this),
                    new OpenedProperty(this),
                    new CardPositionProperty(this),
                    new TypeProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new OpenMethod(this),
                    new CloseMethod(this),
                    new TurnLightOnMethod(this),
                    new FlashLightMethod(this),
                    new TurnLightOffMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new OpenChangedEvent(this),
                    new StatusChangedEvent(this),
                    new CardPositionChangedEvent(this),
                    new DataReadEvent(this),
                    new InvalidDataReadEvent(this),
                    new TimedOutEvent(this),
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
        public class TypeProperty : TerminalDeviceProperty<CardReader,
            GetReaderTypeCommand, GetReaderTypeResponse>
        {
            public TypeProperty(ITerminalDevice device)
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

        public class OpenMethod :
        TerminalDeviceMethod<OpenMagStripeReaderCommand, OpenMagStripeReaderResponse>
        {
            public OpenMethod(ITerminalDevice device)
                : base(device, "Open")
            {
                InvokeCommand = new TerminalDeviceCommand<OpenMagStripeReaderCommand, OpenMagStripeReaderResponse>(
                    this,
                    Name,
                    () => new OpenMagStripeReaderCommand
                        {
                            Timeout = 30,
                            TimeoutSpecified = true,
                        }
                    );
            }
        }

        public class CloseMethod :
            TerminalDeviceMethod<CloseMagStripeReaderCommand, CloseMagStripeReaderResponse>
        {
            public CloseMethod(ITerminalDevice device)
                : base(device, "Close")
            {
                InvokeCommand = new TerminalDeviceCommand<CloseMagStripeReaderCommand, CloseMagStripeReaderResponse>(
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

        public class DataReadEvent : TerminalDeviceEvent<MagStripeData>
        {
            public DataReadEvent(ITerminalDevice device)
                : base(device, "DataRead")
            {
            }
        }

        public class InvalidDataReadEvent : TerminalDeviceEvent<MagStripeInvalidData>
        {
            public InvalidDataReadEvent(ITerminalDevice device)
                : base(device, "InvalidDataRead")
            {
            }
        }

        public class TimedOutEvent : TerminalDeviceEvent<MagStripeReaderTimedOut>
        {
            public TimedOutEvent(ITerminalDevice device)
                : base(device, "TimedOut")
            {
            }
        }

        #endregion
    }
}
