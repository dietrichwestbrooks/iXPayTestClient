﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Behaviors
{
    public class BindableTreeViewSelectedItemBehavior : Behavior<TreeView>
    {
         public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof (object), typeof (BindableTreeViewSelectedItemBehavior),
                new UIPropertyMetadata(null, OnSelectedItemChanged));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as TreeViewItem;
            item?.SetValue(TreeViewItem.IsSelectedProperty, true);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
                AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}
