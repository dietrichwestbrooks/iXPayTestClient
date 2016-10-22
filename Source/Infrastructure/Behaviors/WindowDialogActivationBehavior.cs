namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Behaviors
{
    public class WindowDialogActivationBehavior : DialogActivationBehavior
    {
        /// <summary>
        /// Creates a wrapper for the WPF <see cref="System.Windows.Window"/>.
        /// </summary>
        /// <returns>Instance of the <see cref="System.Windows.Window"/> wrapper.</returns>
        protected override IWindow CreateWindow()
        {
            return new WindowWrapper();
        }
    }
}
