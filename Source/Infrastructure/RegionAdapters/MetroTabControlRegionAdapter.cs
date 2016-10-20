using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Prism.Regions;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.RegionAdapters
{
    [Export]
    public class MetroTabControlRegionAdapter : RegionAdapterBase<MetroTabControl>
    {
        [ImportingConstructor]
        public MetroTabControlRegionAdapter(IRegionBehaviorFactory factory)
            : base(factory)
        {

        }

        protected override void Adapt(IRegion region, MetroTabControl regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var view in e.NewItems)
                    {
                        var tabItem = new TabItem
                            {
                                DataContext = view,
                                Content = view,
                            };

                        if (regionTarget.Items.Add(tabItem) == 0)
                            regionTarget.SelectedItem = tabItem;
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
