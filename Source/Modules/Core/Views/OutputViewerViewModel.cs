using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    [Export(typeof(IOutputViewerViewModel))]
    public class OutputViewerViewModel : ViewModelBase, IOutputViewerViewModel
    {
        private string _title;
        private object _activeView;

        public OutputViewerViewModel()
        {
            Title = "Output";

            EventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public IEnumerable<object> Views => RegionManager.Regions[RegionNames.OutputRegion].Views;

        public object ActiveView
        {
            get { return _activeView; }
            set
            {
                SetProperty(ref _activeView, value);
                RegionManager.Regions[RegionNames.OutputRegion].Activate(_activeView);
            }
        }

        private void OnModulesInitialized()
        {
            var view = RegionManager.Regions[RegionNames.OutputRegion].ActiveViews.FirstOrDefault();

            if (view != null)
                ActiveView = view;
        }
    }
}
