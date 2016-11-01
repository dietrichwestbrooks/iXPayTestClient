using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Emv.Devices
{
    public class EmvModule
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<EMVModuleCommand, EMVModuleResponse>(
                    "EmvModule", new TerminalRequestHandlerByName("Terminal"), typeof(EmvModule));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<EMVModuleStatus, GetEMVModuleStatusCommand, GetEMVModuleStatusResponse>("Status",
                "EMVModuleState", typeof(EmvModule));

        public static readonly TerminalDeviceProperty
            ApplicationExclusionListProperty =
                TerminalDeviceProperty.Register<ApplicationExclusion[],
                    GetApplicationExclusionListCommand, GetApplicationExclusionListResponse,
                    SetApplicationExclusionListCommand, SetApplicationExclusionListResponse>(
                    "ApplicationExclusionList", "ApplicationExclusion",
                        typeof (EmvModule), null, () => new SetApplicationExclusionListCommand
                            {
                                ApplicationExclusion = new[]
                                    {
                                        new ApplicationExclusion {App = "A0000002771010", ExcludeAll = true},
                                        new ApplicationExclusion {App = "A0000000050001", Exclude = "A0000000043060"},
                                    }
                            });

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod SetTimeoutMethod =
            TerminalDeviceMethod.Register<SetTimeoutCommand, SetTimeoutResponse>("SetTimeout",
                typeof (EmvModule), () => new SetTimeoutCommand
                    {
                        TimeoutValue = 60
                    });

        public static readonly TerminalDeviceMethod InitiateTransactionMethod =
            TerminalDeviceMethod.Register<InitiateTransactionCommand, InitiateTransactionResponse>(
                "InitiateTransaction",
                typeof (EmvModule), () => new InitiateTransactionCommand
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
                    });

        public static readonly TerminalDeviceMethod SetAmountsMethod =
            TerminalDeviceMethod.Register<SetAmountsCommand, SetAmountsResponse>("SetAmounts",
                typeof (EmvModule), () => new SetAmountsCommand
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
                    });

        public static readonly TerminalDeviceMethod ContinueTransactionMethod =
            TerminalDeviceMethod.Register<ContinueTransactionCommand, ContinueTransactionResponse>(
                "ContinueTransaction",
                typeof (EmvModule), () => new ContinueTransactionCommand
                    {
                        EmvDataElement = new[]
                            {
                                new EmvDataElement
                                    {
                                        Value = new byte[] {0xC1, 0x01, 0xFF},
                                    },
                            }
                    });

        public static readonly TerminalDeviceMethod ReenterOnlinePinMethod =
            TerminalDeviceMethod.Register<ReenterOnlinePINCommand, ReenterOnlinePINResponse>(
                "ReenterOnlinePin",
                typeof(EmvModule));

        public static readonly TerminalDeviceMethod CompleteOnlineTransactionMethod =
            TerminalDeviceMethod.Register<CompleteOnlineTransactionCommand, CompleteOnlineTransactionResponse>(
                "CompleteOnlineTransaction",
                typeof (EmvModule), () => new CompleteOnlineTransactionCommand
                    {
                        EmvDataElement = new[]
                            {
                                new EmvDataElement
                                    {
                                        Value = new byte[] {0x8A, 0x2, 0x30, 0x30},
                                    },
                            }
                    });

        public static readonly TerminalDeviceMethod ReadCardDataMethod =
            TerminalDeviceMethod.Register<ReadCardDataCommand, ReadCardDataResponse>(
                "ReadCardData",
                typeof (EmvModule), () => new ReadCardDataCommand
                    {
                        EmvDataElement = new[]
                            {
                                new EmvDataElement
                                    {
                                        Value = new byte[] {0x9F, 0x02},
                                    },
                            }
                    });

        public static readonly TerminalDeviceMethod AbortTransactionMethod =
            TerminalDeviceMethod.Register<AbortTransactionCommand, AbortTransactionResponse>(
                "AbortTransaction",
                typeof(EmvModule));

        public static readonly TerminalDeviceMethod DownloadApplicationMethod =
            TerminalDeviceMethod.Register<DownloadApplicationCommand, DownloadApplicationResponse>(
                "DownloadApplication",
                typeof(EmvModule), PrepareDownloadApplicationCommand);

        private static DownloadApplicationCommand PrepareDownloadApplicationCommand()
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

        public static readonly TerminalDeviceMethod DeleteApplicationMethod =
           TerminalDeviceMethod.Register<DeleteApplicationCommand, DeleteApplicationResponse>(
               "DeleteApplication",
               typeof(EmvModule), PrepareDeleteApplicationCommand);

        private static DeleteApplicationCommand PrepareDeleteApplicationCommand()
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

        public static readonly TerminalDeviceMethod DownloadApplicationPublicKeysMethod =
           TerminalDeviceMethod.Register<DownloadApplicationPublicKeysCommand, DownloadApplicationPublicKeysResponse>(
               "DownloadApplicationPublicKeys",
               typeof(EmvModule), PrepareDownloadApplicationPublicKeysCommand);

        private static DownloadApplicationPublicKeysCommand PrepareDownloadApplicationPublicKeysCommand()
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

        public static readonly TerminalDeviceMethod DeleteApplicationPublicKeysMethod =
           TerminalDeviceMethod.Register<DeleteApplicationPublicKeysCommand, DeleteApplicationPublicKeysResponse>(
               "DeleteApplicationPublicKeys",
               typeof(EmvModule), PrepareDeleteApplicationPublicKeysCommand);

        private static DeleteApplicationPublicKeysCommand PrepareDeleteApplicationPublicKeysCommand()
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

        public static readonly TerminalDeviceMethod ClearAllApplicationsAndKeysMethod =
          TerminalDeviceMethod.Register<ClearAllApplicationsAndKeysCommand, ClearAllApplicationsAndKeysResponse>(
              "ClearAllApplicationsAndKeys",
              typeof(EmvModule));

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent StatusChangedEvent =
           TerminalDeviceEvent.Register<EMVModuleStatusChanged>("StatusChanged", typeof(EmvModule));

        public static readonly TerminalDeviceEvent ReenterOnlinePinFailedEvent =
           TerminalDeviceEvent.Register<ReenterOnlinePinFailedData>("ReenterOnlinePinFailed", typeof(EmvModule));

        public static readonly TerminalDeviceEvent ApplicationSelectedEvent =
           TerminalDeviceEvent.Register<ApplicationSelectedData>("ApplicationSelected", typeof(EmvModule));

        public static readonly TerminalDeviceEvent CardDetailsEvent =
           TerminalDeviceEvent.Register<CardDetailsData>("CardDetails", typeof(EmvModule));

        public static readonly TerminalDeviceEvent OnlineApprovalRequestDataEvent =
           TerminalDeviceEvent.Register<OnlineApprovalRequestData>("OnlineApprovalRequest", typeof(EmvModule));

        public static readonly TerminalDeviceEvent TransactionResultEvent =
           TerminalDeviceEvent.Register<TransactionResultData>("TransactionResult", typeof(EmvModule));

        public static readonly TerminalDeviceEvent CardDataReadEvent =
           TerminalDeviceEvent.Register<CardDataReadData>("CardDataRead", typeof(EmvModule));

        public static readonly TerminalDeviceEvent RemovedApplicationEvent =
           TerminalDeviceEvent.Register<RemovedApplicationData>("RemovedApplication", typeof(EmvModule));

        #endregion
    }
}
