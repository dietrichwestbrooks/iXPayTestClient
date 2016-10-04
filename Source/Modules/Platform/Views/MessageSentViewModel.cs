using System;
using System.Threading;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class MessageSentViewModel : ViewModelBase, IViewModel
    {
        private string _title;
        private string _xml;
        private int _sequenceNumber;
        private DateTime _time;
        private MessageSentState _state;
        private Timer _responseTimer;

        public MessageSentViewModel(TerminalMessage message)
        {
            State = MessageSentState.Sent;
            _responseTimer = new Timer(WaitResponseElapsed, null, TimeSpan.FromSeconds(15), Timeout.InfiniteTimeSpan);
            EventAggregator.GetEvent<ResponseReceivedEvent>().Subscribe(OnResponseReceived);

            object command = message.GetLastItem();

            Time = DateTime.Now;
            SequenceNumber = message.GetCommandSequenceNumber();
            Title = command.GetType().Name;
            Xml = message.Serialize();

        }

        public DateTime Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
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

        public MessageSentState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private void WaitResponseElapsed(object state)
        {
            if (State == MessageSentState.Sent)
            {
                State = MessageSentState.Waiting;
                _responseTimer.Change(TimeSpan.FromSeconds(15), Timeout.InfiniteTimeSpan);
            }
            else if (State == MessageSentState.Waiting)
            {
                State = MessageSentState.Abandoned;
                _responseTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            }
        }

        private void OnResponseReceived(TerminalMessage message)
        {
            if (SequenceNumber == message.GetResponseSequenceNumber())
            {
                State = MessageSentState.Responded;
                _responseTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            }
        }
    }

    public enum MessageSentState
    {
        Sent,
        Responded,
        Waiting,
        Abandoned,
    }
}
