using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    [Export(typeof(IOutputViewModel))]
    public class OutputViewModel : ViewModelBase, IOutputViewModel
    {
        private string _title;
        private string _outputText;

        private Dictionary<OutputTextCategory, List<string>> _textLines = new Dictionary<OutputTextCategory, List<string>>();

        private OutputTextCategory _selectedCategory;

        public OutputViewModel()
        {
            Title = "Output";

            _selectedCategory = OutputTextCategory.Script;

            EventAggregator.GetEvent<OutputTextEvent>().Subscribe(OnOutputText);
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public OutputTextCategory SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }

        public string OutputText
        {
            get { return _outputText; }
            set { SetProperty(ref _outputText, value); }
        }

        public ObservableCollection<string> Categories => new ObservableCollection<string>(Enum.GetNames(typeof(OutputTextCategory)));

        private void OnOutputText(OutputTextEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.Text))
                return;

            args.Text = args.Text.Replace(Environment.NewLine, string.Empty);

            args.Text = args.Text + Environment.NewLine;

            //OutputText += args.Text;
            if (!_textLines.ContainsKey(args.Category))
                _textLines.Add(args.Category, new List<string>());

            _textLines[args.Category].Add(args.Text);

            if (args.Category == SelectedCategory)
                OutputText = string.Concat(_textLines[args.Category]);
        }
    }
}
