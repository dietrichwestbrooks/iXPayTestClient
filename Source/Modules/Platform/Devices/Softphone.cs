using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class Softphone
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<SoftphoneCommand, SoftphoneResponse, SoftphoneEvent>(
                "Softphone", new TerminalRequestHandlerByName("Terminal"), typeof (Softphone));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty CallStatusProperty =
        TerminalDeviceProperty.Register<CallStatus, GetCallStatusCommand, GetCallStatusResponse>("CallStatus",
            "CallStatus", typeof(Softphone));

        public static readonly TerminalDeviceProperty PhonenumberProperty =
            TerminalDeviceProperty.Register<string, GetPhonenumberCommand, GetPhonenumberResponse,
                SetPhonenumberCommand, SetPhonenumberResponse>("Phonenumber",
                    "Phonenumber", typeof (Softphone), null, () => new SetPhonenumberCommand
                        {
                            Phonenumber = "2891"
                        });

        public static readonly TerminalDeviceProperty VoipAccountProperty =
            TerminalDeviceProperty.Register<VoipAccount, GetVoipAccountCommand, GetVoipAccountResponse,
                SetVoipAccountCommand, SetVoipAccountResponse>("VoipAccount",
                    "VoipAccount", typeof (Softphone), null, () => new SetVoipAccountCommand
                        {
                            VoipAccount = new VoipAccount
                                {
                                    serverName = "10.222.230.18",
                                    uri = "2890@10.222.230.18",
                                    accountName = "2890",
                                    accountpassword = "",
                                    transport = "udp",
                                    authtype = "digest",
                                }
                        });

        public static readonly TerminalDeviceProperty VoipProtocolProperty =
        TerminalDeviceProperty.Register<VoipProtocol, GetVoipProtocolCommand, GetVoipProtocolResponse>("VoipProtocol",
            "VoipProtocol", typeof(Softphone));

        public static readonly TerminalDeviceProperty ConnectStatusProperty =
        TerminalDeviceProperty.Register<ConnectStatus, GetConnectStatusCommand, GetConnectStatusResponse>("ConnectStatus",
            "ConnectStatus", typeof(Softphone));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod LiftReceiverMethod =
         TerminalDeviceMethod.Register<LiftReceiverCommand, LiftReceiverResponse>("LiftReceiver",
             typeof(Softphone), () => new LiftReceiverCommand
                 {
                     PhoneNumber = string.Empty,
                 });

        public static readonly TerminalDeviceMethod HangupReceiverMethod =
         TerminalDeviceMethod.Register<HangupReceiverCommand, HangupReceiverResponse>("HangupReceiver",
             typeof(Softphone));

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent CallStateChangedEvent =
            TerminalDeviceEvent.Register<CallStateChanged>("CallStateChanged", typeof(Softphone));

        public static readonly TerminalDeviceEvent ConnectStateChangedEvent =
            TerminalDeviceEvent.Register<ConnectStateChanged>("ConnectStateChanged", typeof(Softphone));

        public static readonly TerminalDeviceEvent IncomingCallEvent =
            TerminalDeviceEvent.Register<IncomingCall>("IncomingCall", typeof(Softphone));

        #endregion
    }
}
