using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events
{
    public class ModulesInitializedEvent : PubSubEvent
    {
    }

    public class CommandSelectedEvent : PubSubEvent<ITerminalDeviceCommand>
    {
    }

    public class CommandEditedEvent : PubSubEvent<TerminalMessage>
    {
    }
        
    public class MessageEditedEvent : PubSubEvent<TerminalMessage>
    {
    }

    public class ResponseReceivedEvent : PubSubEvent<TerminalMessage>
    {
    }

    public class EventReceivedEvent : PubSubEvent<TerminalMessage>
    {
    }

    public class MessageSentEvent : PubSubEvent<TerminalMessage>
    {
    }

    public class ConnectionStatusEvent : PubSubEvent<ConnectionEventArgs>
    {
    }

    public class ScriptOutputEvent : PubSubEvent<string>
    {
    }

    public class PrepareScriptEvent : PubSubEvent<IScriptService>
    {
    }

    public class DeviceRegisteredEvent : PubSubEvent<ITerminalDevice>
    {
    }
}
