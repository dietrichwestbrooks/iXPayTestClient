using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Windows;
using Prism.Regions;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Prism
{
    //[Export(typeof(IRegionBehavior))]
    public class RegionManagerAwareBehavior : RegionBehavior
    {
        public const string BehaviorKey = "RegionManagerAwareBehavior";

        protected override void OnAttach()
        {
            Region.Views.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    IRegionManager regionManager = Region.RegionManager;

                    FrameworkElement element = item as FrameworkElement;
                    if (element != null)
                    {
                        IRegionManager scopedRegionManager = element.GetValue(RegionManager.RegionManagerProperty) as IRegionManager;
                        if (scopedRegionManager != null)
                        {
                            regionManager = scopedRegionManager;
                        }
                    }

                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = regionManager);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = null);
                }
            }
        }

        private static void InvokeOnRegionManagerAwareElement(object item, Action<IRegionManagerAware> invocation)
        {
            var regionManagerAwareItem = item as IRegionManagerAware;
            if (regionManagerAwareItem != null)
            {
                invocation(regionManagerAwareItem);
            }

            var element = item as FrameworkElement;
            IRegionManagerAware regionManagerAwareDataContext = element?.DataContext as IRegionManagerAware;
            if (regionManagerAwareDataContext != null)
            {
                var parent = element.Parent as FrameworkElement;
                var regionManagerAwareDataContextParent = parent?.DataContext as IRegionManagerAware;
                if (regionManagerAwareDataContextParent != null)
                {
                    if (regionManagerAwareDataContext == regionManagerAwareDataContextParent)
                    {
                        return;
                    }
                }

                invocation(regionManagerAwareDataContext);
            }
        }
    }
}
