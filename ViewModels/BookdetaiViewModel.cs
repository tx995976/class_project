using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace book_manager.ViewModels;

public partial class BookdetailViewModel : ObservableObject
{
    public Views.Windows.BookdetailWindow? window { get; set;}

    #region info_for_book

    [ObservableProperty]
    public bool _canreserve;

    [ObservableProperty]
    public int _last_num;

    



    #endregion
    



}