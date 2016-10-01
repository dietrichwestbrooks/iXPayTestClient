using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    /// <summary>
    /// Interaction logic for RightTitlebarCommands.xaml
    /// </summary>
    [Export]
    public partial class RightTitlebarCommands
    {
        [ImportingConstructor]
        public RightTitlebarCommands(IConnectViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
