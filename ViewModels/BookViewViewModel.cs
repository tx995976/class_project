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
    public BookViewViewModel() {
        book_res = App.GetService<Services.BookService>().get_book_titles();
    }

    async public void OnNavigatedTo() {
        Page_now = 1;
        await Task.Run(() => flush_book());
        Page_str = "当前第1页"; 
    }

    public void OnNavigatedFrom() {

    }

    #region book_pages

    [ObservableProperty]
    private IEnumerable<Models.Title>? _book_result;

    [ObservableProperty]
    private int _page_now;

    [ObservableProperty]
    private string? _page_str = "当前第1页";

    public static int pagesize = 20;
    public int total_books;

    private ISugarQueryable<Models.Title>? book_res { get; set; }

    async public Task flush_book() {
        Process_visible = Visibility.Visible;
        Book_result = await book_res!.ToPageListAsync(Page_now, pagesize,total_books);
        Process_visible = Visibility.Collapsed;
    }

    [RelayCommand]
    private void Prepage()
    {
        if (Page_now == 1)
            return;
        --Page_now;
        Page_str = $"当前第{Page_now}页";
        Task.Run(() => flush_book());
    }

    [RelayCommand]
    private void Nextpage()
    {
        Page_now++;
        Page_str = $"当前第{Page_now}页";
        Task.Run(() => flush_book());
    }

    [RelayCommand]
    private void Showbook_detail(object para){
        var detail_window = App.GetService<Views.Windows.BookdetailWindow>();
        detail_window.ViewModel.flush_detail((long)para);
        detail_window.Show();
    }

    #endregion

    #region loadprocessor

    [ObservableProperty]
    private Visibility _process_visible;

    #endregion

    #region search_book_titles


    #endregion



}