using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Emv.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class EmvModule : TerminalDevice<EMVModuleCommand, EMVModuleResponse>, IPartImportsSatisfiedNotification
    {
        public EmvModule()
            : base("EmvModule")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new EmvModuleStatusProperty(this),
                    new ApplicationExclusionListProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new EmvModuleSetTimeoutMethod(this),
                    new EmvModuleInitiateTransactionMethod(this),
                    new EmvModuleSetAmountsMethod(this),
                    new EmvModuleContinueTransactionMethod(this),
                    new EmvModuleReenterOnlinePinMethod(this),
                    new EmvModuleCompleteOnlineTransactionMethod(this),
                    new EmvModuleReadCardDataMethod(this),
                    new EmvModuleAbortTransactionMethod(this),
                    new EmvModuleDownloadApplicationMethod(this),
                    new EmvModuleDeleteApplicationMethod(this),
                    new EmvModuleDownloadApplicationPublicKeysMethod(this),
                    new EmvModuleDeleteApplicationPublicKeysMethod(this),
                    new EmvModuleClearAllApplicationsAndKeysMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new EmvModuleStatusChangedEvent(this),
                    new EmvModuleReenterOnlinePinFailedEvent(this),
                    new EmvModuleApplicationSelectedEvent(this),
                    new EmvModuleEmvModuleCardDetailsEvent(this),
                    new EmvModuleOnlineApprovalRequestDataEvent(this),
                    new EmvModuleTransactionResultEvent(this),
                    new EmvModuleCardDataReadEvent(this),
                    new EmvModuleRemovedApplicationEvent(this),
                });
        }

        public void OnImportsSatisfied()
        {
            var terminalService = ServiceLocator.Current.GetInstance<ITerminalService>();
            Successor = terminalService.Devices["Terminal"];
        }
    }

    #region Properties

    [ValueProperty("EMVModuleState")]
    public class EmvModuleStatusProperty : TerminalDeviceProperty<EMVModuleStatus,
        GetEMVModuleStatusCommand, GetEMVModuleStatusResponse>
    {
        public EmvModuleStatusProperty(ITerminalDevice device)
            : base(device, "Status")
        {
            GetCommand = new TerminalDeviceCommand<GetEMVModuleStatusCommand, GetEMVModuleStatusResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    [ValueProperty("ApplicationExclusion")]
    public class ApplicationExclusionListProperty : TerminalDeviceProperty<ApplicationExclusion[],
        GetApplicationExclusionListCommand, GetApplicationExclusionListResponse>
    {
        public ApplicationExclusionListProperty(ITerminalDevice device)
            : base(device, "ApplicationExclusionList")
        {
            GetCommand = new TerminalDeviceCommand<GetApplicationExclusionListCommand, GetApplicationExclusionListResponse>(
                this,
                $"get_{Name}"
                );

            SetCommand = new TerminalDeviceCommand<SetApplicationExclusionListCommand, SetApplicationExclusionListResponse>(
                this,
                $"set_{Name}",
                () => new SetApplicationExclusionListCommand
                    {
                        ApplicationExclusion = new[]
                                {
                                    new ApplicationExclusion { App = "A0000002771010", ExcludeAll = true},
                                    new ApplicationExclusion { App = "A0000000050001", Exclude = "A0000000043060"},
                                }
                    }
                );
        }
    }

    #endregion

    #region Methods

    public class EmvModuleSetTimeoutMethod : TerminalDeviceMethod<SetTimeoutCommand, SetTimeoutResponse>
    {
        public EmvModuleSetTimeoutMethod(ITerminalDevice device)
            : base(device, "SetTimeout")
        {
            InvokeCommand = new TerminalDeviceCommand<SetTimeoutCommand, SetTimeoutResponse>(
                this,
                Name,
                () => new SetTimeoutCommand
                    {
                        TimeoutValue = 60
                    }
                );
        }
    }

    public class EmvModuleInitiateTransactionMethod : TerminalDeviceMethod<InitiateTransactionCommand, InitiateTransactionResponse>
    {
        public EmvModuleInitiateTransactionMethod(ITerminalDevice device)
            : base(device, "InitiateTransaction")
        {
            InvokeCommand = new TerminalDeviceCommand<InitiateTransactionCommand, InitiateTransactionResponse>(
                this,
                Name,
                () => new InitiateTransactionCommand
                    {
                        EmvDataElement = new[]
                                {
                                    new EmvDataElement {Value = ConvertHelper.ToHexByteArray("E11B9F02060000000005009F02060000000010009F0206000000002000")},
                                }
                    }
                );
        }
    }

    public class EmvModuleSetAmountsMethod : TerminalDeviceMethod<SetAmountsCommand, SetAmountsResponse>
    {
        public EmvModuleSetAmountsMethod(ITerminalDevice device)
            : base(device, "SetAmounts")
        {
            InvokeCommand = new TerminalDeviceCommand<SetAmountsCommand, SetAmountsResponse>(
                this,
                Name,
                () => new SetAmountsCommand
                    {
                        EmvDataElement = new[]
                                {
                                    new EmvDataElement {Value = ConvertHelper.ToHexByteArray("E11B9F02060000000005009F02060000000010009F0206000000002000")},
                                }
                    }
                );
        }
    }

    public class EmvModuleContinueTransactionMethod : TerminalDeviceMethod<ContinueTransactionCommand, ContinueTransactionResponse>
    {
        public EmvModuleContinueTransactionMethod(ITerminalDevice device)
            : base(device, "ContinueTransaction")
        {
            InvokeCommand = new TerminalDeviceCommand<ContinueTransactionCommand, ContinueTransactionResponse>(
                this,
                Name);
        }
    }

    public class EmvModuleReenterOnlinePinMethod : TerminalDeviceMethod<ReenterOnlinePINCommand, ReenterOnlinePINResponse>
    {
        public EmvModuleReenterOnlinePinMethod(ITerminalDevice device)
            : base(device, "ReenterOnlinePin")
        {
            InvokeCommand = new TerminalDeviceCommand<ReenterOnlinePINCommand, ReenterOnlinePINResponse>(
                this,
                Name
                );
        }
    }

    public class EmvModuleCompleteOnlineTransactionMethod : TerminalDeviceMethod<CompleteOnlineTransactionCommand, CompleteOnlineTransactionResponse>
    {
        public EmvModuleCompleteOnlineTransactionMethod(ITerminalDevice device)
            : base(device, "CompleteOnlineTransaction")
        {
            InvokeCommand = new TerminalDeviceCommand<CompleteOnlineTransactionCommand, CompleteOnlineTransactionResponse>(
                this,
                Name,
                () => new CompleteOnlineTransactionCommand
                    {
                        EmvDataElement = new[]
                                {
                                    new EmvDataElement {Value = ConvertHelper.ToHexByteArray("C101FF")},
                                }
                    }
                );
        }
    }

    public class EmvModuleReadCardDataMethod : TerminalDeviceMethod<ReadCardDataCommand, ReadCardDataResponse>
    {
        public EmvModuleReadCardDataMethod(ITerminalDevice device)
            : base(device, "ReadCardData")
        {
            InvokeCommand = new TerminalDeviceCommand<ReadCardDataCommand, ReadCardDataResponse>(
                this,
                Name,
                () => new ReadCardDataCommand
                    {
                        EmvDataElement = new[]
                                {
                                    new EmvDataElement {Value = ConvertHelper.ToHexByteArray("C101FF")},
                                }
                    }
                );
        }
    }

    public class EmvModuleAbortTransactionMethod : TerminalDeviceMethod<AbortTransactionCommand, AbortTransactionResponse>
    {
        public EmvModuleAbortTransactionMethod(ITerminalDevice device)
            : base(device, "AbortTransaction")
        {
            InvokeCommand = new TerminalDeviceCommand<AbortTransactionCommand, AbortTransactionResponse>(
                this,
                Name
                );
        }
    }

    public class EmvModuleDownloadApplicationMethod : TerminalDeviceMethod<DownloadApplicationCommand, DownloadApplicationResponse>
    {
        public EmvModuleDownloadApplicationMethod(ITerminalDevice device)
            : base(device, "DownloadApplication")
        {
            InvokeCommand = new TerminalDeviceCommand<DownloadApplicationCommand, DownloadApplicationResponse>(
                this,
                Name,
                () => new DownloadApplicationCommand
                    {
                        Name = "Visa",
                        Rid = ConvertHelper.ToHexByteArray("A000000003"),
                        Pix = ConvertHelper.ToHexByteArray("1010"),
                        Version = ConvertHelper.ToHexByteArray("84"),
                        TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("D84000A800"),
                        TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0010000000"),
                        TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("D84004F800"),
                        TDOL = ConvertHelper.ToHexByteArray("9F02065F2A029A039C0195059F3704"),
                        DDOL = ConvertHelper.ToHexByteArray("9F3704"),
                        SelectionIndicator = ConvertHelper.ToHexByteArray("01"),
                        FloorLimit = 10000,
                        ThresholdValue = 500,
                        TargetPercentage = 30,
                        MaxTargetPercentage = 90,
                        MaxTransactionValue = 5000,
                        MaxNoCVMTransactionValue = 3000,
                        ApprovalAmount = 100,
                        EmvDataElement = new[]
                            {
                                new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F150400000100")},
                            },
                        OverrideParameters = new[]
                            {
                                new OverrideParameters
                                    {
                                        BIN = "6011",
                                        Name = "Visa",
                                        TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("D84000A800"),
                                        TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0010000000"),
                                        TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("D84004F800"),
                                        TDOL = ConvertHelper.ToHexByteArray("9F02065F2A029A039C0195059F3704"),
                                        DDOL = ConvertHelper.ToHexByteArray("9F3704"),
                                        FloorLimit = 10000,
                                        FloorLimitSpecified = true,
                                        ThresholdValue = 500,
                                        ThresholdValueSpecified = true,
                                        TargetPercentage = 30,
                                        TargetPercentageSpecified = true,
                                        MaxTargetPercentage = 90,
                                        MaxTargetPercentageSpecified = true,
                                        MaxTransactionValue = 5000,
                                        MaxTransactionValueSpecified = true,
                                        MaxNoCVMTransactionValue = 3000,
                                        MaxNoCVMTransactionValueSpecified = true,
                                        ApprovalAmount = 100,
                                        ApprovalAmountSpecified = true,
                                    },
                            },
                    }
                );
        }
    }

    public class EmvModuleDeleteApplicationMethod : TerminalDeviceMethod<DeleteApplicationCommand, DeleteApplicationResponse>
    {
        public EmvModuleDeleteApplicationMethod(ITerminalDevice device)
            : base(device, "DeleteApplication")
        {
            InvokeCommand = new TerminalDeviceCommand<DeleteApplicationCommand, DeleteApplicationResponse>(
                this,
                Name,
                () => new DeleteApplicationCommand
                    {
                        Rid = "A000000003",
                        Pix = "1010",
                        Version = "84",
                    }
                );
        }
    }

    public class EmvModuleDownloadApplicationPublicKeysMethod : TerminalDeviceMethod<DownloadApplicationPublicKeysCommand, DownloadApplicationPublicKeysResponse>
    {
        public EmvModuleDownloadApplicationPublicKeysMethod(ITerminalDevice device)
            : base(device, "DownloadApplicationPublicKeys")
        {
            InvokeCommand = new TerminalDeviceCommand
                <DownloadApplicationPublicKeysCommand, DownloadApplicationPublicKeysResponse>(
                this,
                Name,
                () => new DownloadApplicationPublicKeysCommand
                    {
                        Rid = "A000000003",
                        KeyDefinition = new[]
                            {
                                new KeyDefinition
                                    {
                                        SignAlgorithm = "01",
                                        Number = "09",
                                        Exponent = "03",
                                        HashAlgorithm = "01",
                                        HashValue = "2015497BE4B86F104BBF337691825EED64E101CA",
                                        Modulus =
                                            "C2490747FE17EB0584C88D47B1602704150ADC88C5B998BD59CE043EDEBF0FFEE3093AC7956AD3B6AD4554C6DE19A178D6DA295BE15D5220645E3C8131666FA4BE5B84FE131EA44B039307638B9E74A8C42564F892A64DF1CB15712B736E3374F1BBB6819371602D8970E97B900793C7C2A89A4A1649A59BE680574DD0B60145",
                                    }
                            },
                    }
                );
        }
    }

    public class EmvModuleDeleteApplicationPublicKeysMethod : TerminalDeviceMethod<DeleteApplicationPublicKeysCommand, DeleteApplicationPublicKeysResponse>
    {
        public EmvModuleDeleteApplicationPublicKeysMethod(ITerminalDevice device)
            : base(device, "DeleteApplicationPublicKeys")
        {
            InvokeCommand = new TerminalDeviceCommand<DeleteApplicationPublicKeysCommand, DeleteApplicationPublicKeysResponse>(
                this,
                Name,
                () => new DeleteApplicationPublicKeysCommand
                    {
                        Rid = "A000000003",
                    }
                );
        }
    }

    public class EmvModuleClearAllApplicationsAndKeysMethod : TerminalDeviceMethod<ClearAllApplicationsAndKeysCommand, ClearAllApplicationsAndKeysResponse>
    {
        public EmvModuleClearAllApplicationsAndKeysMethod(ITerminalDevice device)
            : base(device, "ClearAllApplicationsAndKeys")
        {
            InvokeCommand = new TerminalDeviceCommand<ClearAllApplicationsAndKeysCommand, ClearAllApplicationsAndKeysResponse>(
                this,
                Name
                );
        }
    }

    #endregion

    #region Events

    public class EmvModuleStatusChangedEvent : TerminalDeviceEvent<EMVModuleStatusChanged>
    {
        public EmvModuleStatusChangedEvent(ITerminalDevice device)
            : base(device, "StatusChanged")
        {
        }
    }

    public class EmvModuleReenterOnlinePinFailedEvent : TerminalDeviceEvent<ReenterOnlinePinFailedData>
    {
        public EmvModuleReenterOnlinePinFailedEvent(ITerminalDevice device)
            : base(device, "ReenterOnlinePinFailed")
        {
        }
    }

    public class EmvModuleApplicationSelectedEvent : TerminalDeviceEvent<ApplicationSelectedData>
    {
        public EmvModuleApplicationSelectedEvent(ITerminalDevice device)
            : base(device, "ApplicationSelected")
        {
        }
    }

    public class EmvModuleEmvModuleCardDetailsEvent : TerminalDeviceEvent<CardDetailsData>
    {
        public EmvModuleEmvModuleCardDetailsEvent(ITerminalDevice device)
            : base(device, "CardDetails")
        {
        }
    }

    public class EmvModuleOnlineApprovalRequestDataEvent : TerminalDeviceEvent<OnlineApprovalRequestData>
    {
        public EmvModuleOnlineApprovalRequestDataEvent(ITerminalDevice device)
            : base(device, "OnlineApprovalRequest")
        {
        }
    }

    public class EmvModuleTransactionResultEvent : TerminalDeviceEvent<TransactionResultData>
    {
        public EmvModuleTransactionResultEvent(ITerminalDevice device)
            : base(device, "TransactionResult")
        {
        }
    }

    public class EmvModuleCardDataReadEvent : TerminalDeviceEvent<CardDataReadData>
    {
        public EmvModuleCardDataReadEvent(ITerminalDevice device)
            : base(device, "CardDataRead")
        {
        }
    }

    public class EmvModuleRemovedApplicationEvent : TerminalDeviceEvent<RemovedApplicationData>
    {
        public EmvModuleRemovedApplicationEvent(ITerminalDevice device)
            : base(device, "RemovedApplication")
        {
        }
    }

    #endregion
}
