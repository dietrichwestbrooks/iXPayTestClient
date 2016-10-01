using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    /// <summary>
    /// Interaction logic for CommandEditorView.xaml
    /// </summary>
    [Export(typeof(ICommandEditorView))]
    public partial class CommandEditorView : ICommandEditorView
    {
        [ImportingConstructor]
        public CommandEditorView(ICommandEditorViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
