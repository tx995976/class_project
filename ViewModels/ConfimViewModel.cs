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

    [ObservableProperty]
    private Visibility _process_visible;

    async public void flush_info(){
        Process_visible = Visibility.Visible;
        var service = App.GetService<Services.UserInfoService>();
        await service.book_user_flush();
        all_confims = service.confims;
        Confims = all_confims;
        Process_visible = Visibility.Collapsed;
    }

    public long stack_confim;

    [RelayCommand]
    async private void Onconfimbutton(object id_solve){
        var service = App.GetService<Services.UserInfoService>();
        var confim = await service.get_confimAsync((long)id_solve);
        var selector = App.GetService<Views.Windows.TimeSelectWindow>();

        switch(confim!.type){
            case Models.waiting_solve.solve_type.reservation_to_loan:
                stack_confim = (long)id_solve;
                selector.Show();
                selector.ViewModel.selectend += Commit_loan;
                break;
            case Models.waiting_solve.solve_type.ext_loan:
                stack_confim = (long)id_solve;
                selector.Show();
                selector.ViewModel.selectend += Commit_ext;
                break;
            case Models.waiting_solve.solve_type.lose_solve:
                Commit_lose((long)id_solve);
                break;
            case Models.waiting_solve.solve_type.loan_end:
                Commit_return((long)id_solve);
                break;
        }
    }

    async public void Commit_loan(DateTime end){
        App.GetService<Services.BookService>().confim_loan(stack_confim,end);
        await Task.Run(() => flush_info());
    }

    async public void Commit_ext(DateTime end){
        App.GetService<Services.BookService>().confim_ext(stack_confim,end);
        await Task.Run(() => flush_info());
    }

    async public void Commit_lose(long id_solve){
        App.GetService<Services.BookService>().confim_lose(id_solve);
        await Task.Run(() => flush_info());
    }

    async public void Commit_return(long id_solve){
        App.GetService<Services.BookService>().confim_return(id_solve);
        await Task.Run(() => flush_info());
    }



    #endregion
}