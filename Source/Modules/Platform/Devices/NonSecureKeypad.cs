using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class NonSecureKeypad : TerminalDevice<NonSecureKeypadCommand, NonSecureKeypadResponse, NonSecureKeypadEvent>
    {
        public NonSecureKeypad() 
            : base("NonSecureKeypad")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new StatusProperty(this),
                    new OpenedProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new DisableMethod(this),
                    new EnableMethod(this),
                    new EnableForPinMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new OpenChangedEvent(this),
                    new StatusChangedEvent(this),
                    new KeyPressedEvent(this),
                    new EntryCompleteEvent(this),
                    new PinEntryCompleteEvent(this),
                });
        }

        #region Device Properties

        [ValueProperty("State")]
        public class StatusProperty : TerminalDeviceProperty<Status, GetStatusCommand, GetStatusResponse>
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
        public class OpenedProperty : TerminalDeviceProperty<bool, GetOpenedCommand, GetOpenedResponse>
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

        #endregion

        #region Device Methods

        public class DisableMethod :
        TerminalDeviceMethod<DisableKeypadCommand, DisableKeypadResponse>
        {
            public DisableMethod(ITerminalDevice device)
                : base(device, "Disable")
            {
                InvokeCommand = new TerminalDeviceCommand<DisableKeypadCommand, DisableKeypadResponse>(
                    this,
                    Name
                    );
            }
        }

        public class EnableMethod :
        TerminalDeviceMethod<EnableKeypadCommand, EnableKeypadResponse>
        {
            public EnableMethod(ITerminalDevice device)
                : base(device, "Enable")
            {
                InvokeCommand = new TerminalDeviceCommand
                    <EnableKeypadCommand, EnableKeypadResponse>
                    (
                    this,
                    Name,
                    () => new EnableKeypadCommand
                        {
                            Location = new Location {Left = 0, Top = 0},
                            Font = new Font {Name = "Wayne20*24", Size = 0, Style = 0},
                            NonSecureKeypadKeys = new NonSecureKeypadKeys
                                {
                                    NonSecureKeypadKey = new[]
                                        {
                                            new NonSecureKeypadKey
                                                {
                                                    Beep = true,
                                                    EchoCharSpecified = false,
                                                    Location = 0x20,
                                                    Lock = true
                                                },
                                            new NonSecureKeypadKey
                                                {
                                                    Beep = true,
                                                    EchoCharSpecified = false,
                                                    Location = 0x13,
                                                    Lock = true
                                                },
                                            new NonSecureKeypadKey
                                                {
                                                    Beep = true,
                                                    EchoCharSpecified = false,
                                                    Location = 0x23,
                                                    Lock = true
                                                },
                                            new NonSecureKeypadKey
                                                {
                                                    Beep = true,
                                                    EchoCharSpecified = false,
                                                    Location = 0x33,
                                                    Lock = true
                                                },
                                        }
                                }
                        }
                    );
            }
        }

        public class EnableForPinMethod :
        TerminalDeviceMethod<EnableKeypadForPINCommand, EnableKeypadForPINResponse>
        {
            public EnableForPinMethod(ITerminalDevice device)
                : base(device, "EnableForPIN")
            {
                InvokeCommand = new TerminalDeviceCommand<EnableKeypadForPINCommand, EnableKeypadForPINResponse>(
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

        public class KeyPressedEvent : TerminalDeviceEvent<KeyPressed>
        {
            public KeyPressedEvent(ITerminalDevice device)
                : base(device, "KeyPressed")
            {
            }
        }

        public class EntryCompleteEvent : TerminalDeviceEvent<EntryComplete>
        {
            public EntryCompleteEvent(ITerminalDevice device)
                : base(device, "EntryComplete")
            {
            }
        }

        public class PinEntryCompleteEvent : TerminalDeviceEvent<PINEntryComplete>
        {
            public PinEntryCompleteEvent(ITerminalDevice device)
                : base(device, "PinEntryComplete")
            {
            }
        }

        #endregion
    }
}
