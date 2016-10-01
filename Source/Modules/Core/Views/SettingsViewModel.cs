using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    [Export(typeof(ISettingsViewModel))]
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        private string _title = "Settings";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
