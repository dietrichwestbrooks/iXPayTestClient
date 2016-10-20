using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(IDeviceListViewModel))]
    public class DeviceListViewModel : ViewModelBase, IDeviceListViewModel
    {
        private ObservableCollection<DeviceViewModel> _devices;

        private string _title;

        public DeviceListViewModel()
        {
            Title = "Devices";

            SelectedObjectChangedCommand = new DelegateCommand<object>(OnSelectedObjectChanged);
        }

        public ICommand SelectedObjectChangedCommand { get; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<DeviceViewModel> Devices
        {
            get
            {
                if (_devices == null)
                {
                    _devices =
                        new ObservableCollection<DeviceViewModel>(
                            TerminalClientService.Devices.Select(d => new DeviceViewModel(d)));

                    TerminalClientService.Devices.DeviceAdded += (sender, d) => _devices.Add(new DeviceViewModel(d));
                }

                return _devices;
            }
        }

        [Import]
        private ITerminalClientService TerminalClientService { get; set; }

        private void OnSelectedObjectChanged(object item)
        {
            var command = item as IDeviceCommandViewModel;

            if (command != null)
            {
                EventAggregator.GetEvent<CommandSelectedEvent>().Publish(command.Object);
                return;
            }

            var property = item as IDevicePropertyViewModel;

            if (property != null)
            {
                EventAggregator.GetEvent<PropertySelectedEvent>().Publish(property.Object);
                return;
            }

            var method = item as IDeviceMethodViewModel;

            if (method != null)
            {
                EventAggregator.GetEvent<MethodSelectedEvent>().Publish(method.Object);
                return;
            }

            var @event = item as IDeviceEventViewModel;

            if (@event != null)
            {
                EventAggregator.GetEvent<EventSelectedEvent>().Publish(@event.Object);
            }

            var device = item as IDeviceViewModel;

            if (device != null)
            {
                EventAggregator.GetEvent<DeviceSelectedEvent>().Publish(device.Object);
            }
        }
    }
}
