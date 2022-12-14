using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace book_manager.Views.Windows
{
    /// <summary>
    /// BookdetailWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BookdetailWindow : UiWindow
    {
        public ViewModels.BookdetailViewModel ViewModel { get; }

        public BookdetailWindow(ViewModels.BookdetailViewModel Viewmodels)
        {
            ViewModel = Viewmodels;
            DataContext = this;
            ViewModel.window = this;

            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

    }
}

