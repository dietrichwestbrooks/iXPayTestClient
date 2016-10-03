using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Convert = Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Utility.Convert;

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

        #region Convenience Methods and Properties
        // Add methods and properties that make it more convienent to send commands
        // to this device from scripting environment

        public void SetPromptWithImage(int promptId, string promptText, int promptLeft, int promptTop, string language, int imageLeft, int imageTop, int imageId, string imagePath)
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MessageSentEvent>().Publish(Methods.First().GetInvokeMessage(new CommandParameters()));
            eventAggregator.GetEvent<ResponseReceivedEvent>().Publish(new TerminalMessage
            {
                Item = new TerminalResponse
                {
                    Item = new DisplayResponse
                    {
                        Item = new DisplayPromptResponse
                        {
                            Success = true,
                            Message = "OK"
                        }
                    }
                }
            });

            eventAggregator.GetEvent<EventReceivedEvent>().Publish(new TerminalMessage
            {
                Item = new TerminalEvent
                {
                    Item = new DisplayEvent
                    {
                        Item = new CurrentLanguageChanged
                        {
                            Language = "en"
                        }
                    }
                }
            });
            return;

            var imageValue = new Convert().ToImageByteArray(imagePath);
            var filename = Path.GetFileNameWithoutExtension(imagePath);

            ((dynamic) this).set_ImageDetails(imageId: imageId, language: language, filename: filename, value: imageValue);

            ((dynamic) this).set_PromptDetails(language: language, value: new PromptDetails
                {
                    PromptId = promptId,
                    PromptText = promptText,
                    Location = new Location
                        {
                            Left = promptLeft,
                            Top = promptTop
                        },
                    Images = new Images
                        {
                            ImageInstance = new[]
                                {
                                    new ImageInstance
                                        {
                                            ImageId = imageId,
                                            Location = new Location
                                                {
                                                    Left = imageLeft,
                                                    Top = imageTop
                                                },
                                        }
                                },
                        }
                });
        }

        #endregion
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

            CreateSetCommand = p => new SetCurrentLanguageCommand
                {
                    Language = p.GetValue("value", 0, "en"),
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
                    Language = p.GetValue("language", 0, "en"),
                    PromptDetails = p.GetValue<PromptDetails>("value", 1),
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

            CreateSetCommand = p => new SetImageDetailsCommand
                {
                    ImageId = p.GetValue("imageId", 0, 1),
                    Filename = p.GetValue("filename", 1, "bac1re1.bmp"),
                    Description = p.GetValue<string>("description", 2),
                    Language = p.GetValue("language", 3, "en"),
                    TransparentColor = p.GetValue<byte[]>("transparentColor", 4),
                    Value = p.GetValue<byte[]>("value", 5),
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
            CreateInvokeCommand = p => new DisplayPromptCommand
                {
                    Id = p.GetValue("id", 0, -1),
                    Language = p.GetValue<string>("language", 1),
                    IgnoreSoftKeySet = p.GetValue("ignoreSoftKeySet", 2, false),
                    IgnoreSoftKeySetSpecified = p.GetValue("ignoreSoftKeySetSpecified", 3, false),
                    SubstituteParameter = p.GetArray("SubstituteParameter", 4, new[]
                        {
                            new SubstituteParameter {Text = "A Text"}
                        })
                };
        }
    }

    public class DeletePromptMethod : MethodCommandT<DeletePromptCommand, DeletePromptResponse>
    {
        public DeletePromptMethod()
            : base("DeletePrompt")
        {
            CreateInvokeCommand = p => new DeletePromptCommand
                {
                    Id = p.GetValue("id", 0, -1),
                    Language = p.GetValue("language", 1, "en")
                };
        }
    }

    public class DisplayStringMethod : MethodCommandT<DisplayStringCommand, DisplayStringResponse>
    {
        public DisplayStringMethod()
            : base("DisplayString")
        {
            CreateInvokeCommand = p => new DisplayStringCommand
                {
                    Message = p.GetValue("message", 0, "Please Pay inside (E01)"),
                    ShowDataEntry = p.GetValue("showDataEntry", 1, false)
                };
        }
    }
}
