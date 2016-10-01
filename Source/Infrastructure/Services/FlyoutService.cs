using System.ComponentModel.Composition;
using MahApps.Metro.Controls;
using Prism.Commands;
using Prism.Regions;
using System.Linq;
using System.Windows.Input;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services
{
    [Export(typeof(IFlyoutService))]
    public class FlyoutService : IFlyoutService
    {
        IRegionManager _regionManager;

        public ICommand ShowFlyoutCommand { get; }

        [ImportingConstructor]
        public FlyoutService(IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;

            ShowFlyoutCommand = new DelegateCommand<string>(ShowFlyout, CanShowFlyout);
            applicationCommands.ShowFlyoutCommand.RegisterCommand(ShowFlyoutCommand);
        }

        public void ShowFlyout(string flyoutName)
        {
            var region = _regionManager.Regions[RegionNames.FlyoutRegion];

            var flyout = region?.Views.FirstOrDefault(v => v is IFlyoutView && ((IFlyoutView)v).FlyoutName.Equals(flyoutName)) as Flyout;

            if (flyout != null)
            {
                flyout.IsOpen = !flyout.IsOpen;
            }
        }

        public bool CanShowFlyout(string flyoutName)
        {
            return true;
        }
    }
}
