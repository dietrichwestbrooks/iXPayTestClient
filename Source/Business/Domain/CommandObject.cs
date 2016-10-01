namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public abstract class CommandObject
    {
        protected CommandObject(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public ITerminalRequestHandler Successor { get; set; }

        public bool Result { get; protected set; }

        public string ResultMessage { get; protected set; }

        public bool ProcessResponse(object response)
        {
            var baseResponse = response as BaseResponse;

            if (baseResponse != null)
            {
                ResultMessage = baseResponse.Message;
                return (Result = baseResponse.Success);
            }

            var hexResponse = response as BaseSimpleHexResponse;

            if (hexResponse != null)
            {
                ResultMessage = hexResponse.Message;
                return (Result = hexResponse.Success);
            }

            return (Result = false);
        }

        protected TerminalMessage GetMessage(object commandItem)
        {
            return new TerminalMessage { Item = BuildCommand(commandItem, Successor) };
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
    }
}
