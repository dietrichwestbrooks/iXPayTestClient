using System.Collections.ObjectModel;
using System.Linq;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class DeviceViewModel : ViewModelBase, IDeviceViewModel
    {
        private string _title;
        private ObservableCollection<IDeviceMemberViewModel> _members;

        public DeviceViewModel(ITerminalDevice device)
        {
            Object = device;
            Title = device.Name;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<IDeviceMemberViewModel> Members
        {
            get
            {
                if (_members == null)
                {
                    _members = new ObservableCollection<IDeviceMemberViewModel>();

                    _members.AddRange(new ObservableCollection<IDeviceMemberViewModel>(
                        Object.Methods.Select(m => new DeviceMethodViewModel(m))));

                    Object.Methods.MethodAdded += (sender, m) => _members.Add(new DeviceMethodViewModel(m));

                    _members.AddRange(new ObservableCollection<DevicePropertyViewModel>(
                        Object.Properties.Select(p => new DevicePropertyViewModel(p))));

                    Object.Properties.PropertyAdded += (sender, p) => _members.Add(new DevicePropertyViewModel(p));

                    _members.AddRange(new ObservableCollection<DeviceEventViewModel>(
                        Object.Events.Select(p => new DeviceEventViewModel(p))));

                    Object.Properties.PropertyAdded += (sender, p) => _members.Add(new DevicePropertyViewModel(p));
                }

                return new ObservableCollection<IDeviceMemberViewModel>(_members.OrderBy(c => c.Title));
            }
        }

        public ITerminalDevice Object { get; }
    }
}
