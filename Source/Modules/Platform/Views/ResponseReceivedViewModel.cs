using System;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class ResponseReceivedViewModel : ViewModelBase, IViewModel
    {
        private string _title;
        private string _xml;
        private int _sequenceNumber;
        private DateTime _time;
        private bool _success;
        private string _responseMessage;

        public ResponseReceivedViewModel(TerminalMessage message)
        {
            Success = message.GetResponseSuccess();
            ResponseMessage = message.GetResponseMessage();
            Time = DateTime.Now;
            SequenceNumber = message.GetResponseSequenceNumber();
            Title = message.GetLastItem().GetType().Name;
            message.TrySerialize(out _xml);
        }

        public string ResponseMessage
        {
            get { return _responseMessage; }
            set { SetProperty(ref _responseMessage, value); }
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

        public int SequenceNumber
        {
            get { return _sequenceNumber; }
            set { SetProperty(ref _sequenceNumber, value); }
        }

        public string Xml
        {
            get { return _xml?.FormattedXml(); }
            set { SetProperty(ref _xml, value); }
        }

        public bool Success
        {
            get { return _success; }
            set { SetProperty(ref _success, value); }
        }
    }
}
