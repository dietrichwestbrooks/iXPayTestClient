using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class ContactlessReader
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<ContactlessReaderCommand, ContactlessReaderResponse, ContactlessReaderEvent>(
                    "ContactlessReader", new TerminalRequestHandlerByName("Terminal"), typeof(ContactlessReader));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(ContactlessReader));

        public static readonly TerminalDeviceProperty OpenedProperty =
            TerminalDeviceProperty.Register<bool, GetOpenedCommand, GetOpenedResponse>("Opened",
                "Open", typeof(ContactlessReader));

        public static readonly TerminalDeviceProperty CapabilitiesProperty =
            TerminalDeviceProperty.Register<bool, GetContactlessReaderCapabilitiesCommand, GetContactlessReaderCapabilitiesResponse>(
                "Capabilities", "SupportedPaymentScheme",
                typeof(ContactlessReader));

        public static readonly TerminalDeviceProperty OperationalStateProperty =
            TerminalDeviceProperty.Register<bool, GetContactlessOperationalStateCommand, GetContactlessOperationalStateResponse>(
                "OperationalState", "OperationalState",
                typeof(ContactlessReader));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod GetGlobalEmvDataMethod =
         TerminalDeviceMethod.Register<GetGlobalEMVDataCommand, GetGlobalEMVDataResponse>("GetGlobalEMVData",
             typeof(ContactlessReader));

        public static readonly TerminalDeviceMethod SetGlobalEmvDataMethod =
         TerminalDeviceMethod.Register<SetGlobalEMVDataCommand, SetGlobalEMVDataResponse>("SetGlobalEMVData",
             typeof(ContactlessReader), () => new SetGlobalEMVDataCommand
                 {
                     EmvDataElement = new []
                         {
                             new EmvDataElement
                                 {
                                     Value = ConvertHelper.ToHexByteArray("5F360102"),
                                 },
                             new EmvDataElement
                                 {
                                     Value = ConvertHelper.ToHexByteArray("5F2A020840"),
                                 },
                             new EmvDataElement
                                 {
                                     Value = ConvertHelper.ToHexByteArray("9F1A020840"),
                                 },
                         }
             });

        public static readonly TerminalDeviceMethod SetBrandTerminalCapabilitiesMethod =
         TerminalDeviceMethod.Register<SetBrandTerminalCapabilitiesCommand, SetBrandTerminalCapabilitiesResponse>(
             "SetBrandTerminalCapabilities",
             typeof(ContactlessReader), PrepareSetBrandTerminalCapabilitiesCommand);

        private static SetBrandTerminalCapabilitiesCommand PrepareSetBrandTerminalCapabilitiesCommand()
        {
            var uiService = ServiceLocator.Current.GetInstance<IUiService>();

            var cmd = new SetBrandTerminalCapabilitiesCommand();

            switch (uiService.SelectedCardBrand)
            {
                case CardBrand.MasterCard:
                    cmd.Name = BrandName.MasterCard;
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F3303406888"),
                                },
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F350121"),
                                },
                        };
                    break;

                case CardBrand.Visa:
                    cmd.Name = BrandName.Visa;
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F3303406840"),
                                },
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F350122"),
                                },
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F6604A6004000"),
                                },
                        };
                    break;

                case CardBrand.Amex:
                    cmd.Name = BrandName.AmericanExpress;
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F3303406088"),
                                },
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F350122"),
                                },
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F6D01C0"),
                                },
                        };
                    break;

                case CardBrand.Discover:
                    cmd.Name = BrandName.Discover;
                    // none for Discover
                    break;

                case CardBrand.Interac:
                    cmd.Name = BrandName.Interac;
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F3303004808"),
                                },
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F350122"),
                                },
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F40056000B01001"),
                                },
                            new EmvDataElement
                                {
                                    Value = ConvertHelper.ToHexByteArray("9F59039C0000"),
                                },
                        };
                    break;

                case CardBrand.Jcb:
                    //mNotSupportedLabel.Visible = true;
                    //_commandNotSupported = true;
                    break;
            }

            return cmd;
        }

        public static readonly TerminalDeviceMethod GetBrandTerminalCapabilitiesMethod =
         TerminalDeviceMethod.Register<GetBrandTerminalCapabilitiesCommand, GetBrandTerminalCapabilitiesResponse>(
             "GetBrandTerminalCapabilities",
             typeof(ContactlessReader), PrepareGetBrandTerminalCapabilities);

        private static GetBrandTerminalCapabilitiesCommand PrepareGetBrandTerminalCapabilities()
        {
            var uiService = ServiceLocator.Current.GetInstance<IUiService>();

            var cmd = new GetBrandTerminalCapabilitiesCommand();

            switch (uiService.SelectedCardBrand)
            {
                case CardBrand.MasterCard:
                    cmd.Name = BrandName.MasterCard;
                    break;

                case CardBrand.Visa:
                    cmd.Name = BrandName.Visa;
                    break;

                case CardBrand.Amex:
                    cmd.Name = BrandName.AmericanExpress;
                    break;

                case CardBrand.Discover:
                    cmd.Name = BrandName.Discover;
                    break;

                case CardBrand.Interac:
                    cmd.Name = BrandName.Interac;
                    break;
            }

            return cmd;
        }

        public static readonly TerminalDeviceMethod GetBrandModeMethod =
         TerminalDeviceMethod.Register<GetBrandModeCommand, GetBrandModeResponse>(
             "GetBrandMode",
             typeof(ContactlessReader), PrepareGetBrandModeCommand);

        private static GetBrandModeCommand PrepareGetBrandModeCommand()
        {
            var uiService = ServiceLocator.Current.GetInstance<IUiService>();

            var cmd = new GetBrandModeCommand();

            switch (uiService.SelectedCardBrand)
            {
                case CardBrand.MasterCard:
                    cmd.Name = BrandName.MasterCard;
                    break;

                case CardBrand.Visa:
                    cmd.Name = BrandName.Visa;
                    break;

                case CardBrand.Amex:
                    cmd.Name = BrandName.AmericanExpress;
                    break;

                case CardBrand.Discover:
                    cmd.Name = BrandName.Discover;
                    break;

                case CardBrand.Interac:
                    cmd.Name = BrandName.Interac;
                    break;
            }

            return cmd;
        }

        public static readonly TerminalDeviceMethod SetBrandModeMethod =
         TerminalDeviceMethod.Register<SetBrandModeCommand, SetBrandModeResponse>(
             "SetBrandMode",
             typeof(ContactlessReader), PrepareSetBrandModeCommand);

        private static SetBrandModeCommand PrepareSetBrandModeCommand()
        {
            var uiService = ServiceLocator.Current.GetInstance<IUiService>();

            var cmd = new SetBrandModeCommand();

            switch (uiService.SelectedCardBrand)
            {
                case CardBrand.MasterCard:
                    cmd.Name = BrandName.MasterCard;
                    cmd.Mode = 0;
                    break;

                case CardBrand.Visa:
                    cmd.Name = BrandName.Visa;
                    cmd.Mode = 0x24;
                    break;

                case CardBrand.Amex:
                    cmd.Name = BrandName.AmericanExpress;
                    cmd.Mode = 0;
                    break;

                case CardBrand.Discover:
                    cmd.Name = BrandName.Discover;
                    cmd.Mode = 0;
                    break;

                case CardBrand.Interac:
                    cmd.Name = BrandName.Interac;
                    cmd.Mode = 0;
                    break;
            }

            return cmd;
        }

        public static readonly TerminalDeviceMethod GetApplicationMethod =
         TerminalDeviceMethod.Register<GetContactlessApplicationCommand, GetContactlessApplicationResponse>(
             "GetApplication",
             typeof(ContactlessReader), PrepareGetContactlessApplicationCommand);

        private static GetContactlessApplicationCommand PrepareGetContactlessApplicationCommand()
        {
            var uiService = ServiceLocator.Current.GetInstance<IUiService>();

            var cmd = new GetContactlessApplicationCommand();

            switch (uiService.SelectedCardBrand)
            {
                case CardBrand.MasterCard:
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000000041010");
                    cmd.ApplicationIndex = 0;
                    break;

                case CardBrand.Visa:
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000000031010");
                    cmd.ApplicationIndex = 1;
                    break;

                case CardBrand.Amex:
                    cmd.AID = ConvertHelper.ToHexByteArray("A00000002501");
                    cmd.ApplicationIndex = 2;
                    break;

                case CardBrand.Discover:
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000003241010");
                    cmd.ApplicationIndex = 4;
                    break;

                case CardBrand.Interac:
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000002771010");
                    cmd.ApplicationIndex = 5;
                    break;

                case CardBrand.Jcb:
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000000651010");
                    cmd.ApplicationIndex = 6;
                    break;
            }

            return cmd;
        }

        public static readonly TerminalDeviceMethod DownloadApplicationMethod =
         TerminalDeviceMethod.Register<DownloadContactlessApplicationCommand, DownloadContactlessApplicationResponse>(
             "DownloadApplication",
             typeof(ContactlessReader), PrepareDownloadContactlessApplicationCommand);

        private static DownloadContactlessApplicationCommand PrepareDownloadContactlessApplicationCommand()
        {
            var uiService = ServiceLocator.Current.GetInstance<IUiService>();

            var cmd = new DownloadContactlessApplicationCommand();

            switch (uiService.SelectedCardBrand)
            {
                case CardBrand.MasterCard:
                    cmd.Name = BrandName.MasterCard;
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000000041010");
                    cmd.ApplicationIndex = 0;
                    cmd.ApplicationSchemeIndicator = 1;
                    cmd.TransactionTypeIndicator = 1;
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F09020002")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F6D020001")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F15020101")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F1B0400000000")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("500A4D617374657243617264")},
                        };
                    cmd.TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0000000000");
                    cmd.TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("0000000000");
                    cmd.TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("0000000000");
                    cmd.TransactionFloorLimit = ConvertHelper.ToHexByteArray("000000010000");
                    cmd.TransactionLimit = ConvertHelper.ToHexByteArray("000000030000");
                    cmd.CVMRequiredLimit = ConvertHelper.ToHexByteArray("000000001000");
                    break;

                case CardBrand.Visa:
                    cmd.Name = BrandName.Visa;
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000000031010");
                    cmd.ApplicationIndex = 1;
                    cmd.ApplicationSchemeIndicator = 1;
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F09020105")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F15020011")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F1B0400002710")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("500456534443")},
                        };
                    cmd.TransactionFloorLimit = ConvertHelper.ToHexByteArray("000000010000");
                    cmd.TransactionLimit = ConvertHelper.ToHexByteArray("000000030000");
                    cmd.CVMRequiredLimit = ConvertHelper.ToHexByteArray("000000005000");
                    cmd.ZeroAmountOption = 0;
                    cmd.ZeroAmountOptionSpecified = true;
                    cmd.TransactionTypeIndicator = 0;
                    break;

                case CardBrand.Amex:
                    cmd.Name = BrandName.AmericanExpress;
                    cmd.AID = ConvertHelper.ToHexByteArray("A00000002501");
                    // "9F0606A00000002501;500450495053;C00103;DF0B0102;DF0C0101;9F09020001;9F15023030;DF0D050400000000;DF0E05C400000000;DF0F050400000000;DF130100;DF190100";
                    cmd.ApplicationIndex = 2;
                    cmd.ApplicationSchemeIndicator = 1;
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F09020001")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F15023030")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("500450495053")},
                        };
                    cmd.TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0400000000");
                    cmd.TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("C400000000");
                    cmd.TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("0400000000");
                    cmd.ZeroAmountOption = 0;
                    cmd.ZeroAmountOptionSpecified = true;
                    cmd.TransactionTypeIndicator = 0;
                    break;

                case CardBrand.Discover:
                    cmd.Name = BrandName.Discover;
                    cmd.ApplicationIndex = 4;
                    cmd.ApplicationSchemeIndicator = 1;
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000003241010");
                    // "9F0607A0000003241010;5008446973636F766572;C00104;DF0B0104;DF0C0101;9F09020044;9F15023030;DF190100";
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F09020044")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F15023030")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("5008446973636F766572")},
                        };
                    cmd.TransactionTypeIndicator = 0;
                    break;

                case CardBrand.Interac:
                    cmd.Name = BrandName.Interac;
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000002771010");
                    cmd.ApplicationIndex = 5;
                    cmd.ApplicationSchemeIndicator = 1;
                    cmd.TransactionTypeIndicator = 1;
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F09020002")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F15020101")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F1B0400002710")},
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("5007496E7465726163")},
                        };
                    //"9F0607A0000002771010;5007496E7465726163;C00108;DF0B0105;DF0C0101;9F09020002;9F15020101;9F1B0400002710;DF230114;
                    // DF25013C;DF240400001388;DF0D050010000000;DF0E05FC68FCE800;DF0F05FC68FCE800;DF1106000000010000;DF190101;DF220100";
                    cmd.TerminalActionCodeDenial = ConvertHelper.ToHexByteArray("0010000000");
                    cmd.TerminalActionCodeOnline = ConvertHelper.ToHexByteArray("FC68FCE800");
                    cmd.TerminalActionCodeDefault = ConvertHelper.ToHexByteArray("FC68FCE800");
                    cmd.TransactionFloorLimit = ConvertHelper.ToHexByteArray("000000010000");
                    cmd.TargetPercentage = 0x14;
                    cmd.TargetPercentageSpecified = true;
                    cmd.MaxTargetPercentage = 0x3C;
                    cmd.MaxTargetPercentageSpecified = true;
                    cmd.ThresholdValue = ConvertHelper.ToHexByteArray("00001388");
                    cmd.StatusCheck = 0;
                    cmd.StatusCheckSpecified = true;
                    break;

                case CardBrand.Jcb:
                    //mNotSupportedLabel.Visible = true;
                    //_commandNotSupported = true;
                    break;
            }

            return cmd;
        }

        public static readonly TerminalDeviceMethod DownloadPublicKeysMethod =
         TerminalDeviceMethod.Register<DownloadContactlessPublicKeysCommand, DownloadContactlessPublicKeysResponse>(
             "DownloadPublicKeys",
             typeof(ContactlessReader), PrepareDownloadContactlessPublicKeysCommand);

        private static DownloadContactlessPublicKeysCommand PrepareDownloadContactlessPublicKeysCommand()
        {
            var uiService = ServiceLocator.Current.GetInstance<IUiService>();

            var cmd = new DownloadContactlessPublicKeysCommand();

            switch (uiService.SelectedCardBrand)
            {
                case CardBrand.MasterCard:
                    cmd.Rid = "A000000004";
                    cmd.ContactlessKey = new[]
                        {
                            new ContactlessKey
                                {
                                    Index = "F1",
                                    Modulus =
                                        "B0A0DCF4BDE19C3546B4B6F0414D174DDE294AABBB828C5A834D73AAE27C99B0B053A90278007239B6459FF0BBCD7B4B9C6C50AC02CE91368DA1BD21AAEADBC65347337D89B68F5C99A09D05BE02DD1F8C5BA20E2F13FB2A27C41D3F85CAD5CF6668E75851EC66EDBF98851FD4E42C44C1D59F5984703B27D5B9F21B8FA0D93279FBBF69E090642909C9EA27F898959541AA6757F5F624104F6E1D3A9532F2A6E51515AEAD1B43B3D7835088A2FAFA7BE7",
                                    Exponent = "03",
                                    Checksum = "D8E68DA167AB5A85D8C3D55ECB9B0517A1A5B4BB",
                                    ExpirationDate = "00000000"
                                },
                        };
                    break;

                case CardBrand.Visa:
                    cmd.Rid = "A000000003";
                    cmd.ContactlessKey = new[]
                        {
                            new ContactlessKey
                                {
                                    Index = "92",
                                    Modulus =
                                        "81B0996AF56F569187D09293C14810450ED8EE3357397B18A2458EFAA92DA3B6DF6514EC060195318FD43BE9B8F0CC669E3F844057CBDDF8BDA191BB64473BC8DC9A730DB8F6B4EDE3924186FFD9B8C7735789C23A36BA0B8AF65372EB57EA5D89E7D14E9C7B6B557460F10885DA16AC923F15AF3758F0F03EBD3C5C2C949CBA306DB44E6A2C076C5F67E281D7EF56785DC4D75945E491F01918800A9E2DC66F60080566CE0DAF8D17EAD46AD8E30A247C9F",
                                    Exponent = "03",
                                    Checksum = "14429C954A3859CEF91295F663C963E582ED6EB253",
                                    ExpirationDate = "00000000"
                                },
                        };
                    break;

                case CardBrand.Amex:
                    cmd.Rid = "A000000025";
                    cmd.ContactlessKey = new[]
                        {
                            new ContactlessKey
                                {
                                    Index = "C7",
                                    Modulus =
                                        "cd237e34e0299de48f1a2c94f478fe972896011e1ca6ab462b68fe0f6109c9a97c2dbeea65932cde0625138b9f162b92979daab019d3b5561d31eb2d4f09f12f927ea8f740ce0e87154965505e2272f69042b15d57ccc7f771919123978283b3cce524d9715207bf5f5ad369102176f0f7a78a6deb2bff0edce165f3b14f14d0035b2756861fe03c43396ed002c894a3",
                                    Exponent = "03",
                                    Checksum = "6221E0C726BAC8F8AC25F8F93B811D1FFD4C131C",
                                    ExpirationDate = "00000000"
                                },
                            new ContactlessKey
                                {
                                    Index = "C8",
                                    Modulus =
                                        "d4d03c0304b3a78e3bdef36ff7d67d0bf48a240b97b4d02fb9a61f9381733c69c8e9c5e37a838ff1fec5b9f0818f81381a8ac6793b68e1dfe923de1023c66a591e69b0e4eb573210d7d91ae0643022b1f9239e5ba067cd1f293a29271352bf83dec1a1fcab87e37a219eaa571d4aa45df8e1771486407bb9cfd3b6aae13e6e8c3a4a27cf7b498c3d75bc0b8ae929374e9263b5086b949536beb2fb2b95f551bafb405e8ae509d67a342465279136407d",
                                    Exponent = "03",
                                    Checksum = "75AFDD6CD392394F5647257C8686409C4D3B38C3",
                                    ExpirationDate = "00000000"
                                },
                            new ContactlessKey
                                {
                                    Index = "C9",
                                    Modulus =
                                        "c0a03306ebe4a01f0900463c90fce65bcdaa7ca2a94e1898b88209de934f8a4373a0b2a725e6d914f49b675773b36239a7f15455fd900dedf8e8c4e2da025d926697966972cedc4c342174810ece17c926a621fc8b5b63640d78a62a56f8ce09b69dc673119609833c5ac5d0e8d9ebb77ccc5489416a5e774a2b41ae452b11a094306f9e22e76fe7dbbdaae7eadc626f61c8c6dcbb469349112adcf277ce9ca1cd575e7cbe81d9b1b86f0c96cdb0edd206e8ff7f7950cb8affdec0df503a9fc3406d394cc9d5b80ed2fda4aff1eb9ad010e190f68b6b038235f2f7bf3f1511a532f718a9b5c5eeaae45a21bd0b2a98f904c2bca77ab5f929",
                                    Exponent = "03",
                                    Checksum = "B1D9B101C226F0008A800B0E996343AFDAACD42D",
                                    ExpirationDate = "00000000"
                                },
                        };
                    break;

                case CardBrand.Discover:
                    cmd.Rid = "A000000324";
                    cmd.ContactlessKey = new[]
                        {
                            new ContactlessKey
                                {
                                    Index = "",
                                    Modulus = "",
                                    Exponent = "",
                                    Checksum = "",
                                    ExpirationDate = ""
                                }
                        };
                    break;

                case CardBrand.Interac:
                    cmd.Rid = "A000000277";
                    cmd.ContactlessKey = new ContactlessKey[3];
                    cmd.ContactlessKey[0] = new ContactlessKey
                    {
                        Index = "09",
                        Modulus = "8180F802C308544873AD2225A81943732A4B7CFFA4E3157D17CD5A7723F858F0B11E636D2930FA933778F27C7C49127E0CCA317021CFE8E0F773785EB3FF07587E98CE8ED4FE9E1CA1859F41A9CF2572D8A093C5465F5A29612A45B1700F4DA13814C3D4DF075EAADE8DB4BE4D7B3AE0256F7A0C12E34BD416CAC4F9250C38B7E13B",
                        Exponent = "010001",
                        Checksum = "14A2974D6B8F302A923740E2BC6217075202037B8B",
                        ExpirationDate = "00000000"
                    };
                    break;

                case CardBrand.Jcb:
                    //mNotSupportedLabel.Visible = true;
                    //_commandNotSupported = true;
                    break;
            }

            return cmd;
        }

        public static readonly TerminalDeviceMethod DeleteApplicationPublicKeysMethod =
         TerminalDeviceMethod.Register<DeleteApplicationPublicKeysCommand, DeleteApplicationPublicKeysResponse>(
             "DeleteApplicationPublicKeys",
             typeof(ContactlessReader), PrepareDeleteApplicationPublicKeysCommand);

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
                    cmd.Rid = "A0000003241";
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

        public static readonly TerminalDeviceMethod SetBrandApplicationParametersMethod =
         TerminalDeviceMethod.Register<SetBrandApplicationParametersCommand, SetBrandApplicationParametersResponse>(
             "SetBrandApplicationParameters",
             typeof(ContactlessReader), PrepareSetBrandApplicationParametersCommand);

        private static SetBrandApplicationParametersCommand PrepareSetBrandApplicationParametersCommand()
        {
            var uiService = ServiceLocator.Current.GetInstance<IUiService>();

            var cmd = new SetBrandApplicationParametersCommand();

            switch (uiService.SelectedCardBrand)
            {
                case CardBrand.MasterCard:
                    cmd.Name = BrandName.MasterCard;
                    cmd.OfflineChipData =
                        ConvertHelper.ToHexByteArray(
                            "9A9F219F029F039F539F1A5F2A9F339F409F359F1E82849F095A5F34959B9F349F269F279F109F379F369C9F418A899F6E50579F119F12");
                    cmd.OnlineChipData =
                        ConvertHelper.ToHexByteArray(
                            "9F019F025F255F245A5F349F269F109F159F169F399F279F1A5F2A9A9F219C959B829F349F36849F6E509F119F129F339F409F379F35");
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F530100")}
                        };
                    break;

                case CardBrand.Visa:
                    cmd.Name = BrandName.Visa;
                    cmd.OfflineChipData =
                        ConvertHelper.ToHexByteArray(
                            "575A5F205F245F2A5F3482959A9C9F109F1A9F269F369F379F5D9F6E9F7C9F279F219F029F039F669F339F40898A9F399F35");
                    cmd.OnlineChipData =
                        ConvertHelper.ToHexByteArray(
                            "575A5F205F245F2A5F3482959A9C9F109F1A9F269F369F379F5D9F6E9F7C9F279F219F029F039F669F339F409F399F35");
                    cmd.StatusCheckAmount = ConvertHelper.ToHexByteArray("000000000100");
                    break;

                case CardBrand.Amex:
                    cmd.Name = BrandName.AmericanExpress;
                    cmd.OfflineChipData =
                        ConvertHelper.ToHexByteArray(
                            "9A9F219F029F039F539F1A5F2A5F369F339F409F359F1E82849F095A5F34959B9F349F269F279F109F379F369C9F418A899F6E50579F119F12");
                    cmd.OnlineChipData =
                        ConvertHelper.ToHexByteArray(
                            "9A9F219F019F029F035F255F245A575F349F269F109F159F399F279F1A5F2A5F369C9F41959B829F349F36849F6E509F119F129F339F409F379F35");
                    cmd.StatusCheckAmount = ConvertHelper.ToHexByteArray("000000010000");
                    break;

                case CardBrand.Discover:
                    cmd.Name = BrandName.Discover;
                    // this is not used for Discover
                    cmd.OfflineChipData = null;
                    cmd.OnlineChipData = null;
                    cmd.OnlineTimeout = new byte[] { 0x13, 0x88 }; // set to 5 seconds
                    break;

                case CardBrand.Interac:
                    cmd.Name = BrandName.Interac;
                    cmd.OfflineChipData =
                        ConvertHelper.ToHexByteArray(
                            "9F269F369F10575F34829F029F039F1A955F2A9A9C9F379F459F4C9F345F249F39");
                    cmd.OnlineChipData =
                        ConvertHelper.ToHexByteArray(
                            "9F269F369F10575F34829F029F039F1A955F2A9A9C9F379F459F4C9F345F249F39");
                    cmd.EmvDataElement = new[]
                        {
                            new EmvDataElement {Value = ConvertHelper.ToHexByteArray("9F580103")}
                        };
                    break;

                case CardBrand.Jcb:
                    //mNotSupportedLabel.Visible = true;
                    //_commandNotSupported = true;
                    break;
            }

            return cmd;
        }

        public static readonly TerminalDeviceMethod GetBrandApplicationParametersMethod =
         TerminalDeviceMethod.Register<GetBrandApplicationParametersCommand, GetBrandApplicationParametersResponse>(
             "GetBrandApplicationParameters",
             typeof(ContactlessReader), PrepareGetBrandApplicationParametersCommand);

        private static GetBrandApplicationParametersCommand PrepareGetBrandApplicationParametersCommand()
        {
            var uiService = ServiceLocator.Current.GetInstance<IUiService>();

            var cmd = new GetBrandApplicationParametersCommand();

            switch (uiService.SelectedCardBrand)
            {
                case CardBrand.MasterCard:
                    cmd.Name = BrandName.MasterCard;
                    break;

                case CardBrand.Visa:
                    cmd.Name = BrandName.Visa;
                    break;

                case CardBrand.Amex:
                    cmd.Name = BrandName.AmericanExpress;
                    break;

                case CardBrand.Discover:
                    cmd.Name = BrandName.Discover;
                    break;

                case CardBrand.Interac:
                    cmd.Name = BrandName.Interac;
                    break;
            }

            return cmd;
        }

        public static readonly TerminalDeviceMethod DeleteApplicationMethod =
         TerminalDeviceMethod.Register<DeleteContactlessApplicationCommand, DeleteContactlessApplicationResponse>(
             "DeleteApplication",
             typeof(ContactlessReader), PrepareDeleteContactlessApplicationCommand);

        private static DeleteContactlessApplicationCommand PrepareDeleteContactlessApplicationCommand()
        {
            var uiService = ServiceLocator.Current.GetInstance<IUiService>();

            var cmd = new DeleteContactlessApplicationCommand();

            switch (uiService.SelectedCardBrand)
            {
                case CardBrand.MasterCard:
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000000041010");
                    cmd.ApplicationIndex = 0;
                    break;

                case CardBrand.Visa:
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000000031010");
                    cmd.ApplicationIndex = 1;
                    break;

                case CardBrand.Amex:
                    cmd.AID = ConvertHelper.ToHexByteArray("A00000002501");
                    cmd.ApplicationIndex = 2;
                    break;

                case CardBrand.Discover:
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000003241010");
                    cmd.ApplicationIndex = 4;
                    break;

                case CardBrand.Interac:
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000002771010");
                    cmd.ApplicationIndex = 5;
                    break;

                case CardBrand.Jcb:
                    cmd.AID = ConvertHelper.ToHexByteArray("A0000000651010");
                    cmd.ApplicationIndex = 6;
                    break;
            }

            return cmd;
        }

        public static readonly TerminalDeviceMethod SetTransactionIndicatorMethod =
         TerminalDeviceMethod.Register<SetTransactionIndicatorCommand, SetTransactionIndicatorResponse>(
             "SetTransactionIndicator",
             typeof(ContactlessReader), () => new SetTransactionIndicatorCommand
                 {
                      TerminalTransactionResult = TerminalTransactionResult.Approved,
                 });

        public static readonly TerminalDeviceMethod OpenMethod =
            TerminalDeviceMethod.Register<OpenContactlessReaderCommand, OpenContactlessReaderResponse>(
                "Open",
                typeof(ContactlessReader), () => new OpenContactlessReaderCommand
                {
                    EnablePayment = new EnablePayment
                    {
                        PaymentScheme = PaymentSchemes.StandardPayment,
                    },
                    EnableWallet = new EnableWallet
                    {
                        WalletScheme = WalletSchemes.NotSupported,
                    },
                });

        public static readonly TerminalDeviceMethod CloseMethod =
            TerminalDeviceMethod.Register<CloseContactlessReaderCommand, CloseContactlessReaderResponse>(
                "Close",
                typeof(ContactlessReader));

        public static readonly TerminalDeviceMethod ResetConfigurationMethod =
            TerminalDeviceMethod.Register<ResetContactlessConfigurationCommand, ResetContactlessConfigurationResponse>(
                "ResetConfiguration",
                typeof(ContactlessReader));

        public static readonly TerminalDeviceMethod SetTransactionOnlineCompletionMethod =
            TerminalDeviceMethod.Register<SetTransactionOnlineCompletionCommand, SetTransactionOnlineCompletionResponse>(
                "SetTransactionOnlineCompletion",
                typeof(ContactlessReader), () => new SetTransactionOnlineCompletionCommand
                    {
                        EmvDataElement = new[]
                            {
                                new EmvDataElement { Value = ConvertHelper.ToHexByteArray("8A") },
                                new EmvDataElement { Value = ConvertHelper.ToHexByteArray("71") },
                                new EmvDataElement { Value = ConvertHelper.ToHexByteArray("72") },
                            }
                });

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent OpenChangedEvent =
           TerminalDeviceEvent.Register<OpenChanged>("OpenChanged", typeof(ContactlessReader));

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(ContactlessReader));

        public static readonly TerminalDeviceEvent DataReadEvent =
            TerminalDeviceEvent.Register<ContactlessData>("DataRead", typeof(ContactlessReader));

        public static readonly TerminalDeviceEvent OperationalStateChangedEvent =
            TerminalDeviceEvent.Register<ContactlessOperationalStateChanged>("OperationalStateChanged", typeof(ContactlessReader));

        public static readonly TerminalDeviceEvent NotificationDataReadEvent =
            TerminalDeviceEvent.Register<NotificationData>("NotificationDataRead", typeof(ContactlessReader));

        #endregion
    }
}
