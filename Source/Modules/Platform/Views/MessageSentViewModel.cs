using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class MessageSentViewModel : ViewModelBase, IViewModel
    {
        private readonly TerminalMessage _message;
        private string _title;
        private string _xml;
        private int _sequenceNumber;

        public MessageSentViewModel(TerminalMessage message)
        {
            _message = message;

            var command = _message.GetBaseCommand();

            SequenceNumber = _message.GetCommandSequenceNumber();
            Title = command.GetType().Name;
            Xml = _message.Serialize();
        }

        public int SequenceNumber
        {
            get { return _sequenceNumber; }
            set { SetProperty(ref _sequenceNumber, value); }
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
