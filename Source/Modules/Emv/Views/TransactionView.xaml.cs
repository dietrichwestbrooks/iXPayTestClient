using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Emv.Views
{
    /// <summary>
    /// Interaction logic for TransactionView.xaml
    /// </summary>
    [Export(typeof(ITransactionView))]
    public partial class TransactionView : ITransactionView
    {
        [ImportingConstructor]
        public TransactionView(ITransactionViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
