using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(IMessageEditorViewModel))]
    public class MessageEditorViewModel : ViewModelBase, IMessageEditorViewModel
    {
        private string _title = "Message Editor";
        private string _message;

        public MessageEditorViewModel()
        {
            EventAggregator.GetEvent<CommandSelectedEvent>().Subscribe(OnCommandSelected);
            EventAggregator.GetEvent<CommandEditedEvent>().Subscribe(OnCommandEdited);

            MessageSerializer = ServiceLocator.Current.GetInstance<ITerminalMessageSerializer>();

            if (MessageSerializer == null)
                throw new InvalidOperationException("Unable to locate Terminal Message Serializer");
        }

        private ITerminalMessageSerializer MessageSerializer { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Message
        {
            get { return _message.FormattedXml(); }
            set
            {
                SetProperty(ref _message, value);
                FireMessageEdited(value);
            }
        }

        private void FireMessageEdited(string value)
        {
            TerminalMessage terminalMessage;

            if (MessageSerializer.TryDeserialize(value, out terminalMessage))
                EventAggregator.GetEvent<MessageEditedEvent>().Publish(terminalMessage);
        }

        private void OnCommandSelected(ITerminalDeviceCommand command)
        {
            var property = command as ITerminalDeviceProperty;

            if (property != null)
            {
                Message = property.GetGetMessage(new CommandParameters()).Serialize();
                return;
            }

            var method = command as ITerminalDeviceMethod;

            if (method != null)
            {
                Message = method.GetInvokeMessage(new CommandParameters()).Serialize();
            }
        }

        private void OnCommandEdited(TerminalMessage message)
        {
            Message = message.Serialize();
        }
    }
}
