using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

using book_manager.Models;

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
        var service = App.GetService<Services.UserInfoService>();
        await Task.Run(() => service.book_user_flush());
        Loans = service.user_loans;
        Loses = service.user_loses;
        Reserv = service.user_reservations;
    }

    #endregion

    [RelayCommand]
    private void OnextCommand(object loan_id){

    }

    [RelayCommand]
    private void OnreportloseCommand(object item_id){

    }

}