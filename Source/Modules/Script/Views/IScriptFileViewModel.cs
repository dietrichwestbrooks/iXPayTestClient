using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views
{
    public interface IScriptFileViewModel : IViewModel
    {
        void SaveFile();
        void ExecuteScript();
        string FileName { get; }
    }
}
