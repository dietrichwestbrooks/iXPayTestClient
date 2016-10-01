using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Emv.Views
{
    [Export(typeof(ITransactionViewModel))]
    public class TransactionViewModel : ViewModelBase, ITransactionViewModel
    {
        private string _title = "EMV Transaction Flow";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
