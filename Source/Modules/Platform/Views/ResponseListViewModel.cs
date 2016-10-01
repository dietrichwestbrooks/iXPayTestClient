using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(IResponseListViewModel))]
    public class ResponseListViewModel : ViewModelBase, IResponseListViewModel
    {
        private string _title = "Responses";
        private Dispatcher _dispatcher;

        public ResponseListViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            EventAggregator.GetEvent<ResponseReceivedEvent>().Subscribe(OnResponseReceived);
        }

        private void OnResponseReceived(TerminalMessage message)
        {
            _dispatcher.BeginInvoke(DispatcherPriority.DataBind, (Action)(() => Messages.Add(new ResponseReceivedViewModel(message))));
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<ResponseReceivedViewModel> Messages { get; } = new ObservableCollection<ResponseReceivedViewModel>();
    }
}
