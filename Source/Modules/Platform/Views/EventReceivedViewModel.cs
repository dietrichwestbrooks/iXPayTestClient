using System;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class EventReceivedViewModel : ViewModelBase, IViewModel
    {
        private string _title;
        private string _xml;
        private DateTime _time;

        public EventReceivedViewModel(TerminalMessage message)
        {
            Time = DateTime.Now;
            Title = message.GetLastItem().GetType().Name;
            Xml = message.Serialize();
        }

        public DateTime Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
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
