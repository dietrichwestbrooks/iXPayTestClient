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
    public class BarcodeReader : TerminalDevice<BarcodeReaderCommand, BarcodeReaderResponse, BarcodeReaderEvent>
    {
        public BarcodeReader()
            : base("BarcodeReader")
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new BarcodeReaderStatusProperty(this),
                    new BarcodeReaderOpenedProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new BarcodeReaderOpenMethod(this),
                    new BarcodeReaderCloseMethod(this),
                    new BarcodeReaderFlashLightMethod(this),
                    new BarcodeReaderTurnLightOnMethod(this),
                    new BarcodeReaderTurnLightOffMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new BarcodeReaderOpenChangedEvent(this),
                    new BarcodeReaderStatusChangedEvent(this),
                    new BarcodeReaderDataReadEvent(this),
                    new BarcodeReaderInvalidDataReadEvent(this),
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
    public class BarcodeReaderStatusProperty : TerminalDeviceProperty<Status,
        GetStatusCommand, GetStatusResponse>
    {
        public BarcodeReaderStatusProperty(ITerminalDevice device)
            : base(device, "Status")
        {
            GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Open")]
    public class BarcodeReaderOpenedProperty : TerminalDeviceProperty<bool,
        GetOpenedCommand, GetOpenedResponse>
    {
        public BarcodeReaderOpenedProperty(ITerminalDevice device)
            : base(device, "Opened")
        {
            GetCommand = new TerminalDeviceCommand<GetOpenedCommand, GetOpenedResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    #endregion

    #region Methods

    public class BarcodeReaderOpenMethod :
    TerminalDeviceMethod<OpenBarcodeReaderCommand, OpenBarcodeReaderResponse>
    {
        public BarcodeReaderOpenMethod(ITerminalDevice device)
            : base(device, "Open")
        {
            InvokeCommand = new TerminalDeviceCommand<OpenBarcodeReaderCommand, OpenBarcodeReaderResponse>(
                this,
                Name
                );
        }
    }

    public class BarcodeReaderCloseMethod :
        TerminalDeviceMethod<CloseBarCodeReaderCommand, CloseBarCodeReaderResponse>
    {
        public BarcodeReaderCloseMethod(ITerminalDevice device)
            : base(device, "Close")
        {
            InvokeCommand = new TerminalDeviceCommand<CloseBarCodeReaderCommand, CloseBarCodeReaderResponse>(
                this,
                Name
                );
        }
    }

    public class BarcodeReaderTurnLightOnMethod :
        TerminalDeviceMethod<TurnLightOnCommand, TurnLightOnResponse>
    {
        public BarcodeReaderTurnLightOnMethod(ITerminalDevice device)
            : base(device, "TurnLightOn")
        {
            InvokeCommand = new TerminalDeviceCommand<TurnLightOnCommand, TurnLightOnResponse>(
                this,
                Name
                );
        }
    }

    public class BarcodeReaderFlashLightMethod :
        TerminalDeviceMethod<FlashLightCommand, FlashLightResponse>
    {
        public BarcodeReaderFlashLightMethod(ITerminalDevice device)
            : base(device, "FlashLight")
        {
            InvokeCommand = new TerminalDeviceCommand<FlashLightCommand, FlashLightResponse>(
                this,
                Name,
                () => new FlashLightCommand
                    {
                        OnTime = 1000,
                        OffTime = 100,
                    }
                );
        }
    }

    public class BarcodeReaderTurnLightOffMethod :
        TerminalDeviceMethod<TurnLightOffCommand, TurnLightOffResponse>
    {
        public BarcodeReaderTurnLightOffMethod(ITerminalDevice device)
            : base(device, "TurnLightOff")
        {
            InvokeCommand = new TerminalDeviceCommand<TurnLightOffCommand, TurnLightOffResponse>(
                this,
                Name
                );
        }
    }

    #endregion

    #region Events

    public class BarcodeReaderOpenChangedEvent : TerminalDeviceEvent<OpenChanged>
    {
        public BarcodeReaderOpenChangedEvent(ITerminalDevice device)
            : base(device, "OpenChanged")
        {
        }
    }

    public class BarcodeReaderStatusChangedEvent : TerminalDeviceEvent<StatusChanged>
    {
        public BarcodeReaderStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class BarcodeReaderDataReadEvent : TerminalDeviceEvent<BarcodeData>
    {
        public BarcodeReaderDataReadEvent(ITerminalDevice device)
            : base(device, "DataRead")
        {
        }
    }

    public class BarcodeReaderInvalidDataReadEvent : TerminalDeviceEvent<BarcodeInvalidData>
    {
        public BarcodeReaderInvalidDataReadEvent(ITerminalDevice device)
            : base(device, "InvalidDataRead")
        {
        }
    }

    #endregion
}
