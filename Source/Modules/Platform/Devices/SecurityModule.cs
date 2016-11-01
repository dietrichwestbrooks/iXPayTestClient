using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class SecurityModule
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<SecurityModuleCommand, SecurityModuleResponse>(
                    "SecurityModule", new TerminalRequestHandlerByName("Terminal"), typeof(SecurityModule));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty HierarchyInUseProperty =
            TerminalDeviceProperty.Register<int, GetHierarchyInUseCommand, GetHierarchyInUseResponse,
                SetHierarchyInUseCommand, SetHierarchyInUseResponse>("HierarchyInUse",
                    "Id", typeof (SecurityModule), null, () => new SetHierarchyInUseCommand
                        {
                            Id = 1,
                        });

        public static readonly TerminalDeviceProperty SupportedKeyHierarchiesProperty =
            TerminalDeviceProperty.Register<SupportedKeyHierarchy[],
            GetSupportedKeyHierarchiesCommand, GetSupportedKeyHierarchiesResponse>("SupportedKeyHierarchies",
                "SupportedKeyHierarchy", typeof(SecurityModule));

        public static readonly TerminalDeviceProperty SupportedWorkingKeysProperty =
            TerminalDeviceProperty.Register<WorkingKey[],
            GetSupportedWorkingKeysCommand, GetSupportedWorkingKeysResponse>("SupportedWorkingKeys",
                "WorkingKey", typeof(SecurityModule));

        public static readonly TerminalDeviceProperty SerialNumberProperty =
            TerminalDeviceProperty.Register<byte[], GetSerialNumberCommand, GetSerialNumberResponse>("SerialNumber",
                "SerialNumber", typeof(SecurityModule));

        public static readonly TerminalDeviceProperty BankSerialNumberProperty =
            TerminalDeviceProperty.Register<byte[], GetBankSerialNumberCommand, GetBankSerialNumberResponse>("BankSerialNumber",
                "BankSerialNumber", typeof(SecurityModule));

        public static readonly TerminalDeviceProperty TerminalDeviceProperty =
            TerminalDeviceProperty.Register<bool,
            GetSecurityModuleCapabilitiesCommand, GetSecurityModuleCapabilitiesResponse>("Capabilities",
                "MultipleKeyHierarchies", typeof(SecurityModule));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod SwitchKeyHierarchyMethod =
         TerminalDeviceMethod.Register<SwitchKeyHierarchyCommand, SwitchKeyHierarchyResponse>("SwitchKeyHierarchy",
             typeof(SecurityModule));

        public static readonly TerminalDeviceMethod CalculateMacMethod =
         TerminalDeviceMethod.Register<CalculateMACCommand, CalculateMACResponse>("CalculateMac",
             typeof(SecurityModule), () => new CalculateMACCommand
             {
                 Value = new byte[] { 0x45, 0x4C, 0x67, 0x75, 0x56, 0x63, 0x87, 0x3A },
             });

        public static readonly TerminalDeviceMethod VerifyMacMethod =
         TerminalDeviceMethod.Register<VerifyMACCommand, VerifyMACResponse>("VerifyMac",
             typeof(SecurityModule), () => new VerifyMACCommand
             {
                 Value = new byte[] { 0x45, 0x4C, 0x67, 0x75, 0x56, 0x63, 0x87, 0x3A },
                 MACValue = new byte[] { 0x34, 0x42, 0x68, 0x31 },
                 KSN = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01 },
             });

        public static readonly TerminalDeviceMethod EncryptMessageMethod =
         TerminalDeviceMethod.Register<EncryptMessageCommand, EncryptMessageResponse>("EncryptMessage",
             typeof(SecurityModule), () => new EncryptMessageCommand
             {
                 Value = new byte[] { 0x45, 0x4C, 0x67, 0x75, 0x56, 0x63, 0x87, 0x3A },
             });

        public static readonly TerminalDeviceMethod SetWorkingKeyMethod =
         TerminalDeviceMethod.Register<SetWorkingKeyCommand, SetWorkingKeyResponse>("SetWorkingKey",
             typeof(SecurityModule), () => new SetWorkingKeyCommand
             {
                 Value = new byte[] { 0x45, 0x4C, 0x67, 0x75, 0x56, 0x63, 0x87, 0x3A },
                 KeyType = WorkingKeyType.PIN,
             });

        public static readonly TerminalDeviceMethod GetWorkingKeyStatisticsMethod =
         TerminalDeviceMethod.Register<GetWorkingKeyStatisticsCommand, GetWorkingKeyStatisticsResponse>("GetWorkingKeyStatistics",
             typeof(SecurityModule), () => new GetWorkingKeyStatisticsCommand
             {
                 KeyType = WorkingKeyType.PIN,
             });

        public static readonly TerminalDeviceMethod IncrementKsnMethod =
         TerminalDeviceMethod.Register<IncrementKSNCommand, IncrementKSNResponse>("IncrementKsn",
             typeof(SecurityModule));

        #endregion
    }
}
