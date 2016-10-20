using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class DevicePropertyViewModel : ViewModelBase, IDevicePropertyViewModel
    {
        private string _title;

        public DevicePropertyViewModel(ITerminalDeviceProperty property)
        {
            Object = property;
            Title = Object.Name;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<IDeviceCommandViewModel> Commands
        {
            get
            {
                List<IDeviceCommandViewModel> commands = new List<IDeviceCommandViewModel>
                    {
                        new DeviceCommandViewModel(Object.GetCommand, DeviceCommandInvokeType.Get)
                    };


                if (Object.SetCommand != null)
                    commands.Add(new DeviceCommandViewModel(Object.SetCommand, DeviceCommandInvokeType.Set));

                return new ObservableCollection<IDeviceCommandViewModel>(commands);
            }
        }

        public ITerminalDeviceProperty Object { get; }
    }
}
