﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using System.Windows.Threading;
using Prism.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(IResponseListViewModel))]
    public class ResponseListViewModel : ViewModelBase, IResponseListViewModel
    {
        private string _title = "Responses";
        private Dispatcher _dispatcher;
        private bool _clearOnRun;

        public ResponseListViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            ClearOnRun = true;

            EventAggregator.GetEvent<ResponseReceivedEvent>().Subscribe(OnResponseReceived);

            DelteAllCommand = new DelegateCommand(OnDeleteAll);

            EventAggregator.GetEvent<PreviewRunCommandsEvent>().Subscribe(OnPreviewRunCommands);
        }

        public ICommand DelteAllCommand { get; }

        public bool ClearOnRun
        {
            get { return _clearOnRun; }
            set { SetProperty(ref _clearOnRun, value); }
        }

        private void OnResponseReceived(TerminalMessage message)
        {
            _dispatcher.BeginInvoke(DispatcherPriority.DataBind, (Action)(() => Messages.Add(new ResponseReceivedViewModel(message))));
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<ResponseReceivedViewModel> Messages { get; } = new ObservableCollection<ResponseReceivedViewModel>();

        private void OnDeleteAll()
        {
            Messages.Clear();
        }

        private void OnPreviewRunCommands()
        {
            if (ClearOnRun)
                Messages.Clear();
        }
    }
}
