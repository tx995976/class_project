using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;


namespace book_manager.ViewModels;


public partial class ConfimViewModel :ObservableObject , INavigationAware
{
    public ConfimViewModel() {}


    async public void OnNavigatedTo() {
        await Task.Run(() => flush_info());
        Confims = all_confims;
    }

    async public void OnNavigatedFrom() {}

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

    [RelayCommand]
    private void Onconfimbutton(object id_solve){
        
    }



    #endregion
}