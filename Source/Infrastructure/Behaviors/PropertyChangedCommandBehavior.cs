using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Behaviors
{
    public class PropertyChangedCommandBehavior : Behavior<DependencyObject>
    {
        private DependencyPropertyDescriptor _descriptor;

        public static readonly DependencyProperty PropertyChangedCommandProperty =
            DependencyProperty.Register("PropertyChangedCommand", typeof (ICommand),
                typeof (PropertyChangedCommandBehavior), new PropertyMetadata(null, ApplyChanged));

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof (string),
                typeof (PropertyChangedCommandBehavior), new PropertyMetadata(string.Empty, ApplyChanged));

        public ICommand PropertyChangedCommand
        {
            get { return (ICommand)GetValue(PropertyChangedCommandProperty); }
            set { SetValue(PropertyChangedCommandProperty, value); }
        }

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        private static void ApplyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as PropertyChangedCommandBehavior;
            behavior?.Setup();
        }

        protected override void OnAttached()
        {
            Setup();
        }

        protected override void OnDetaching()
        {
            if (_descriptor != null && AssociatedObject != null)
                _descriptor.RemoveValueChanged(AssociatedObject, OnPropertyValueChanged);
        }

        private void Setup()
        {
            if (_descriptor != null || string.IsNullOrEmpty(PropertyName) || AssociatedObject == null)
                return;

            _descriptor = DependencyPropertyDescriptor.FromName(PropertyName, AssociatedObject.GetType(), AssociatedObject.GetType());
            _descriptor?.AddValueChanged(AssociatedObject, OnPropertyValueChanged);
        }

        private void OnPropertyValueChanged(object sender, EventArgs e)
        {
            var value = AssociatedObject.GetValue(_descriptor.DependencyProperty);

            if (PropertyChangedCommand == null || !PropertyChangedCommand.CanExecute(value))
                return;

            PropertyChangedCommand.Execute(value);
        }
    }
}
