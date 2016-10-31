using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    [Export(NavigateViewNames.LogView, typeof(ILogView))]
    public partial class LogView : ILogView
    {
        [ImportingConstructor]
        public LogView(ILogViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
