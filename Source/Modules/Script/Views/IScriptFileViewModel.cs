using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views
{
    public interface IScriptFileViewModel : IViewModel
    {
        void Save(bool newFile = false);
        void ExecuteScript();
        string FileName { get; }
    }
}
