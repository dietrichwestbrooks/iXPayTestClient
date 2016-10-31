using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(IMessageEditorViewModel))]
    public class MessageEditorViewModel : ViewModelBase, IMessageEditorViewModel
    {
        private string _title = "Message Editor";
        private string _xml;
        private bool _enableMessageEditedEvent = true;

        public MessageEditorViewModel()
        {
            EventAggregator.GetEvent<CommandSelectedEvent>().Subscribe(OnCommandSelected);
            EventAggregator.GetEvent<CommandEditedEvent>().Subscribe(OnCommandEdited);

            MessageSerializer = ServiceLocator.Current.GetInstance<ITerminalMessageSerializer>();
            TerminalService = ServiceLocator.Current.GetInstance<ITerminalService>();

            ExecuteCommand = new DelegateCommand<string>(OnExecute, CanExecute);
        }

        public ICommand ExecuteCommand { get; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Xml
        {
            get { return _xml; }
            set
            {
                SetProperty(ref _xml, value);
                if (_enableMessageEditedEvent)
                    FireMessageEdited(value);
            }
        }

        private void FireMessageEdited(string xml)
        {
            TerminalMessage message;

            if (MessageSerializer.TryDeserialize(xml, out message))
                EventAggregator.GetEvent<MessageEditedEvent>().Publish(message);
        }

        private ITerminalService TerminalService { get; }

        private ITerminalMessageSerializer MessageSerializer { get; }

        private void OnCommandSelected(ITerminalDeviceCommand command)
        {
            TerminalMessage message = command.GetMessage();

            string xml;
            message.TrySerialize(out xml);

            if (xml != null)
                Xml = xml.FormattedXml();
        }

        private void OnCommandEdited(TerminalMessage message)
        {
            try
            {
                _enableMessageEditedEvent = false;

                Xml = message.Serialize().FormattedXml();
            }
            finally
            {
                _enableMessageEditedEvent = true;
            }
        }

        private bool CanExecute(string xml)
        {
            return true;
        }

        private void OnExecute(string xml)
        {
            EventAggregator.GetEvent<PreviewRunCommandsEvent>().Publish();

            TerminalMessage message = MessageSerializer.Deserialize(xml);

            TerminalService.SendMessage(message);
        }
    }
}
