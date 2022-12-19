using Wpf.Ui.Common.Interfaces;
using book_manager.ViewModels;


namespace book_manager.Views.Pages
{
    /// <summary>
    /// ItemPage.xaml 的交互逻辑
    /// </summary>
    public partial class ItemPage : INavigableView<ItemViewModel>
    {
        public ViewModels.ItemViewModel ViewModel {get;}

        public ItemPage(ItemViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

      
    }
}
