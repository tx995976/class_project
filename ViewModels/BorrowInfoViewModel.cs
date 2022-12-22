using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using book_manager.Models;
using System.Windows;

namespace book_manager.ViewModels;


public partial class BorrowInfoViewModel :ObservableObject , INavigationAware
{
    async public void OnNavigatedTo() {
        await Task.Run(() => flush_info());
    }

    public void OnNavigatedFrom() {}

    public BorrowInfoViewModel() {}

    #region info

    [ObservableProperty]
    public IEnumerable<info_loan>? _loans;

    [ObservableProperty]
    public IEnumerable<info_lose>? _loses;

    [ObservableProperty]
    public IEnumerable<info_reservation>? _reserv;

    async public void flush_info(){
        Process_visible = Visibility.Visible;
        var service = App.GetService<Services.UserInfoService>();
        await service.normal_user_flush();
        Loans = service.user_loans;
        Loses = service.user_loses;
        Reserv = service.user_reservations;
        Process_visible = Visibility.Collapsed;
    }

    [ObservableProperty]
    private Visibility _process_visible;

    #endregion

    [RelayCommand]
    async private void Onext(object item_id){
        App.GetService<Services.BookService>().ext_new((long)item_id);
        await Task.Run(() =>flush_info());
    }

    [RelayCommand]
    async private void Onreportlose(object item_id){
        App.GetService<Services.BookService>().lose_new((long)item_id);
        await Task.Run(() =>flush_info());
    }

   

}