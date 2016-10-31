using System.Collections.Specialized;
using System.ComponentModel.Composition;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;
using Xceed.Wpf.AvalonDock.Layout;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.RegionAdapters
{
    [Export]
    public class LayoutAnchorGroupRegionAdapter : RegionAdapterBase<LayoutAnchorGroup>
    {
        [ImportingConstructor]
        protected LayoutAnchorGroupRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) 
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, LayoutAnchorGroup regionTarget)
        {
            region.Views.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (IView view in args.NewItems)
                    {
                        regionTarget.InsertChildAt(regionTarget.ChildrenCount, new LayoutAnchorable
                            {
                                Title = ((IViewModel)view.DataContext).Title,
                                Content = view,
                                IsActive = false,
                                ContentId = ((IViewModel)view.DataContext).Title.ToLower(),
                            });
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
