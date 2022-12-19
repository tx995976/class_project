using Wpf.Ui.Common.Interfaces;

namespace book_manager.Views.Pages
{
    public partial class BookViewPage : INavigableView<ViewModels.BookViewViewModel>
    {
        public ViewModels.BookViewViewModel ViewModel {get;}


        public BookViewPage(ViewModels.BookViewViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
