using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    /// <summary>
    /// Interaction logic for OutputView.xaml
    /// </summary>
    [Export(typeof(IOutputView))]
    public partial class OutputView : IOutputView
    {
        [ImportingConstructor]
        public OutputView(IOutputViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
