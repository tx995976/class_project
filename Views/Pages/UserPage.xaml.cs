using Wpf.Ui.Common.Interfaces;


namespace book_manager.Views.Pages;


public partial class UserPage : INavigableView<ViewModels.UserViewModel>
{
    public ViewModels.UserViewModel ViewModel {get;}


    public UserPage(ViewModels.UserViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }
}