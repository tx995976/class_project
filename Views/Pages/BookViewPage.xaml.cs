using Wpf.Ui.Common.Interfaces;

namespace book_manager.Views.Pages
{
    /// <summary>
    /// BookViewPage.xaml 的交互逻辑
    /// </summary>
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
