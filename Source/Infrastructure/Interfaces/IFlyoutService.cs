﻿namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces
{
    public interface IFlyoutService
    {
        void ShowFlyout(string flyoutName);

        bool CanShowFlyout(string flyoutName);
    }
}
