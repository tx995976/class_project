using System.ComponentModel;
using Wpf.Ui.Controls;

namespace book_manager.Views.Windows
{
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
            ViewModel.Onclose();
            Hide();
            e.Cancel = true;
        }

    }
}

