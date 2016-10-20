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
                    new BarcodeStatusProperty(this),
                    new BarcodeOpenedProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new OpenBarcodeMethod(this),
                    new CloseBarcodeMethod(this),
                    new FlashLightMethod(this),
                    new TurnBarcodeLightOnMethod(this),
                    new TurnBarcodeLightOffMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new BarcodeOpenChangedEvent(this),
                    new BarcodeStatusChangedEvent(this),
                    new BarcodeDataEvent(this),
                    new BarcodeInvalidDataEvent(this),
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
    public class BarcodeStatusProperty : TerminalDeviceProperty<Status,
        GetStatusCommand, GetStatusResponse>
    {
        public BarcodeStatusProperty(ITerminalDevice device)
            : base(device, "Status")
        {
            GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Open")]
    public class BarcodeOpenedProperty : TerminalDeviceProperty<bool,
        GetOpenedCommand, GetOpenedResponse>
    {
        public BarcodeOpenedProperty(ITerminalDevice device)
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

    public class OpenBarcodeMethod :
    TerminalDeviceMethod<OpenBarcodeReaderCommand, OpenBarcodeReaderResponse>
    {
        public OpenBarcodeMethod(ITerminalDevice device)
            : base(device, "Open")
        {
            InvokeCommand = new TerminalDeviceCommand<OpenBarcodeReaderCommand, OpenBarcodeReaderResponse>(
                this,
                Name
                );
        }
    }

    public class CloseBarcodeMethod :
        TerminalDeviceMethod<CloseBarCodeReaderCommand, CloseBarCodeReaderResponse>
    {
        public CloseBarcodeMethod(ITerminalDevice device)
            : base(device, "Close")
        {
            InvokeCommand = new TerminalDeviceCommand<CloseBarCodeReaderCommand, CloseBarCodeReaderResponse>(
                this,
                Name
                );
        }
    }

    public class TurnBarcodeLightOnMethod :
        TerminalDeviceMethod<TurnLightOnCommand, TurnLightOnResponse>
    {
        public TurnBarcodeLightOnMethod(ITerminalDevice device)
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
                Name,
                new SortedList<string, object>
                    {
                        {"onTime", 1000},
                        {"offTime", 100},
                    }
                );
        }
    }

    public class TurnBarcodeLightOffMethod :
        TerminalDeviceMethod<TurnLightOffCommand, TurnLightOffResponse>
    {
        public TurnBarcodeLightOffMethod(ITerminalDevice device)
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

    public class BarcodeOpenChangedEvent : TerminalDeviceEvent<OpenChanged>
    {
        public BarcodeOpenChangedEvent(ITerminalDevice device)
            : base(device, "OpenChanged")
        {
        }
    }

    public class BarcodeStatusChangedEvent : TerminalDeviceEvent<StatusChanged>
    {
        public BarcodeStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class BarcodeDataEvent : TerminalDeviceEvent<BarcodeData>
    {
        public BarcodeDataEvent(ITerminalDevice device)
            : base(device, "Data")
        {
        }
    }

    public class BarcodeInvalidDataEvent : TerminalDeviceEvent<BarcodeInvalidData>
    {
        public BarcodeInvalidDataEvent(ITerminalDevice device)
            : base(device, "InvalidData")
        {
        }
    }

    #endregion
}
