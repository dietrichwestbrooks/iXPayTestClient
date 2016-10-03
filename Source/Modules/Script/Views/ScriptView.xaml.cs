using System.ComponentModel.Composition;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views
{
    /// <summary>
    /// Interaction logic for ScriptView.xaml
    /// </summary>
    [Export(typeof(IScriptView))]
    public partial class ScriptView : IScriptView
    {
        [ImportingConstructor]
        public ScriptView(IScriptViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            CodeTextEditor.SyntaxHighlighting =
                HighlightingLoader.Load(new XmlTextReader("ICSharpCode.PythonBinding.Resources.Python.xshd"),
                    HighlightingManager.Instance);
        }
    }
}
