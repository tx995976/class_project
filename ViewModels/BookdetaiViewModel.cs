using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

using book_manager.Models;
using book_manager.Services;
using book_manager.Helpers;
namespace book_manager.ViewModels;

public partial class BookdetailViewModel : ObservableObject
{
    public Views.Windows.BookdetailWindow? window { get; set;}

    #region info_for_book

    [ObservableProperty]
    public bool _canreserve;

    [ObservableProperty]
    public string? _num_detail;

    [ObservableProperty]
    public Title? _ctitle;

    [ObservableProperty]
    public string? _description;

    [ObservableProperty]
    public string? _reservation_snowid;

    bool flag_lose {get; set;}
    bool flag_book_any {get; set;}
    bool flag_loan_full {get; set;}
    bool flag_already_have {get; set;}

    async public void flush_detail(long isbn){
        var bookservice = App.GetService<BookService>();
        var userinfo = App.GetService<UserInfoService>();

        Ctitle = bookservice.get_title(isbn);
        
        flag_lose = (userinfo!.user_loses!.Count != 0 && BookService.ban_lose_to_loan);
        flag_book_any = Ctitle!.last_num != 0;
        flag_loan_full = (userinfo!.user_loans!.Count+userinfo!.user_reservations!.Count < BookService.user_max_book_loan);
        flag_already_have = await dbhelper.Db.Queryable<info_loan>()
                            .Includes(x => x.item)
                            .Where(x => x.item!.isbn == isbn && x.id_borrower == userinfo!.currentUser!.id)
                            .AnyAsync() 
                         || await dbhelper.Db.Queryable<info_reservation>()
                            .Includes(x => x.item)
                            .Where(x => x.item!.isbn == isbn && x.id_borrower == userinfo!.currentUser!.id)
                            .AnyAsync();

        Canreserve = !flag_lose && flag_loan_full && flag_book_any && !flag_already_have;
        Description = $"书名: {Ctitle.name}\nisbn: {Ctitle.isbn}\n作者: {Ctitle.author}\n类型: {Ctitle.type}\n价格: {Ctitle.price}\n简介: {Ctitle.description}\n";

        Num_detail = flag_lose ? $"您有未处理的报失" : flag_already_have ? "您已经预约或借阅" : $"共 {Ctitle.total_num}本 馆存 {Ctitle.last_num}本";

    }

    [RelayCommand]
    private void Onreservation(object isbn){
        var snowid = App.GetService<BookService>().reservation_new((long)isbn);
        Task.Run(() => flush_detail(Ctitle!.isbn));
        Reservation_snowid = $"您预约的书本号为: {snowid}";
    }



    #endregion
    



}