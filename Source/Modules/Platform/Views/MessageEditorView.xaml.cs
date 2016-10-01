using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    /// <summary>
    /// Interaction logic for MessageEditorView.xaml
    /// </summary>
    [Export(typeof(IMessageEditorView))]
    public partial class MessageEditorView : IMessageEditorView
    {
        [ImportingConstructor]
        public MessageEditorView(IMessageEditorViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
