using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.RegionAdapters
{
    [Export]
    public class TabControlRegionAdapter : RegionAdapterBase<TabControl>
    {
        [ImportingConstructor]
        public TabControlRegionAdapter(IRegionBehaviorFactory factory)
            : base(factory)
        {

        }

        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var view in e.NewItems)
                    {
                        var tabItem = new TabItem
                            {
                                Content = view,
                                Header = ((IViewModel) ((IView) view).DataContext).Title
                            };

                        if (regionTarget.Items.Add(tabItem) == 0)
                            regionTarget.SelectedItem = tabItem;
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}
