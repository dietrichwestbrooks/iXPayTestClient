using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    public class Display
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<DisplayCommand, DisplayResponse, DisplayEvent>(
                    "Display", new TerminalRequestHandlerByName("Terminal"), typeof(Display));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty CurrentLanguageProperty =
            TerminalDeviceProperty.Register<string,
            GetCurrentLanguageCommand, GetCurrentLanguageResponse,
            SetCurrentLanguageCommand, SetCurrentLanguageResponse>("CurrentLanguage",
                "Language", typeof(Display), null, () => new SetCurrentLanguageCommand
                {
                    Language = "en",
                });

        public static readonly TerminalDeviceProperty MessageWindowPropertiesProperty =
            TerminalDeviceProperty.Register<MessageWindowProperties,
            GetMessageWindowPropertiesCommand, GetMessageWindowPropertiesResponse,
            SetMessageWindowPropertiesCommand, SetMessageWindowPropertiesResponse>(
                "MessageWindowProperties", "MessageWindowProperties",
                typeof(Display), null, () => new SetMessageWindowPropertiesCommand
                {
                    MessageWindowProperties = new MessageWindowProperties
                    {
                        BackColor = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF },
                        ForeColor = new byte[] { 0xFF, 0x00, 0x00, 0x00 },
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Middle,
                        Location = new Location { Top = 0, Left = 0 },
                        Size = new Size { Width = 320, Height = 240 },
                        Font = new Font { Name = "Arial", Size = 22, Style = 0 }
                    }
                });

        public static readonly TerminalDeviceProperty SoftKeyPropertiesProperty =
            TerminalDeviceProperty.Register<SoftKeyProperties,
            GetSoftKeyPropertiesCommand, GetSoftKeyPropertiesResponse,
            SetSoftKeyPropertiesCommand, SetSoftKeyPropertiesResponse>(
                "SoftKeyProperties", "SoftKeyProperties",
                typeof(Display), null, () => 
                new SetSoftKeyPropertiesCommand
                {
                    SoftKeyProperties = new SoftKeyProperties
                    {
                        BackColor = new byte[] { 0xFF, 0x00, 0x00, 0x00 },
                        ForeColor = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF },
                        PixelsFromEdge = 5,
                        SoftKeyAlignment = SoftKeyAlignment.Center,
                        SoftKeyHeight = 38,
                        SoftKeyWidth = 150,
                        Font = new Font { Name = "Arial", Size = 30, Style = 0 }
                    }
                });

        public static readonly TerminalDeviceProperty DataEntryWindowPropertiesProperty =
            TerminalDeviceProperty.Register<DataEntryWindowProperties,
            GetDataEntryWindowPropertiesCommand, GetDataEntryWindowPropertiesResponse,
            SetDataEntryWindowPropertiesCommand, SetDataEntryWindowPropertiesResponse>(
                "DataEntryWindowProperties", "DataEntryWindowProperties",
                typeof(Display), null, () => new SetDataEntryWindowPropertiesCommand
                {
                    DataEntryWindowProperties = new DataEntryWindowProperties
                    {
                        ForeColor = new byte[] { 0xFF, 0x00, 0x00, 0x00 },
                        Location = new Location { Left = 76, Top = 145 },
                        Size = new Size { Height = 37, Width = 160 },
                        Font = new Font { Name = "Arial", Size = 18, Style = 0 },
                    }
                });

        public static readonly TerminalDeviceProperty PromptDetailsProperty =
            TerminalDeviceProperty.Register<PromptDetails,
            GetPromptDetailsCommand, GetPromptDetailsResponse,
            SetPromptDetailsCommand, SetPromptDetailsResponse>("PromptDetails",
                "PromptDetails", typeof(Display), PrepareGetPromptDetailsCommand, PrepareSetPromptDetailsCommand);

        private static GetPromptDetailsCommand PrepareGetPromptDetailsCommand()
        {
            return new GetPromptDetailsCommand
                {
                    PromptId = 1,
                    Language = "en",
                };
        }

        private static SetPromptDetailsCommand PrepareSetPromptDetailsCommand()
        {
            return new SetPromptDetailsCommand
                {
                    Language = "en",
                    PromptDetails = new PromptDetails
                        {
                            PromptId = 1,
                            PromptText = "Please Insert Card",
                            Images = new Images
                                {
                                    ImageInstance = new[]
                                        {
                                            new ImageInstance
                                                {
                                                    ImageId = 1,
                                                    Location = new Location {Left = 0, Top = 0},
                                                },
                                        }
                                },
                            Animations = new Animations
                                {
                                    AnimationInstance = new[]
                                        {
                                            new AnimationInstance
                                                {
                                                    AnimationId = 1,
                                                    Location = new Location {Left = 50, Top = 200},
                                                }
                                        }
                                }
                        },
                };
        }

        public static readonly TerminalDeviceProperty PromptListProperty =
            TerminalDeviceProperty.Register<PromptSummary[],
            GetPromptListCommand, GetPromptListResponse>("PromptList",
                "PromptSummary", typeof(Display), () => new GetPromptListCommand
                {
                    Language = "en",
                });

        public static readonly TerminalDeviceProperty ImageDetailsProperty =
            TerminalDeviceProperty.Register<byte[],
            GetImageDetailsCommand, GetImageDetailsResponse,
            SetImageDetailsCommand, SetImageDetailsResponse>("ImageDetails",
                "Value", typeof(Display), PrepareGetImageDetailsCommand, PrepareSetImageDetailsCommand);

        private static GetImageDetailsCommand PrepareGetImageDetailsCommand()
        {
            return new GetImageDetailsCommand
                {
                    ImageId = 1,
                    Language = "en",
                };
        }

        private static SetImageDetailsCommand PrepareSetImageDetailsCommand()
        {
            return new SetImageDetailsCommand
                {
                    ImageId = 1,
                    Filename = "bac1re1.bmp",
                    Language = "en",
                };
        }

        public static readonly TerminalDeviceProperty ImageListProperty =
            TerminalDeviceProperty.Register<ImageSummary[], GetImageListCommand, GetImageListResponse>("ImageList",
                "ImageSummary", typeof(Display), () => new GetImageListCommand
                {
                    Language = "en",
                });

        public static readonly TerminalDeviceProperty AnimationDetailsProperty =
            TerminalDeviceProperty.Register<AnimationDetails,
            GetAnimationDetailsCommand, GetAnimationDetailsResponse,
            SetAnimationDetailsCommand, SetAnimationDetailsResponse>("AnimationDetails",
                "AnimationDetails", typeof(Display), PrepareGetAnimationDetailsCommand, PrepareSetAnimationDetailsCommand);

        private static GetAnimationDetailsCommand PrepareGetAnimationDetailsCommand()
        {
            return new GetAnimationDetailsCommand
                {
                    AnimationId = 1,
                    Language = "en",
                };
        }

        private static SetAnimationDetailsCommand PrepareSetAnimationDetailsCommand()
        {
            return new SetAnimationDetailsCommand
                {
                    Language = "en",
                    AnimationDetails = new AnimationDetails
                        {
                            AnimationId = 1,
                            Description = "Card Swipe",
                            AnimationFrame = new[]
                                {
                                    new AnimationFrame {ImageId = 1, Duration = 500},
                                    new AnimationFrame {ImageId = 2, Duration = 500},
                                    new AnimationFrame {ImageId = 3, Duration = 500},
                                },
                        },
                };
        }

        public static readonly TerminalDeviceProperty AnimationListProperty =
            TerminalDeviceProperty.Register<AnimationSummary[], GetAnimationListCommand, GetAnimationListResponse>("AnimationList",
                "AnimationSummary", typeof(Display), () => new GetAnimationListCommand
                {
                    Language = "en",
                });

        public static readonly TerminalDeviceProperty FontsProperty =
            TerminalDeviceProperty.Register<PrinterFont[], GetFontsCommand, GetFontsResponse>("Fonts",
                "Font", typeof(Display));

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                "State", typeof(Display));

        public static readonly TerminalDeviceProperty SupportedLanguagesProperty =
            TerminalDeviceProperty.Register<SupportedLanguage[], GetSupportedLanguagesCommand, GetSupportedLanguagesResponse>(
                "SupportedLanguages", "SupportedLanguage",
                typeof(Display));

        public static readonly TerminalDeviceProperty DisplayTypeProperty =
            TerminalDeviceProperty.Register<DisplayType, GetDisplayTypeCommand, GetDisplayTypeResponse>(
                "DisplayType", "DisplayType",
                typeof(Display));

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod DisplayPromptMethod =
            TerminalDeviceMethod.Register<DisplayPromptCommand, DisplayPromptResponse>("DisplayPrompt",
                typeof (Display), () => new DisplayPromptCommand
                    {
                        Id = 1,
                        Language = "en",
                        IgnoreSoftKeySet = false,
                        IgnoreSoftKeySetSpecified = true,
                        SubstituteParameter = new[]
                            {
                                new SubstituteParameter {Text = "A Text"}
                            },
                    });

        public static readonly TerminalDeviceMethod DisplayPromptImmediateMethod =
            TerminalDeviceMethod.Register<DisplayPromptImmediateCommand, DisplayPromptImmediateResponse>(
                "DisplayPromptImmediate",
                typeof (Display));

        public static readonly TerminalDeviceMethod DeletePromptMethod =
            TerminalDeviceMethod.Register<DeletePromptCommand, DeletePromptResponse>("DeletePrompt",
                typeof (Display), () => new DeletePromptCommand
                    {
                        Id = 1,
                        Language = "en",
                    });

        public static readonly TerminalDeviceMethod DeleteAllPromptsMethod =
            TerminalDeviceMethod.Register<DeleteAllPromptsCommand, DeleteAllPromptsResponse>("DeleteAllPrompts",
                typeof (Display), () => new DeleteAllPromptsCommand
                    {
                        Language = "en",
                    });

        public static readonly TerminalDeviceMethod DeleteImageMethod =
            TerminalDeviceMethod.Register<DeleteImageCommand, DeleteImageResponse>("DeleteImage",
                typeof (Display), () => new DeleteImageCommand
                    {
                        Id = 1,
                        Language = "en",
                    });

        public static readonly TerminalDeviceMethod DeleteAllImagesMethod =
            TerminalDeviceMethod.Register<DeleteAllImagesCommand, DeleteAllImagesResponse>("DeleteAllImages",
                typeof (Display), () => new DeleteAllImagesCommand
                    {
                        Language = "en",
                    });

        public static readonly TerminalDeviceMethod DeleteAnimationMethod =
            TerminalDeviceMethod.Register<DeleteAnimationCommand, DeleteAnimationResponse>("DeleteAnimation",
                typeof (Display), () => new DeleteAnimationCommand
                    {
                        Id = 1,
                        Language = "en",
                    });

        public static readonly TerminalDeviceMethod DeleteAllAnimationsMethod =
            TerminalDeviceMethod.Register<DeleteAllAnimationsCommand, DeleteAllAnimationsResponse>(
                "DeleteAllAnimations",
                typeof (Display), () => new DeleteAllAnimationsCommand
                    {
                        Language = "en",
                    });

        public static readonly TerminalDeviceMethod DeleteAllMethod =
            TerminalDeviceMethod.Register<DeleteAllCommand, DeleteAllResponse>(
                "DeleteAll",
                typeof (Display), () => new DeleteAllCommand
                    {
                        Language = "en",
                    });

        public static readonly TerminalDeviceMethod RestoreDefaultMethod =
            TerminalDeviceMethod.Register<RestoreDefaultCommand, RestoreDefaultResponse>(
                "RestoreDefault",
                typeof(Display));

        public static readonly TerminalDeviceMethod SetVideoWindowMethod =
            TerminalDeviceMethod.Register<SetVideoWindowCommand, SetVideoWindowResponse>(
                "SetVideoWindow",
                typeof (Display), () => new SetVideoWindowCommand
                    {
                        FullScreen = true,
                    });

        public static readonly TerminalDeviceMethod StringMethod =
            TerminalDeviceMethod.Register<DisplayStringCommand, DisplayStringResponse>(
                "DisplayString",
                typeof (Display), () => new DisplayStringCommand
                    {
                        Message = "Please Pay inside (E01)",
                        Id = 1,
                        ShowDataEntry = false,
                        ShowDataEntrySpecified = true,
                        ClearScreen = false,
                        ClearScreenSpecified = true,
                    });

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent CurrentLanguageChangedEvent =
            TerminalDeviceEvent.Register<CurrentLanguageChanged>("CurrentLanguageChanged", typeof(Display));

        #endregion
    }
}
