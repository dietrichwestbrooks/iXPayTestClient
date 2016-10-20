using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views
{
    [Export(typeof(IScriptEditorViewModel))]
    public class ScriptEditorViewModel : ViewModelBase, IScriptEditorViewModel
    {
        private string _title;
        private string _code;
        private object _selectedItem;

        [ImportingConstructor]
        public ScriptEditorViewModel()
        {
            Title = "Script Editor";

            FileService = ServiceLocator.Current.GetInstance<IFileService>();

            OpenFileCommand = new DelegateCommand(OnOpenFile, CanOpenFile);
            NewFileCommand = new DelegateCommand(OnNewFile, CanNewFile);
            SaveFileCommand = new DelegateCommand(OnSaveFile, CanSaveFile);
            ExecuteCommand = new DelegateCommand(OnExecute, CanExecute);

            EventAggregator.GetEvent<DeviceSelectedEvent>().Subscribe(OnDeviceSelected);
            EventAggregator.GetEvent<MethodSelectedEvent>().Subscribe(OnMethodSelected);
            EventAggregator.GetEvent<PropertySelectedEvent>().Subscribe(OnPropertySelected);
            EventAggregator.GetEvent<CommandSelectedEvent>().Subscribe(OnCommandSelected);
            EventAggregator.GetEvent<EventSelectedEvent>().Subscribe(OnEventSelected);
            EventAggregator.GetEvent<EventSelectedEvent>().Subscribe(OnEventSelected);
        }

        private void OnDeviceSelected(ITerminalDevice device)
        {
            var builder = new StringBuilder();

            builder.AppendLine("from Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands import *");
            builder.AppendLine($"\n#{device.Name} Device");

            BuildScript(device, builder);

            var parameters = new NavigationParameters
                {
                    {"SelectedTarget", true},
                    {"Title", $"{device.Name}"},
                    {"Description", $"{device.Name}"},
                    {"Code", $"{builder}"},
                };

            RegionManager.RequestNavigate(RegionNames.ScriptEditorRegion, "ScriptFileView", parameters);
        }

        private void OnMethodSelected(ITerminalDeviceMethod method)
        {
            var builder = new StringBuilder();

            builder.AppendLine("from Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands import *");
            builder.AppendLine($"\n#{method.Name} Method");

            BuildScript(method, builder);

            var parameters = new NavigationParameters
                {
                    {"SelectedTarget", true},
                    {"Title", $"{method.Name}"},
                    {"Description", $"{method.Name}"},
                    {"Code", $"{builder}"},
                };

            RegionManager.RequestNavigate(RegionNames.ScriptEditorRegion, "ScriptFileView", parameters);
        }

        private void OnPropertySelected(ITerminalDeviceProperty property)
        {
            var builder = new StringBuilder();

            builder.AppendLine("from Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands import *");
            builder.AppendLine($"\n#{property.Name} Property");

            BuildScript(property, builder);

            var parameters = new NavigationParameters
                {
                    {"SelectedTarget", true},
                    {"Title", $"{property.Name}"},
                    {"Description", $"{property.Name}"},
                    {"Code", $"{builder}"},
                };

            RegionManager.RequestNavigate(RegionNames.ScriptEditorRegion, "ScriptFileView", parameters);
        }

        private void OnEventSelected(ITerminalDeviceEvent @event)
        {
            var builder = new StringBuilder();

            builder.AppendLine("from Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands import *");
            builder.AppendLine($"\n#{@event.Name} Event");

            BuildScript(@event, builder);

            var parameters = new NavigationParameters
                {
                    {"SelectedTarget", true},
                    {"Title", $"{@event.Name}"},
                    {"Description", $"{@event.Name}"},
                    {"Code", $"{builder}"},
                };

            RegionManager.RequestNavigate(RegionNames.ScriptEditorRegion, "ScriptFileView", parameters);
        }

        private void OnCommandSelected(ITerminalDeviceCommand command)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("from Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands import *");
            builder.AppendLine($"\n#{command.CommandType.Name} Command");

            BuildScript(command, builder);

            var parameters = new NavigationParameters
                {
                    {"SelectedTarget", true},
                    {"Title", $"{command.CommandType.Name}"},
                    {"Description", $"{command.CommandType.Name}"},
                    {"Code", $"{builder}"},
                };

            RegionManager.RequestNavigate(RegionNames.ScriptEditorRegion, "ScriptFileView", parameters);
        }

        public ICommand OpenFileCommand { get; set; }

        public ICommand SaveFileCommand { get; set; }

        public ICommand NewFileCommand { get; set; }

        public ICommand ExecuteCommand { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private IFileService FileService { get; }

        private bool CanExecute()
        {
            return true;
        }

        private void OnExecute()
        {
            IRegion region = RegionManager.Regions[RegionNames.ScriptEditorRegion];

            var view = region.ActiveViews.FirstOrDefault() as IScriptFileView;

            if (view == null)
                return;

            var viewModel = view.DataContext as IScriptFileViewModel;

            viewModel?.ExecuteScript();
        }

        private bool CanNewFile()
        {
            return true;
        }

        private void OnNewFile()
        {
            RegionManager.RequestNavigate(RegionNames.ScriptEditorRegion, "ScriptFileView");
        }

        private bool CanOpenFile()
        {
            return true;
        }

        private void OnOpenFile()
        {
            string filePath = FileService.OpenScriptFileDialog();

            if (string.IsNullOrWhiteSpace(filePath))
                return;

            var parameters = new NavigationParameters
                {
                    {"FilePath", filePath},
                };

            RegionManager.RequestNavigate(RegionNames.ScriptEditorRegion, "ScriptFileView", parameters);
        }

        private bool CanSaveFile()
        {
            return true;
        }

        private void OnSaveFile()
        {
            IRegion region = RegionManager.Regions[RegionNames.ScriptEditorRegion];

            var view = region.ActiveViews.FirstOrDefault() as IScriptFileView;

            if (view == null)
                return;

            var viewModel = view.DataContext as IScriptFileViewModel;

            viewModel?.SaveFile();
        }

        private void BuildScript(ITerminalDevice device, StringBuilder builder)
        {
            if (device == null)
                return;

            builder.AppendLine($"\n#{device.Name} Methods");

            foreach (var method in device.Methods)
            {
                builder.AppendLine($"\n#{method.Name} Method");
                BuildScript(method, builder);
            }

            builder.AppendLine($"\n#{device.Name} Properties");

            foreach (var property in device.Properties)
            {
                builder.AppendLine($"\n#{property.Name} Property");
                BuildScript(property, builder);
            }

            builder.AppendLine($"\n#{device.Name} Events");

            foreach (var @event in device.Events)
            {
                builder.AppendLine($"\n#{@event.Name} Event");
                BuildScript(@event, builder);
            }
        }

        private void BuildScript(ITerminalDeviceMethod method, StringBuilder builder)
        {
            if (method == null)
                return;

            BuildScript(method.InvokeCommand, builder);
        }

        private void BuildScript(ITerminalDeviceProperty property, StringBuilder builder)
        {
            if (property == null)
                return;

            BuildScript(property.GetCommand, builder);

            if (property.SetCommand == null)
                return;

            BuildScript(property.SetCommand, builder);
        }

        private void BuildScript(ITerminalDeviceEvent @event, StringBuilder builder)
        {
            if (@event == null)
                return;

            string deviceName = @event.Device.Name;
            string eventName = @event.Name;

            builder.AppendLine($"\ndef On{eventName}(object sender, EventArgs e):");

            var properties = @event.EventType.GetProperties();

            foreach (var p in properties)
            {
                builder.AppendLine($"\t{p.Name.ToLower()} = e.{p.Name}");
            }

            builder.AppendLine($"\t#Handler logic here");

            builder.AppendLine($"\n{deviceName}.{eventName} += On{eventName}");
        }

        private void BuildScript(ITerminalDeviceCommand command, StringBuilder builder)
        {
            if (command == null)
                return;

            string deviceName = command.Member.Device.Name;
            string methodName = command.Name;

            //var commandItem = command.GetMessage().GetLastItem();

            var props = command.CommandType.GetProperties().Where(p => p.Name != "SequenceNumber").ToList();

            builder.Append($"\nresponse = {deviceName}.{methodName}(");

            foreach (var p in props)
            {
                //string value = GetScriptValue(p, commandItem);
                //builder.Append($"{p.Name.ToLower()}={value},");
                builder.Append($"{p.Name.ToLower()}={{{p.GetGetMethod().ReturnType.Name}}},");
            }

            if (props.Any())
            {
                builder.Length--;
            }

            builder.AppendLine(")\n");

            props = command.ResponseType.GetProperties().Where(p => p.Name != "SequenceNumber").ToList();

            foreach (var p in props)
            {
                builder.AppendLine($"{p.Name.ToLower()} = response.{p.Name}");
            }
        }

        //private string GetScriptValue(PropertyInfo prop, object instance)
        //{
        //    string value;

        //    Type valueType = prop.GetGetMethod().ReturnType;

        //    if (valueType == typeof(string))
        //    {
        //        var stringValue = (string) prop.GetValue(instance);
        //        value = stringValue == null ? "null" : $"\"{stringValue}\",";
        //    }
        //    else if (valueType == typeof(bool))
        //    {
        //        value = ((bool)prop.GetValue(instance)) ? "True" : "False";
        //    }
        //    else
        //    {
        //        value = prop.GetValue(instance)?.ToString();
        //    }

        //    return value;
        //}
    }
}
