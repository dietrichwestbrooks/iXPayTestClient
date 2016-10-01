using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class ResponseReceivedViewModel : ViewModelBase, IViewModel
    {
        private readonly object _response;
        private string _title;
        private string _xml;
        private int _sequenceNumber;

        public ResponseReceivedViewModel(TerminalMessage message)
        {
            _response = message.GetBaseResponse();

            SequenceNumber = message.GetResponseSequenceNumber();
            Title = _response.GetType().Name;
            Xml = message.Serialize();
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
    }
}
