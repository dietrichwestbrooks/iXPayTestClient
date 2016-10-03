using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class EventReceivedViewModel : ViewModelBase, IViewModel
    {
        private object _event;
        private string _title;
        private string _xml;

        public EventReceivedViewModel(TerminalMessage message)
        {
            _event = message.GetBaseEvent();

            Title = _event.GetType().Name;
            Xml = message.Serialize();
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Xml
        {
            get { return _xml?.FormattedXml(); }
            set { SetProperty(ref _xml, value); }
        }
    }
}
