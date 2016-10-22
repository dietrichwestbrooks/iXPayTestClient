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
    public class ChipCardReader : TerminalDevice<ChipCardReaderCommand, ChipCardReaderResponse, ChipCardReaderEvent>
    {
        public ChipCardReader()
            : base("ChipCardReader")
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new ChipCardReaderStatusProperty(this),
                    new ChipCardReaderOpenedProperty(this),
                    new ChipCardReaderCardPositionProperty(this),
                    new ChipCardReaderTypeProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new ChipCardReaderOpenMethod(this),
                    new ChipCardReaderCloseMethod(this),
                    new ChipCardReaderDeactivateMethod(this),
                    new ChipCardReaderSoftResetMethod(this),
                    new ChipCardReaderProcessApduMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new ChipCardReaderOpenChangedEvent(this),
                    new ChipCardReaderStatusChangedEvent(this),
                    new ChipCardReaderCardPositionChangedEvent(this),
                    new ChipCardReaderDataReadEvent(this),
                    new ChipCardReaderInvalidDataReadEvent(this),
                    new ChipCardReaderTimedOutEvent(this),
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
    public class ChipCardReaderStatusProperty : TerminalDeviceProperty<Status,
        GetStatusCommand, GetStatusResponse>
    {
        public ChipCardReaderStatusProperty(ITerminalDevice device)
            : base(device, "Status")
        {
            GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Open")]
    public class ChipCardReaderOpenedProperty : TerminalDeviceProperty<bool,
        GetOpenedCommand, GetOpenedResponse>
    {
        public ChipCardReaderOpenedProperty(ITerminalDevice device)
            : base(device, "Opened")
        {
            GetCommand = new TerminalDeviceCommand<GetOpenedCommand, GetOpenedResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("CardPosition")]
    public class ChipCardReaderCardPositionProperty : TerminalDeviceProperty<CardPosition,
        GetCardPositionCommand, GetCardPositionResponse>
    {
        public ChipCardReaderCardPositionProperty(ITerminalDevice device)
            : base(device, "CardPosition")
        {
            GetCommand = new TerminalDeviceCommand<GetCardPositionCommand, GetCardPositionResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("ReaderType")]
    public class ChipCardReaderTypeProperty : TerminalDeviceProperty<CardReader,
        GetReaderTypeCommand, GetReaderTypeResponse>
    {
        public ChipCardReaderTypeProperty(ITerminalDevice device)
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

    public class ChipCardReaderOpenMethod : TerminalDeviceMethod<OpenChipCardReaderCommand, OpenChipCardReaderResponse>
    {
        public ChipCardReaderOpenMethod(ITerminalDevice device)
            : base(device, "Open")
        {
            InvokeCommand = new TerminalDeviceCommand<OpenChipCardReaderCommand, OpenChipCardReaderResponse>(
                this,
                Name,
                () => new OpenChipCardReaderCommand
                    {
                        AllowMagStripeFallback = true,
                        AllowMagStripeFallbackSpecified = true,
                        Timeout = 0,
                        TimeoutSpecified = false,
                    }
                );
        }
    }

    public class ChipCardReaderCloseMethod :
        TerminalDeviceMethod<CloseChipCardReaderCommand, CloseChipCardReaderResponse>
    {
        public ChipCardReaderCloseMethod(ITerminalDevice device)
            : base(device, "Close")
        {
            InvokeCommand = new TerminalDeviceCommand<CloseChipCardReaderCommand, CloseChipCardReaderResponse>(
                this,
                Name
                );
        }
    }

    public class ChipCardReaderDeactivateMethod : TerminalDeviceMethod<DeactivateChipCardCommand, DeactivateChipCardResponse>
    {
        public ChipCardReaderDeactivateMethod(ITerminalDevice device)
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

    public class ChipCardReaderSoftResetMethod : TerminalDeviceMethod<SoftResetChipCardCommand, SoftResetChipCardResponse>
    {
        public ChipCardReaderSoftResetMethod(ITerminalDevice device)
            : base(device, "SoftReset")
        {
            InvokeCommand = new TerminalDeviceCommand<SoftResetChipCardCommand, SoftResetChipCardResponse>(
                this,
                Name
                );
        }
    }

    public class ChipCardReaderProcessApduMethod : TerminalDeviceMethod<ChipCardProcessAPDUCommand, ChipCardProcessAPDUResponse>
    {
        public ChipCardReaderProcessApduMethod(ITerminalDevice device)
            : base(device, "ProcessAPDU")
        {
            InvokeCommand = new TerminalDeviceCommand<ChipCardProcessAPDUCommand, ChipCardProcessAPDUResponse>(
                this,
                Name,
                () => new ChipCardProcessAPDUCommand
                    {
                        CAPDU = ConvertHelper.ToHexByteArray("00A4040007A0000000041010"),
                    }
                );
        }
    }

    #endregion

    #region Events

    public class ChipCardReaderOpenChangedEvent : TerminalDeviceEvent<OpenChanged>
    {
        public ChipCardReaderOpenChangedEvent(ITerminalDevice device)
            : base(device, "OpenChanged")
        {
        }
    }

    public class ChipCardReaderStatusChangedEvent : TerminalDeviceEvent<StatusChanged>
    {
        public ChipCardReaderStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class ChipCardReaderCardPositionChangedEvent : TerminalDeviceEvent<CardPositionChanged>
    {
        public ChipCardReaderCardPositionChangedEvent(ITerminalDevice device)
            : base(device, "CardPositionChanged")
        {
        }
    }

    public class ChipCardReaderDataReadEvent : TerminalDeviceEvent<ChipCardData>
    {
        public ChipCardReaderDataReadEvent(ITerminalDevice device)
            : base(device, "DataRead")
        {
        }
    }

    public class ChipCardReaderInvalidDataReadEvent : TerminalDeviceEvent<ChipCardInvalidData>
    {
        public ChipCardReaderInvalidDataReadEvent(ITerminalDevice device)
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
