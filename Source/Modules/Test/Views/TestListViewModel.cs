using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Test.Views
{
    [Export(typeof(ITestListViewModel))]
    public class TestListViewModel : ViewModelBase, ITestListViewModel
    {
        private string _title = "Test Runner";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
