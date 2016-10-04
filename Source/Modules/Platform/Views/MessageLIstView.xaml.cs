using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    /// <summary>
    /// Interaction logic for MessageLIstView.xaml
    /// </summary>
    [Export(typeof(IMessageListView))]
    public partial class MessageLIstView : IMessageListView
    {
        [ImportingConstructor]
        public MessageLIstView(IMessageListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void DataGrid_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;

            if (grid != null)
            {
                FrameworkElement element = e.OriginalSource as FrameworkElement;

                if (element?.DataContext is IViewModel)
                {
                    if (grid.SelectedItem == (IViewModel)((FrameworkElement)e.OriginalSource).DataContext)
                    {
                        grid.SelectedIndex = -1;
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
