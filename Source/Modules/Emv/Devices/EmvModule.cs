using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Emv.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class EmvModule : TerminalDevice<EMVModuleCommand, EMVModuleResponse>
    {
        public EmvModule()
            : base("EmvModule")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new StatusProperty(this),
                    new ApplicationExclusionListProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new SetTimeoutMethod(this),
                    new InitiateTransactionMethod(this),
                    new SetAmountsMethod(this),
                    new ContinueTransactionMethod(this),
                    new ReenterOnlinePinMethod(this),
                    new CompleteOnlineTransactionMethod(this),
                    new ReadCardDataMethod(this),
                    new AbortTransactionMethod(this),
                    new DownloadApplicationMethod(this),
                    new DeleteApplicationMethod(this),
                    new DownloadApplicationPublicKeysMethod(this),
                    new DeleteApplicationPublicKeysMethod(this),
                    new ClearAllApplicationsAndKeysMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new StatusChangedEvent(this),
                    new ReenterOnlinePinFailedEvent(this),
                    new ApplicationSelectedEvent(this),
                    new CardDetailsEvent(this),
                    new OnlineApprovalRequestDataEvent(this),
                    new TransactionResultEvent(this),
                    new CardDataReadEvent(this),
                    new RemovedApplicationEvent(this),
                });
        }

        #region Device Properties

        [ValueProperty("EMVModuleState")]
        public class StatusProperty : TerminalDeviceProperty<EMVModuleStatus,
            GetEMVModuleStatusCommand, GetEMVModuleStatusResponse>
        {
            public StatusProperty(ITerminalDevice device)
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

                SetCommand = new TerminalDeviceCommand
                    <SetApplicationExclusionListCommand, SetApplicationExclusionListResponse>(
                    this,
                    $"set_{Name}",
                    () => new SetApplicationExclusionListCommand
                        {
                            ApplicationExclusion = new[]
                                {
                                    new ApplicationExclusion {App = "A0000002771010", ExcludeAll = true},
                                    new ApplicationExclusion {App = "A0000000050001", Exclude = "A0000000043060"},
                                }
                        }
                    );
            }
        }

        #endregion

        #region Device Methods

        public class SetTimeoutMethod : TerminalDeviceMethod<SetTimeoutCommand, SetTimeoutResponse>
        {
            public SetTimeoutMethod(ITerminalDevice device)
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

        public class InitiateTransactionMethod : TerminalDeviceMethod<InitiateTransactionCommand, InitiateTransactionResponse>
        {
            public InitiateTransactionMethod(ITerminalDevice device)
                : base(device, "InitiateTransaction")
            {
                InvokeCommand = new TerminalDeviceCommand<InitiateTransactionCommand, InitiateTransactionResponse>(
                    this,
                    Name,
                    () => new InitiateTransactionCommand
                        {
                            EmvDataElement = new[]
                                {
                                    new EmvDataElement
                                        {
                                            Value = new byte[] {0x9F, 0x02},
                                        },
                                    new EmvDataElement
                                        {
                                            Value = new byte[] {0xC2},
                                        },
                                }
                        }
                    );
            }
        }

        public class SetAmountsMethod : TerminalDeviceMethod<SetAmountsCommand, SetAmountsResponse>
        {
            public SetAmountsMethod(ITerminalDevice device)
                : base(device, "SetAmounts")
            {
                InvokeCommand = new TerminalDeviceCommand<SetAmountsCommand, SetAmountsResponse>(
                    this,
                    Name,
                    () => new SetAmountsCommand
                        {
                            EmvDataElement = new[]
                                {
                                    new EmvDataElement
                                        {
                                            Value = new byte[] {0x9F, 0x02},
                                        },
                                    new EmvDataElement
                                        {
                                            Value = new byte[] {0xC2},
                                        },
                                }
                        }
                    );
            }
        }

        public class ContinueTransactionMethod : TerminalDeviceMethod<ContinueTransactionCommand, ContinueTransactionResponse>
        {
            public ContinueTransactionMethod(ITerminalDevice device)
                : base(device, "ContinueTransaction")
            {
                InvokeCommand = new TerminalDeviceCommand<ContinueTransactionCommand, ContinueTransactionResponse>(
                    this,
                    Name,
                    () => new ContinueTransactionCommand
                        {
                            EmvDataElement = new[]
                                {
                                    new EmvDataElement
                                        {
                                            Value = new byte[] {0xC1, 0x01, 0xFF},
                                        },
                                }
                        }
                    );
            }
        }

        public class ReenterOnlinePinMethod : TerminalDeviceMethod<ReenterOnlinePINCommand, ReenterOnlinePINResponse>
        {
            public ReenterOnlinePinMethod(ITerminalDevice device)
                : base(device, "ReenterOnlinePin")
            {
                InvokeCommand = new TerminalDeviceCommand<ReenterOnlinePINCommand, ReenterOnlinePINResponse>(
                    this,
                    Name
                    );
            }
        }

        public class CompleteOnlineTransactionMethod : TerminalDeviceMethod<CompleteOnlineTransactionCommand, CompleteOnlineTransactionResponse>
        {
            public CompleteOnlineTransactionMethod(ITerminalDevice device)
                : base(device, "CompleteOnlineTransaction")
            {
                InvokeCommand = new TerminalDeviceCommand
                    <CompleteOnlineTransactionCommand, CompleteOnlineTransactionResponse>(
                    this,
                    Name,
                    () => new CompleteOnlineTransactionCommand
                        {
                            EmvDataElement = new[]
                                {
                                    new EmvDataElement
                                        {
                                            Value = new byte[] {0x8A, 0x2, 0x30, 0x30},
                                        },
                                }
                        }
                    );
            }
        }

        public class ReadCardDataMethod : TerminalDeviceMethod<ReadCardDataCommand, ReadCardDataResponse>
        {
            public ReadCardDataMethod(ITerminalDevice device)
                : base(device, "ReadCardData")
            {
                InvokeCommand = new TerminalDeviceCommand<ReadCardDataCommand, ReadCardDataResponse>(
                    this,
                    Name,
                    () => new ReadCardDataCommand
                        {
                            EmvDataElement = new[]
                                {
                                    new EmvDataElement
                                        {
                                            Value = new byte[] {0x9F, 0x02},
                                        },
                                }
                        }
                    );
            }
        }

        public class AbortTransactionMethod : TerminalDeviceMethod<AbortTransactionCommand, AbortTransactionResponse>
        {
            public AbortTransactionMethod(ITerminalDevice device)
                : base(device, "AbortTransaction")
            {
                InvokeCommand = new TerminalDeviceCommand<AbortTransactionCommand, AbortTransactionResponse>(
                    this,
                    Name
                    );
            }
        }

        public class DownloadApplicationMethod :
            TerminalDeviceMethod<DownloadApplicationCommand, DownloadApplicationResponse>
        {
            public DownloadApplicationMethod(ITerminalDevice device)
                : base(device, "DownloadApplication")
            {
                InvokeCommand = new TerminalDeviceCommand<DownloadApplicationCommand, DownloadApplicationResponse>(
                    this,
                    Name,
                    () =>
                    {
                        var uiService = ServiceLocator.Current.GetInstance<IUiService>();

                        var cmd = new DownloadApplicationCommand
                            {
                                SelectionIndicator = ConvertHelper.ToHexByteArray("01"),
                                TDOL = ConvertHelper.ToHexByteArray("9F02065F2A029A039C0195059F3704"),
                                DDOL = ConvertHelper.ToHexByteArray("9F3704"),
                            };

                        switch (uiService.SelectedCardBrand)
                        {
                            case CardBrand.MasterCard:
                                cmd.Name = "Mastercard";
                                cmd.Rid = ConvertHelper.ToHexByteArray("A000000004");
                                cmd.Pix = ConvertHelper.ToHexByteArray("1010");
                                cmd.Version = ConvertHelper.ToHexByteArray("02");
                                cmd.TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("F850A8A000");
                                cmd.TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0000000000");
                                cmd.TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("F850A8F800");
                                break;
                            case CardBrand.Visa:
                                cmd.Name = "Visa";
                                cmd.Rid = ConvertHelper.ToHexByteArray("A000000003");
                                cmd.Pix = ConvertHelper.ToHexByteArray("1010");
                                cmd.Version = ConvertHelper.ToHexByteArray("84");
                                cmd.TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("D84000A800");
                                cmd.TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0010000000");
                                cmd.TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("D84004F800");
                                break;
                            case CardBrand.Amex:
                                cmd.Name = "Amex";
                                cmd.Rid = ConvertHelper.ToHexByteArray("A000000025");
                                cmd.Pix = ConvertHelper.ToHexByteArray("01");
                                cmd.Version = ConvertHelper.ToHexByteArray("01");
                                cmd.TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("C800000000");
                                cmd.TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0000000000");
                                cmd.TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("C800000000");
                                break;
                            case CardBrand.Discover:
                                cmd.Name = "Discover";
                                cmd.Rid = ConvertHelper.ToHexByteArray("A000000152");
                                cmd.Pix = ConvertHelper.ToHexByteArray("10");
                                cmd.Version = ConvertHelper.ToHexByteArray("01");
                                cmd.TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("0000000000");
                                cmd.TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0000000000");
                                cmd.TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("0000000000");
                                break;
                            case CardBrand.Interac:
                                cmd.Name = "Interac";
                                cmd.Rid = ConvertHelper.ToHexByteArray("A000000277");
                                cmd.Pix = ConvertHelper.ToHexByteArray("1010");
                                cmd.Version = ConvertHelper.ToHexByteArray("01");
                                cmd.TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("0000000000");
                                cmd.TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0000000000");
                                cmd.TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("0000000000");
                                break;
                            case CardBrand.Jcb:
                                cmd.Name = "Interac";
                                cmd.Rid = ConvertHelper.ToHexByteArray("A000000065");
                                cmd.Pix = ConvertHelper.ToHexByteArray("1010");
                                cmd.Version = ConvertHelper.ToHexByteArray("0200");
                                cmd.TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("F860242800");
                                cmd.TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0010000000");
                                cmd.TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("F860ACF800");
                                break;
                        }

                        return cmd;
                    }
                    );
            }
        }

        public class DeleteApplicationMethod : TerminalDeviceMethod<DeleteApplicationCommand, DeleteApplicationResponse>
        {
            public DeleteApplicationMethod(ITerminalDevice device)
                : base(device, "DeleteApplication")
            {
                InvokeCommand = new TerminalDeviceCommand<DeleteApplicationCommand, DeleteApplicationResponse>(
                    this,
                    Name,
                    () =>
                    {
                        var uiService = ServiceLocator.Current.GetInstance<IUiService>();

                        var cmd = new DeleteApplicationCommand();

                        switch (uiService.SelectedCardBrand)
                        {
                            case CardBrand.MasterCard:
                                cmd.Rid = "A000000004";
                                cmd.Pix = "1010";
                                cmd.Version = "02";
                                break;
                            case CardBrand.Visa:
                                cmd.Rid = "A000000003";
                                cmd.Pix = "1010";
                                cmd.Version = "84";
                                break;
                            case CardBrand.Amex:
                                cmd.Rid = "A000000025";
                                cmd.Pix = "01";
                                cmd.Version = "01";
                                break;
                            case CardBrand.Discover:
                                cmd.Rid = "A000000152";
                                cmd.Pix = "10";
                                cmd.Version = "01";
                                break;
                            case CardBrand.Interac:
                                cmd.Rid = "A000000277";
                                cmd.Pix = "1010";
                                cmd.Version = "01";
                                break;
                            case CardBrand.Jcb:
                                cmd.Rid = "A000000065";
                                cmd.Pix = "1010";
                                cmd.Version = "0200";
                                break;
                        }

                        return cmd;
                    }
                    );
            }
        }

        public class DownloadApplicationPublicKeysMethod : TerminalDeviceMethod<DownloadApplicationPublicKeysCommand, DownloadApplicationPublicKeysResponse>
        {
            public DownloadApplicationPublicKeysMethod(ITerminalDevice device)
                : base(device, "DownloadApplicationPublicKeys")
            {
                InvokeCommand = new TerminalDeviceCommand
                    <DownloadApplicationPublicKeysCommand, DownloadApplicationPublicKeysResponse>(
                    this,
                    Name,
                    () =>
                    {
                        var uiService = ServiceLocator.Current.GetInstance<IUiService>();

                        var cmd = new DownloadApplicationPublicKeysCommand();

                        switch (uiService.SelectedCardBrand)
                        {
                            case CardBrand.MasterCard:
                                cmd.Rid = "A000000004";
                                cmd.KeyDefinition = new[]
                                    {
                                        new KeyDefinition
                                            {
                                                SignAlgorithm = "01",
                                                Number = "09",
                                                Exponent = "03",
                                                Modulus =
                                                    "C2490747FE17EB0584C88D47B1602704150ADC88C5B998BD59CE043EDEBF0FFEE3093AC7956AD3B6AD4554C6DE19A178D6DA295BE15D5220645E3C8131666FA4BE5B84FE131EA44B039307638B9E74A8C42564F892A64DF1CB15712B736E3374F1BBB6819371602D8970E97B900793C7C2A89A4A1649A59BE680574DD0B60145"
                                            },
                                    };
                                break;
                            case CardBrand.Visa:
                                cmd.Rid = "A000000003";
                                cmd.KeyDefinition = new[]
                                    {
                                        new KeyDefinition
                                            {
                                                SignAlgorithm = "01",
                                                Number = "09",
                                                Exponent = "03",
                                                Modulus =
                                                    "C696034213D7D8546984579D1D0F0EA519CFF8DEFFC429354CF3A871A6F7183F1228DA5C7470C055387100CB935A712C4E2864DF5D64BA93FE7E63E71F25B1E5F5298575EBE1C63AA617706917911DC2A75AC28B251C7EF40F2365912490B939BCA2124A30A28F54402C34AECA331AB67E1E79B285DD5771B5D9FF79EA630B75"
                                            },
                                    };
                                break;
                            case CardBrand.Amex:
                                cmd.Rid = "A000000025";
                                cmd.KeyDefinition = new[]
                                    {
                                        new KeyDefinition
                                            {
                                                SignAlgorithm = "01",
                                                Number = "09",
                                                Exponent = "03",
                                                Modulus =
                                                    "AF4B8D230FDFCB1538E975795A1DB40C396A5359FAA31AE095CB522A5C82E7FFFB252860EC2833EC3D4A665F133DD934EE1148D81E2B7E03F92995DDF7EB7C90A75AB98E69C92EC91A533B21E1C4918B43AFED5780DE13A32BBD37EBC384FA3DD1A453E327C56024DACAEA74AA052C4D"
                                            },
                                    };
                                break;
                            case CardBrand.Discover:
                                cmd.Rid = "A000000152";
                                cmd.KeyDefinition = new[]
                                    {
                                        new KeyDefinition
                                            {
                                                SignAlgorithm = "01",
                                                Number = "09",
                                                Exponent = "03",
                                                Modulus =
                                                    "C2490747FE17EB0584C88D47B1602704150ADC88C5B998BD59CE043EDEBF0FFEE3093AC7956AD3B6AD4554C6DE19A178D6DA295BE15D5220645E3C8131666FA4BE5B84FE131EA44B039307638B9E74A8C42564F892A64DF1CB15712B736E3374F1BBB6819371602D8970E97B900793C7C2A89A4A1649A59BE680574DD0B60145"
                                            },
                                    };
                                break;
                            case CardBrand.Interac:
                                cmd.Rid = "A000000277";
                                cmd.KeyDefinition = new[]
                                    {
                                        new KeyDefinition
                                            {
                                                SignAlgorithm = "01",
                                                Number = "09",
                                                Exponent = "03",
                                                Modulus =
                                                    "f802c308544873ad2225a81943732a4b7cffa4e3157d17cd5a7723f858f0b11e636d2930fa933778f27c7c49127e0cca317021cfe8e0f773785eb3ff07587e98ce8ed4fe9e1ca1859f41a9cf2572d8a093c5465f5a29612a45b1700f4da13814c3d4df075eaade8db4be4d7b3ae0256f7a0c12e34bd416cac4f9250c38b7e13b"
                                            },
                                    };
                                break;
                            case CardBrand.Jcb:
                                cmd.Rid = "A000000065";
                                cmd.KeyDefinition = new[]
                                    {
                                        new KeyDefinition
                                            {
                                                SignAlgorithm = "01",
                                                Number = "09",
                                                Exponent = "03",
                                                Modulus =
                                                    "99B63464EE0B4957E4FD23BF923D12B61469B8FFF8814346B2ED6A780F8988EA9CF0433BC1E655F05EFA66D0C98098F25B659D7A25B8478A36E489760D071F54CDF7416948ED733D816349DA2AADDA227EE45936203CBF628CD033AABA5E5A6E4AE37FBACB4611B4113ED427529C636F6C3304F8ABDD6D9AD660516AE87F7F2DDF1D2FA44C164727E56BBC9BA23C0285"
                                            },
                                    };
                                break;
                        }

                        return cmd;
                    }
                    );
            }
        }

        public class DeleteApplicationPublicKeysMethod : TerminalDeviceMethod<DeleteApplicationPublicKeysCommand, DeleteApplicationPublicKeysResponse>
        {
            public DeleteApplicationPublicKeysMethod(ITerminalDevice device)
                : base(device, "DeleteApplicationPublicKeys")
            {
                InvokeCommand = new TerminalDeviceCommand<DeleteApplicationPublicKeysCommand, DeleteApplicationPublicKeysResponse>(
                    this,
                    Name,
                    () =>
                    {
                        var uiService = ServiceLocator.Current.GetInstance<IUiService>();

                        var cmd = new DeleteApplicationPublicKeysCommand();

                        switch (uiService.SelectedCardBrand)
                        {
                            case CardBrand.MasterCard:
                                cmd.Rid = "A000000004";
                                break;
                            case CardBrand.Visa:
                                cmd.Rid = "A000000003";
                                break;
                            case CardBrand.Amex:
                                cmd.Rid = "A000000025";
                                break;
                            case CardBrand.Discover:
                                cmd.Rid = "A000000152";
                                break;
                            case CardBrand.Interac:
                                cmd.Rid = "A000000277";
                                break;
                            case CardBrand.Jcb:
                                cmd.Rid = "A000000065";
                                break;
                        }

                        return cmd;
                    }
                    );
            }
        }

        public class ClearAllApplicationsAndKeysMethod : TerminalDeviceMethod<ClearAllApplicationsAndKeysCommand, ClearAllApplicationsAndKeysResponse>
        {
            public ClearAllApplicationsAndKeysMethod(ITerminalDevice device)
                : base(device, "ClearAllApplicationsAndKeys")
            {
                InvokeCommand = new TerminalDeviceCommand<ClearAllApplicationsAndKeysCommand, ClearAllApplicationsAndKeysResponse>(
                    this,
                    Name
                    );
            }
        }

        #endregion

        #region Device Events

        public class StatusChangedEvent : TerminalDeviceEvent<EMVModuleStatusChanged>
        {
            public StatusChangedEvent(ITerminalDevice device)
                : base(device, "StatusChanged")
            {
            }
        }

        public class ReenterOnlinePinFailedEvent : TerminalDeviceEvent<ReenterOnlinePinFailedData>
        {
            public ReenterOnlinePinFailedEvent(ITerminalDevice device)
                : base(device, "ReenterOnlinePinFailed")
            {
            }
        }

        public class ApplicationSelectedEvent : TerminalDeviceEvent<ApplicationSelectedData>
        {
            public ApplicationSelectedEvent(ITerminalDevice device)
                : base(device, "ApplicationSelected")
            {
            }
        }

        public class CardDetailsEvent : TerminalDeviceEvent<CardDetailsData>
        {
            public CardDetailsEvent(ITerminalDevice device)
                : base(device, "CardDetails")
            {
            }
        }

        public class OnlineApprovalRequestDataEvent : TerminalDeviceEvent<OnlineApprovalRequestData>
        {
            public OnlineApprovalRequestDataEvent(ITerminalDevice device)
                : base(device, "OnlineApprovalRequest")
            {
            }
        }

        public class TransactionResultEvent : TerminalDeviceEvent<TransactionResultData>
        {
            public TransactionResultEvent(ITerminalDevice device)
                : base(device, "TransactionResult")
            {
            }
        }

        public class CardDataReadEvent : TerminalDeviceEvent<CardDataReadData>
        {
            public CardDataReadEvent(ITerminalDevice device)
                : base(device, "CardDataRead")
            {
            }
        }

        public class RemovedApplicationEvent : TerminalDeviceEvent<RemovedApplicationData>
        {
            public RemovedApplicationEvent(ITerminalDevice device)
                : base(device, "RemovedApplication")
            {
            }
        }

        #endregion
    }
}
