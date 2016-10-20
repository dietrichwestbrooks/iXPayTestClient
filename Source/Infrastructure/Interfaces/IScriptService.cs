using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces
{
    public interface IScriptService
    {
        void ExecuteScript(string code);
        void ExecuteScriptFile(string path);
        T GetVariable<T>(string name);
        void SetVariable(string name, object value, bool global = false);
        void Write(string message);
        void WriteIf(bool condition, string message);
        void WriteLine(string message);
        void WriteLineIf(bool condition, string message);
        void RemoveVariable(string name);
        ITerminalDevice CreateDeviceFromFile(string path);
        TFunc GetFunction<TFunc>(object function) where TFunc : class;
    }
}
