using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
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
        private string _deviceName;

        public EventReceivedViewModel(TerminalMessage message)
        {
            TerminalService = ServiceLocator.Current.GetInstance<ITerminalService>();

            Time = DateTime.Now;
            Title = message.GetLastItem().GetType().Name;
            message.TrySerialize(out _xml);

            object eventObject = message.GetSecondToLastItem();

            if (eventObject != null)
            {
                Type eventType = eventObject.GetType();

                ITerminalDevice device = TerminalService.Devices.FirstOrDefault(d => d.EventType == eventType);

                DeviceName = device?.Name ?? string.Empty;
            }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string DeviceName
        {
            get { return _deviceName; }
            set { SetProperty(ref _deviceName, value); }
        }

        public DateTime Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        public string Xml
        {
            get { return _xml?.FormattedXml(); }
            set { SetProperty(ref _xml, value); }
        }

        private ITerminalService TerminalService { get; }
    }
}
