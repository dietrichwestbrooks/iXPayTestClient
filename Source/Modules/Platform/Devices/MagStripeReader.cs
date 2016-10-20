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
                    new MagStripeStatusProperty(this),
                    new MagStripeOpenedProperty(this),
                    new MagStripeCardPositionProperty(this),
                    new MagStripeReaderTypeProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new OpenMagStripeMethod(this),
                    new CloseMagStripeMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new MagStripeOpenChangedEvent(this),
                    new MagStripeStatusChangedEvent(this),
                    new MagStripeCardPositionChangedEvent(this),
                    new MagStripeDataEvent(this),
                    new MagStripeInvalidDataEvent(this),
                    new MagStripeReaderTimedOutEvent(this),
                });
        }

        public void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalClientService>();
            Successor = terminal.Devices["Terminal"];
        }
    }

    #region Properties

    [ValueProperty("State")]
    public class MagStripeStatusProperty : TerminalDeviceProperty<Status,
        GetStatusCommand, GetStatusResponse>
    {
        public MagStripeStatusProperty(ITerminalDevice device)
            : base(device, "Status")
        {
            GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Open")]
    public class MagStripeOpenedProperty : TerminalDeviceProperty<bool,
        GetOpenedCommand, GetOpenedResponse>
    {
        public MagStripeOpenedProperty(ITerminalDevice device)
            : base(device, "Opened")
        {
            GetCommand = new TerminalDeviceCommand<GetOpenedCommand, GetOpenedResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("CardPosition")]
    public class MagStripeCardPositionProperty : TerminalDeviceProperty<CardPosition,
        GetCardPositionCommand, GetCardPositionResponse>
    {
        public MagStripeCardPositionProperty(ITerminalDevice device)
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

    public class OpenMagStripeMethod :
    TerminalDeviceMethod<OpenMagStripeReaderCommand, OpenMagStripeReaderResponse>
    {
        public OpenMagStripeMethod(ITerminalDevice device)
            : base(device, "Open")
        {
            InvokeCommand = new TerminalDeviceCommand<OpenMagStripeReaderCommand, OpenMagStripeReaderResponse>(
                this,
                Name,
                new SortedList<string, object>
                    {
                        {"timeout", 0},
                        {"timeoutSpecified", false},
                    }
                );
        }
    }

    public class CloseMagStripeMethod :
        TerminalDeviceMethod<CloseMagStripeReaderCommand, CloseMagStripeReaderResponse>
    {
        public CloseMagStripeMethod(ITerminalDevice device)
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

    public class MagStripeOpenChangedEvent : TerminalDeviceEvent<OpenChanged>
    {
        public MagStripeOpenChangedEvent(ITerminalDevice device)
            : base(device, "OpenChanged")
        {
        }
    }

    public class MagStripeStatusChangedEvent : TerminalDeviceEvent<StatusChanged>
    {
        public MagStripeStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class MagStripeCardPositionChangedEvent : TerminalDeviceEvent<CardPositionChanged>
    {
        public MagStripeCardPositionChangedEvent(ITerminalDevice device)
            : base(device, "CardPositionChanged")
        {
        }
    }

    public class MagStripeDataEvent : TerminalDeviceEvent<MagStripeData>
    {
        public MagStripeDataEvent(ITerminalDevice device)
            : base(device, "Data")
        {
        }
    }

    public class MagStripeInvalidDataEvent : TerminalDeviceEvent<MagStripeInvalidData>
    {
        public MagStripeInvalidDataEvent(ITerminalDevice device)
            : base(device, "InvalidData")
        {
        }
    }

    public class MagStripeReaderTimedOutEvent : TerminalDeviceEvent<MagStripeReaderTimedOut>
    {
        public MagStripeReaderTimedOutEvent(ITerminalDevice device)
            : base(device, "ReaderTimedOut")
        {
        }
    }

    #endregion
}
