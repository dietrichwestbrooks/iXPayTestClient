using Prism.Regions;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces
{
    public interface IConfirmCloseRequest : IConfirmNavigationRequest
    {
        bool CanClose { get; }

        bool ConfirmCloseRequest(NavigationContext navigationContext);
    }
}
