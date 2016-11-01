using System.Collections.Generic;
using System.Text;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class Keypad
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<KeypadCommand, KeypadResponse, KeypadEvent>(
                    "Keypad", new TerminalRequestHandlerByName("Terminal"), typeof(Keypad));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(Keypad));

        public static readonly TerminalDeviceProperty OpenedProperty =
            TerminalDeviceProperty.Register<bool, GetOpenedCommand, GetOpenedResponse>("Opened",
                "Open", typeof(Keypad));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod DisableMethod =
         TerminalDeviceMethod.Register<DisableKeypadCommand, DisableKeypadResponse>("Disable",
             typeof(Keypad));

        public static readonly TerminalDeviceMethod EnableFunctionKeysMethod =
         TerminalDeviceMethod.Register<EnableKeypadFunctionKeysCommand, EnableKeypadFunctionKeysResponse>("EnableFunctionKeys",
             typeof(Keypad), () => new EnableKeypadFunctionKeysCommand
             {
                 KeypadFunctionKey = new[]
                                {
                                    new KeypadFunctionKey {Function = KeypadFunctionKeyEnum.Num1, ReturnValue = 1},
                                    new KeypadFunctionKey {Function = KeypadFunctionKeyEnum.Num2, ReturnValue = 2},
                                }
             });

        public static readonly TerminalDeviceMethod ValidatePinBlockMethod =
         TerminalDeviceMethod.Register<ValidatePINBlockCommand, ValidatePINBlockResponse>("ValidatePinBlock",
             typeof(Keypad), () => new ValidatePINBlockCommand
             {
                 PINBlock = new ASCIIEncoding().GetBytes("01020304CDFE4F")
             });

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent OpenChangedEvent =
            TerminalDeviceEvent.Register<OpenChanged>("OpenChanged", typeof(Keypad));

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(Keypad));

        public static readonly TerminalDeviceEvent KeyPressedEvent =
            TerminalDeviceEvent.Register<KeyPressed>("KeyPressed", typeof(Keypad));

        public static readonly TerminalDeviceEvent FunctionKeyPressedEvent =
            TerminalDeviceEvent.Register<FunctionKeyPressed>("FunctionKeyPressed", typeof(Keypad));

        public static readonly TerminalDeviceEvent EntryCompleteEvent =
            TerminalDeviceEvent.Register<EntryComplete>("EntryComplete", typeof(Keypad));

        public static readonly TerminalDeviceEvent PinEntryCompleteEvent =
            TerminalDeviceEvent.Register<PINEntryComplete>("PinEntryComplete", typeof(Keypad));

        public static readonly TerminalDeviceEvent EntryTimedOutEvent =
            TerminalDeviceEvent.Register<KeypadEntryTimedOut>("EntryTimedOut", typeof(Keypad));

        #endregion
    }
}
