using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    [Export(typeof(IDispenser))]
    public class Dispenser : IDispenser
    {
        [Import]
        public IPumpController Pump { get; set; }

        [Import]
        public ITerminalController Terminal { get; set; }
    }
}
