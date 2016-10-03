using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class DeviceViewModel : ViewModelBase, IViewModel
    {
        private ITerminalDevice _device;
        private string _title;
        private ObservableCollection<ICommandViewModel> _commands;

        public DeviceViewModel(ITerminalDevice device)
        {
            _device = device;
            Title = device.Name;

            CommandSelectedCommand = new DelegateCommand<ICommandViewModel>(OnCommandSelected);
        }

        public ICommand CommandSelectedCommand { get; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<ICommandViewModel> Commands
        {
            get
            {
                if (_commands == null)
                {
                    _commands = new ObservableCollection<ICommandViewModel>();

                    _commands.AddRange(new ObservableCollection<ICommandViewModel>(
                        _device.Methods.Select(m => new DeviceMethodViewModel(m))));

                    _device.Methods.MethodAdded += (sender, m) => _commands.Add(new DeviceMethodViewModel(m));

                    _commands.AddRange(new ObservableCollection<DevicePropertyViewModel>(
                        _device.Properties.Select(p => new DevicePropertyViewModel(p))));

                    _device.Properties.PropertyAdded += (sender, p) => _commands.Add(new DevicePropertyViewModel(p));
                }

                return new ObservableCollection<ICommandViewModel>(_commands.OrderBy(c => c.Title));
            }
        }

        private void OnCommandSelected(ICommandViewModel command)
        {
            EventAggregator.GetEvent<CommandSelectedEvent>().Publish(command.Object);
        }
    }
}
