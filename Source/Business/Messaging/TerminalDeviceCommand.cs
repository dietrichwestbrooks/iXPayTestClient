using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class TerminalDeviceCommand<TCommand, TResponse> : ITerminalDeviceCommand
        where TCommand : class, new()
        where TResponse : class
    {
        private readonly Func<TCommand> _getDefaultCommand;

        public TerminalDeviceCommand(ITerminalDeviceMember member, string name, Func<TCommand> getDefaultCommand = null)
        {
            Member = member;
            Name = name;
            _getDefaultCommand = getDefaultCommand ?? (() => new TCommand());
        }

        public ITerminalDeviceMember Member { get; }

        public string Name { get; }

        public Type CommandType => typeof(TCommand);

        public Type ResponseType => typeof(TResponse);

        public bool Result { get; private set; }

        public string ResultMessage { get; private set; }

        public TerminalMessage GetMessage(CommandParameters parameters = null)
        {
            TCommand command = (parameters == null ? _getDefaultCommand() : (TCommand)CreateCommand(parameters));

            return new TerminalMessage { Item = BuildCommand(command, Member.Device) };
        }

        private object CreateCommand(CommandParameters parameters)
        {
            var command = new TCommand();

            PropertyInfo[] properties = typeof(TCommand).GetProperties();

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

        public object Execute(CommandParameters parameters)
        {
            object response;

            try
            {
                TerminalMessage message = GetMessage(parameters);

                response = SendMessageAsync(message).Result;
                ProcessResponse(response);

                if (!Result)
                    throw new InvalidOperationException(ResultMessage);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException ?? ex;
            }

            return response;
        }

        protected virtual Task<object> SendMessageAsync(TerminalMessage message)
        {
            return new MessageBrokerTask(message).Task;
        }

        public bool ProcessResponse(object response)
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
