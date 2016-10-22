using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class MagStripeReader : TerminalDevice<MagStripeReaderCommand, MagStripeReaderResponse, MagStripeReaderEvent>
    {
        public MagStripeReader()
            : base("MagStripeReader")
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new MagStripeReaderStatusProperty(this),
                    new MagStripeReaderOpenedProperty(this),
                    new MagStripeReaderCardPositionProperty(this),
                    new MagStripeReaderTypeProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new MagStripeReaderOpenMethod(this),
                    new MagStripeReaderCloseMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new MagStripeReaderOpenChangedEvent(this),
                    new MagStripeReaderStatusChangedEvent(this),
                    new MagStripeReaderCardPositionChangedEvent(this),
                    new MagStripeReaderDataReadEvent(this),
                    new MagStripeReaderInvalidDataReadEvent(this),
                    new MagStripeReaderTimedOutEvent(this),
                });
        }

        public void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalService>();
            Successor = terminal.Devices["Terminal"];
        }
    }

    #region Properties

    [ValueProperty("State")]
    public class MagStripeReaderStatusProperty : TerminalDeviceProperty<Status,
        GetStatusCommand, GetStatusResponse>
    {
        public MagStripeReaderStatusProperty(ITerminalDevice device)
            : base(device, "Status")
        {
            GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Open")]
    public class MagStripeReaderOpenedProperty : TerminalDeviceProperty<bool,
        GetOpenedCommand, GetOpenedResponse>
    {
        public MagStripeReaderOpenedProperty(ITerminalDevice device)
            : base(device, "Opened")
        {
            GetCommand = new TerminalDeviceCommand<GetOpenedCommand, GetOpenedResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("CardPosition")]
    public class MagStripeReaderCardPositionProperty : TerminalDeviceProperty<CardPosition,
        GetCardPositionCommand, GetCardPositionResponse>
    {
        public MagStripeReaderCardPositionProperty(ITerminalDevice device)
            : base(device, "CardPosition")
        {
            GetCommand = new TerminalDeviceCommand<GetCardPositionCommand, GetCardPositionResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("ReaderType")]
    public class MagStripeReaderTypeProperty : TerminalDeviceProperty<CardReader,
        GetReaderTypeCommand, GetReaderTypeResponse>
    {
        public MagStripeReaderTypeProperty(ITerminalDevice device)
            : base(device, "ReaderType")
        {
            GetCommand = new TerminalDeviceCommand<GetReaderTypeCommand, GetReaderTypeResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    #endregion

    #region Methods

    public class MagStripeReaderOpenMethod :
    TerminalDeviceMethod<OpenMagStripeReaderCommand, OpenMagStripeReaderResponse>
    {
        public MagStripeReaderOpenMethod(ITerminalDevice device)
            : base(device, "Open")
        {
            InvokeCommand = new TerminalDeviceCommand<OpenMagStripeReaderCommand, OpenMagStripeReaderResponse>(
                this,
                Name,
                () => new OpenMagStripeReaderCommand
                    {
                        Timeout = 0,
                        TimeoutSpecified = false,
                    }
                );
        }
    }

    public class MagStripeReaderCloseMethod :
        TerminalDeviceMethod<CloseMagStripeReaderCommand, CloseMagStripeReaderResponse>
    {
        public MagStripeReaderCloseMethod(ITerminalDevice device)
            : base(device, "Close")
        {
            InvokeCommand = new TerminalDeviceCommand<CloseMagStripeReaderCommand, CloseMagStripeReaderResponse>(
                this,
                Name
                );
        }
    }

    #endregion

    #region Events

    public class MagStripeReaderOpenChangedEvent : TerminalDeviceEvent<OpenChanged>
    {
        public MagStripeReaderOpenChangedEvent(ITerminalDevice device)
            : base(device, "OpenChanged")
        {
        }
    }

    public class MagStripeReaderStatusChangedEvent : TerminalDeviceEvent<StatusChanged>
    {
        public MagStripeReaderStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class MagStripeReaderCardPositionChangedEvent : TerminalDeviceEvent<CardPositionChanged>
    {
        public MagStripeReaderCardPositionChangedEvent(ITerminalDevice device)
            : base(device, "CardPositionChanged")
        {
        }
    }

    public class MagStripeReaderDataReadEvent : TerminalDeviceEvent<MagStripeData>
    {
        public MagStripeReaderDataReadEvent(ITerminalDevice device)
            : base(device, "DataRead")
        {
        }
    }

    public class MagStripeReaderInvalidDataReadEvent : TerminalDeviceEvent<MagStripeInvalidData>
    {
        public MagStripeReaderInvalidDataReadEvent(ITerminalDevice device)
            : base(device, "InvalidDataRead")
        {
        }
    }

    public class MagStripeReaderTimedOutEvent : TerminalDeviceEvent<MagStripeReaderTimedOut>
    {
        public MagStripeReaderTimedOutEvent(ITerminalDevice device)
            : base(device, "TimedOut")
        {
        }
    }

    #endregion
}
