using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using System.Windows.Threading;
using Prism.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(IMessageListViewModel))]
    public class MessageListViewModel : ViewModelBase, IMessageListViewModel
    {
        private string _title = "Messages";
        private Dispatcher _dispatcher;
        private bool _clearOnRun;

        public MessageListViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            ClearOnRun = true;

            EventAggregator.GetEvent<MessageSentEvent>().Subscribe(OnMessageSent);

            DelteAllCommand = new DelegateCommand(OnDeleteAll);

            EventAggregator.GetEvent<PreviewRunCommandsEvent>().Subscribe(OnPreviewRunCommands);
        }

        public ICommand DelteAllCommand { get; }

        public bool ClearOnRun
        {
            get { return _clearOnRun; }
            set { SetProperty(ref _clearOnRun, value); }
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

        private void OnDeleteAll()
        {
            Messages.Clear();
        }

        private void OnPreviewRunCommands()
        {
            if (ClearOnRun)
                Messages.Clear();
        }
    }
}
