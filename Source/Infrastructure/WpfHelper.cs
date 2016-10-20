using System.Windows;
using System.Windows.Media;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure
{
    public static class WpfHelper
    {
        public static T FindParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            if (child == null)
                return null;

            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            var parent = parentObject as T;
            if (parent != null)
                return parent;

            return FindParent<T>(parentObject);
        }
    }
}
