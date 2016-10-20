using System;
using System.ComponentModel.Composition;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views
{
    /// <summary>
    /// Interaction logic for ScriptView.xaml
    /// </summary>
    [Export(typeof(IScriptEditorView))]
    public partial class ScriptEditorView : IScriptEditorView
    {
        [ImportingConstructor]
        public ScriptEditorView(IScriptEditorViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();

            //CodeTextEditor.SyntaxHighlighting =
            //    HighlightingLoader.Load(new XmlTextReader("ICSharpCode.PythonBinding.Resources.Python.xshd"),
            //        HighlightingManager.Instance);
        }
    }
}
