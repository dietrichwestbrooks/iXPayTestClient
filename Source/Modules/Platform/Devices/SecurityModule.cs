using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class SecurityModule : TerminalDevice<SecurityModuleCommand, SecurityModuleResponse>
    {
        public SecurityModule() 
            : base("SecurityModule")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new HierarchyInUseProperty(this),
                    new SupportedKeyHierarchiesProperty(this),
                    new SupportedWorkingKeysProperty(this),
                    new SerialNumberProperty(this),
                    new BankSerialNumberProperty(this),
                    new CapabilitiesProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new SwitchKeyHierarchyMethod(this),
                    new CalculateMacMethod(this),
                    new VerifyMacMethod(this),
                    new EncryptMessageMethod(this),
                    new SetWorkingKeyMethod(this),
                    new GetWorkingKeyStatisticsMethod(this),
                    new IncrementKsnMethod(this),
                });
        }

        #region Device Properties

        [ValueProperty("Id")]
        public class HierarchyInUseProperty : TerminalDeviceProperty<int,
        GetHierarchyInUseCommand, GetHierarchyInUseResponse,
        SetHierarchyInUseCommand, SetHierarchyInUseResponse>
        {
            public HierarchyInUseProperty(ITerminalDevice device)
                : base(device, "HierarchyInUse")
            {
                GetCommand = new TerminalDeviceCommand<GetHierarchyInUseCommand, GetHierarchyInUseResponse>(
               this,
               $"get_{Name}"
               );

                SetCommand = new TerminalDeviceCommand<SetHierarchyInUseCommand, SetHierarchyInUseResponse>(
                    this,
                    $"set_{Name}",
                    () => new SetHierarchyInUseCommand
                        {
                            Id = 1,
                        }
                    );
            }
        }

        [ValueProperty("SupportedKeyHierarchy")]
        public class SupportedKeyHierarchiesProperty : TerminalDeviceProperty<SupportedKeyHierarchy[],
            GetSupportedKeyHierarchiesCommand, GetSupportedKeyHierarchiesResponse>
        {
            public SupportedKeyHierarchiesProperty(ITerminalDevice device)
                : base(device, "SupportedKeyHierarchies")
            {
                GetCommand = new TerminalDeviceCommand
                    <GetSupportedKeyHierarchiesCommand, GetSupportedKeyHierarchiesResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("WorkingKey")]
        public class SupportedWorkingKeysProperty : TerminalDeviceProperty<WorkingKey[],
            GetSupportedWorkingKeysCommand, GetSupportedWorkingKeysResponse>
        {
            public SupportedWorkingKeysProperty(ITerminalDevice device)
                : base(device, "SupportedWorkingKeys")
            {
                GetCommand = new TerminalDeviceCommand<GetSupportedWorkingKeysCommand, GetSupportedWorkingKeysResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("SerialNumber")]
        public class SerialNumberProperty : TerminalDeviceProperty<byte[],
            GetSerialNumberCommand, GetSerialNumberResponse>
        {
            public SerialNumberProperty(ITerminalDevice device)
                : base(device, "SerialNumber")
            {
                GetCommand = new TerminalDeviceCommand<GetSerialNumberCommand, GetSerialNumberResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("BankSerialNumber")]
        public class BankSerialNumberProperty : TerminalDeviceProperty<byte[],
            GetBankSerialNumberCommand, GetBankSerialNumberResponse>
        {
            public BankSerialNumberProperty(ITerminalDevice device)
                : base(device, "BankSerialNumber")
            {
                GetCommand = new TerminalDeviceCommand<GetBankSerialNumberCommand, GetBankSerialNumberResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("MultipleKeyHierarchies")]
        public class CapabilitiesProperty : TerminalDeviceProperty<bool,
            GetSecurityModuleCapabilitiesCommand, GetSecurityModuleCapabilitiesResponse>
        {
            public CapabilitiesProperty(ITerminalDevice device)
                : base(device, "Capabilities")
            {
                GetCommand = new TerminalDeviceCommand
                    <GetSecurityModuleCapabilitiesCommand, GetSecurityModuleCapabilitiesResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        #endregion

        #region Device Methods

        public class SwitchKeyHierarchyMethod : TerminalDeviceMethod<SwitchKeyHierarchyCommand, SwitchKeyHierarchyResponse>
        {
            public SwitchKeyHierarchyMethod(ITerminalDevice device)
                : base(device, "SwitchKeyHierarchy")
            {
                InvokeCommand = new TerminalDeviceCommand<SwitchKeyHierarchyCommand, SwitchKeyHierarchyResponse>(
                    this,
                    Name
                    );
            }
        }

        public class CalculateMacMethod : TerminalDeviceMethod<CalculateMACCommand, CalculateMACResponse>
        {
            public CalculateMacMethod(ITerminalDevice device)
                : base(device, "CalculateMac")
            {
                InvokeCommand = new TerminalDeviceCommand<CalculateMACCommand, CalculateMACResponse>(
                    this,
                    Name,
                    () => new CalculateMACCommand
                        {
                            Value = new byte[] {0x45, 0x4C, 0x67, 0x75, 0x56, 0x63, 0x87, 0x3A},
                        }
                    );
            }
        }

        public class VerifyMacMethod : TerminalDeviceMethod<VerifyMACCommand, VerifyMACResponse>
        {
            public VerifyMacMethod(ITerminalDevice device)
                : base(device, "VerifyMac")
            {
                InvokeCommand = new TerminalDeviceCommand<VerifyMACCommand, VerifyMACResponse>(
                    this,
                    Name,
                    () => new VerifyMACCommand
                        {
                            Value = new byte[] {0x45, 0x4C, 0x67, 0x75, 0x56, 0x63, 0x87, 0x3A},
                            MACValue = new byte[] {0x34, 0x42, 0x68, 0x31},
                            KSN = new byte[] {0x00, 0x00, 0x00, 0x00, 0x01},
                        }
                    );
            }
        }

        public class EncryptMessageMethod : TerminalDeviceMethod<EncryptMessageCommand, EncryptMessageResponse>
        {
            public EncryptMessageMethod(ITerminalDevice device)
                : base(device, "EncryptMessage")
            {
                InvokeCommand = new TerminalDeviceCommand<EncryptMessageCommand, EncryptMessageResponse>(
                    this,
                    Name,
                    () => new EncryptMessageCommand
                        {
                            Value = new byte[] {0x45, 0x4C, 0x67, 0x75, 0x56, 0x63, 0x87, 0x3A},
                        }
                    );
            }
        }

        public class SetWorkingKeyMethod : TerminalDeviceMethod<SetWorkingKeyCommand, SetWorkingKeyResponse>
        {
            public SetWorkingKeyMethod(ITerminalDevice device)
                : base(device, "SetWorkingKey")
            {
                InvokeCommand = new TerminalDeviceCommand<SetWorkingKeyCommand, SetWorkingKeyResponse>(
                    this,
                    Name,
                    () => new SetWorkingKeyCommand
                        {
                            Value = new byte[] {0x45, 0x4C, 0x67, 0x75, 0x56, 0x63, 0x87, 0x3A},
                            KeyType = WorkingKeyType.PIN,
                        }
                    );
            }
        }

        public class GetWorkingKeyStatisticsMethod : TerminalDeviceMethod<GetWorkingKeyStatisticsCommand, GetWorkingKeyStatisticsResponse>
        {
            public GetWorkingKeyStatisticsMethod(ITerminalDevice device)
                : base(device, "GetWorkingKeyStatistics")
            {
                InvokeCommand = new TerminalDeviceCommand
                    <GetWorkingKeyStatisticsCommand, GetWorkingKeyStatisticsResponse>(
                    this,
                    Name,
                    () => new GetWorkingKeyStatisticsCommand
                        {
                            KeyType = WorkingKeyType.PIN,
                        }
                    );
            }
        }

        public class IncrementKsnMethod : TerminalDeviceMethod<IncrementKSNCommand, IncrementKSNResponse>
        {
            public IncrementKsnMethod(ITerminalDevice device)
                : base(device, "IncrementKsn")
            {
                InvokeCommand = new TerminalDeviceCommand<IncrementKSNCommand, IncrementKSNResponse>(
                    this,
                    Name
                    );
            }
        }

        #endregion
    }
}
