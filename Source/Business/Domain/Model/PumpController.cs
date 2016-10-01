
using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    [Export(typeof(IPumpController))]
    public class PumpController : IPumpController
    {
    }
}
