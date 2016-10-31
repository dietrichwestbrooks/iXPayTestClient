using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class Display : TerminalDevice<DisplayCommand, DisplayResponse, DisplayEvent>
    {
        public Display()
            : base("Display")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new CurrentLanguageProperty(this),
                    new MessageWindowPropertiesProperty(this),
                    new SoftKeyPropertiesProperty(this),
                    new DataEntryWindowPropertiesProperty(this),
                    new ImageDetailsProperty(this),
                    new ImageListProperty(this),
                    new AnimationDetailsProperty(this),
                    new AnimationListProperty(this),
                    new PromptDetailsProperty(this),
                    new PromptListProperty(this),
                    new FontsProperty(this),
                    new StatusProperty(this),
                    new FontsProperty(this),
                    new TypeProperty(this),
                    new SupportedLanguagesProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new DisplayPromptMethod(this),
                    new DisplayPromptImmediateMethod(this),
                    new DeletePromptMethod(this),
                    new DeleteAllPromptsMethod(this),
                    new DeleteImageMethod(this),
                    new DeleteAllImagesMethod(this),
                    new DeleteAnimationMethod(this),
                    new DeleteAllAnimationsMethod(this),
                    new DeleteAllMethod(this),
                    new RestoreDefaultMethod(this),
                    new SetVideoWindowMethod(this),
                    new StringMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new CurrentLanguageChangedEvent(this),
                });
        }

        #region Device Properties

        [ValueProperty("Language")]
        public class CurrentLanguageProperty : TerminalDeviceProperty<string,
            GetCurrentLanguageCommand, GetCurrentLanguageResponse,
            SetCurrentLanguageCommand, SetCurrentLanguageResponse>
        {
            public CurrentLanguageProperty(ITerminalDevice device)
                : base(device, "CurrentLanguage")
            {
                GetCommand = new TerminalDeviceCommand<GetCurrentLanguageCommand, GetCurrentLanguageResponse>(
                    this,
                    $"get_{Name}"
                    );

                SetCommand = new TerminalDeviceCommand<SetCurrentLanguageCommand, SetCurrentLanguageResponse>(
                    this,
                    $"set_{Name}",
                    () => new SetCurrentLanguageCommand
                        {
                            Language = "en",
                        }
                    );
            }
        }

        [ValueProperty("MessageWindowProperties")]
        public class MessageWindowPropertiesProperty : TerminalDeviceProperty<MessageWindowProperties,
            GetMessageWindowPropertiesCommand, GetMessageWindowPropertiesResponse,
            SetMessageWindowPropertiesCommand, SetMessageWindowPropertiesResponse>
        {
            public MessageWindowPropertiesProperty(ITerminalDevice device)
                : base(device, "MessageWindowProperties")
            {
                GetCommand = new TerminalDeviceCommand<GetMessageWindowPropertiesCommand, GetMessageWindowPropertiesResponse>(
                    this,
                    $"get_{Name}"
                    );

                SetCommand = new TerminalDeviceCommand
                    <SetMessageWindowPropertiesCommand, SetMessageWindowPropertiesResponse>(
                    this,
                    $"set_{Name}",
                    () => new SetMessageWindowPropertiesCommand
                        {
                            MessageWindowProperties = new MessageWindowProperties
                                {
                                    BackColor = new byte[] {0xFF, 0xFF, 0xFF, 0xFF},
                                    ForeColor = new byte[] {0xFF, 0x00, 0x00, 0x00},
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Middle,
                                    Location = new Location {Top = 0, Left = 0},
                                    Size = new Size {Width = 320, Height = 240},
                                    Font = new Font {Name = "Arial", Size = 22, Style = 0}
                                }
                        }
                    );
            }
        }

        [ValueProperty("SoftKeyProperties")]
        public class SoftKeyPropertiesProperty : TerminalDeviceProperty<SoftKeyProperties,
            GetSoftKeyPropertiesCommand, GetSoftKeyPropertiesResponse,
            SetSoftKeyPropertiesCommand, SetSoftKeyPropertiesResponse>
        {
            public SoftKeyPropertiesProperty(ITerminalDevice device)
                : base(device, "SoftKeyProperties")
            {
                GetCommand = new TerminalDeviceCommand<GetSoftKeyPropertiesCommand, GetSoftKeyPropertiesResponse>(
                    this,
                    $"get_{Name}"
                    );

                SetCommand = new TerminalDeviceCommand<SetSoftKeyPropertiesCommand, SetSoftKeyPropertiesResponse>(
                    this,
                    $"set_{Name}",
                    () => new SetSoftKeyPropertiesCommand
                        {
                            SoftKeyProperties = new SoftKeyProperties
                                {
                                    BackColor = new byte[] {0xFF, 0x00, 0x00, 0x00},
                                    ForeColor = new byte[] {0xFF, 0xFF, 0xFF, 0xFF},
                                    PixelsFromEdge = 5,
                                    SoftKeyAlignment = SoftKeyAlignment.Center,
                                    SoftKeyHeight = 38,
                                    SoftKeyWidth = 150,
                                    Font = new Font {Name = "Arial", Size = 30, Style = 0}
                                }
                        }
                    );
            }
        }

        [ValueProperty("DataEntryWindowProperties")]
        public class DataEntryWindowPropertiesProperty : TerminalDeviceProperty<DataEntryWindowProperties,
            GetDataEntryWindowPropertiesCommand, GetDataEntryWindowPropertiesResponse,
            SetDataEntryWindowPropertiesCommand, SetDataEntryWindowPropertiesResponse>
        {
            public DataEntryWindowPropertiesProperty(ITerminalDevice device)
                : base(device, "DataEntryWindowProperties")
            {
                GetCommand = new TerminalDeviceCommand<GetDataEntryWindowPropertiesCommand, GetDataEntryWindowPropertiesResponse>(
                    this,
                    $"get_{Name}"
                    );

                SetCommand = new TerminalDeviceCommand
                    <SetDataEntryWindowPropertiesCommand, SetDataEntryWindowPropertiesResponse>(
                    this,
                    $"set_{Name}",
                    () => new SetDataEntryWindowPropertiesCommand
                        {
                            DataEntryWindowProperties = new DataEntryWindowProperties
                                {
                                    ForeColor = new byte[] {0xFF, 0x00, 0x00, 0x00},
                                    Location = new Location {Left = 76, Top = 145},
                                    Size = new Size {Height = 37, Width = 160},
                                    Font = new Font {Name = "Arial", Size = 18, Style = 0},
                                }
                        }
                    );
            }
        }

        [ValueProperty("PromptDetails")]
        public class PromptDetailsProperty : TerminalDeviceProperty<PromptDetails,
            GetPromptDetailsCommand, GetPromptDetailsResponse,
            SetPromptDetailsCommand, SetPromptDetailsResponse>
        {
            public PromptDetailsProperty(ITerminalDevice device)
                : base(device, "PromptDetails")
            {
                GetCommand = new TerminalDeviceCommand<GetPromptDetailsCommand, GetPromptDetailsResponse>(
                    this,
                    $"get_{Name}",
                    () => new GetPromptDetailsCommand
                        {
                            PromptId = 1,
                            Language = "en",
                        }
                    );

                SetCommand = new TerminalDeviceCommand<SetPromptDetailsCommand, SetPromptDetailsResponse>(
                    this,
                    $"set_{Name}",
                    () => new SetPromptDetailsCommand
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
                                            AnimationInstance = new []
                                                {
                                                    new AnimationInstance
                                                        {
                                                            AnimationId = 1,
                                                            Location = new Location {Left = 50, Top = 200},
                                                        }
                                                }
                                        }
                                },
                        }
                    );
            }
        }

        [ValueProperty("PromptSummary")]
        public class PromptListProperty : TerminalDeviceProperty<PromptSummary[],
            GetPromptListCommand, GetPromptListResponse>
        {
            public PromptListProperty(ITerminalDevice device)
                : base(device, "PromptList")
            {
                GetCommand = new TerminalDeviceCommand<GetPromptListCommand, GetPromptListResponse>(
                    this,
                    $"get_{Name}",
                    () => new GetPromptListCommand
                        {
                            Language = "en",
                        }
                    );
            }
        }

        [ValueProperty("Value")]
        public class ImageDetailsProperty : TerminalDeviceProperty<byte[],
            GetImageDetailsCommand, GetImageDetailsResponse,
            SetImageDetailsCommand, SetImageDetailsResponse>
        {
            public ImageDetailsProperty(ITerminalDevice device)
                : base(device, "ImageDetails")
            {
                GetCommand = new TerminalDeviceCommand<GetImageDetailsCommand, GetImageDetailsResponse>(
                    this,
                    $"get_{Name}",
                    () => new GetImageDetailsCommand
                        {
                            ImageId = 1,
                            Language = "en",
                        }
                    );

                SetCommand = new TerminalDeviceCommand<SetImageDetailsCommand, SetImageDetailsResponse>(
                    this,
                    $"set_{Name}",
                    () => new SetImageDetailsCommand
                        {
                            ImageId = 1,
                            Filename = "bac1re1.bmp",
                            Language = "en",
                        }
                    );
            }
        }

        [ValueProperty("ImageSummary")]
        public class ImageListProperty : TerminalDeviceProperty<ImageSummary[],
            GetImageListCommand, GetImageListResponse>
        {
            public ImageListProperty(ITerminalDevice device)
                : base(device, "ImageList")
            {
                GetCommand = new TerminalDeviceCommand<GetImageListCommand, GetImageListResponse>(
                    this,
                    $"get_{Name}",
                    () => new GetImageListCommand
                        {
                            Language = "en",
                        }
                    );
            }
        }

        [ValueProperty("AnimationDetails")]
        public class AnimationDetailsProperty : TerminalDeviceProperty<AnimationDetails,
            GetAnimationDetailsCommand, GetAnimationDetailsResponse,
            SetAnimationDetailsCommand, SetAnimationDetailsResponse>
        {
            public AnimationDetailsProperty(ITerminalDevice device)
                : base(device, "AnimationDetails")
            {
                GetCommand = new TerminalDeviceCommand<GetAnimationDetailsCommand, GetAnimationDetailsResponse>(
                    this,
                    $"get_{Name}",
                    () => new GetAnimationDetailsCommand
                        {
                            AnimationId = 1,
                            Language = "en",
                        }
                    );

                SetCommand = new TerminalDeviceCommand<SetAnimationDetailsCommand, SetAnimationDetailsResponse>(
                    this,
                    $"set_{Name}",
                    () => new SetAnimationDetailsCommand
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
                        }
                    );
            }
        }

        [ValueProperty("AnimationSummary")]
        public class AnimationListProperty : TerminalDeviceProperty<AnimationSummary[],
            GetAnimationListCommand, GetAnimationListResponse>
        {
            public AnimationListProperty(ITerminalDevice device)
                : base(device, "AnimationList")
            {
                GetCommand = new TerminalDeviceCommand<GetAnimationListCommand, GetAnimationListResponse>(
                    this,
                    $"get_{Name}",
                    () => new GetAnimationListCommand
                        {
                            Language = "en",
                        }
                    );
            }
        }

        [ValueProperty("Font")]
        public class FontsProperty : TerminalDeviceProperty<PrinterFont[],
            GetFontsCommand, GetFontsResponse>
        {
            public FontsProperty(ITerminalDevice device)
                : base(device, "Fonts")
            {
                GetCommand = new TerminalDeviceCommand<GetFontsCommand, GetFontsResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("State")]
        public class StatusProperty : TerminalDeviceProperty<Status,
            GetStatusCommand, GetStatusResponse>
        {
            public StatusProperty(ITerminalDevice device)
                : base(device, "Status")
            {
                GetCommand = new TerminalDeviceCommand<GetStatusCommand, GetStatusResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("SupportedLanguage")]
        public class SupportedLanguagesProperty : TerminalDeviceProperty<SupportedLanguage[],
            GetSupportedLanguagesCommand, GetSupportedLanguagesResponse>
        {
            public SupportedLanguagesProperty(ITerminalDevice device)
                : base(device, "SupportedLanguages")
            {
                GetCommand = new TerminalDeviceCommand<GetSupportedLanguagesCommand, GetSupportedLanguagesResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        [ValueProperty("DisplayType")]
        public class TypeProperty : TerminalDeviceProperty<DisplayType,
            GetDisplayTypeCommand, GetDisplayTypeResponse>
        {
            public TypeProperty(ITerminalDevice device)
                : base(device, "DisplayType")
            {
                GetCommand = new TerminalDeviceCommand<GetDisplayTypeCommand, GetDisplayTypeResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        #endregion

        #region Device Methods

        public class DisplayPromptMethod : TerminalDeviceMethod<DisplayPromptCommand, DisplayPromptResponse>
        {
            public DisplayPromptMethod(ITerminalDevice device)
                : base(device, "DisplayPrompt")
            {
                InvokeCommand = new TerminalDeviceCommand<DisplayPromptCommand, DisplayPromptResponse>(
                    this,
                    Name,
                    () => new DisplayPromptCommand
                        {
                            Id = 1,
                            Language = "en",
                            IgnoreSoftKeySet = false,
                            IgnoreSoftKeySetSpecified = true,
                            SubstituteParameter = new[]
                                {
                                    new SubstituteParameter {Text = "A Text"}
                                },
                        }
                    );
            }
        }

        public class DisplayPromptImmediateMethod : TerminalDeviceMethod<DisplayPromptImmediateCommand, DisplayPromptImmediateResponse>
        {
            public DisplayPromptImmediateMethod(ITerminalDevice device)
                : base(device, "DisplayPromptImmediate")
            {
                InvokeCommand = new TerminalDeviceCommand<DisplayPromptImmediateCommand, DisplayPromptImmediateResponse>(
                    this,
                    Name
                    );
            }
        }

        public class DeletePromptMethod : TerminalDeviceMethod<DeletePromptCommand, DeletePromptResponse>
        {
            public DeletePromptMethod(ITerminalDevice device)
                : base(device, "DeletePrompt")
            {
                InvokeCommand = new TerminalDeviceCommand<DeletePromptCommand, DeletePromptResponse>(
                    this,
                    Name,
                    () => new DeletePromptCommand
                        {
                            Id = 1,
                            Language = "en",
                        }
                    );
            }
        }

        public class DeleteAllPromptsMethod : TerminalDeviceMethod<DeleteAllPromptsCommand, DeleteAllPromptsResponse>
        {
            public DeleteAllPromptsMethod(ITerminalDevice device)
                : base(device, "DeleteAllPrompts")
            {
                InvokeCommand = new TerminalDeviceCommand<DeleteAllPromptsCommand, DeleteAllPromptsResponse>(
                    this,
                    Name,
                    () => new DeleteAllPromptsCommand
                        {
                            Language = "en",
                        }
                    );
            }
        }

        public class DeleteImageMethod : TerminalDeviceMethod<DeleteImageCommand, DeleteImageResponse>
        {
            public DeleteImageMethod(ITerminalDevice device)
                : base(device, "DeleteImage")
            {
                InvokeCommand = new TerminalDeviceCommand<DeleteImageCommand, DeleteImageResponse>(
                    this,
                    Name,
                    () => new DeleteImageCommand
                        {
                            Id = 1,
                            Language = "en",
                        }
                    );
            }
        }

        public class DeleteAllImagesMethod : TerminalDeviceMethod<DeleteAllImagesCommand, DeleteAllImagesResponse>
        {
            public DeleteAllImagesMethod(ITerminalDevice device)
                : base(device, "DeleteAllImages")
            {
                InvokeCommand = new TerminalDeviceCommand<DeleteAllImagesCommand, DeleteAllImagesResponse>(
                    this,
                    Name,
                    () => new DeleteAllImagesCommand
                        {
                            Language = "en",
                        }
                    );
            }
        }

        public class DeleteAnimationMethod : TerminalDeviceMethod<DeleteAnimationCommand, DeleteAnimationResponse>
        {
            public DeleteAnimationMethod(ITerminalDevice device)
                : base(device, "DeleteAnimation")
            {
                InvokeCommand = new TerminalDeviceCommand<DeleteAnimationCommand, DeleteAnimationResponse>(
                    this,
                    Name,
                    () => new DeleteAnimationCommand
                        {
                            Id = 1,
                            Language = "en",
                        }
                    );
            }
        }

        public class DeleteAllAnimationsMethod : TerminalDeviceMethod<DeleteAllAnimationsCommand, DeleteAllAnimationsResponse>
        {
            public DeleteAllAnimationsMethod(ITerminalDevice device)
                : base(device, "DeleteAllAnimations")
            {
                InvokeCommand = new TerminalDeviceCommand<DeleteAllAnimationsCommand, DeleteAllAnimationsResponse>(
                    this,
                    Name,
                    () => new DeleteAllAnimationsCommand
                        {
                            Language = "en",
                        }
                    );
            }
        }

        public class DeleteAllMethod : TerminalDeviceMethod<DeleteAllCommand, DeleteAllResponse>
        {
            public DeleteAllMethod(ITerminalDevice device)
                : base(device, "DeleteAll")
            {
                InvokeCommand = new TerminalDeviceCommand<DeleteAllCommand, DeleteAllResponse>(
                    this,
                    Name,
                    () => new DeleteAllCommand
                        {
                            Language = "en",
                        }
                    );
            }
        }

        public class RestoreDefaultMethod : TerminalDeviceMethod<RestoreDefaultCommand, RestoreDefaultResponse>
        {
            public RestoreDefaultMethod(ITerminalDevice device)
                : base(device, "RestoreDefault")
            {
                InvokeCommand = new TerminalDeviceCommand<RestoreDefaultCommand, RestoreDefaultResponse>(
                    this,
                    Name
                    );
            }
        }

        public class SetVideoWindowMethod : TerminalDeviceMethod<SetVideoWindowCommand, SetVideoWindowResponse>
        {
            public SetVideoWindowMethod(ITerminalDevice device)
                : base(device, "SetVideoWindow")
            {
                InvokeCommand = new TerminalDeviceCommand<SetVideoWindowCommand, SetVideoWindowResponse>(
                    this,
                    Name,
                    () => new SetVideoWindowCommand
                        {
                            FullScreen = true,
                        }
                    );
            }
        }

        public class StringMethod : TerminalDeviceMethod<DisplayStringCommand, DisplayStringResponse>
        {
            public StringMethod(ITerminalDevice device)
                : base(device, "DisplayString")
            {
                InvokeCommand = new TerminalDeviceCommand<DisplayStringCommand, DisplayStringResponse>(
                    this,
                    Name,
                    () => new DisplayStringCommand
                        {
                            Message = "Please Pay inside (E01)",
                            Id = 1,
                            ShowDataEntry = false,
                            ShowDataEntrySpecified = true,
                            ClearScreen = false,
                            ClearScreenSpecified = true,
                        }
                    );
            }
        }

        #endregion

        #region Device Events

        public class CurrentLanguageChangedEvent : TerminalDeviceEvent<CurrentLanguageChanged>
        {
            public CurrentLanguageChangedEvent(ITerminalDevice device)
                : base(device, "CurrentLanguageChanged")
            {
            }
        }

        #endregion
    }
}
