using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(IResendMessagePopUpViewModel))]
    public class ResendMessagePopUpViewModel : ViewModelBase, IResendMessagePopUpViewModel, INavigationAware, IDataErrorInfo
    {
        private string _title;
        private TerminalMessage _message;
        private string _xml;
        private string _error;

        public ResendMessagePopUpViewModel()
        {
            Title = "Re-send Message";

            ExecuteCommand = new DelegateCommand<IClosable>(OnExecute, CanExecute);
        }

        public ICommand ExecuteCommand { get; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.Any(p => p.Key == "Message"))
                _message = (TerminalMessage)navigationContext.Parameters["Message"];

            if (_message == null)
                return;

            Xml = _message.Serialize().FormattedXml();
        }

        public string Xml
        {
            get { return _xml; }
            set { SetProperty(ref _xml, value); }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private bool CanExecute(IClosable view)
        {
            return true;
        }

        private void OnExecute(IClosable view)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Xml))
                {
                    EventAggregator.GetEvent<PreviewRunCommandsEvent>().Publish();

                    var terminalService = ServiceLocator.Current.GetInstance<ITerminalService>();
                    terminalService.SendMessage(_message);
                }

                view.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Xml")
                {
                    var messageSerializer = ServiceLocator.Current.GetInstance<ITerminalMessageSerializer>();
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(Xml))
                            _message = messageSerializer.Deserialize(Xml);
                    }
                    catch (Exception ex)
                    {
                        Error = ex.Message;
                    }
                }

                return Error;
            }
        }

        public string Error
        {
            get { return _error; }
            private set { SetProperty(ref _error, value); }
        }
    }
}
