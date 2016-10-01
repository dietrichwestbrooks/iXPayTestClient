using System;
using System.Reflection;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Extensions
{
    public static class TerminalResponseExtensions
    {
        public static BaseResponse GetBaseResponse(this TerminalResponse source)
        {
            BaseResponse response = null;

            object responseItem = source.Item;

            while (responseItem != null)
            {
                response = responseItem as BaseResponse;

                if (response != null)
                    break;

                responseItem = responseItem.GetType().InvokeMember("Item",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty,
                    Type.DefaultBinder, responseItem, null);
            }

            return response;
        }
    }
}
