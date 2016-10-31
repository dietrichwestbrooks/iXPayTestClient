using System.ComponentModel.Composition;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views
{
    /// <summary>
    /// Interaction logic for ScriptFileView.xaml
    /// </summary>
    [Export(NavigateViewNames.ScriptFileView, typeof(IScriptFileView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ScriptFileView : IScriptFileView
    {
        [ImportingConstructor]
        public ScriptFileView(IScriptFileViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            CodeTextEditor.SyntaxHighlighting =
                HighlightingLoader.Load(new XmlTextReader("ICSharpCode.PythonBinding.Resources.Python.xshd"),
                    HighlightingManager.Instance);
        }
    }
}
