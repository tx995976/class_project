using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

using book_manager.Views.Windows;

namespace book_manager.ViewModels;


public partial class ItemViewModel :ObservableObject , INavigationAware
{
    public ItemViewModel(){
        item_query = App.GetService<Services.BookService>().get_items();
    }

    async public void OnNavigatedTo() {
        Page_now = 1;
        await Task.Run(() => flush_item());
        Page_str = "当前第1页"; 
    }

    public void OnNavigatedFrom() {}

    #region item_info
    [ObservableProperty]
    public IEnumerable<Models.item>? _items;

    [ObservableProperty]
    private int _page_now;


    private ISugarQueryable<Models.item>? item_query;

    async public Task flush_item() {
        Items = await item_query!.ToPageListAsync(Page_now,20);
    }

    #endregion

    #region  page
    [ObservableProperty]
    private string? _page_str = "当前第1页";

    [RelayCommand]
    private void Prepage()
    {
        if (Page_now == 1)
            return;
        --Page_now;
        Page_str = $"当前第{Page_now}页";
        Task.Run(() => flush_item());
    }

    [RelayCommand]
    private void Nextpage()
    {
        Page_now++;
        Page_str = $"当前第{Page_now}页";
        Task.Run(() => flush_item());
    }

    #endregion

    #region item_update

    [RelayCommand]
    private void Onadditem(){
        App.GetService<ItemAddWindow>().Show();
    }


    #endregion

}