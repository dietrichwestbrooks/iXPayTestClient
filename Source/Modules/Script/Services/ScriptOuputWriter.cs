using System;
using System.IO;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Services
{
    internal class ScriptOuputWriter : StreamWriter
    {
        public event EventHandler<CaptureEventArgs> Capture;

        public ScriptOuputWriter(Stream s) : base(s)
        { }

        private void OnCapture(string value)
        {
            Capture?.Invoke(this, new CaptureEventArgs(value));
        }

        public override void Write(string value)
        {
            base.Write(value);
            OnCapture(value);
        }

        public override void Write(bool value)
        {
            base.Write(value);
            OnCapture(value.ToString());
        }
    }

    internal class CaptureEventArgs : EventArgs
    {
        public string Text { get; private set; }

        public CaptureEventArgs(string text)
        {
            Text = text;
        }
    }
}
