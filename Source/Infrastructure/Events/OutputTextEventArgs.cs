using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events
{
    public class OutputTextEventArgs : EventArgs
    {
        public OutputTextCategory Category { get; set; }

        public string Text { get; set; }
    }

    public enum OutputTextCategory
    {
        Error,
        Script
    }
}
