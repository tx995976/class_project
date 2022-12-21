using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System;


namespace book_manager.ViewModels;


public partial class TimeSelectViewModel :ObservableObject 
{
    public TimeSelectViewModel() {}

    public Views.Windows.TimeSelectWindow? window { get; set;}

    [ObservableProperty]
    public DateTime _timest = DateTime.Today.AddDays(1);

    public Action<DateTime>? selectend;

    [RelayCommand]
    private void Onselectend(object selectdate){
        selectend?.Invoke((DateTime)selectdate);
        window!.Hide();
        clear_deleg();
    }

    public void clear_deleg(){
        if(selectend is null)
            return;
        foreach(var it in selectend.GetInvocationList())
            selectend -= it as Action<DateTime>;
    }


}