using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using Prism.Logging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    [Export(typeof(ILogViewModel))]
    [Export(typeof(IOutputViewModel))]
    public class LogViewModel : ViewModelBase, ILogViewModel, ILogTarget
    {
        private Dispatcher _dispatcher;
        private string _title;
        private bool _isActive;
        private string _text;

        public LogViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            Title = "Log";

            Logger.AddTarget(this);
        }

        public ObservableCollection<LogEntryViewModel> LogEntries { get; } = new ObservableCollection<LogEntryViewModel>();

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                FireActiveChanged();
            }
        }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public event EventHandler IsActiveChanged;

        private void FireActiveChanged()
        {
            IsActiveChanged?.Invoke(this, new EventArgs());
        }

        public void Log(string message, Category category, Priority priority)
        {
            _dispatcher.BeginInvoke(DispatcherPriority.DataBind, 
                (Action)(() => LogEntries.Add(new LogEntryViewModel(message, category, priority))));
        }
    }
}
