using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions
{
    public static class TerminalMessageExtensions
    {
        public static object GetLastItem(this TerminalMessage source)
        {
            object lastItem = source;

            object item = source.Item;

            while (item != null)
            {
                lastItem = item;

                if (!lastItem.GetType().GetProperties().Any(p => p.Name == "Item"))
                    break;

                item = lastItem.GetType().InvokeMember("Item",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty,
                    Type.DefaultBinder, item, null);
            }

            return lastItem;
        }

        public static object GetSecondToLastItem(this TerminalMessage source)
        {
            object lastItem = source;

            object secondToLastItem = lastItem;

            object item = source.Item;

            while (item != null)
            {
                secondToLastItem = lastItem;
                lastItem = item;

                if (!lastItem.GetType().GetProperties().Any(p => p.Name == "Item"))
                    break;

                item = lastItem.GetType().InvokeMember("Item",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty,
                    Type.DefaultBinder, item, null);
            }

            return secondToLastItem;
        }

        public static int GetResponseSequenceNumber(this TerminalMessage source)
        {
            object response = source.GetLastItem();

            var baseResponse = response as BaseResponse;

            if (baseResponse != null)
                return baseResponse.SequenceNumber;

            var hexResponse = response as BaseSimpleHexResponse;

            return hexResponse?.SequenceNumber ?? 0;
        }

        public static int GetCommandSequenceNumber(this TerminalMessage source)
        {
            object command = source.GetLastItem();

            var baseCommand = command as BaseCommand;

            if (baseCommand != null)
                return baseCommand.SequenceNumber;

            var hexCommand = command as BaseSimpleHexCommand;

            return hexCommand?.SequenceNumber ?? 0;
        }

        public static bool GetResponseSuccess(this TerminalMessage source)
        {
            object response = source.GetLastItem();

            var baseResponse = response as BaseResponse;

            if (baseResponse != null)
                return baseResponse.Success;

            var hexResponse = response as BaseSimpleHexResponse;

            return hexResponse?.Success ?? false;
        }

        public static string GetResponseMessage(this TerminalMessage source)
        {
            object response = source.GetLastItem();

            var baseResponse = response as BaseResponse;

            if (baseResponse != null)
                return baseResponse.Message;

            var hexResponse = response as BaseSimpleHexResponse;

            return hexResponse?.Message ?? String.Empty;
        }

        public static string Serialize(this TerminalMessage source)
        {
            var serializer = ServiceLocator.Current.GetInstance<ITerminalMessageSerializer>();
            return serializer.Serialize(source);

        }

        public static bool TrySerialize(this TerminalMessage source, out string xml)
        {
            var serializer = ServiceLocator.Current.GetInstance<ITerminalMessageSerializer>();
            return serializer.TrySerialize(source, out xml);

        }

        public static byte[] GetBytes(this TerminalMessage source)
        {
            return Encoding.UTF8.GetBytes(source.Serialize());
        }

        public static void SetCommandSequenceNumber(this TerminalMessage source, int sequenceNumber)
        {
            object command = source.GetLastItem();

            var baseCommand = command as BaseCommand;

            if (baseCommand != null)
                baseCommand.SequenceNumber = sequenceNumber;

            var hexCommand = command as BaseSimpleHexCommand;

            if (hexCommand != null)
                hexCommand.SequenceNumber = sequenceNumber;
        }
    }
}
