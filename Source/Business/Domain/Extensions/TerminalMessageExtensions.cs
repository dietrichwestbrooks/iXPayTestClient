﻿using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using static System.String;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions
{
    public static class TerminalMessageExtensions
    {
        public static object GetLastItem(this TerminalMessage source)
        {
            object lastItem = null;

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

        public static object GetBaseCommand(this TerminalMessage source)
        {
            object command = null;

            object commandItem = source.Item;

            while (commandItem != null)
            {
                command = commandItem as BaseCommand;

                if (command != null)
                    break;

                command = commandItem as BaseSimpleHexCommand;

                if (command != null)
                    break;

                commandItem = commandItem.GetType().InvokeMember("Item",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty,
                    Type.DefaultBinder, commandItem, null);
            }

            return command;
        }

        public static object GetBaseResponse(this TerminalMessage source)
        {
            object response = null;

            object responseItem = source.Item;

            while (responseItem != null)
            {
                response = responseItem as BaseResponse;

                if (response != null)
                    break;

                response = responseItem as BaseSimpleHexResponse;

                if (response != null)
                    break;

                responseItem = responseItem.GetType().InvokeMember("Item",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty,
                    Type.DefaultBinder, responseItem, null);
            }

            return response;
        }

        public static int GetResponseSequenceNumber(this TerminalMessage source)
        {
            object response = source.GetBaseResponse();

            var baseResponse = response as BaseResponse;

            if (baseResponse != null)
                return baseResponse.SequenceNumber;

            var hexResponse = response as BaseSimpleHexResponse;

            return hexResponse?.SequenceNumber ?? 0;
        }

        public static int GetCommandSequenceNumber(this TerminalMessage source)
        {
            object command = source.GetBaseCommand();

            var baseCommand = command as BaseCommand;

            if (baseCommand != null)
                return baseCommand.SequenceNumber;

            var hexCommand = command as BaseSimpleHexCommand;

            return hexCommand?.SequenceNumber ?? 0;
        }

        public static bool GetResponseSuccess(this TerminalMessage source)
        {
            object response = source.GetBaseResponse();

            var baseResponse = response as BaseResponse;

            if (baseResponse != null)
                return baseResponse.Success;

            var hexResponse = response as BaseSimpleHexResponse;

            return hexResponse?.Success ?? false;
        }

        public static string GetResponseMessage(this TerminalMessage source)
        {
            object response = source.GetBaseResponse();

            var baseResponse = response as BaseResponse;

            if (baseResponse != null)
                return baseResponse.Message;

            var hexResponse = response as BaseSimpleHexResponse;

            return hexResponse?.Message ?? Empty;
        }

        public static object GetBaseEvent(this TerminalMessage source)
        {
            return source?.Item;
        }

        public static string Serialize(this TerminalMessage source)
        {
            var serializer = ServiceLocator.Current.GetInstance<ITerminalMessageSerializer>();
            return serializer.Serialize(source);

        }

        public static byte[] GetBytes(this TerminalMessage source)
        {
            return Encoding.UTF8.GetBytes(source.Serialize());
        }

        public static void SetCommandSequenceNumber(this TerminalMessage source, int sequenceNumber)
        {
            object command = source.GetBaseCommand();

            var baseCommand = command as BaseCommand;

            if (baseCommand != null)
                baseCommand.SequenceNumber = sequenceNumber;

            var hexCommand = command as BaseSimpleHexCommand;

            if (hexCommand != null)
                hexCommand.SequenceNumber = sequenceNumber;
        }
    }
}
