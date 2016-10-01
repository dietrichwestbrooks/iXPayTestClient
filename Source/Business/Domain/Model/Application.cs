using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    [Export(typeof(IApplication))]
    public class Application : IApplication
    {
        public string Title { get; } = "iXPayTestClient";

        [Import]
        public IDispenser Dispenser { get; set; }
    }
}
