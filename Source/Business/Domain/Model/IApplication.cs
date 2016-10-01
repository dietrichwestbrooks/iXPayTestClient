namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    public interface IApplication
    {
        string Title { get; }

        IDispenser Dispenser { get; }
    }
}
