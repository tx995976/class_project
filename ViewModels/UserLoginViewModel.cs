using Wpf.Ui.Common.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace book_manager.ViewModels;

public partial class UserLoginViewModel :ObservableObject
{

    [ObservableProperty]
    private string? _account;

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private string? _log;

    public Views.Windows.UserLoginWindow? window { get; set;}

    public UserLoginViewModel()
    {

    }

    [RelayCommand]
    private void Onloginbutton() {
        //Console.WriteLine($"name:{_account},password:{_password}");
        var res = App.GetService<UserService>().login(Account, Password);
        if(res == 0)
            window?.Hide();
        else{
            Log = res == -1 ? "no user" : "incorrect password";
        }
    }



}