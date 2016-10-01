using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    [Export(typeof(ISettingsView))]
    public partial class SettingsView : ISettingsView
    {
        [ImportingConstructor]
        public SettingsView(ISettingsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        public string FlyoutName { get; } = FlyoutNames.SettingsFlyout;
    }
}
