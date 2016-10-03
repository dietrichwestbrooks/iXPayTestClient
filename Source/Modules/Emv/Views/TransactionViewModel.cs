using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;
using Convert = Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Utility.Convert;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Emv.Views
{
    [Export(typeof(ITransactionViewModel))]
    public class TransactionViewModel : ViewModelBase, ITransactionViewModel
    {
        private string _title = "EMV Transaction Flow";
        private string _initiateTransactionCommandXml;
        private TerminalMessage _initiateTransactionCommand;
        private string _setAmountsCommandXml;
        private TerminalMessage _setAmountsCommand;
        private string _continueTransactionCommandXml;
        private TerminalMessage _continueTransactionCommand;
        private bool _useSetAmounts;

        public TransactionViewModel()
        {
            _initiateTransactionCommand = new TerminalMessage
                {
                    Item = new TerminalCommand
                        {
                            Item = new EMVModuleCommand
                                {
                                    Item = new InitiateTransactionCommand
                                        {
                                            EmvDataElement = new[]
                                                {
                                                    new EmvDataElement
                                                        {
                                                            Value =
                                                                new Convert().ToHexByteArray(
                                                                    "E11B9F02060000000005009F02060000000010009F0206000000002000")
                                                        }
                                                }
                                        }
                                }
                        }
                };

            _setAmountsCommand = new TerminalMessage
                {
                    Item = new TerminalCommand
                        {
                            Item = new EMVModuleCommand
                                {
                                    Item = new SetAmountsCommand
                                        {
                                            EmvDataElement = new[]
                                                {
                                                    new EmvDataElement
                                                        {
                                                            Value =
                                                                new Convert().ToHexByteArray(
                                                                    "E11B9F02060000000005009F02060000000010009F0206000000002000")
                                                        }
                                                }
                                        }
                                }
                        }
                };

            _continueTransactionCommand = new TerminalMessage
                {
                    Item = new TerminalCommand
                        {
                            Item = new EMVModuleCommand
                                {
                                    Item = new ContinueTransactionCommand()
                                }
                        }
                };

            _initiateTransactionCommandXml = _initiateTransactionCommand.Serialize().FormattedXml();
            _setAmountsCommandXml = _setAmountsCommand.Serialize().FormattedXml();
            _continueTransactionCommandXml = _continueTransactionCommand.Serialize().FormattedXml();
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string InitiateTransactionCommandXml
        {
            get { return _initiateTransactionCommandXml; }
            set
            {
                SetProperty(ref _initiateTransactionCommandXml, value);
                _initiateTransactionCommand = new TerminalMessageSerializer().Deserialize(value);
            }
        }

        public string SetAmountsCommandXml
        {
            get { return _setAmountsCommandXml; }
            set
            {
                SetProperty(ref _setAmountsCommandXml, value);
                _setAmountsCommand = new TerminalMessageSerializer().Deserialize(value);
            }
        }

        public string ContinueTransactionCommandXml
        {
            get { return _continueTransactionCommandXml; }
            set
            {
                SetProperty(ref _continueTransactionCommandXml, value);
                _continueTransactionCommand = new TerminalMessageSerializer().Deserialize(value);
            }
        }

        public bool UseSetAmounts
        {
            get { return _useSetAmounts; }
            set { SetProperty(ref _useSetAmounts, value); }
        }
    }
}
