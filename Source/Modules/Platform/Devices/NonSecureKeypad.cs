using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class NonSecureKeypad
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<NonSecureKeypadCommand, NonSecureKeypadResponse, NonSecureKeypadEvent>(
                    "NonSecureKeypad", new TerminalRequestHandlerByName("Terminal"), typeof(NonSecureKeypad));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(NonSecureKeypad));

        public static readonly TerminalDeviceProperty OpenedProperty =
            TerminalDeviceProperty.Register<bool, GetOpenedCommand, GetOpenedResponse>("Opened",
                "Open", typeof(NonSecureKeypad));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod DisableMethod =
         TerminalDeviceMethod.Register<DisableKeypadCommand, DisableKeypadResponse>("Disable",
             typeof(NonSecureKeypad));

        public static readonly TerminalDeviceMethod EnableMethod =
         TerminalDeviceMethod.Register<EnableKeypadCommand, EnableKeypadResponse>("Enable",
             typeof(NonSecureKeypad), PrepareEnableKeypadCommand);

        private static EnableKeypadCommand PrepareEnableKeypadCommand()
        {
            return new EnableKeypadCommand
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
                };
        }

        public static readonly TerminalDeviceMethod EnableForPinMethod =
         TerminalDeviceMethod.Register<EnableKeypadForPINCommand, EnableKeypadForPINResponse>("EnableForPIN",
             typeof(NonSecureKeypad));

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent OpenChangedEvent =
            TerminalDeviceEvent.Register<OpenChanged>("OpenChanged", typeof(NonSecureKeypad));

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(NonSecureKeypad));

        public static readonly TerminalDeviceEvent KeyPressedEvent =
            TerminalDeviceEvent.Register<KeyPressed>("KeyPressed", typeof(NonSecureKeypad));

        public static readonly TerminalDeviceEvent EntryCompleteEvent =
            TerminalDeviceEvent.Register<EntryComplete>("EntryComplete", typeof(NonSecureKeypad));

        public static readonly TerminalDeviceEvent PinEntryCompleteEvent =
            TerminalDeviceEvent.Register<PINEntryComplete>("PINEntryComplete", typeof(NonSecureKeypad));

        #endregion
    }
}
