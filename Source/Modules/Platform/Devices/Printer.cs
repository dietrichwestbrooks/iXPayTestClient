using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class Printer : TerminalDevice<PrinterCommand, PrinterResponse, PrinterEvent>
    {
        public Printer()
            : base("Printer")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new StatusProperty(this),
                    new PaperStatusProperty(this),
                    new CapabilitiesProperty(this),
                    new WidthProperty(this),
                    new TypeFacesProperty(this),
                    new FontsProperty(this),
                    new SupportedImageTypesProperty(this),
                    new SupportedBarCodeSymbologiesProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new PrintMethod(this),
                    new SetMemoryImageMethod(this),
                    new FlashLightMethod(this),
                    new TurnLightOnMethod(this),
                    new TurnLightOffMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new StatusChangedEvent(this),
                    new PaperStatusChangedEvent(this),
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

        [ValueProperty("PrinterPaperStatus")]
        public class PaperStatusProperty : TerminalDeviceProperty<PrinterPaperStatus,
            GetPaperStatusCommand, GetPaperStatusResponse>
        {
            public PaperStatusProperty(ITerminalDevice device)
                : base(device, "PaperStatus")
            {
                GetCommand = new TerminalDeviceCommand<GetPaperStatusCommand, GetPaperStatusResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("BarCode")]
        public class CapabilitiesProperty : TerminalDeviceProperty<bool,
            GetCapabilitiesCommand, GetCapabilitiesResponse>
        {
            public CapabilitiesProperty(ITerminalDevice device)
                : base(device, "Capabilities")
            {
                GetCommand = new TerminalDeviceCommand<GetCapabilitiesCommand, GetCapabilitiesResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("Width")]
        public class WidthProperty : TerminalDeviceProperty<int, GetWidthCommand, GetWidthResponse>
        {
            public WidthProperty(ITerminalDevice device)
                : base(device, "Width")
            {
                GetCommand = new TerminalDeviceCommand<GetWidthCommand, GetWidthResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("Font")]
        public class FontsProperty : TerminalDeviceProperty<int, GetFontsCommand, GetFontsResponse>
        {
            public FontsProperty(ITerminalDevice device)
                : base(device, "Fonts")
            {
                GetCommand = new TerminalDeviceCommand<GetFontsCommand, GetFontsResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("TypeFaces")]
        public class TypeFacesProperty : TerminalDeviceProperty<int, GetTypeFacesCommand, GetTypeFacesResponse>
        {
            public TypeFacesProperty(ITerminalDevice device)
                : base(device, "TypeFaces")
            {
                GetCommand = new TerminalDeviceCommand<GetTypeFacesCommand, GetTypeFacesResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("ImageType")]
        public class SupportedImageTypesProperty : TerminalDeviceProperty<int, GetSupportedImagesCommand, GetSupportedImagesResponse>
        {
            public SupportedImageTypesProperty(ITerminalDevice device)
                : base(device, "SupportedImageTypes")
            {
                GetCommand = new TerminalDeviceCommand<GetSupportedImagesCommand, GetSupportedImagesResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("Symbology")]
        public class SupportedBarCodeSymbologiesProperty :
            TerminalDeviceProperty<int, GetSupportedBarCodeSymbologiesCommand, GetSupportedBarCodeSymbologiesResponse>
        {
            public SupportedBarCodeSymbologiesProperty(ITerminalDevice device)
                : base(device, "SupportedBarCodeSymbologies")
            {
                GetCommand = new TerminalDeviceCommand<GetSupportedBarCodeSymbologiesCommand, GetSupportedBarCodeSymbologiesResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        #endregion

        #region Device Methods

        public class PrintMethod : TerminalDeviceMethod<PrintCommand, PrintResponse>
        {
            public PrintMethod(ITerminalDevice device)
                : base(device, "Print")
            {
                InvokeCommand = new TerminalDeviceCommand<PrintCommand, PrintResponse>(
                    this,
                    Name
                    );
            }
        }

        public class SetMemoryImageMethod : TerminalDeviceMethod<SetMemoryImageCommand, SetMemoryImageResponse>
        {
            public SetMemoryImageMethod(ITerminalDevice device)
                : base(device, "SetMemoryImage")
            {
                InvokeCommand = new TerminalDeviceCommand<SetMemoryImageCommand, SetMemoryImageResponse>(
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
                    Name,
                    () => new FlashLightCommand
                        {
                            OnTime = 300,
                            OffTime = 100,
                        }
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

        public class StatusChangedEvent : TerminalDeviceEvent<StatusChanged>
        {
            public StatusChangedEvent(ITerminalDevice device)
                : base(device, "StatusChanged")
            {
            }
        }

        public class PaperStatusChangedEvent : TerminalDeviceEvent<PrinterPaperStatusChanged>
        {
            public PaperStatusChangedEvent(ITerminalDevice device)
                : base(device, "PaperStatusChanged")
            {
            }
        }

        #endregion
    }
}
