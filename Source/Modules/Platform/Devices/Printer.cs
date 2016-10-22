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
    public class Printer : TerminalDevice<PrinterCommand, PrinterResponse, PrinterEvent>
    {
        public Printer()
            : base("Printer")
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new PrinterStatusProperty(this),
                    new PrinterPaperStatusProperty(this),
                    new PrinterCapabilitiesProperty(this),
                    new PrinterWidthProperty(this),
                    new PrinterTypeFacesProperty(this),
                    new PrinterFontsProperty(this),
                    new PrinterSupportedImageTypesProperty(this),
                    new PrinterSupportedBarCodeSymbologiesProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new PrinterPrintMethod(this),
                    new PrinterSetMemoryImageMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new PrinterStatusChangedEvent(this),
                    new PrinterPaperStatusChangedEvent(this),
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
    public class PrinterStatusProperty : TerminalDeviceProperty<Status,
        GetStatusCommand, GetStatusResponse>
    {
        public PrinterStatusProperty(ITerminalDevice device)
            : base(device, "Status")
        {
            GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("PrinterPaperStatus")]
    public class PrinterPaperStatusProperty : TerminalDeviceProperty<PrinterPaperStatus,
        GetPaperStatusCommand, GetPaperStatusResponse>
    {
        public PrinterPaperStatusProperty(ITerminalDevice device)
            : base(device, "PaperStatus")
        {
            GetCommand = new TerminalDeviceCommand<GetPaperStatusCommand, GetPaperStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("BarCode")]
    public class PrinterCapabilitiesProperty : TerminalDeviceProperty<bool,
        GetCapabilitiesCommand, GetCapabilitiesResponse>
    {
        public PrinterCapabilitiesProperty(ITerminalDevice device)
            : base(device, "Capabilities")
        {
            GetCommand = new TerminalDeviceCommand<GetCapabilitiesCommand, GetCapabilitiesResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Width")]
    public class PrinterWidthProperty : TerminalDeviceProperty<int, GetWidthCommand, GetWidthResponse>
    {
        public PrinterWidthProperty(ITerminalDevice device)
            : base(device, "Width")
        {
            GetCommand = new TerminalDeviceCommand<GetWidthCommand, GetWidthResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Font")]
    public class PrinterFontsProperty : TerminalDeviceProperty<int, GetFontsCommand, GetFontsResponse>
    {
        public PrinterFontsProperty(ITerminalDevice device)
            : base(device, "Fonts")
        {
            GetCommand = new TerminalDeviceCommand<GetFontsCommand, GetFontsResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("TypeFaces")]
    public class PrinterTypeFacesProperty : TerminalDeviceProperty<int, GetTypeFacesCommand, GetTypeFacesResponse>
    {
        public PrinterTypeFacesProperty(ITerminalDevice device)
            : base(device, "TypeFaces")
        {
            GetCommand = new TerminalDeviceCommand<GetTypeFacesCommand, GetTypeFacesResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("ImageType")]
    public class PrinterSupportedImageTypesProperty : TerminalDeviceProperty<int, GetSupportedImagesCommand, GetSupportedImagesResponse>
    {
        public PrinterSupportedImageTypesProperty(ITerminalDevice device)
            : base(device, "SupportedImageTypes")
        {
            GetCommand = new TerminalDeviceCommand<GetSupportedImagesCommand, GetSupportedImagesResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Symbology")]
    public class PrinterSupportedBarCodeSymbologiesProperty : 
        TerminalDeviceProperty<int, GetSupportedBarCodeSymbologiesCommand, GetSupportedBarCodeSymbologiesResponse>
    {
        public PrinterSupportedBarCodeSymbologiesProperty(ITerminalDevice device)
            : base(device, "SupportedBarCodeSymbologies")
        {
            GetCommand = new TerminalDeviceCommand<GetSupportedBarCodeSymbologiesCommand, GetSupportedBarCodeSymbologiesResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    #endregion

    #region Methods

    public class PrinterPrintMethod : TerminalDeviceMethod<PrintCommand, PrintResponse>
    {
        public PrinterPrintMethod(ITerminalDevice device)
            : base(device, "Print")
        {
            InvokeCommand = new TerminalDeviceCommand<PrintCommand, PrintResponse>(
                this,
                Name,
                () => new PrintCommand
                    {
                        Items = new object[]
                            {
                                new PrintCommandSetTypeFace
                                    {
                                        TypeFace = new[]
                                            {
                                                new PrintCommandSetTypeFaceTypeFace
                                                    {
                                                        Id = TypeFaceEnum.Bold,
                                                        Value = true
                                                    },
                                                new PrintCommandSetTypeFaceTypeFace
                                                    {
                                                        Id = TypeFaceEnum.Underline,
                                                        Value = false
                                                    },
                                            }
                                    },
                                new PrintCommandSetFont
                                    {
                                        Font = new PrintCommandSetFontFont {FontName = "Courier"}
                                    },
                                new PrintCommandPrintText
                                    {
                                        Justification = HorizontalJustificationsEnum.Left,
                                        LineFeed = true,
                                        Text = new PrintCommandPrintTextText
                                            {
                                                Text = new[] {"Document text goes here"}
                                            }
                                    },
                                new PrintCommandPrintBarCode
                                    {
                                        Characters = "045235401239",
                                        Symbology = BarCodeSymbologiesEnum.UPC_A,
                                        Justification = HorizontalJustificationsEnum.Center,
                                        JustificationSpecified = true,
                                        TextPosition = PrintCommandPrintBarCodeTextPosition.NoText,
                                        TextPositionSpecified = true,
                                        Width = 300,
                                        WidthSpecified = true,
                                        Height = 40,
                                        HeightSpecified = true,
                                    },
                                new PrintCommandPrintDownloadedImage
                                    {
                                        Id = 1,
                                        Justification = HorizontalJustificationsEnum.Center,
                                    },
                                new PrintCommandPrintImage
                                    {
                                        FileName = "..\\ImageLogo.bmp",
                                        Justification = HorizontalJustificationsEnum.Center,
                                    },
                                new PrintCommandCutPaper
                                    {
                                        FullCut = true,
                                        FullCutSpecified = true,
                                        Eject = false,
                                        EjectSpecified = true,
                                    }
                            }
                    }
                );
        }
    }

    public class PrinterSetMemoryImageMethod : TerminalDeviceMethod<SetMemoryImageCommand, SetMemoryImageResponse>
    {
        public PrinterSetMemoryImageMethod(ITerminalDevice device)
            : base(device, "SetMemoryImage")
        {
            InvokeCommand = new TerminalDeviceCommand<SetMemoryImageCommand, SetMemoryImageResponse>(
                this,
                Name,
                () => new SetMemoryImageCommand
                    {
                        Id = 1,
                        FileName = "..\\Image\\Logo.bmp",
                    }
                );
        }
    }

    #endregion

    #region Events

    public class PrinterStatusChangedEvent : TerminalDeviceEvent<StatusChanged>
    {
        public PrinterStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class PrinterPaperStatusChangedEvent : TerminalDeviceEvent<PrinterPaperStatusChanged>
    {
        public PrinterPaperStatusChangedEvent(ITerminalDevice device)
            : base(device, "PaperStatusChanged")
        {
        }
    }

    #endregion
}
