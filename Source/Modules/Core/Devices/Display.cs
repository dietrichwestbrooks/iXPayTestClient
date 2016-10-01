using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Scriptable;
using Font = Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging.Font;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [Export(typeof(ITerminalRequestHandler))]
    [Export(typeof(IDynamicTerminalDevice))]
    public class Display : DeviceObjectT<DisplayCommand, DisplayResponse>, IPartImportsSatisfiedNotification
    {
        public Display()
        {
            MethodCommands = new List<MethodCommand>
                {
                    new DisplayPrompt {SuccessorType = GetType()},
                    new DeletePrompt {SuccessorType = GetType()},
                };

            PropertyCommands = new List<PropertyCommand>
                {
                    new CurrentLanguage {SuccessorType = GetType()},
                    new MessageWindowProperties {SuccessorType = GetType()},
                };
        }

        public class CurrentLanguage : PropertyCommandT<string,
            GetCurrentLanguageCommand, GetCurrentLanguageResponse,
            SetCurrentLanguageCommand, SetCurrentLanguageResponse>
        {
            public CurrentLanguage()
            {
                CreateGetCommand = () => new GetCurrentLanguageCommand();

                ProcessGetResponse = response => response.Language;

                CreateSetCommand = p => InternalCreateGetCommand(
                   p.GetValue("language", 0, "en"));
            }

            private SetCurrentLanguageCommand InternalCreateGetCommand(string language)
            {
                return new SetCurrentLanguageCommand
                    {
                        Language = language,
                    };
            }
        }

        public class MessageWindowProperties : PropertyCommandT<Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging.MessageWindowProperties,
            GetMessageWindowPropertiesCommand, GetMessageWindowPropertiesResponse,
            SetMessageWindowPropertiesCommand, SetMessageWindowPropertiesResponse>
        {
            public MessageWindowProperties()
            {
                CreateGetCommand = () => new GetMessageWindowPropertiesCommand();
                
                ProcessGetResponse = response => response.MessageWindowProperties;

                CreateSetCommand = p => p.Has("MessageWindowProperties")
                    ? InternalCreateSetCommand(p.GetValue<Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging.MessageWindowProperties>("properties", 0))
                    : InternalCreateSetCommand(
                        p.GetValue("left", 0, 150),
                        p.GetValue("top", 1, 150),
                        p.GetValue("width", 2, 340),
                        p.GetValue("height", 3, 400),
                        p.GetValue("horizontalAlignment", 4, HorizontalAlignment.Center),
                        p.GetValue("verticalAlignment", 5, VerticalAlignment.Top),
                        p.GetValue("backColor", 6, BitConverter.GetBytes(-16777216)),
                        p.GetValue("foreColor", 7, BitConverter.GetBytes(-1)),
                        p.GetValue("fontName", 8, "Tahoma"),
                        p.GetValue("fontSize", 9, 18),
                        p.GetValue("fontStyle", 10, 1));
            }

            private SetMessageWindowPropertiesCommand InternalCreateSetCommand(int left, int top, int width, int height,
                HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, byte[] backColor,
                byte[] foreColor, string fontName, int fontSize, int fontStyle)
            {
                return new SetMessageWindowPropertiesCommand
                    {
                        MessageWindowProperties = new Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging.MessageWindowProperties
                            {
                                Location = new Location
                                    {
                                        Left = left,
                                        Top = top
                                    },
                                Size = new Size
                                    {
                                        Width = width,
                                        Height = height,
                                    },
                                HorizontalAlignment = horizontalAlignment,
                                VerticalAlignment = verticalAlignment,
                                BackColor = backColor,
                                ForeColor = foreColor,
                                Font = new Font
                                    {
                                        Name = fontName,
                                        Size = fontSize,
                                        Style = fontStyle,
                                    },
                            },
                    };
            }

            private SetMessageWindowPropertiesCommand InternalCreateSetCommand(Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging.MessageWindowProperties properties)
            {
                return new SetMessageWindowPropertiesCommand
                    {
                        MessageWindowProperties = properties,
                    };
            }
        }

        public class DisplayPrompt : MethodCommandT<DisplayPromptCommand, DisplayPromptResponse>
        {
            public DisplayPrompt()
            {
                CreateCommand = p => InternalCreateCommand(
                    p.GetValue("id", 0, -1), 
                    p.GetValue("language", 1, "en"),
                    p.GetValue("ignoreSoftKeySet", 2, false), 
                    p.GetValue("ignoreSoftKeySetSpecified", 3, false),
                    p.GetArray<Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging.SubstituteParameter>("SubstituteParameter", 4));
            }

            private DisplayPromptCommand InternalCreateCommand(int id, string language, bool ignoreSoftKeySet,
                bool ignoreSoftKeySetSpecified, Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging.SubstituteParameter[] substituteParameter)
            {
                return new DisplayPromptCommand
                    {
                        Id = id,
                        Language = language,
                        IgnoreSoftKeySet = ignoreSoftKeySet,
                        IgnoreSoftKeySetSpecified = ignoreSoftKeySetSpecified,
                        SubstituteParameter = substituteParameter
                    };
            }
        }

        public class DeletePrompt : MethodCommandT<DeletePromptCommand, DeletePromptResponse>
        {
            public DeletePrompt()
            {
                CreateCommand = p => InternalCreateCommand(
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

        public void OnImportsSatisfied()
        {
            var terminal = ServiceLocator.Current.GetInstance<object>("Terminal");
            SuccessorType = terminal.GetType();
        }
    }
}
