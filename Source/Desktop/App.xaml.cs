using System.ComponentModel.Composition.Hosting;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Desktop.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            var container = ServiceLocator.Current.GetInstance<CompositionContainer>();

            var mainWindow = container.GetExportedValue<IShellView>() as MetroWindow;

            mainWindow?.ShowMessageAsync("Application Error", e.Exception.Message, MessageDialogStyle.Affirmative,
                mainWindow.MetroDialogOptions);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Current);

            //ThemeManager.ChangeAppStyle(Current, ThemeManager.GetAccent("Blue"), ThemeManager.GetAppTheme("BaseLight"));
            ThemeManager.ChangeAppStyle(Current, ThemeManager.GetAccent("Steel"), ThemeManager.GetAppTheme("BaseDark"));

            base.OnStartup(e);

            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
