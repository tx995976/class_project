using Wpf.Ui.Common.Interfaces;


namespace book_manager.Views.Pages;


public partial class ConfimPage : INavigableView<ViewModels.ConfimViewModel>
{
    public ViewModels.ConfimViewModel ViewModel {get;}


    public ConfimPage(ViewModels.ConfimViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }
}   