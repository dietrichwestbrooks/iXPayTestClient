using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(IMessageListViewModel))]
    public class MessageListViewModel : ViewModelBase, IMessageListViewModel
    {
        private string _title = "Messages";
        private Dispatcher _dispatcher;

        public MessageListViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            EventAggregator.GetEvent<MessageSentEvent>().Subscribe(OnMessageSent);
        }

        private void OnMessageSent(TerminalMessage message)
        {
            _dispatcher.BeginInvoke(DispatcherPriority.DataBind, (Action)(() => Messages.Add(new MessageSentViewModel(message))));
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<MessageSentViewModel> Messages { get; } = new ObservableCollection<MessageSentViewModel>();
    }
}
