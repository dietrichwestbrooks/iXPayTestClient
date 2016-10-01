using System.Collections.Specialized;
using System.ComponentModel.Composition;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Xceed.Wpf.AvalonDock.Layout;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.RegionAdapters
{
    [Export]
    public class LayoutAnchorSideRegionAdapter : RegionAdapterBase<LayoutAnchorSide>
    {
        [ImportingConstructor]
        public LayoutAnchorSideRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) 
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, LayoutAnchorSide regionTarget)
        {
            region.Views.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (IView view in args.NewItems)
                    {
                        var group = new LayoutAnchorGroup();
                        group.InsertChildAt(group.ChildrenCount, new LayoutAnchorable
                            {
                                Title = ((IViewModel) view.DataContext).Title,
                                Content = view,
                                IsActive = false,
                                ContentId = ((IViewModel) view.DataContext).Title.ToLower(),
                            });

                        regionTarget.InsertChildAt(regionTarget.ChildrenCount, group);
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
