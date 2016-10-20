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
    public class SecurityModule : TerminalDevice<SecurityModuleCommand, SecurityModuleResponse>
    {
        public SecurityModule() 
            : base("SecurityModule")
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new HierarchyInUseProperty(this),
                    new SupportedKeyHierarchiesProperty(this),
                    new SupportedWorkingKeysProperty(this),
                    new SerialNumberProperty(this),
                    new BankSerialNumberProperty(this),
                    new SecurityModuleCapabilitiesProperty(this),
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

        public void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalClientService>();
            Successor = terminal.Devices["Terminal"];
        }
    }

    #region Properties

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
                new SortedList<string, object>
                    {
                        {"value", 1},
                    }
                );
        }
    }

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

    public class SecurityModuleCapabilitiesProperty : TerminalDeviceProperty<bool,
        GetSecurityModuleCapabilitiesCommand, GetSecurityModuleCapabilitiesResponse>
    {
        public SecurityModuleCapabilitiesProperty(ITerminalDevice device)
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

    #region Methods

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
            : base(device, "CalculateMAC")
        {
            InvokeCommand = new TerminalDeviceCommand<CalculateMACCommand, CalculateMACResponse>(
                this,
                Name,
                new SortedList<string, object>
                    {
                        {"value", ConvertHelper.ToHexByteArray("454C67755663873A")},
                    }
                );
        }
    }

    public class VerifyMacMethod : TerminalDeviceMethod<VerifyMACCommand, VerifyMACResponse>
    {
        public VerifyMacMethod(ITerminalDevice device)
            : base(device, "VerifyMAC")
        {
            InvokeCommand = new TerminalDeviceCommand<VerifyMACCommand, VerifyMACResponse>(
                this,
                Name,
                new SortedList<string, object>
                    {
                        {"value", ConvertHelper.ToHexByteArray("454C67755663873A")},
                        {"macValue", ConvertHelper.ToHexByteArray("34426831")},
                        {"ksn", ConvertHelper.ToHexByteArray("0000000001")},
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
                new SortedList<string, object>
                    {
                        {"value", ConvertHelper.ToHexByteArray("454C67755663873A")},
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
                new SortedList<string, object>
                    {
                        {"value", ConvertHelper.ToHexByteArray("454C67755663873A")},
                        {"keyType", WorkingKeyType.PIN},
                    }
                );
        }
    }

    public class GetWorkingKeyStatisticsMethod : TerminalDeviceMethod<GetWorkingKeyStatisticsCommand, GetWorkingKeyStatisticsResponse>
    {
        public GetWorkingKeyStatisticsMethod(ITerminalDevice device)
            : base(device, "GetWorkingKeyStatistics")
        {
            InvokeCommand = new TerminalDeviceCommand<GetWorkingKeyStatisticsCommand, GetWorkingKeyStatisticsResponse>(
                this,
                Name,
                new SortedList<string, object>
                    {
                        {"keyType", WorkingKeyType.PIN},
                    }
                );
        }
    }

    public class IncrementKsnMethod : TerminalDeviceMethod<IncrementKSNCommand, IncrementKSNResponse>
    {
        public IncrementKsnMethod(ITerminalDevice device)
            : base(device, "IncrementKSN")
        {
            InvokeCommand = new TerminalDeviceCommand<IncrementKSNCommand, IncrementKSNResponse>(
                this,
                Name
                );
        }
    }

    #endregion
}
