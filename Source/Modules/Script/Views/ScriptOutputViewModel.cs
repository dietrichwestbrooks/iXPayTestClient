using System;
using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views
{
    [Export(typeof(IScriptOutputViewModel))]
    [Export(typeof(IOutputViewModel))]
    public class ScriptOutputViewModel : ViewModelBase, IScriptOutputViewModel
    {
        private string _title;
        private string _outputText;
        private bool _isActive;

        public ScriptOutputViewModel()
        {
            Title = "Script";

            EventAggregator.GetEvent<ScriptOutputTextEvent>().Subscribe(OnOutputText);
            IsActive = true;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string OutputText
        {
            get { return _outputText; }
            set { SetProperty(ref _outputText, value); }
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

        public event EventHandler IsActiveChanged;

        private void FireActiveChanged()
        {
            IsActiveChanged?.Invoke(this, new EventArgs());
        }

        private void OnOutputText(string text)
        {
            OutputText += text;
        }
    }
}
