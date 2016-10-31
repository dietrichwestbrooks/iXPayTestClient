using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class DallasKey : TerminalDevice<DallasKeyCommand, DallasKeyResponse, DallasKeyEvent>
    {
        public DallasKey() 
            : base("DallasKey")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new StatusProperty(this),
                    new OpenedProperty(this),
                    new PositionProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new OpenMethod(this),
                    new CloseMethod(this),
                    new TurnLightOnMethod(this),
                    new TurnLightOffMethod(this),
                    new FlashLightMethod(this),
                    new ReadDataMethod(this),
                    new WriteDataMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new StatusChangedEvent(this),
                    new OpenChangedEvent(this),
                    new PositionChangedEvent(this),
                    new DataReadEvent(this),
                    new InvalidDataReadEvent(this),
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

        [ValueProperty("Position")]
        public class PositionProperty : TerminalDeviceProperty<DallasKeyPosition,
            GetPositionCommand, GetPositionResponse>
        {
            public PositionProperty(ITerminalDevice device)
                : base(device, "Position")
            {
                GetCommand = new TerminalDeviceCommand<GetPositionCommand, GetPositionResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        #endregion

        #region Device Methods

        public class OpenMethod :
        TerminalDeviceMethod<OpenDallasKeyReaderCommand, OpenDallasKeyReaderResponse>
        {
            public OpenMethod(ITerminalDevice device)
                : base(device, "Open")
            {
                InvokeCommand = new TerminalDeviceCommand<OpenDallasKeyReaderCommand, OpenDallasKeyReaderResponse>(
                    this,
                    Name,
                    () => new OpenDallasKeyReaderCommand
                        {
                            MaxReadSize = 96,
                            MaxReadSizeSpecified = true,
                        }
                    );
            }
        }

        public class CloseMethod :
            TerminalDeviceMethod<CloseDallasKeyReaderCommand, CloseDallasKeyReaderResponse>
        {
            public CloseMethod(ITerminalDevice device)
                : base(device, "Close")
            {
                InvokeCommand = new TerminalDeviceCommand<CloseDallasKeyReaderCommand, CloseDallasKeyReaderResponse>(
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

        public class ReadDataMethod :
        TerminalDeviceMethod<ReadDallasKeyDataCommand, ReadDallasKeyDataResponse>
        {
            public ReadDataMethod(ITerminalDevice device)
                : base(device, "ReadData")
            {
                InvokeCommand = new TerminalDeviceCommand<ReadDallasKeyDataCommand, ReadDallasKeyDataResponse>(
                    this,
                    Name,
                    () => new ReadDallasKeyDataCommand
                    {
                        Offset = 96,
                        Length = 16,
                    }
                    );
            }
        }

        public class WriteDataMethod :
        TerminalDeviceMethod<WriteDallasKeyDataCommand, WriteDallasKeyDataResponse>
        {
            public WriteDataMethod(ITerminalDevice device)
                : base(device, "WriteData")
            {
                InvokeCommand = new TerminalDeviceCommand<WriteDallasKeyDataCommand, WriteDallasKeyDataResponse>(
                    this,
                    Name,
                    () => new WriteDallasKeyDataCommand
                        {
                            ROMData = new byte[] {0x12, 0x34, 0x56, 0x78, 0x90, 0x12, 0x34, 0x56},
                            DallasKeyWriteData = new[]
                                {
                                    new DallasKeyWriteData
                                        {
                                            Offset = 96,
                                            Data = new byte[] {0xab, 0xcd, 0xef},
                                        },
                                    new DallasKeyWriteData
                                        {
                                            Offset = 96,
                                            Data = new byte[] {0xfe, 0xdc, 0xab},
                                        },
                                }
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

        public class DataReadEvent : TerminalDeviceEvent<DallasKeyData>
        {
            public DataReadEvent(ITerminalDevice device)
                : base(device, "DataRead")
            {
            }
        }

        public class InvalidDataReadEvent : TerminalDeviceEvent<DallasKeyInvalidData>
        {
            public InvalidDataReadEvent(ITerminalDevice device)
                : base(device, "InvalidDataRead")
            {
            }
        }

        public class PositionChangedEvent : TerminalDeviceEvent<PositionChanged>
        {
            public PositionChangedEvent(ITerminalDevice device)
                : base(device, "PositionChanged")
            {
            }
        }

        #endregion
    }
}
