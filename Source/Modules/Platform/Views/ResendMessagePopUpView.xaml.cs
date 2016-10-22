using System.ComponentModel.Composition;
using System.Windows;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    /// <summary>
    /// Interaction logic for ResendMessagePopUpView.xaml
    /// </summary>
    [Export("ResendMessagePopUpView")]
    public partial class ResendMessagePopUpView : IResendMessagePopUpView, IClosable
    {
        [ImportingConstructor]
        public ResendMessagePopUpView(IResendMessagePopUpViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        public void Close()
        {
            var window = WpfHelper.FindParent<Window>(this);
            window?.Close();
        }
    }
}
