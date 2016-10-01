using System.Windows.Media;
using Prism.Mvvm;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views
{
    public abstract class ViewModelDialogPopupBase : BindableBase
    {
        public abstract string Title { get; }

        public abstract ImageSource Icon { get; }
    }
}
