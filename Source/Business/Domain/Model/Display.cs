using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    [Attributes.TerminalDevice(Constants.DeviceNames.Display, typeof(Display))]
    public class Display : TerminalDevice
    {
        //public override string Name { get; } = "Display";

        private readonly ITerminalController _terminalController;

        [ImportingConstructor]
        public Display(ITerminalController terminalController)
        {
            if (terminalController == null)
                throw new ArgumentNullException(nameof(terminalController));

            _terminalController = terminalController;
        }

        public override bool IsActive => true;

        [Attributes.TerminalDeviceProperty(typeof(GetFontsCommand), typeof(GetFontsResponse), Successor = typeof(DisplayRequestHandler))]
        public IEnumerable<Font> Fonts { get; }

        [Attributes.TerminalDeviceProperty(typeof(GetStatusCommand), typeof(GetStatusResponse), Successor = typeof(DisplayRequestHandler))]
        public StatusState Status { get; }

        [Attributes.TerminalDeviceProperty(typeof(GetSupportedLanguagesCommand), typeof(GetSupportedLanguagesResponse), Successor = typeof(DisplayRequestHandler))]
        public IEnumerable<SupportedLanguage> SupportedLanguages { get; }

        [Attributes.TerminalDeviceProperty(typeof (GetCurrentLanguageCommand), typeof (GetCurrentLanguageResponse),
            typeof (SetCurrentLanguageCommand), typeof (SetCurrentLanguageResponse),
            Successor = typeof (DisplayRequestHandler))]
        public string CurrentLanguage
        {
            get
            {
                string message;
                GetCurrentLanguageResponse response;

                var command = new GetCurrentLanguageCommand();

                if (!_terminalController.SendCommand(this, command, out response, out message))
                    throw new InvalidOperationException(message);

                string value = string.Empty;

                return value;
            }
            set
            {
                string message;
                SetCurrentLanguageResponse response;

                var command = new SetCurrentLanguageCommand()
                    {
                        Language = value
                    };

                if (!_terminalController.SendCommand(this, command, out response, out message))
                    throw new InvalidOperationException(message);
            }
        }

        [Attributes.TerminalDeviceProperty(typeof (GetMessageWindowPropertiesCommand), typeof (GetMessageWindowPropertiesResponse),
            typeof (SetMessageWindowPropertiesCommand), typeof (SetMessageWindowPropertiesResponse), Successor = typeof(DisplayRequestHandler))]
        public MessageWindowProperties MessageWindowProperties { get; set; }

        [Attributes.TerminalDeviceCommand(typeof(DisplayPromptCommand), typeof(DisplayPromptResponse), Successor = typeof(DisplayRequestHandler))]
        public void DisplayPrompt(string language = "en", bool ignoreSoftKeySet = false, params SubstituteParameter[] substituteParameters)
        {
            string message;
            DisplayPromptResponse response;

            var command = new DisplayPromptCommand
                {
                    IgnoreSoftKeySet = ignoreSoftKeySet,
                    Language = language,
                    IgnoreSoftKeySetSpecified = true,
                    SubstituteParameter = null,
                };

            if (!_terminalController.SendCommand(this, command, out response, out message))
                throw new InvalidOperationException(message);
        }

        [Attributes.TerminalDeviceCommand(typeof(DeletePromptCommand), typeof(DeletePromptResponse), Successor = typeof(DisplayRequestHandler))]
        public void DeletePrompt(string language = "en")
        {
            string message;
            DeletePromptResponse response;

            var command = new DeletePromptCommand
                {
                    Language = language,
                };

            if (!_terminalController.SendCommand(this, command, out response, out message))
                throw new InvalidOperationException(message);
        }

        [Attributes.TerminalDeviceEvent(typeof(CurrentLanguageChanged))]
        private event EventHandler<string> CurrentLanguageChanged;
    }
}
