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
                    new SecurityModuleHierarchyInUseProperty(this),
                    new SecurityModuleSupportedKeyHierarchiesProperty(this),
                    new SecurityModuleSupportedWorkingKeysProperty(this),
                    new SecurityModuleSerialNumberProperty(this),
                    new SecurityModuleBankSerialNumberProperty(this),
                    new SecurityModuleCapabilitiesProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new SecurityModuleSwitchKeyHierarchyMethod(this),
                    new SecurityModuleCalculateMacMethod(this),
                    new SecurityModuleVerifyMacMethod(this),
                    new SecurityModuleEncryptMessageMethod(this),
                    new SecurityModuleSetWorkingKeyMethod(this),
                    new SecurityModuleGetWorkingKeyStatisticsMethod(this),
                    new SecurityModuleIncrementKsnMethod(this),
                });
        }

        public void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalService>();
            Successor = terminal.Devices["Terminal"];
        }
    }

    #region Properties

    public class SecurityModuleHierarchyInUseProperty : TerminalDeviceProperty<int,
    GetHierarchyInUseCommand, GetHierarchyInUseResponse,
    SetHierarchyInUseCommand, SetHierarchyInUseResponse>
    {
        public SecurityModuleHierarchyInUseProperty(ITerminalDevice device)
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

    public class SecurityModuleSupportedKeyHierarchiesProperty : TerminalDeviceProperty<SupportedKeyHierarchy[],
        GetSupportedKeyHierarchiesCommand, GetSupportedKeyHierarchiesResponse>
    {
        public SecurityModuleSupportedKeyHierarchiesProperty(ITerminalDevice device)
            : base(device, "SupportedKeyHierarchies")
        {
            GetCommand = new TerminalDeviceCommand
                <GetSupportedKeyHierarchiesCommand, GetSupportedKeyHierarchiesResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    public class SecurityModuleSupportedWorkingKeysProperty : TerminalDeviceProperty<WorkingKey[],
        GetSupportedWorkingKeysCommand, GetSupportedWorkingKeysResponse>
    {
        public SecurityModuleSupportedWorkingKeysProperty(ITerminalDevice device)
            : base(device, "SupportedWorkingKeys")
        {
            GetCommand = new TerminalDeviceCommand<GetSupportedWorkingKeysCommand, GetSupportedWorkingKeysResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    public class SecurityModuleSerialNumberProperty : TerminalDeviceProperty<byte[],
        GetSerialNumberCommand, GetSerialNumberResponse>
    {
        public SecurityModuleSerialNumberProperty(ITerminalDevice device)
            : base(device, "SerialNumber")
        {
            GetCommand = new TerminalDeviceCommand<GetSerialNumberCommand, GetSerialNumberResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    public class SecurityModuleBankSerialNumberProperty : TerminalDeviceProperty<byte[],
        GetBankSerialNumberCommand, GetBankSerialNumberResponse>
    {
        public SecurityModuleBankSerialNumberProperty(ITerminalDevice device)
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

    public class SecurityModuleSwitchKeyHierarchyMethod : TerminalDeviceMethod<SwitchKeyHierarchyCommand, SwitchKeyHierarchyResponse>
    {
        public SecurityModuleSwitchKeyHierarchyMethod(ITerminalDevice device)
            : base(device, "SwitchKeyHierarchy")
        {
            InvokeCommand = new TerminalDeviceCommand<SwitchKeyHierarchyCommand, SwitchKeyHierarchyResponse>(
                this,
                Name
                );
        }
    }

    public class SecurityModuleCalculateMacMethod : TerminalDeviceMethod<CalculateMACCommand, CalculateMACResponse>
    {
        public SecurityModuleCalculateMacMethod(ITerminalDevice device)
            : base(device, "CalculateMac")
        {
            InvokeCommand = new TerminalDeviceCommand<CalculateMACCommand, CalculateMACResponse>(
                this,
                Name,
                () => new CalculateMACCommand
                    {
                        Value = ConvertHelper.ToHexByteArray("454C67755663873A"),
                    }
                );
        }
    }

    public class SecurityModuleVerifyMacMethod : TerminalDeviceMethod<VerifyMACCommand, VerifyMACResponse>
    {
        public SecurityModuleVerifyMacMethod(ITerminalDevice device)
            : base(device, "VerifyMac")
        {
            InvokeCommand = new TerminalDeviceCommand<VerifyMACCommand, VerifyMACResponse>(
                this,
                Name,
                () => new VerifyMACCommand
                    {
                        Value = ConvertHelper.ToHexByteArray("454C67755663873A"),
                        MACValue = ConvertHelper.ToHexByteArray("34426831"),
                        KSN = ConvertHelper.ToHexByteArray("0000000001"),
                    }
                );
        }
    }

    public class SecurityModuleEncryptMessageMethod : TerminalDeviceMethod<EncryptMessageCommand, EncryptMessageResponse>
    {
        public SecurityModuleEncryptMessageMethod(ITerminalDevice device)
            : base(device, "EncryptMessage")
        {
            InvokeCommand = new TerminalDeviceCommand<EncryptMessageCommand, EncryptMessageResponse>(
                this,
                Name,
                () => new EncryptMessageCommand
                    {
                        Value = ConvertHelper.ToHexByteArray("454C67755663873A"),
                    }
                );
        }
    }

    public class SecurityModuleSetWorkingKeyMethod : TerminalDeviceMethod<SetWorkingKeyCommand, SetWorkingKeyResponse>
    {
        public SecurityModuleSetWorkingKeyMethod(ITerminalDevice device)
            : base(device, "SetWorkingKey")
        {
            InvokeCommand = new TerminalDeviceCommand<SetWorkingKeyCommand, SetWorkingKeyResponse>(
                this,
                Name,
                () => new SetWorkingKeyCommand
                    {
                        Value = ConvertHelper.ToHexByteArray("454C67755663873A"),
                        KeyType = WorkingKeyType.PIN,
                    }
                );
        }
    }

    public class SecurityModuleGetWorkingKeyStatisticsMethod : TerminalDeviceMethod<GetWorkingKeyStatisticsCommand, GetWorkingKeyStatisticsResponse>
    {
        public SecurityModuleGetWorkingKeyStatisticsMethod(ITerminalDevice device)
            : base(device, "GetWorkingKeyStatistics")
        {
            InvokeCommand = new TerminalDeviceCommand<GetWorkingKeyStatisticsCommand, GetWorkingKeyStatisticsResponse>(
                this,
                Name,
                () => new GetWorkingKeyStatisticsCommand
                    {
                        KeyType = WorkingKeyType.PIN,
                    }
                );
        }
    }

    public class SecurityModuleIncrementKsnMethod : TerminalDeviceMethod<IncrementKSNCommand, IncrementKSNResponse>
    {
        public SecurityModuleIncrementKsnMethod(ITerminalDevice device)
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
