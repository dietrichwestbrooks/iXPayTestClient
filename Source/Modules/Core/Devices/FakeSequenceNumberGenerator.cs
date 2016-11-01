namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Devices
{
    internal class FakeSequenceNumberGenerator
    {
        private static FakeSequenceNumberGenerator _instance;
        private static object _instLock = new object();

        private FakeSequenceNumberGenerator()
        {
        }

        public static FakeSequenceNumberGenerator Instance
        {
            get { lock (_instLock) return _instance ?? (_instance = new FakeSequenceNumberGenerator()); }
        }

        private int _sequenceNumber = 101;

        public int Increment()
        {
            return _sequenceNumber++;
        }
    }
}
