using System;
using System.Linq;
using System.Reflection;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    public class RequestHandler : ITerminalRequestHandler
    {
        public Type SuccessorType { get; protected set; }

        public object HandleRequest(object command)
        {
            var reqHandlerAttr = GetType()
                    .GetCustomAttributes(typeof (Attributes.TerminalRequestHandlerAttribute), false)
                    .Cast<Attributes.TerminalRequestHandlerAttribute>()
                    .FirstOrDefault();

            if (reqHandlerAttr == null)
                throw new InvalidOperationException($"{typeof(Attributes.TerminalRequestHandlerAttribute).Name} must be defined on {GetType().Name}");

            var outerCommand = Activator.CreateInstance(reqHandlerAttr.Command);

            outerCommand.GetType().InvokeMember("Item",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, outerCommand, new[] {command});

            return outerCommand;
        }

        public object HandleResponse(object response)
        {
            var reqHandlerAttr = GetType()
                    .GetCustomAttributes(typeof(Attributes.TerminalRequestHandlerAttribute), false)
                    .Cast<Attributes.TerminalRequestHandlerAttribute>()
                    .FirstOrDefault();

            if (reqHandlerAttr == null)
                throw new InvalidOperationException($"{typeof(Attributes.TerminalRequestHandlerAttribute).Name} must be defined on {GetType().Name}");

            if (reqHandlerAttr.Response != response.GetType())
                throw new InvalidOperationException($"Expected response type {reqHandlerAttr.Response.Name} received {response.GetType().Name}");

            var responseItem = reqHandlerAttr.Response.InvokeMember("Item",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty,
                Type.DefaultBinder, response, null);

            return responseItem;
        }
    }

    [Attributes.TerminalRequestHandler(typeof (DisplayCommand), typeof (DisplayResponse), Successor = typeof (TerminalRequestHandler))]
    public class DisplayRequestHandler : RequestHandler
    {
        
    }

    [Attributes.TerminalRequestHandler(typeof(BarcodeReaderCommand), typeof(BarcodeReaderResponse), Successor = typeof(TerminalRequestHandler))]
    public class BarcodeReaderRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(BeeperCommand), typeof(BeeperResponse), Successor = typeof(TerminalRequestHandler))]
    public class BeeperRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(BillAcceptorCommand), typeof(BillAcceptorResponse), Successor = typeof(TerminalRequestHandler))]
    public class BillAcceptorRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(ChipCardReaderCommand), typeof(ChipCardReaderResponse), Successor = typeof(TerminalRequestHandler))]
    public class ChipCardReaderRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(ContactlessReaderCommand), typeof(ContactlessReaderResponse), Successor = typeof(TerminalRequestHandler))]
    public class ContactlessReaderRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(DallasKeyCommand), typeof(DallasKeyResponse), Successor = typeof(TerminalRequestHandler))]
    public class DallasKeyRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(EMVModuleCommand), typeof(EMVModuleResponse), Successor = typeof(TerminalRequestHandler))]
    public class EmvModuleRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(KeypadCommand), typeof(KeypadResponse), Successor = typeof(TerminalRequestHandler))]
    public class KeypadRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(MagStripeReaderCommand), typeof(MagStripeReaderResponse), Successor = typeof(TerminalRequestHandler))]
    public class MagStripeReaderRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(NonSecureKeypadCommand), typeof(NonSecureKeypadResponse), Successor = typeof(TerminalRequestHandler))]
    public class NonSecureKeypadRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(PrinterCommand), typeof(PrinterResponse), Successor = typeof(TerminalRequestHandler))]
    public class PrinterRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(SAMReaderCommand), typeof(SAMReaderResponse), Successor = typeof(TerminalRequestHandler))]
    public class SamReaderRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(SecurityModuleCommand), typeof(SecurityModuleResponse), Successor = typeof(TerminalRequestHandler))]
    public class SecurityModuleRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(SoftphoneCommand), typeof(SoftphoneResponse), Successor = typeof(TerminalRequestHandler))]
    public class SoftphoneRequestHandler : RequestHandler
    {

    }


    [Attributes.TerminalRequestHandler(typeof(TamperDetectorsCommand), typeof(TamperDetectorsResponse), Successor = typeof(TerminalRequestHandler))]
    public class TamperDetectorsRequestHandler : RequestHandler
    {

    }

    [Attributes.TerminalRequestHandler(typeof(TerminalCommand), typeof(TerminalResponse))]
    public class TerminalRequestHandler : RequestHandler
    {
    }
}
