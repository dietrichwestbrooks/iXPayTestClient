using System.ComponentModel.Composition;
using Prism.Commands;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Commands
{
    public static class ApplicationCommands
    {
        public static CompositeCommand ShowFlyoutCommand = new CompositeCommand();
        public static CompositeCommand ShowNotificationCommand = new CompositeCommand();
    }

    public interface IApplicationCommands
    {
        CompositeCommand ShowFlyoutCommand { get; }
        CompositeCommand ShowNotificationCommand { get; }
    }

    [Export(typeof(IApplicationCommands))]
    public class ApplicationCommandsProxy : IApplicationCommands
    {
        public CompositeCommand ShowFlyoutCommand => ApplicationCommands.ShowFlyoutCommand;
        public CompositeCommand ShowNotificationCommand => ApplicationCommands.ShowNotificationCommand;
    }
}
