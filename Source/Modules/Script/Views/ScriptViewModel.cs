using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace iXPayTestClient.Modules.Script.Views
{
    [Export(typeof(IScriptViewModel))]
    public class ScriptViewModel : ViewModelBase, IScriptViewModel
    {
        private string _title = "Script Editor";
        private string _outputText;
        private string _code;

        [ImportingConstructor]
        public ScriptViewModel()
        {
            ScriptService = ServiceLocator.Current.GetInstance<IScriptService>();

            EventAggregator.GetEvent<ScriptOutputEvent>().Subscribe(OnScriptOutput);

            ExecuteCommand = new DelegateCommand<string>(OnExecute, CanExecute);

            Code = System.IO.File.ReadAllText("~AutoSave.py");
        }

        public DelegateCommand<string> ExecuteCommand { get; set; }

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

        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        private IScriptService ScriptService { get; }

        private void OnScriptOutput(string message)
        {
            OutputText += message;
        }

        private bool CanExecute(object arg)
        {
            return true;
        }

        private void OnExecute(string code)
        {
            try
            {
                OutputText = string.Empty;

                System.IO.File.WriteAllText("~AutoSave.py", code);

                ScriptService.ExecuteScript(code);
            }
            catch (Exception ex)
            {
                ScriptService.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
