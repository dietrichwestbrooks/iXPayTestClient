using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    /// <summary>
    /// Interaction logic for ResponseListView.xaml
    /// </summary>
    [Export(typeof(IResponseListView))]
    public partial class ResponseListView : IResponseListView
    {
        [ImportingConstructor]
        public ResponseListView(IResponseListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
