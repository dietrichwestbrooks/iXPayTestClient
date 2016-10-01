using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

            ThemeManager.ChangeAppStyle(Current, ThemeManager.GetAccent("Blue"), ThemeManager.GetAppTheme("BaseLight"));

            base.OnStartup(e);

            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
