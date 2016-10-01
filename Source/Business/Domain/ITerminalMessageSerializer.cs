namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public interface ITerminalMessageSerializer
    {
        string Serialize(TerminalMessage terminalMessage);
        bool TryDeserialize(string xmlMessage, out TerminalMessage terminalMessage);
        TerminalMessage Deserialize(string xmlMessage);
    }
}