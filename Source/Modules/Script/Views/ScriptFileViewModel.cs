using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Prism;
using Prism.Logging;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views
{
    [Export(typeof(IScriptFileViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ScriptFileViewModel : ViewModelBase, IScriptFileViewModel, IConfirmCloseRequest, IActiveAware
    {
        private string _title;
        private string _description;
        private string _code;
        private string _fileName;
        private string _filePath;
        private bool _isActive;

        public ScriptFileViewModel()
        {
            ScriptService = ServiceLocator.Current.GetInstance<IScriptService>();

            FileService = ServiceLocator.Current.GetInstance<IFileService>();
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                SetProperty(ref _fileName, value);
                Title = value;
            }
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                SetProperty(ref _filePath, value);
                FileName = Path.GetFileName(value);
                Description = value;
            }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        public void SaveFile()
        {
            if (IsSelectedTarget)
            {
                string filePath = FileService.SaveScriptFileDialog();

                if (string.IsNullOrWhiteSpace(filePath))
                    return;

                FileService.SaveTextFile(filePath, Code);

                var parameters = new NavigationParameters
                {
                    {"FilePath", filePath},
                };

                RegionManager.RequestNavigate(RegionNames.ScriptEditorRegion, "ScriptFileView", parameters);

                return;
            }

            if (IsNewFile)
            {
                string filePath = FileService.SaveScriptFileDialog(FilePath);

                if (string.IsNullOrWhiteSpace(filePath))
                    return;

                DeleteTempFile();
                FilePath = filePath;
            }

            FileService.SaveTextFile(FilePath, Code);
            DeleteTempFile();
            IsNewFile = false;
        }

        public void ExecuteScript()
        {
            try
            {
                EventAggregator.GetEvent<PreviewRunCommandsEvent>().Publish();

                SaveTempFile();

                ScriptService.ExecuteScript(Code);
            }
            catch (Exception ex)
            {
                ScriptService.WriteLine($"Exception: {ex.Message}");
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.Any(p => p.Key == "SelectedTarget"))
                IsSelectedTarget = (bool)navigationContext.Parameters["SelectedTarget"];

            if (IsSelectedTarget)
            {
                if (string.IsNullOrWhiteSpace(FilePath))
                    FilePath = Path.Combine(Path.GetTempPath(), "iXPayTestClient\\ScriptFile.py");

                IsNewFile = true;
                CanClose = false;
                Title = "(Select Command)";
                Description = "Select a Device command from the Device list";
                Code = string.Empty;
            }
            else
            {
                if (navigationContext.Parameters.Any(p => p.Key == "FilePath"))
                    FilePath = (string)navigationContext.Parameters["FilePath"];

                IsNewFile = string.IsNullOrWhiteSpace(FilePath);

                if (IsNewFile)
                {
                    IRegion region = RegionManager.Regions[RegionNames.ScriptEditorRegion];

                    int fileIndex = 1;

                    foreach (var v in region.Views.Cast<IScriptFileView>())
                    {
                        var vm = v.DataContext as IScriptFileViewModel;
                        Debug.Assert(vm != null);

                        if (vm.FileName == $"ScriptFile{fileIndex}.py")
                            fileIndex++;
                    }

                    FilePath = Path.Combine(Directory.GetCurrentDirectory(), $"Scripts\\ScriptFile{fileIndex}.py");
                }

                if (IsNewFile)
                    Code = "from Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands import *\n\nprint(\"Done\")";
                else
                    OpenFile();

                if (navigationContext.Parameters.Any(p => p.Key == "CanClose"))
                    CanClose = (bool)navigationContext.Parameters["CanClose"];
            }

            if (navigationContext.Parameters.Any(p => p.Key == "Title"))
                Title = (string)navigationContext.Parameters["Title"];

            if (navigationContext.Parameters.Any(p => p.Key == "Description"))
                Description = (string)navigationContext.Parameters["Description"];

            if (navigationContext.Parameters.Any(p => p.Key == "Code"))
                Code = (string)navigationContext.Parameters["Code"];
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            bool navigateToSelectedTarget = false;

            if (navigationContext.Parameters.Any(p => p.Key == "SelectedTarget"))
                navigateToSelectedTarget = (bool)navigationContext.Parameters["SelectedTarget"];

            return ((navigateToSelectedTarget && IsSelectedTarget) || (FilePath == (string)navigationContext.Parameters["FilePath"]));
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        public bool CanClose { get; private set; } = true;

        public bool ConfirmCloseRequest(NavigationContext navigationContext)
        {
            if (IsNewFile)
                SaveFile();

            DeleteTempFile();

            return true;
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                FireActiveChanged();
            }
        }

        public event EventHandler IsActiveChanged;

        private void FireActiveChanged()
        {
            IsActiveChanged?.Invoke(this, new EventArgs());
        }

        private IFileService FileService { get; }

        private IScriptService ScriptService { get; }

        private bool IsNewFile { get; set; }

        private bool IsSelectedTarget { get; set; }

        private string GetTempFilePath()
        {
            string filePath = Path.GetDirectoryName(FilePath);
            Debug.Assert(filePath != null);

            string fileName = Path.GetFileName(FilePath);

            return Path.Combine(filePath, $"~{fileName}");
        }

        private void SaveTempFile()
        {
            try
            {
                FileService.SaveTextFile(GetTempFilePath(), Code);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Category.Exception, Priority.Low);
            }
        }

        private void DeleteTempFile()
        {
            try
            {
                FileService.DeleteFile(GetTempFilePath());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Category.Exception, Priority.Low);
            }
        }

        private void OpenFile()
        {
            Code = FileService.OpenTextFile(FilePath, true);
        }
    }
}
