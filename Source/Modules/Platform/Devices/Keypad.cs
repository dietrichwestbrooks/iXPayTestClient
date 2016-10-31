using System.Collections.Generic;
using System.Text;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class Keypad : TerminalDevice<KeypadCommand, KeypadResponse, KeypadEvent>
    {
        public Keypad() 
            : base("Keypad")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new StatusProperty(this),
                    new OpenedProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new DisableMethod(this),
                    new EnableFunctionKeysMethod(this),
                    new ValidatePinBlockMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new OpenChangedEvent(this),
                    new StatusChangedEvent(this),
                    new KeyPressedEvent(this),
                    new FunctionKeyPressedEvent(this),
                    new EntryCompleteEvent(this),
                    new PinEntryCompleteEvent(this),
                    new EntryTimedOutEvent(this),
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

        public class EnableFunctionKeysMethod :
        TerminalDeviceMethod<EnableKeypadFunctionKeysCommand, EnableKeypadFunctionKeysResponse>
        {
            public EnableFunctionKeysMethod(ITerminalDevice device)
                : base(device, "EnableFunctionKeys")
            {
                InvokeCommand = new TerminalDeviceCommand
                    <EnableKeypadFunctionKeysCommand, EnableKeypadFunctionKeysResponse>
                    (
                    this,
                    Name,
                    () => new EnableKeypadFunctionKeysCommand
                        {
                            KeypadFunctionKey = new[]
                                {
                                    new KeypadFunctionKey {Function = KeypadFunctionKeyEnum.Num1, ReturnValue = 1},
                                    new KeypadFunctionKey {Function = KeypadFunctionKeyEnum.Num2, ReturnValue = 2},
                                }
                        }
                    );
            }
        }

        public class ValidatePinBlockMethod :
        TerminalDeviceMethod<ValidatePINBlockCommand, ValidatePINBlockResponse>
        {
            public ValidatePinBlockMethod(ITerminalDevice device)
                : base(device, "ValidatePinBlock")
            {
                InvokeCommand = new TerminalDeviceCommand<ValidatePINBlockCommand, ValidatePINBlockResponse>(
                    this,
                    Name,
                    () => new ValidatePINBlockCommand
                        {
                            PINBlock = new ASCIIEncoding().GetBytes("01020304CDFE4F")
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

        public class KeyPressedEvent : TerminalDeviceEvent<KeyPressed>
        {
            public KeyPressedEvent(ITerminalDevice device)
                : base(device, "KeyPressed")
            {
            }
        }

        public class FunctionKeyPressedEvent : TerminalDeviceEvent<FunctionKeyPressed>
        {
            public FunctionKeyPressedEvent(ITerminalDevice device)
                : base(device, "FunctionKeyPressed")
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

        public class EntryTimedOutEvent : TerminalDeviceEvent<KeypadEntryTimedOut>
        {
            public EntryTimedOutEvent(ITerminalDevice device)
                : base(device, "EntryTimedOut")
            {
            }
        }

        #endregion
    }
}
