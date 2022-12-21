using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace book_manager.ViewModels;


public partial class ConfimViewModel :ObservableObject , INavigationAware
{
    public ConfimViewModel() {}


    async public void OnNavigatedTo() {
        await Task.Run(() => flush_info());
        Confims = all_confims;
    }

    public void OnNavigatedFrom() {}

    #region Confim_info
    
    [ObservableProperty]
    private IEnumerable<Models.waiting_solve>? _confims;

    public List<Models.waiting_solve>? all_confims {get; set; }

    async public void flush_info(){
        var service = App.GetService<Services.UserInfoService>();
        await Task.Run(() => service.book_user_flush());
        all_confims = service.confims;
        Confims = all_confims;
    }

    public long stack_confim;

    [RelayCommand]
    async private void Onconfimbutton(object id_solve){
        var service = App.GetService<Services.UserInfoService>();
        var confim = await service.get_confimAsync((long)id_solve);
        
        switch(confim!.type){
            case Models.waiting_solve.solve_type.reservation_to_loan:
                stack_confim = (long)id_solve;
                var selector = App.GetService<Views.Windows.TimeSelectWindow>();
                selector.Show();
                selector.ViewModel.selectend += Confim_loan;
                break;
            case Models.waiting_solve.solve_type.ext_loan:
                break;
            case Models.waiting_solve.solve_type.lose_solve:
                break;
            case Models.waiting_solve.solve_type.loan_end:
                break;
        }
    }

    async public void Confim_loan(DateTime end){
        App.GetService<Services.BookService>().confim_loan(stack_confim,end);
        await Task.Run(() => flush_info());
    }

    async public void Confim_ext(long id_solve){
        await Task.Run(() => flush_info());
    }

    async public void Confim_lose(long id_solve){
        await Task.Run(() => flush_info());
    }

    async public void Confim_return(long id_solve){
        App.GetService<Services.BookService>().confim_return(id_solve);
        await Task.Run(() => flush_info());
    }



    #endregion
}