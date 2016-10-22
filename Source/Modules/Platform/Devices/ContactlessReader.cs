using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class ContactlessReader : TerminalDevice<ContactlessReaderCommand, ContactlessReaderResponse>, IPartImportsSatisfiedNotification
    {
        public ContactlessReader()
            : base("ContactlessReader")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new ContactlessReaderStatusProperty(this),
                    new ContactlessReaderOpenedProperty(this),
                    new ContactlessReaderCapabilitiesProperty(this),
                    new ContactlessReaderOperationalStateProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new ContactlessReaderGetGlobalEmvDataMethod(this),
                    new ContactlessReaderSetGlobalEmvDataMethod(this),
                    new ContactlessReaderGetBrandTerminalCapabilitiesMethod(this),
                    new ContactlessReaderSetBrandTerminalCapabilitiesMethod(this),
                    new ContactlessReaderGetBrandModeMethod(this),
                    new ContactlessReaderSetBrandModeMethod(this),
                    new ContactlessReaderGetApplicationMethod(this),
                    new ContactlessReaderDownloadApplicationMethod(this),
                    new ContactlessReaderDownloadPublicKeysMethod(this),
                    new ContactlessReaderDeleteApplicationPublicKeysMethod(this),
                    new ContactlessReaderSetBrandApplicationParametersMethod(this),
                    new ContactlessReaderGetBrandApplicationParametersMethod(this),
                    new ContactlessReaderDeleteApplicationMethod(this),
                    new ContactlessReaderSetTransactionIndicatorMethod(this),
                    new ContactlessReaderOpenMethod(this),
                    new ContactlessReaderCloseMethod(this),
                    new ContactlessReaderResetConfigurationMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new ContactlessReaderStatusChangedEvent(this),
                    new ContactlessReaderOpenChangedEvent(this),
                    new ContactlessReaderOperationalStateChangedEvent(this),
                    new ContactlessReaderDataReadEvent(this),
                });
        }

        public void OnImportsSatisfied()
        {
            var terminalService = ServiceLocator.Current.GetInstance<ITerminalService>();
            Successor = terminalService.Devices["Terminal"];
        }
    }

    #region Properties

    [ValueProperty("State")]
    public class ContactlessReaderStatusProperty : TerminalDeviceProperty<Status, GetStatusCommand, GetStatusResponse>
    {
        public ContactlessReaderStatusProperty(ITerminalDevice device)
            : base(device, "Status")
        {
            GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("Open")]
    public class ContactlessReaderOpenedProperty : TerminalDeviceProperty<bool, GetOpenedCommand, GetOpenedResponse>
    {
        public ContactlessReaderOpenedProperty(ITerminalDevice device)
            : base(device, "Opened")
        {
            GetCommand = new TerminalDeviceCommand<GetOpenedCommand, GetOpenedResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("SupportedPaymentScheme")]
    public class ContactlessReaderCapabilitiesProperty : TerminalDeviceProperty<bool,
        GetContactlessReaderCapabilitiesCommand, GetContactlessReaderCapabilitiesResponse>
    {
        public ContactlessReaderCapabilitiesProperty(ITerminalDevice device)
            : base(device, "Capabilities")
        {
            GetCommand = new TerminalDeviceCommand<GetContactlessReaderCapabilitiesCommand, GetContactlessReaderCapabilitiesResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("OperationalState")]
    public class ContactlessReaderOperationalStateProperty : TerminalDeviceProperty<bool,
        GetContactlessOperationalStateCommand, GetContactlessOperationalStateResponse>
    {
        public ContactlessReaderOperationalStateProperty(ITerminalDevice device)
            : base(device, "OperationalState")
        {
            GetCommand = new TerminalDeviceCommand<GetContactlessOperationalStateCommand, GetContactlessOperationalStateResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    #endregion

    #region Methods

    public class ContactlessReaderGetGlobalEmvDataMethod : TerminalDeviceMethod<GetGlobalEMVDataCommand, GetGlobalEMVDataResponse>
    {
        public ContactlessReaderGetGlobalEmvDataMethod(ITerminalDevice device)
            : base(device, "GetGlobalEMVData")
        {
            InvokeCommand = new TerminalDeviceCommand<GetGlobalEMVDataCommand, GetGlobalEMVDataResponse>(
                this,
                Name
                );
        }
    }

    public class ContactlessReaderSetGlobalEmvDataMethod :
        TerminalDeviceMethod<SetGlobalEMVDataCommand, SetGlobalEMVDataResponse>
    {
        public ContactlessReaderSetGlobalEmvDataMethod(ITerminalDevice device)
            : base(device, "SetGlobalEMVData")
        {
            InvokeCommand = new TerminalDeviceCommand<SetGlobalEMVDataCommand, SetGlobalEMVDataResponse>(
                this,
                Name,
                () => new SetGlobalEMVDataCommand
                {
                    EmvDataElement = new[]
                            {
                                new EmvDataElement { Value = ConvertHelper.ToHexByteArray("9F1A020840")},
                            }
                }
                );
        }
    }

    public class ContactlessReaderGetBrandTerminalCapabilitiesMethod :
        TerminalDeviceMethod<GetBrandTerminalCapabilitiesCommand, GetBrandTerminalCapabilitiesResponse>
    {
        public ContactlessReaderGetBrandTerminalCapabilitiesMethod(ITerminalDevice device)
            : base(device, "GetBrandTerminalCapabilities")
        {
            InvokeCommand = new TerminalDeviceCommand
                <GetBrandTerminalCapabilitiesCommand, GetBrandTerminalCapabilitiesResponse>(
                this,
                Name,
                () => new GetBrandTerminalCapabilitiesCommand
                {
                    Name = BrandName.MasterCard,
                }
                );
        }
    }

    public class ContactlessReaderSetBrandTerminalCapabilitiesMethod :
        TerminalDeviceMethod<SetBrandTerminalCapabilitiesCommand, SetBrandTerminalCapabilitiesResponse>
    {
        public ContactlessReaderSetBrandTerminalCapabilitiesMethod(ITerminalDevice device)
            : base(device, "SetBrandTerminalCapabilities")
        {
            InvokeCommand = new TerminalDeviceCommand
                <SetBrandTerminalCapabilitiesCommand, SetBrandTerminalCapabilitiesResponse>(
                this,
                Name,
                () => new SetBrandTerminalCapabilitiesCommand
                {
                    Name = BrandName.MasterCard,
                    EmvDataElement = new[]
                            {
                                new EmvDataElement { Value = ConvertHelper.ToHexByteArray("9F350121")},
                            }
                }
                );
        }
    }

    public class ContactlessReaderGetBrandModeMethod :
        TerminalDeviceMethod<GetBrandModeCommand, GetBrandModeResponse>
    {
        public ContactlessReaderGetBrandModeMethod(ITerminalDevice device)
            : base(device, "GetBrandMode")
        {
            InvokeCommand = new TerminalDeviceCommand
                <GetBrandModeCommand, GetBrandModeResponse>(
                this,
                Name,
                () => new GetBrandModeCommand
                    {
                        Name = BrandName.MasterCard,
                    }
                );
        }
    }

    public class ContactlessReaderSetBrandModeMethod :
        TerminalDeviceMethod<SetBrandModeCommand, SetBrandModeResponse>
    {
        public ContactlessReaderSetBrandModeMethod(ITerminalDevice device)
            : base(device, "SetBrandMode")
        {
            InvokeCommand = new TerminalDeviceCommand
                <SetBrandModeCommand, SetBrandModeResponse>(
                this,
                Name,
                () => new SetBrandModeCommand
                {
                    Name = BrandName.MasterCard,
                    Mode = 0,
                }
                );
        }
    }

    public class ContactlessReaderGetApplicationMethod :
        TerminalDeviceMethod<GetContactlessApplicationCommand, GetContactlessApplicationResponse>
    {
        public ContactlessReaderGetApplicationMethod(ITerminalDevice device)
            : base(device, "GetApplication")
        {
            InvokeCommand = new TerminalDeviceCommand
                <GetContactlessApplicationCommand, GetContactlessApplicationResponse>(
                this,
                Name,
                () => new GetContactlessApplicationCommand
                    {
                        AID = ConvertHelper.ToHexByteArray("A0000000041010"),
                        ApplicationIndex = 1,
                    }
                );
        }
    }

    public class ContactlessReaderDownloadApplicationMethod :
        TerminalDeviceMethod<DownloadContactlessApplicationCommand, DownloadContactlessApplicationResponse>
    {
        public ContactlessReaderDownloadApplicationMethod(ITerminalDevice device)
            : base(device, "DownloadApplication")
        {
            InvokeCommand = new TerminalDeviceCommand
                <DownloadContactlessApplicationCommand, DownloadContactlessApplicationResponse>(
                this,
                Name,
                () => new DownloadContactlessApplicationCommand
                    {
                        Name = BrandName.MasterCard,
                        AID = ConvertHelper.ToHexByteArray("A0000000041010"),
                        ApplicationIndex = 1,
                        ApplicationSchemeIndicator = 1,
                        TransactionTypeIndicator = 1,
                        TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("0000000000"),
                        TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0010000000"),
                        TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("0000000000"),
                        TransactionLimit = ConvertHelper.ToHexByteArray("000000030000"),
                        TransactionFloorLimit = ConvertHelper.ToHexByteArray("000000100000"),
                        CVMRequiredLimit = ConvertHelper.ToHexByteArray("000000001000"),
                        EmvDataElement = new[]
                            {
                                new EmvDataElement {Value = ConvertHelper.ToHexByteArray("500A4D617374657243617264")},
                            }
                    }
                );
        }
    }

    public class ContactlessReaderDownloadPublicKeysMethod :
        TerminalDeviceMethod<DownloadContactlessPublicKeysCommand, DownloadContactlessPublicKeysResponse>
    {
        public ContactlessReaderDownloadPublicKeysMethod(ITerminalDevice device)
            : base(device, "DownloadPublicKeys")
        {
            InvokeCommand = new TerminalDeviceCommand
                <DownloadContactlessPublicKeysCommand, DownloadContactlessPublicKeysResponse>(
                this,
                Name,
                () => new DownloadContactlessPublicKeysCommand
                    {
                        Rid = "A000000003",
                        ContactlessKey = new[]
                            {
                                new ContactlessKey
                                    {
                                        Index = "09",
                                        Exponent = "03",
                                        Modulus =
                                            "C2490747FE17EB0584C88D47B1602704150ADC88C5B998BD59CE043EDEBF0FFEE3093AC7956AD3B6AD4554C6DE19A178D6DA295BE15D5220645E3C8131666FA4BE5B84FE131EA44B039307638B9E74A8C42564F892A64DF1CB15712B736E3374F1BBB6819371602D8970E97B900793C7C2A89A4A1649A59BE680574DD0B60145",
                                        Checksum = "2015497BE4B86F104BBF337691825EED64E101CA",
                                    },
                            }
                    }
                );
        }
    }

    public class ContactlessReaderDeleteApplicationPublicKeysMethod :
        TerminalDeviceMethod<DeleteApplicationPublicKeysCommand, DeleteApplicationPublicKeysResponse>
    {
        public ContactlessReaderDeleteApplicationPublicKeysMethod(ITerminalDevice device)
            : base(device, "DeleteApplicationPublicKeys")
        {
            InvokeCommand = new TerminalDeviceCommand
                <DeleteApplicationPublicKeysCommand, DeleteApplicationPublicKeysResponse>(
                this,
                Name,
                () => new DeleteApplicationPublicKeysCommand
                    {
                        Rid = "A000000003",
                    }
                );
        }
    }

    public class ContactlessReaderSetBrandApplicationParametersMethod :
        TerminalDeviceMethod<SetBrandApplicationParametersCommand, SetBrandApplicationParametersResponse>
    {
        public ContactlessReaderSetBrandApplicationParametersMethod(ITerminalDevice device)
            : base(device, "SetBrandApplicationParameters")
        {
            InvokeCommand = new TerminalDeviceCommand
                <SetBrandApplicationParametersCommand, SetBrandApplicationParametersResponse>(
                this,
                Name,
                () => new SetBrandApplicationParametersCommand
                    {
                        Name = BrandName.MasterCard,
                        OfflineChipData =
                            ConvertHelper.ToHexByteArray(
                                "9A9F219F029F039F539F1A5F2A9F339F409F359F1E82849F095A5F34959B9F349F269F279F109F379F369C9F418A899F6E50579F119F12"),
                        OnlineChipData =
                            ConvertHelper.ToHexByteArray(
                                "369F019F025F255F245A5F349F269F109F159F169F399F279F1A5F2A9A9F219C959B829F349F36849F6E509F119F129F339F409F379F35"),
                        StatusCheckAmount = ConvertHelper.ToHexByteArray("000000010000"),
                        EmvDataElement = new[]
                            {
                                new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F530100")},
                            }
                    }
                );
        }
    }

    public class ContactlessReaderGetBrandApplicationParametersMethod :
        TerminalDeviceMethod<GetBrandApplicationParametersCommand, GetBrandApplicationParametersResponse>
    {
        public ContactlessReaderGetBrandApplicationParametersMethod(ITerminalDevice device)
            : base(device, "GetBrandApplicationParameters")
        {
            InvokeCommand = new TerminalDeviceCommand
                <GetBrandApplicationParametersCommand, GetBrandApplicationParametersResponse>(
                this,
                Name,
                () => new GetBrandApplicationParametersCommand
                    {
                        Name = BrandName.Visa,
                    }
                );
        }
    }

    public class ContactlessReaderDeleteApplicationMethod :
        TerminalDeviceMethod<DeleteContactlessApplicationCommand, DeleteContactlessApplicationResponse>
    {
        public ContactlessReaderDeleteApplicationMethod(ITerminalDevice device)
            : base(device, "DeleteApplication")
        {
            InvokeCommand = new TerminalDeviceCommand
                <DeleteContactlessApplicationCommand, DeleteContactlessApplicationResponse>(
                this,
                Name,
                () => new DeleteContactlessApplicationCommand
                    {
                        ApplicationIndex = 1,
                        AID = ConvertHelper.ToHexByteArray("A0000000041010"),
                    }
                );
        }
    }

    public class ContactlessReaderSetTransactionIndicatorMethod :
        TerminalDeviceMethod<SetTransactionIndicatorCommand, SetTransactionIndicatorResponse>
    {
        public ContactlessReaderSetTransactionIndicatorMethod(ITerminalDevice device)
            : base(device, "SetTransactionIndicator")
        {
            InvokeCommand = new TerminalDeviceCommand
                <SetTransactionIndicatorCommand, SetTransactionIndicatorResponse>(
                this,
                Name,
                () => new SetTransactionIndicatorCommand
                {
                    TerminalTransactionResult = TerminalTransactionResult.OnlineFailed,
                }
                );
        }
    }

    public class ContactlessReaderOpenMethod :
        TerminalDeviceMethod<OpenContactlessReaderCommand, OpenContactlessReaderResponse>
    {
        public ContactlessReaderOpenMethod(ITerminalDevice device)
            : base(device, "Open")
        {
            InvokeCommand = new TerminalDeviceCommand
                <OpenContactlessReaderCommand, OpenContactlessReaderResponse>(
                this,
                Name,
                () => new OpenContactlessReaderCommand
                    {
                        EnablePayment = new EnablePayment {PaymentScheme = PaymentSchemes.StandardPayment},
                        EnableWallet = new EnableWallet {WalletScheme = WalletSchemes.NotSupported}
                    }
                );
        }
    }

    public class ContactlessReaderCloseMethod :
        TerminalDeviceMethod<CloseContactlessReaderCommand, CloseContactlessReaderResponse>
    {
        public ContactlessReaderCloseMethod(ITerminalDevice device)
            : base(device, "Close")
        {
            InvokeCommand = new TerminalDeviceCommand
                <CloseContactlessReaderCommand, CloseContactlessReaderResponse>(
                this,
                Name
                );
        }
    }

    public class ContactlessReaderResetConfigurationMethod :
        TerminalDeviceMethod<ResetContactlessConfigurationCommand, ResetContactlessConfigurationResponse>
    {
        public ContactlessReaderResetConfigurationMethod(ITerminalDevice device)
            : base(device, "ResetConfiguration")
        {
            InvokeCommand = new TerminalDeviceCommand
                <ResetContactlessConfigurationCommand, ResetContactlessConfigurationResponse>(
                this,
                Name
                );
        }
    }

    #endregion

    #region Events

    public class ContactlessReaderOpenChangedEvent : TerminalDeviceEvent<OpenChanged>
    {
        public ContactlessReaderOpenChangedEvent(ITerminalDevice device)
            : base(device, "OpenChanged")
        {
        }
    }

    public class ContactlessReaderStatusChangedEvent : TerminalDeviceEvent<StatusChanged>
    {
        public ContactlessReaderStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class ContactlessReaderOperationalStateChangedEvent : TerminalDeviceEvent<ContactlessOperationalStateChanged>
    {
        public ContactlessReaderOperationalStateChangedEvent(ITerminalDevice device)
            : base(device, "OperationalStateChanged")
        {
        }
    }

    public class ContactlessReaderDataReadEvent : TerminalDeviceEvent<ContactlessData>
    {
        public ContactlessReaderDataReadEvent(ITerminalDevice device)
            : base(device, "DataRead")
        {
        }
    }

    #endregion
}
