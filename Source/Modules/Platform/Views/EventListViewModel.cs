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
    [Export(typeof(IEventListViewModel))]
    public class EventListViewModel : ViewModelBase, IEventListViewModel
    {
        private string _title = "Events";
        private Dispatcher _dispatcher;
        private bool _clearOnRun;

        public EventListViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            ClearOnRun = false;

            EventAggregator.GetEvent<EventReceivedEvent>().Subscribe(OnEventReceived);

            DelteAllCommand = new DelegateCommand(OnDeleteAll);

            EventAggregator.GetEvent<PreviewRunCommandsEvent>().Subscribe(OnPreviewRunCommands);
        }

        public bool ClearOnRun
        {
            get { return _clearOnRun; }
            set { SetProperty(ref _clearOnRun, value); }
        }

        public ICommand DelteAllCommand { get; }

        private void OnEventReceived(TerminalMessage message)
        {
            _dispatcher.BeginInvoke(DispatcherPriority.DataBind, (Action)(() => Messages.Add(new EventReceivedViewModel(message))));
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<EventReceivedViewModel> Messages { get; } = new ObservableCollection<EventReceivedViewModel>();

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
