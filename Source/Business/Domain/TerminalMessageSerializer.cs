using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    [Export(typeof(ITerminalMessageSerializer))]
    public class TerminalMessageSerializer : ITerminalMessageSerializer
    {
        public string Serialize(TerminalMessage terminalMessage)
        {
            if (terminalMessage == null)
                return null;

            var serializer = new XmlSerializer(typeof(TerminalMessage));

            string xmlMessage;

            using (var memorySteam = new MemoryStream())
            using (var writer = new XmlTextWriter(memorySteam, Encoding.UTF8))
            {
                serializer.Serialize(writer, terminalMessage); // memoryStream contains the xml
                memorySteam.Position = 0;
                using (var reader = new StreamReader(memorySteam))
                {
                    xmlMessage = reader.ReadToEnd();
                    reader.Close();
                }
                writer.Close();
            }

            return xmlMessage;
        }

        public bool TryDeserialize(string xmlMessage, out TerminalMessage terminalMessage)
        {
            terminalMessage = null;

            bool result = true;

            try
            {
                terminalMessage = Deserialize(xmlMessage);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public TerminalMessage Deserialize(string xmlMessage)
        {
            if (string.IsNullOrWhiteSpace(xmlMessage))
                throw new ArgumentNullException(nameof(xmlMessage));

            var serializer = new XmlSerializer(typeof(TerminalMessage));

            object result;

            using (var memorySteam = new MemoryStream(Encoding.UTF8.GetBytes(xmlMessage)))
            {
                result = serializer.Deserialize(memorySteam);
            }

            return (TerminalMessage)result;
        }
    }
}
