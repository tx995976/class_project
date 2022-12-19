using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

using book_manager.Models;
using book_manager.Services;
using System;


namespace book_manager.ViewModels;

public partial class ItemAddViewModel : ObservableObject
{
    public Views.Windows.ItemAddWindow? window { get; set; }

    public ItemAddViewModel() {
        window_close += clear_info;
    }

    public Action? window_close;
   
    #region item_info

    [ObservableProperty]
    public Title? _ititle = new Title();

    [ObservableProperty]
    public long _new_snowid = 0;

    [ObservableProperty]
    public long _add_isbn;

    [ObservableProperty]
    public bool _need_info = true;

    [ObservableProperty]
    public string? _check_res;

    [ObservableProperty]
    public bool _once_end = true;

    #endregion

    [RelayCommand]
    private void Check_title(){
        var bookservice = App.GetService<BookService>();
        var title = bookservice.get_title(Add_isbn);
        if(title != null){
            Check_res = "找到图书介绍,可直接添加";
            Ititle = title;
        }
        else{
            Check_res = "无图书介绍,请手动添加";
            Need_info = false;
        }
    }

    [RelayCommand]
    private void New_info_confim(){
        var bookservice = App.GetService<BookService>();
        if(Need_info == false){
            bookservice.add_title(Ititle!);
        }
        var item = new item();
        item.isbn = Add_isbn;
        New_snowid = bookservice.add_item(item);
        Once_end = false;
        Check_res = "添加完成,数字id已生成";
    }

    private void clear_info(){
        Ititle = new Title();
        Add_isbn = 0;
        Check_res = "";
        New_snowid = 0;
    }






}