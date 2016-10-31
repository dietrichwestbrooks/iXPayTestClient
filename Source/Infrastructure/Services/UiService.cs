using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services
{
    [Export(typeof(IUiService))]
    public class UiService : IUiService
    {
        public UiService()
        {
            SelectedCardBrand = CardBrand.MasterCard;
        }

        public CardBrand SelectedCardBrand { get; }
    }
}
