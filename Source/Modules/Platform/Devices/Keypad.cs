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
    public class Keypad : TerminalDevice<KeypadCommand, KeypadResponse, KeypadEvent>
    {
        public Keypad() 
            : base("Keypad")
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new KeyPadStatusProperty(this),
                    new KeyPadOpenedProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new KeypadDisableMethod(this),
                    new KeypadEnableFunctionKeysMethod(this),
                    new KeypadValidatePinBlockMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new KeyPadOpenChangedEvent(this),
                    new KeyPadStatusChangedEvent(this),
                    new KeypadKeyPressedEvent(this),
                    new KeyPadFunctionKeyPressedEvent(this),
                    new KeypadEntryCompleteEvent(this),
                    new KeypadPinEntryCompleteEvent(this),
                    new KeypadEntryTimedOutEvent(this),
                });
        }

        public void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalService>();
            Successor = terminal.Devices["Terminal"];
        }

        #region Convenience Methods and Properties
        // Add methods and properties that make it more convienent to send commands
        // to this device from scripting environment

        #endregion
    }

    #region Properties

    [ValueProperty("State")]
    public class KeyPadStatusProperty : TerminalDeviceProperty<Status, GetStatusCommand, GetStatusResponse>
    {
        public KeyPadStatusProperty(ITerminalDevice device)
            : base(device, "Status")
        {
            GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Open")]
    public class KeyPadOpenedProperty : TerminalDeviceProperty<bool, GetOpenedCommand, GetOpenedResponse>
    {
        public KeyPadOpenedProperty(ITerminalDevice device)
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

    public class KeypadDisableMethod :
    TerminalDeviceMethod<DisableKeypadCommand, DisableKeypadResponse>
    {
        public KeypadDisableMethod(ITerminalDevice device)
            : base(device, "Disable")
        {
            InvokeCommand = new TerminalDeviceCommand<DisableKeypadCommand, DisableKeypadResponse>(
                this,
                Name
                );
        }
    }

    public class KeypadEnableFunctionKeysMethod :
    TerminalDeviceMethod<EnableKeypadFunctionKeysCommand, EnableKeypadFunctionKeysResponse>
    {
        public KeypadEnableFunctionKeysMethod(ITerminalDevice device)
            : base(device, "EnableFunctionKeys")
        {
            InvokeCommand = new TerminalDeviceCommand<EnableKeypadFunctionKeysCommand, EnableKeypadFunctionKeysResponse>
                (
                this,
                Name,
                () => new EnableKeypadFunctionKeysCommand
                    {
                        KeypadFunctionKey = new[]
                                {
                                    new KeypadFunctionKey {Function = KeypadFunctionKeyEnum.FuncL1, ReturnValue = 1},
                                    new KeypadFunctionKey {Function = KeypadFunctionKeyEnum.FuncL2, ReturnValue = 2},
                                    new KeypadFunctionKey {Function = KeypadFunctionKeyEnum.FuncL3, ReturnValue = 3},
                                    new KeypadFunctionKey {Function = KeypadFunctionKeyEnum.FuncL4, ReturnValue = 4},
                                    new KeypadFunctionKey {Function = KeypadFunctionKeyEnum.Num1, ReturnValue = 5},
                                    new KeypadFunctionKey {Function = KeypadFunctionKeyEnum.Num2, ReturnValue = 6},
                                }
                    }
                );
        }
    }

    public class KeypadValidatePinBlockMethod :
    TerminalDeviceMethod<ValidatePINBlockCommand, ValidatePINBlockResponse>
    {
        public KeypadValidatePinBlockMethod(ITerminalDevice device)
            : base(device, "ValidatePinBlock")
        {
            InvokeCommand = new TerminalDeviceCommand<ValidatePINBlockCommand, ValidatePINBlockResponse>(
                this,
                Name,
                () => new ValidatePINBlockCommand
                    {
                        PINBlock = ConvertHelper.ToHexByteArray("01020304CDFE4F")
                    }
                );
        }
    }

    #endregion

    #region Events

    public class KeyPadOpenChangedEvent : TerminalDeviceEvent<OpenChanged>
    {
        public KeyPadOpenChangedEvent(ITerminalDevice device)
            : base(device, "OpenChanged")
        {
        }
    }

    public class KeyPadStatusChangedEvent : TerminalDeviceEvent<StatusChanged>
    {
        public KeyPadStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class KeypadKeyPressedEvent : TerminalDeviceEvent<KeyPressed>
    {
        public KeypadKeyPressedEvent(ITerminalDevice device)
            : base(device, "KeyPressed")
        {
        }
    }

    public class KeyPadFunctionKeyPressedEvent : TerminalDeviceEvent<FunctionKeyPressed>
    {
        public KeyPadFunctionKeyPressedEvent(ITerminalDevice device)
            : base(device, "FunctionKeyPressed")
        {
        }
    }

    public class KeypadEntryCompleteEvent : TerminalDeviceEvent<EntryComplete>
    {
        public KeypadEntryCompleteEvent(ITerminalDevice device)
            : base(device, "EntryComplete")
        {
        }
    }

    public class KeypadPinEntryCompleteEvent : TerminalDeviceEvent<PINEntryComplete>
    {
        public KeypadPinEntryCompleteEvent(ITerminalDevice device)
            : base(device, "PinEntryComplete")
        {
        }
    }

    public class KeypadEntryTimedOutEvent : TerminalDeviceEvent<KeypadEntryTimedOut>
    {
        public KeypadEntryTimedOutEvent(ITerminalDevice device)
            : base(device, "EntryTimedOut")
        {
        }
    }

    #endregion
}
