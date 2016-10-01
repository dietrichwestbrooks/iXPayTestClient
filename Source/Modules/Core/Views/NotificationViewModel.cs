using System;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    [Export(typeof(INotificationViewModel))]
    public class NotificationViewModel : ViewModelBase, INotificationViewModel
    {
        private string _title = "Notifications";
        private string _message;
        private DispatcherTimer _hideTimer;

        public NotificationViewModel()
        {
            ShowNotificationCommand = new DelegateCommand<NotificationParameter>(OnShowNotification, parameter => true);

            var applicationCommands = ServiceLocator.Current.GetInstance<IApplicationCommands>();
            applicationCommands.ShowNotificationCommand.RegisterCommand(ShowNotificationCommand);

            _hideTimer = new DispatcherTimer(TimeSpan.FromSeconds(5), DispatcherPriority.Normal, OnHideNotification, Dispatcher.CurrentDispatcher);
            _hideTimer.Stop();
        }

        private System.Windows.Input.ICommand ShowNotificationCommand { get; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Message
        {
            get { return _message; }
            private set { SetProperty(ref _message, value); }
        }

        private void OnHideNotification(object sender, EventArgs e)
        {
            ApplicationCommands.ShowFlyoutCommand.Execute(FlyoutNames.NotificationFlyout);
            _hideTimer.Stop();
        }

        private void OnShowNotification(NotificationParameter parameter)
        {
            Message = parameter?.Message;

            ApplicationCommands.ShowFlyoutCommand.Execute(FlyoutNames.NotificationFlyout);
            _hideTimer.Start();
        }
    }
}
