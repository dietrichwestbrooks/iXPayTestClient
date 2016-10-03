namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalMessageSerializer
    {
        string Serialize(TerminalMessage terminalMessage);
        bool TryDeserialize(string xmlMessage, out TerminalMessage terminalMessage);
        TerminalMessage Deserialize(string xmlMessage);
    }
}