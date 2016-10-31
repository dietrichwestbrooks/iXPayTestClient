using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    /// <summary>
    /// Interaction logic for OutputViewerView.xaml
    /// </summary>
    [Export(typeof(IOutputViewerView))]
    public partial class OutputViewerView : IOutputViewerView
    {
        [ImportingConstructor]
        public OutputViewerView(IOutputViewerViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
