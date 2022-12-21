using Wpf.Ui.Common.Interfaces;


namespace book_manager.Views.Pages;


public partial class BorrowInfoPage : INavigableView<ViewModels.BorrowInfoViewModel>
{
    public ViewModels.BorrowInfoViewModel ViewModel {get;}


    public BorrowInfoPage(ViewModels.BorrowInfoViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }
}