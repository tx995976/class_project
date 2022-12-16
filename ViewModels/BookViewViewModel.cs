using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace book_manager.ViewModels;

public partial class BookViewViewModel :ObservableObject, INavigationAware
{
    [ObservableProperty]
    private IEnumerable<Models.Title>? _book_result;

    [ObservableProperty]
    private int _page_now;

    #region loadprocessor

    [ObservableProperty]
    private Visibility _process_visible;

    #endregion

    private ISugarQueryable<Models.Title>? book_all { get; set; }
    private ISugarQueryable<Models.Title>? book_res { get; set; }

    public int pagesize = 20;


    public BookViewViewModel() {
        book_all = App.GetService<Services.BookService>().get_book_titles();
    }

    async public void OnNavigatedTo() {
        Page_now = 1;
        await flush_book();
    }

    public void OnNavigatedFrom() {

    }

    async public Task flush_book() {
        Process_visible = Visibility.Visible;
        Book_result = await book_all!.ToPageListAsync(Page_now, pagesize);
        Process_visible = Visibility.Collapsed;
    }




}