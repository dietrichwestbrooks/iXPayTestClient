using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Windows;
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

        public bool LastSelected { get; set; }

        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var view in e.NewItems)
                    {
                        if (LastSelected || regionTarget.Items.Add(view) == 0)
                            regionTarget.SelectedItem = view;
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var view in e.OldItems)
                    {
                        if (regionTarget.Items.Contains(view))
                            regionTarget.Items.Remove(view);
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
