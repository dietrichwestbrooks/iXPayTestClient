using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    [Flags]
    public enum FontStyle
    {
        Normal = 0x0000,
        Bold = 0x0001,
        Italic = 0x0010,
        Underline = 0x0100,
        Strikeout = 0x1000,
    }
    
    public enum StatusState
    {
        Uknown,
        Failed,
        Ok,
    }
}
