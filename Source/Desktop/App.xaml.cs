using System.Windows;
using MahApps.Metro;

namespace Wayne.Payment.Tools.iXPayTestClient.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
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
