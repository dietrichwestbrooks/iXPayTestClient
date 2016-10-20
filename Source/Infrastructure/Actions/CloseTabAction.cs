using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Actions
{
    public class CloseTabAction : TriggerAction<Button>
    {
        protected override void Invoke(object parameter)
        {
            var args = parameter as RoutedEventArgs;

            if (args == null)
                return;

            var tabItem = WpfHelper.FindParent<TabItem>(args.OriginalSource as DependencyObject);
            if (tabItem == null)
                return;

            var tabConrol = WpfHelper.FindParent<TabControl>(tabItem);
            if (tabConrol == null)
                return;

            IRegion region = RegionManager.GetObservableRegion(tabConrol).Value;
            if (region == null)
                return;

            Close(region, tabItem.Content);
        }

        private void Close(IRegion region, object view)
        {
            var navigationContext = new NavigationContext(null, null);

            if (CanClose(view, navigationContext) && region.Views.Contains(view))
                region.Remove(view);
        }

        private bool CanClose(object view, NavigationContext navigationContext)
        {
            bool canClose = true;

            var confirmCloseTarget = view as IConfirmCloseRequest;
            if (confirmCloseTarget != null)
            {
                canClose = confirmCloseTarget.ConfirmCloseRequest(navigationContext);
            }

            var element = view as FrameworkElement;

            if (canClose && element != null)
            {
                confirmCloseTarget = element.DataContext as IConfirmCloseRequest;
                if (confirmCloseTarget != null)
                {
                    canClose = confirmCloseTarget.ConfirmCloseRequest(navigationContext);
                }
            }

            return canClose;
        }
    }
}
