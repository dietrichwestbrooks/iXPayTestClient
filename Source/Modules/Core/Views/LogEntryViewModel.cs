using Prism.Logging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    public class LogEntryViewModel : ViewModelBase, ILogEntryViewModel
    {
        private string _message;
        private Category _category;
        private Priority _priority;
        private string _title;
        private string _entry;

        public LogEntryViewModel(string message, Category category, Priority priority)
        {
            Title = "Log Entry";
            Message = message;
            Category = category;
            Priority = priority;
            Entry = $"Category: {Category}\nPriority: {Priority}\nMessage:\n{Message}";
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public Category Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }

        public Priority Priority
        {
            get { return _priority; }
            set { SetProperty(ref _priority, value); }
        }

        public string Entry
        {
            get { return _entry; }
            set { SetProperty(ref _entry, value); }
        }
    }
}
