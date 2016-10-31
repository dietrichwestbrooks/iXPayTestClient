using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
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
        private FilterOption _filterOption;

        public DeviceListViewModel()
        {
            Title = "Devices";

            SelectedObjectChangedCommand = new DelegateCommand<object>(OnSelectedObjectChanged);

            TerminalService = ServiceLocator.Current.GetInstance<ITerminalService>();

            FilterOption = FilterOption.ShowAll;
        }

        public ICommand SelectedObjectChangedCommand { get; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public FilterOption FilterOption
        {
            get { return _filterOption; }
            set { SetProperty(ref _filterOption, value); }
        }

        public ObservableCollection<DeviceViewModel> Devices
        {
            get
            {
                if (_devices == null)
                {
                    _devices =
                        new ObservableCollection<DeviceViewModel>(
                            TerminalService.Devices.Select(d => new DeviceViewModel(d)));

                    //CollectionViewSource.GetDefaultView(_devices)
                    //    .SortDescriptions
                    //    .Add(new SortDescription("Title", ListSortDirection.Ascending));

                    TerminalService.Devices.DeviceAdded += (sender, d) =>
                    {
                        _devices.Add(new DeviceViewModel(d));

                        //CollectionViewSource.GetDefaultView(_devices).SortDescriptions.Clear();

                        //CollectionViewSource.GetDefaultView(_devices)
                        //.SortDescriptions
                        //.Add(new SortDescription("Title", ListSortDirection.Ascending));
                    };
                }

                return _devices;
            }
        }

        private ITerminalService TerminalService { get; }

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

    public enum FilterOption
    {
        [Description("Show All")]
        ShowAll,

        [Description("Only Properties")]
        OnlyProperties,

        [Description("Only Methods")]
        OnlyMethods,

        [Description("Hide Events")]
        HideEvents
    }
}
