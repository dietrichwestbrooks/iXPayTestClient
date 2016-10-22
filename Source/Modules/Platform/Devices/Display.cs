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
                    new DisplayCurrentLanguageProperty(this),
                    new DisplayMessageWindowPropertiesProperty(this),
                    new DisplaySoftKeyPropertiesProperty(this),
                    new DisplayDataEntryWindowPropertiesProperty(this),
                    new DisplayImageDetailsProperty(this),
                    new DisplayImageListProperty(this),
                    new DisplayAnimationDetailsProperty(this),
                    new DisplayAnimationListProperty(this),
                    new DisplayPromptDetailsProperty(this),
                    new DisplayPromptListProperty(this),
                    new DisplayFontsProperty(this),
                    new DisplayStatusProperty(this),
                    new DisplayTypeProperty(this),
                    new DisplaySupportedLanguagesProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new DisplayPromptMethod(this),
                    new DeletePromptMethod(this),
                    new DisplayDeleteAllPromptsMethod(this),
                    new DisplayDeleteImageMethod(this),
                    new DisplayDeleteAllImagesMethod(this),
                    new DisplayDeleteAnimationMethod(this),
                    new DisplayDeleteAllAnimationsMethod(this),
                    new DisplayDeleteAllMethod(this),
                    new DisplayRestoreDefaultMethod(this),
                    new DisplaySetVideoWindowMethod(this),
                    new DisplayStringMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new DisplayCurrentLanguageChangedEvent(this),
                });
        }

        private void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalService>();
            Successor = terminal.Devices["Terminal"];
        }
    }

    #region Properties

    [ValueProperty("Language")]
    public class DisplayCurrentLanguageProperty : TerminalDeviceProperty<string,
        GetCurrentLanguageCommand, GetCurrentLanguageResponse,
        SetCurrentLanguageCommand, SetCurrentLanguageResponse>
    {
        public DisplayCurrentLanguageProperty(ITerminalDevice device)
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
    public class DisplayMessageWindowPropertiesProperty : TerminalDeviceProperty<MessageWindowProperties,
        GetMessageWindowPropertiesCommand, GetMessageWindowPropertiesResponse,
        SetMessageWindowPropertiesCommand, SetMessageWindowPropertiesResponse>
    {
        public DisplayMessageWindowPropertiesProperty(ITerminalDevice device)
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
    public class DisplaySoftKeyPropertiesProperty : TerminalDeviceProperty<SoftKeyProperties,
        GetSoftKeyPropertiesCommand, GetSoftKeyPropertiesResponse,
        SetSoftKeyPropertiesCommand, SetSoftKeyPropertiesResponse>
    {
        public DisplaySoftKeyPropertiesProperty(ITerminalDevice device)
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
    public class DisplayDataEntryWindowPropertiesProperty : TerminalDeviceProperty<DataEntryWindowProperties,
        GetDataEntryWindowPropertiesCommand, GetDataEntryWindowPropertiesResponse,
        SetDataEntryWindowPropertiesCommand, SetDataEntryWindowPropertiesResponse>
    {
        public DisplayDataEntryWindowPropertiesProperty(ITerminalDevice device)
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
    public class DisplayPromptDetailsProperty : TerminalDeviceProperty<PromptDetails,
        GetPromptDetailsCommand, GetPromptDetailsResponse,
        SetPromptDetailsCommand, SetPromptDetailsResponse>
    {
        public DisplayPromptDetailsProperty(ITerminalDevice device)
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
                        PromptDetails = new PromptDetails(),
                    }
                );
        }
    }

    [ValueProperty("PromptSummary")]
    public class DisplayPromptListProperty : TerminalDeviceProperty<PromptSummary[],
        GetPromptListCommand, GetPromptListResponse>
    {
        public DisplayPromptListProperty(ITerminalDevice device)
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
    public class DisplayImageDetailsProperty : TerminalDeviceProperty<byte[],
        GetImageDetailsCommand, GetImageDetailsResponse,
        SetImageDetailsCommand, SetImageDetailsResponse>
    {
        public DisplayImageDetailsProperty(ITerminalDevice device)
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
    public class DisplayImageListProperty : TerminalDeviceProperty<ImageSummary[],
        GetImageListCommand, GetImageListResponse>
    {
        public DisplayImageListProperty(ITerminalDevice device)
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
    public class DisplayAnimationDetailsProperty : TerminalDeviceProperty<AnimationDetails,
        GetAnimationDetailsCommand, GetAnimationDetailsResponse,
        SetAnimationDetailsCommand, SetAnimationDetailsResponse>
    {
        public DisplayAnimationDetailsProperty(ITerminalDevice device)
            : base(device, "AnimationDetails")
        {
            GetCommand = new TerminalDeviceCommand<GetAnimationDetailsCommand, GetAnimationDetailsResponse>(
                this,
                $"get_{Name}",
                () => new GetAnimationDetailsCommand
                    {
                        Language = "en",
                    }
                );

            SetCommand = new TerminalDeviceCommand<SetAnimationDetailsCommand, SetAnimationDetailsResponse>(
                this,
                $"set_{Name}",
                () => new SetAnimationDetailsCommand
                    {
                        Language = "en",
                    }
                );
        }
    }

    [ValueProperty("AnimationSummary")]
    public class DisplayAnimationListProperty : TerminalDeviceProperty<AnimationSummary[],
        GetAnimationListCommand, GetAnimationListResponse>
    {
        public DisplayAnimationListProperty(ITerminalDevice device)
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
    public class DisplayFontsProperty : TerminalDeviceProperty<PrinterFont[],
        GetFontsCommand, GetFontsResponse>
    {
        public DisplayFontsProperty(ITerminalDevice device)
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
    public class DisplaySupportedLanguagesProperty : TerminalDeviceProperty<SupportedLanguage[],
        GetSupportedLanguagesCommand, GetSupportedLanguagesResponse>
    {
        public DisplaySupportedLanguagesProperty(ITerminalDevice device)
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

    public class DisplayDeleteAllPromptsMethod : TerminalDeviceMethod<DeleteAllPromptsCommand, DeleteAllPromptsResponse>
    {
        public DisplayDeleteAllPromptsMethod(ITerminalDevice device)
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

    public class DisplayDeleteImageMethod : TerminalDeviceMethod<DeleteImageCommand, DeleteImageResponse>
    {
        public DisplayDeleteImageMethod(ITerminalDevice device)
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

    public class DisplayDeleteAllImagesMethod : TerminalDeviceMethod<DeleteAllImagesCommand, DeleteAllImagesResponse>
    {
        public DisplayDeleteAllImagesMethod(ITerminalDevice device)
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

    public class DisplayDeleteAnimationMethod : TerminalDeviceMethod<DeleteAnimationCommand, DeleteAnimationResponse>
    {
        public DisplayDeleteAnimationMethod(ITerminalDevice device)
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

    public class DisplayDeleteAllAnimationsMethod : TerminalDeviceMethod<DeleteAllAnimationsCommand, DeleteAllAnimationsResponse>
    {
        public DisplayDeleteAllAnimationsMethod(ITerminalDevice device)
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

    public class DisplayDeleteAllMethod : TerminalDeviceMethod<DeleteAllCommand, DeleteAllResponse>
    {
        public DisplayDeleteAllMethod(ITerminalDevice device)
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

    public class DisplayRestoreDefaultMethod : TerminalDeviceMethod<RestoreDefaultCommand, RestoreDefaultResponse>
    {
        public DisplayRestoreDefaultMethod(ITerminalDevice device)
            : base(device, "RestoreDefault")
        {
            InvokeCommand = new TerminalDeviceCommand<RestoreDefaultCommand, RestoreDefaultResponse>(
                this,
                Name
                );
        }
    }

    public class DisplaySetVideoWindowMethod : TerminalDeviceMethod<SetVideoWindowCommand, SetVideoWindowResponse>
    {
        public DisplaySetVideoWindowMethod(ITerminalDevice device)
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

    public class DisplayStringMethod : TerminalDeviceMethod<DisplayStringCommand, DisplayStringResponse>
    {
        public DisplayStringMethod(ITerminalDevice device)
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

    #region Events

    public class DisplayCurrentLanguageChangedEvent : TerminalDeviceEvent<CurrentLanguageChanged>
    {
        public DisplayCurrentLanguageChangedEvent(ITerminalDevice device) 
            : base(device, "CurrentLanguageChanged")
        {
        }
    }

    #endregion
}
