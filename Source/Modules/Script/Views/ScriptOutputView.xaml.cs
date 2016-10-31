using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views
{
    /// <summary>
    /// Interaction logic for ScriptOutputView.xaml
    /// </summary>
    [Export(NavigateViewNames.ScriptOutputView, typeof(IScriptOutputView))]
    public partial class ScriptOutputView : IScriptOutputView
    {
        [ImportingConstructor]
        public ScriptOutputView(IScriptOutputViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
