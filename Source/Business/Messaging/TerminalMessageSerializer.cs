using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    [Export(typeof(ITerminalMessageSerializer))]
    public class TerminalMessageSerializer : ITerminalMessageSerializer
    {
        public bool TrySerialize(TerminalMessage message, out string xml)
        {
            xml = null;

            bool result = true;

            try
            {
                xml = Serialize(message);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public string Serialize(TerminalMessage message)
        {
            if (message == null)
                return null;

            var serializer = new XmlSerializer(typeof(TerminalMessage));

            string xml;

            using (var memorySteam = new MemoryStream())
            using (var writer = new XmlTextWriter(memorySteam, Encoding.UTF8))
            {
                serializer.Serialize(writer, message); // memoryStream contains the xml
                memorySteam.Position = 0;
                using (var reader = new StreamReader(memorySteam))
                {
                    xml = reader.ReadToEnd();
                    reader.Close();
                }
                writer.Close();
            }

            return xml;
        }

        public bool TryDeserialize(string xml, out TerminalMessage message)
        {
            message = null;

            bool result = true;

            try
            {
                message = Deserialize(xml);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public TerminalMessage Deserialize(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                throw new ArgumentNullException(nameof(xml));

            var serializer = new XmlSerializer(typeof(TerminalMessage));

            object result;

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                result = serializer.Deserialize(ms);
            }

            return (TerminalMessage)result;
        }
    }
}
