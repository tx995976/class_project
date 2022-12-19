using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

using book_manager.Views.Windows;
using book_manager.Services;

namespace book_manager.ViewModels;


public partial class ItemViewModel :ObservableObject , INavigationAware
{
    public ItemViewModel(){
        item_query = App.GetService<Services.BookService>().get_items();
    }

    async public void OnNavigatedTo() {
        await Task.Run(() => flush_item());
    }

    public void OnNavigatedFrom() {}

    #region item_info
    [ObservableProperty]
    public IEnumerable<Models.item>? _items;

    private ISugarQueryable<Models.item>? item_query;

    async public Task flush_item() {
        Items = await item_query!.ToListAsync();
    }

    #endregion

    #region item_update

    [RelayCommand]
    private void Onadditem(){
        App.GetService<ItemAddWindow>().Show();
    }

    [RelayCommand]
    async private void Onremoveitem(object item_id){
        var Services = App.GetService<BookService>();
        Services.delete_item((long)item_id);
        await Task.Run(() => flush_item());
    }


    #endregion

}