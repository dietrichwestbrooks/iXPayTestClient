using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views
{
    public class TitleViewModel : ViewModelBase, IViewModel
    {
        private string _title;

        public TitleViewModel(string title)
        {
            _title = title;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
