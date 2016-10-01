using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    /// <summary>
    /// Interaction logic for MessageLIstView.xaml
    /// </summary>
    [Export(typeof(IMessageListView))]
    public partial class MessageLIstView : IMessageListView
    {
        [ImportingConstructor]
        public MessageLIstView(IMessageListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
