using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public sealed class TerminalDeviceCommand : ITerminalDeviceCommand
    {
        private readonly Func<object> _prepareCommandFunc;

        public TerminalDeviceCommand(ITerminalDeviceMember member, string name, Type commandType, Type responseType, Func<object> prepCmdFunc)
        {
            Member = member;
            //Successor = Successor;
            Name = name;
            CommandType = commandType;
            ResponseType = responseType;
            _prepareCommandFunc = prepCmdFunc ?? (() => Activator.CreateInstance(CommandType));
        }

        public string Name { get; set; }

        public Type CommandType { get; }

        public Type ResponseType { get; }

        public bool Result { get; private set; }

        public string ResultMessage { get; private set; }

        public ITerminalDeviceMember Member { get; }

        public ITerminalRequestHandler Successor { get; set; }

        public object HandleRequest(object command)
        {
            return command;
        }

        public TerminalMessage GetMessage(CommandParameters parameters = null)
        {
            object command = (parameters == null ? _prepareCommandFunc() : CreateCommand(parameters));

            return new TerminalMessage { Item = BuildCommand(command, Member.Device) };
            //return new TerminalMessage { Item = BuildCommand(command, Successor) };
        }

        public object Execute(CommandParameters parameters)
        {
            object response;

            try
            {
                TerminalMessage message = GetMessage(parameters);

                response = SendMessageAsync(message).Result;

                if (!ProcessResponse(response))
                    throw new InvalidOperationException(ResultMessage);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException ?? ex;
            }

            return response;
        }

        private Task<object> SendMessageAsync(TerminalMessage message)
        {
            return new MessageBrokerTask(message).Task;
        }

        private object CreateCommand(CommandParameters parameters)
        {
            var command = Activator.CreateInstance(CommandType);

            PropertyInfo[] properties = CommandType.GetProperties();

            var namedParams = parameters.Where(p => !string.IsNullOrWhiteSpace(p.Key));

            // Match unnamed parameters with command properties by position and assign property name
            var unamedParams = parameters
                .Where(p => string.IsNullOrWhiteSpace(p.Key))
                .Zip(properties, (param, prop) => new KeyValuePair<string, object>(prop.Name, param.Value));

            var allParams = unamedParams.Concat(namedParams);

            foreach (var parameter in allParams)
            {
                var property =
                    properties.FirstOrDefault(
                        p => string.Equals(p.Name, parameter.Key, StringComparison.InvariantCultureIgnoreCase));

                property?.SetValue(command, parameter.Value);
            }

            return command;
        }

        private object BuildCommand(object commandItem, ITerminalRequestHandler successor)
        {
            while (true)
            {
                if (successor == null)
                    return commandItem;

                var command = successor.HandleRequest(commandItem);

                commandItem = command;

                successor = successor.Successor;
            }
        }

        private bool ProcessResponse(object response)
        {
            BaseResponse baseResponse = response as BaseResponse;

            if (baseResponse != null)
                return ProcessResponse(baseResponse);

            BaseSimpleHexResponse hexResponse = response as BaseSimpleHexResponse;

            if (hexResponse != null)
                return ProcessResponse(hexResponse);

            return false;
        }

        private bool ProcessResponse(BaseResponse response)
        {
            ResultMessage = response.Message;
            return (Result = response.Success);
        }

        private bool ProcessResponse(BaseSimpleHexResponse response)
        {
            ResultMessage = response.Message;
            return (Result = response.Success);
        }
    }
}
