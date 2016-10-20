using Prism.Regions;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Prism
{
    public interface IRegionManagerAware
    {
        IRegionManager RegionManager { get; set; }
    }
}
