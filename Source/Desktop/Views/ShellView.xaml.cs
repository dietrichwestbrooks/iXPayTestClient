using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media.Animation;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;

namespace Wayne.Payment.Tools.iXPayTestClient.Desktop.Views
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export]
    public partial class ShellView : IShellView, IPartImportsSatisfiedNotification
    {
        //[Import(AllowRecomposition = false)]
        //private IModuleManager _moduleManager;

        //[Import(AllowRecomposition = false)]
        //private ILoggerFacade _logger;

        private IRegionManager _regionManager;
        private bool _restartedAnimation;

        [ImportingConstructor]
        public ShellView(IRegionManager regionManager, IShellViewModel viewModel)
        {
            InitializeComponent();

            _regionManager = regionManager;

            // The RegionManager.RegionName attached property XAML-Declaration doesn't work for this scenario (object declarated outside the logical tree)
            // theses objects are not part of the logical tree, hence the parent that has the region manager to use (the Window) cannot be found using LogicalTreeHelper.FindParent 
            // therefore the regionManager is never found and cannot be assigned automatically by Prism.  This means we have to handle this ourselves
            SetRegionManager(LeftWindowCommandsRegion, RegionNames.LeftWindowCommandsRegion);
            SetRegionManager(RightWindowCommandsRegion, RegionNames.RightWindowCommandsRegion);
            SetRegionManager(FlyoutsControlRegion, RegionNames.FlyoutRegion);
            //SetRegionManager(BottomDockRegion, RegionNames.BottomDockRegion);
            //SetRegionManager(MainDockRegion, RegionNames.MainDockRegion);
            //SetRegionManager(LeftDockRegion, RegionNames.LeftDockRegion);
            //SetRegionManager(RightDockRegion, RegionNames.RightDockRegion);

            DataContext = viewModel;
        }

        void SetRegionManager(DependencyObject regionTarget, string regionName)
        {
            RegionManager.SetRegionName(regionTarget, regionName);
            RegionManager.SetRegionManager(regionTarget, _regionManager);
        }

        public void OnImportsSatisfied()
        {
        }

        private void HeartbeatStoryboard_Completed(object sender, EventArgs e)
        {
            var viewModel = (IShellViewModel) DataContext;

            ClockGroup clockGroup = sender as ClockGroup;

            Storyboard heartbeatStoryboard = clockGroup?.Timeline as Storyboard;

            if (heartbeatStoryboard == null)
                return;

            if (viewModel.IsHeartBeating)
            {
                _restartedAnimation = true;
                heartbeatStoryboard.Begin();
            }
            else
            {
                if (_restartedAnimation)
                    heartbeatStoryboard.Stop();

                _restartedAnimation = false;
            }
        }
    }
}
