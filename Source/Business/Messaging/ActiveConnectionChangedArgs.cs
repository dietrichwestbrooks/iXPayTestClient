using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class ActiveConnectionChangedArgs : EventArgs
    {
        public ITerminalConnection OldConnection { get; set; }
        public ITerminalConnection NewConnection { get; set; }
    }
}
