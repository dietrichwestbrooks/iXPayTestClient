using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class Display : TerminalDeviceT<DisplayCommand, DisplayResponse>
    {
        public Display()
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new CurrentLanguageProperty {Successor = this},
                    new MessageWindowPropertiesProperty {Successor = this},
                    new ImageDetailsProperty {Successor = this},
                    new PromptDetailsProperty {Successor = this},
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new DisplayPromptMethod {Successor = this},
                    new DeletePromptMethod {Successor = this},
                });
        }

        public void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalClientService>();
            Successor = terminal.Devices["Terminal"];
        }
    }

    public class CurrentLanguageProperty : PropertyCommandT<string,
        GetCurrentLanguageCommand, GetCurrentLanguageResponse,
        SetCurrentLanguageCommand, SetCurrentLanguageResponse>
    {
        public CurrentLanguageProperty()
            : base("CurrentLanguage")
        {
            CreateGetCommand = p => new GetCurrentLanguageCommand();
            //ProcessGetResponse = response => response.Language;

            CreateSetCommand = p => InternalCreateGetCommand(
               p.GetValue("value", 0, "en"));
        }

        private SetCurrentLanguageCommand InternalCreateGetCommand(string language)
        {
            return new SetCurrentLanguageCommand
            {
                Language = language,
            };
        }

        public override bool ProcessGetResponse(GetCurrentLanguageResponse response)
        {
            if (!base.ProcessGetResponse(response))
                return false;

            Value = response.Language;

            return true;
        }
    }

    public class MessageWindowPropertiesProperty : PropertyCommandT<MessageWindowProperties,
        GetMessageWindowPropertiesCommand, GetMessageWindowPropertiesResponse,
        SetMessageWindowPropertiesCommand, SetMessageWindowPropertiesResponse>
    {
        public MessageWindowPropertiesProperty()
            : base("MessageWindowProperties")
        {
            CreateGetCommand = p => new GetMessageWindowPropertiesCommand();
            //ProcessGetResponse = response => response.MessageWindowProperties;

            CreateSetCommand = p => new SetMessageWindowPropertiesCommand
                {
                    MessageWindowProperties = p.GetValue<MessageWindowProperties>("value", 0),
                };
        }

        public override bool ProcessGetResponse(GetMessageWindowPropertiesResponse response)
        {
            if (!base.ProcessGetResponse(response))
                return false;

            Value = response.MessageWindowProperties;

            return true;
        }
    }

    public class PromptDetailsProperty : PropertyCommandT<PromptDetails,
        GetPromptDetailsCommand, GetPromptDetailsResponse,
        SetPromptDetailsCommand, SetPromptDetailsResponse>
    {
        public PromptDetailsProperty()
            : base("PromptDetails")
        {
            CreateGetCommand = p => new GetPromptDetailsCommand();
            //ProcessGetResponse = response => response.PromptDetails;

            CreateSetCommand = p => new SetPromptDetailsCommand
            {
                PromptDetails = p.GetValue<PromptDetails>("value", 0)
            };
        }

        public override bool ProcessGetResponse(GetPromptDetailsResponse response)
        {
            if (!base.ProcessGetResponse(response))
                return false;

            Value = response.PromptDetails;

            return true;
        }
    }

    public class ImageDetailsProperty : PropertyCommandT<ImageDetails,
        GetImageDetailsCommand, GetImageDetailsResponse,
        SetImageDetailsCommand, SetImageDetailsResponse>
    {
        public ImageDetailsProperty()
            : base("ImageDetails")
        {
            CreateGetCommand = p => new GetImageDetailsCommand();
            //ProcessGetResponse = response => response.Value;

            CreateSetCommand = p => InternalCreateSetCommand(p.GetValue<ImageDetails>("value", 0));
        }

        private SetImageDetailsCommand InternalCreateSetCommand(ImageDetails value)
        {
            Value = value;

            return new SetImageDetailsCommand
            {
                ImageId = value.ImageId,
                Description = value.Description,
                Filename = value.Filename,
                Language = value.Language,
                TransparentColor = value.TransparentColor,
                Value = value.Value,
            };

        }

        public override bool ProcessGetResponse(GetImageDetailsResponse response)
        {
            if (!base.ProcessGetResponse(response))
                return false;
            
            Value.Value = response.Value;

            return true;
        }
    }

    public class DisplayPromptMethod : MethodCommandT<DisplayPromptCommand, DisplayPromptResponse>
    {
        public DisplayPromptMethod()
            : base("DisplayPrompt")
        {
            CreateInvokeCommand = p => InternalCreateCommand(
                p.GetValue("id", 0, -1),
                p.GetValue<string>("language", 1),
                p.GetValue("ignoreSoftKeySet", 2, false),
                p.GetValue("ignoreSoftKeySetSpecified", 3, false),
                p.GetArray<SubstituteParameter>("SubstituteParameter", 4));
        }

        private DisplayPromptCommand InternalCreateCommand(int id, string language, bool ignoreSoftKeySet,
            bool ignoreSoftKeySetSpecified, SubstituteParameter[] substituteParameter)
        {
            return new DisplayPromptCommand
            {
                Id = id,
                Language = language,
                IgnoreSoftKeySet = ignoreSoftKeySet,
                IgnoreSoftKeySetSpecified = ignoreSoftKeySetSpecified,
                SubstituteParameter = substituteParameter ?? new[] { new SubstituteParameter { Text = "A Text" } },
            };
        }
    }

    public class DeletePromptMethod : MethodCommandT<DeletePromptCommand, DeletePromptResponse>
    {
        public DeletePromptMethod()
            : base("DeletePrompt")
        {
            CreateInvokeCommand = p => InternalCreateCommand(
                p.GetValue("id", 0, -1),
                p.GetValue("language", 1, "en"));
        }

        private DeletePromptCommand InternalCreateCommand(int id, string language)
        {
            return new DeletePromptCommand
            {
                Id = id,
                Language = language,
            };
        }
    }

    public class DisplayStringMethod : MethodCommandT<DisplayStringCommand, DisplayStringResponse>
    {
        public DisplayStringMethod()
            : base("DisplayString")
        {
            CreateInvokeCommand = p => InternalCreateCommand(
                p.GetValue("message", 0, "Please Pay inside (E01)"),
                p.GetValue("showDataEntry", 1, false));
        }

        private DisplayStringCommand InternalCreateCommand(string message, bool showDataEntry)
        {
            return new DisplayStringCommand
            {
                Message = message,
                ShowDataEntry = showDataEntry,
            };
        }
    }
}
