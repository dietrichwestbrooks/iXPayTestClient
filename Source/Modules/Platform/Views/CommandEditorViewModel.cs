using System.ComponentModel.Composition;
using Prism.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(ICommandEditorViewModel))]
    public class CommandEditorViewModel : ViewModelBase, ICommandEditorViewModel
    {
        private object _command;
        private string _title = "Properties";
        private TerminalMessage _message;

        public CommandEditorViewModel()
        {
            EventAggregator.GetEvent<MessageEditedEvent>().Subscribe(OnMessageEdited);

            PropertyChangedCommand = new DelegateCommand<object>(OnPropertyChanged, o => true);
        }

        public DelegateCommand<object> PropertyChangedCommand { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public object Command
        {
            get { return _command; }
            set { SetProperty(ref _command, value); }
        }

        private void OnMessageEdited(TerminalMessage message)
        {
            _message = message;

            Command = _message.GetLastItem();
        }

        private void OnPropertyChanged(object obj)
        {
            EventAggregator.GetEvent<CommandEditedEvent>().Publish(_message);
        }
    }
}
