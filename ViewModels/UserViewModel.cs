using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

using book_manager.Models;

namespace book_manager.ViewModels;


public partial class UserViewModel :ObservableObject , INavigationAware
{
    public UserViewModel() {}

    async public void OnNavigatedTo() {
        await Task.Run(() => flush_user());
    }

    public void OnNavigatedFrom() {}

    #region users_view

    [ObservableProperty]
    public IEnumerable<User>? _users;


    async public Task flush_user() {
       var service = App.GetService<Services.UserService>();
       Users = await service.get_users();
    }

    [RelayCommand]
    private void Onuseradd(){

    }

    [RelayCommand]
    private void Onremoveuser(object Account){

    }

    #endregion


}