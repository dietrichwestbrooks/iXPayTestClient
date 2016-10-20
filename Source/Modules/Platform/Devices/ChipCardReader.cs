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
                    new ChipCardStatusProperty(this),
                    new ChipCardOpenedProperty(this),
                    new ChipCardCardPositionProperty(this),
                    new ChipCardReaderTypeProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new OpenChipCardMethod(this),
                    new CloseChipCardMethod(this),
                    new DeactivateChipCardMethod(this),
                    new SoftResetMethod(this),
                    new ProcessApduMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new ChipCardOpenChangedEvent(this),
                    new ChipCardStatusChangedEvent(this),
                    new ChipCardCardPositionChangedEvent(this),
                    new ChipCardDataEvent(this),
                    new ChipCardInvalidDataEvent(this),
                    new ChipCardReaderTimedOutEvent(this),
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
    public class ChipCardStatusProperty : TerminalDeviceProperty<Status,
        GetStatusCommand, GetStatusResponse>
    {
        public ChipCardStatusProperty(ITerminalDevice device)
            : base(device, "Status")
        {
            GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Open")]
    public class ChipCardOpenedProperty : TerminalDeviceProperty<bool,
        GetOpenedCommand, GetOpenedResponse>
    {
        public ChipCardOpenedProperty(ITerminalDevice device)
            : base(device, "Opened")
        {
            GetCommand = new TerminalDeviceCommand<GetOpenedCommand, GetOpenedResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("CardPosition")]
    public class ChipCardCardPositionProperty : TerminalDeviceProperty<CardPosition,
        GetCardPositionCommand, GetCardPositionResponse>
    {
        public ChipCardCardPositionProperty(ITerminalDevice device)
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

    public class OpenChipCardMethod : TerminalDeviceMethod<OpenChipCardReaderCommand, OpenChipCardReaderResponse>
    {
        public OpenChipCardMethod(ITerminalDevice device)
            : base(device, "Open")
        {
            InvokeCommand = new TerminalDeviceCommand<OpenChipCardReaderCommand, OpenChipCardReaderResponse>(
                this,
                Name,
                new SortedList<string, object>
                    {
                        {"allowMagStripeFallback", true},
                        {"allowMagStripeFallbackSpecified", true},
                        {"timeout", 0},
                        {"timeoutSpecified", false},
                    }
                );
        }
    }

    public class CloseChipCardMethod :
        TerminalDeviceMethod<CloseChipCardReaderCommand, CloseChipCardReaderResponse>
    {
        public CloseChipCardMethod(ITerminalDevice device)
            : base(device, "Close")
        {
            InvokeCommand = new TerminalDeviceCommand<CloseChipCardReaderCommand, CloseChipCardReaderResponse>(
                this,
                Name
                );
        }
    }

    public class DeactivateChipCardMethod : TerminalDeviceMethod<DeactivateChipCardCommand, DeactivateChipCardResponse>
    {
        public DeactivateChipCardMethod(ITerminalDevice device)
            : base(device, "Deactivate")
        {
            InvokeCommand = new TerminalDeviceCommand<DeactivateChipCardCommand, DeactivateChipCardResponse>(
                this,
                Name,
                new SortedList<string, object>
                    {
                        {"releaseCard", true},
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
                new SortedList<string, object>
                    {
                        {"capdu", ConvertHelper.ToHexByteArray("00A4040007A0000000041010")},
                    }
                );
        }
    }

    #endregion

    #region Events

    public class ChipCardOpenChangedEvent : TerminalDeviceEvent<OpenChanged>
    {
        public ChipCardOpenChangedEvent(ITerminalDevice device)
            : base(device, "OpenChanged")
        {
        }
    }

    public class ChipCardStatusChangedEvent : TerminalDeviceEvent<StatusChanged>
    {
        public ChipCardStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class ChipCardCardPositionChangedEvent : TerminalDeviceEvent<CardPositionChanged>
    {
        public ChipCardCardPositionChangedEvent(ITerminalDevice device)
            : base(device, "CardPositionChanged")
        {
        }
    }

    public class ChipCardDataEvent : TerminalDeviceEvent<ChipCardData>
    {
        public ChipCardDataEvent(ITerminalDevice device)
            : base(device, "Data")
        {
        }
    }

    public class ChipCardInvalidDataEvent : TerminalDeviceEvent<ChipCardInvalidData>
    {
        public ChipCardInvalidDataEvent(ITerminalDevice device)
            : base(device, "InvalidData")
        {
        }
    }

    public class ChipCardReaderTimedOutEvent : TerminalDeviceEvent<ChipCardReaderTimedOut>
    {
        public ChipCardReaderTimedOutEvent(ITerminalDevice device)
            : base(device, "ReaderTimedOut")
        {
        }
    }

    #endregion
}
