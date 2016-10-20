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
    public class Display : TerminalDevice<DisplayCommand, DisplayResponse, DisplayEvent>
    {
        public Display()
            : base("Display")
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

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
                    new DisplayStatusProperty(this),
                    new DisplayTypeProperty(this),
                    new SupportedLanguagesProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new DisplayPromptMethod(this),
                    new DeletePromptMethod(this),
                    new DeleteAllPromptsMethod(this),
                    new DeleteImageMethod(this),
                    new DeleteAllImagesMethod(this),
                    new DeleteAnimationMethod(this),
                    new DeleteAllAnimationsMethod(this),
                    new DeleteAllMethod(this),
                    new RestoreDefaultMethod(this),
                    new SetVideoWindowMethod(this),
                    new DisplayStringMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new CurrentLanguageChangedEvent(this),
                });
        }

        private void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalClientService>();
            Successor = terminal.Devices["Terminal"];
        }
    }

    #region Properties

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
                new SortedList<string, object>
                    {
                        {"value", "en"},
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

            SetCommand = new TerminalDeviceCommand<SetMessageWindowPropertiesCommand, SetMessageWindowPropertiesResponse>(
                this,
                $"set_{Name}"
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
                $"set_{Name}"
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

            SetCommand = new TerminalDeviceCommand<SetDataEntryWindowPropertiesCommand, SetDataEntryWindowPropertiesResponse>(
                this,
                $"set_{Name}"
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
                new SortedList<string, object>
                    {
                        {"promptId", 1},
                        {"language", "en"},
                    }
                );

            SetCommand = new TerminalDeviceCommand<SetPromptDetailsCommand, SetPromptDetailsResponse>(
                this,
                $"set_{Name}",
                new SortedList<string, object>
                    {
                        {"language", "en"},
                        {"value", new PromptDetails()},
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
                new SortedList<string, object>
                    {
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"imageId", 1},
                        {"language", "en"},
                    }
                );

            SetCommand = new TerminalDeviceCommand<SetImageDetailsCommand, SetImageDetailsResponse>(
                this,
                $"set_{Name}",
                new SortedList<string, object>
                    {
                        {"imageId", 1},
                        {"filename", "bac1re1.bmp"},
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"language", "en"},
                    }
                );

            SetCommand = new TerminalDeviceCommand<SetAnimationDetailsCommand, SetAnimationDetailsResponse>(
                this,
                $"set_{Name}",
                new SortedList<string, object>
                    {
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"language", "en"},
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
    public class DisplayStatusProperty : TerminalDeviceProperty<Status,
        GetStatusCommand, GetStatusResponse>
    {
        public DisplayStatusProperty(ITerminalDevice device)
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
    public class DisplayTypeProperty : TerminalDeviceProperty<DisplayType,
        GetDisplayTypeCommand, GetDisplayTypeResponse>
    {
        public DisplayTypeProperty(ITerminalDevice device)
            : base(device, "DisplayType")
        {
            GetCommand = new TerminalDeviceCommand<GetDisplayTypeCommand, GetDisplayTypeResponse>(
                this,
                $"get_{Name}"
                );
        }
    }

    #endregion

    #region Methods

    public class DisplayPromptMethod : TerminalDeviceMethod<DisplayPromptCommand, DisplayPromptResponse>
    {
        public DisplayPromptMethod(ITerminalDevice device)
            : base(device, "DisplayPrompt")
        {
            InvokeCommand = new TerminalDeviceCommand<DisplayPromptCommand, DisplayPromptResponse>(
                this,
                Name,
                new SortedList<string, object>
                    {
                        {"id", 1},
                        {"language", "en"},
                        {"ignoreSoftKeySet", false},
                        {"ignoreSoftKeySetSpecified", true},
                        {"SubstituteParameter", new[]
                            {
                                new SubstituteParameter {Text = "A Text"}
                            }},
                    }
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
                new SortedList<string, object>
                    {
                        {"id", 1},
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"id", 1},
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"id", 1},
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"id", 1},
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"language", "en"},
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
                new SortedList<string, object>
                    {
                        {"fullScreen", true},
                    }
                );
        }
    }

    public class DisplayStringMethod : TerminalDeviceMethod<DisplayStringCommand, DisplayStringResponse>
    {
        public DisplayStringMethod(ITerminalDevice device)
            : base(device, "DisplayString")
        {
            InvokeCommand = new TerminalDeviceCommand<DisplayStringCommand, DisplayStringResponse>(
               this,
               Name,
                new SortedList<string, object>
                    {
                        {"message", "Please Pay inside (E01)"},
                        {"id", 1},
                        {"showDataEntry", false},
                        {"showDataEntrySpecified", true},
                        {"clearScreen", false},
                        {"clearScreenSpecified", true},
                    }
               );
        }
    }

    #endregion

    #region Events

    public class CurrentLanguageChangedEvent : TerminalDeviceEvent<CurrentLanguageChanged>
    {
        public CurrentLanguageChangedEvent(ITerminalDevice device) 
            : base(device, "CurrentLanguageChanged")
        {
        }
    }

    #endregion
}
