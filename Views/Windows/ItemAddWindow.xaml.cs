using System.ComponentModel;
using Wpf.Ui.Controls;
using System;


namespace book_manager.Views.Windows;


public partial class ItemAddWindow : UiWindow
{
    public ViewModels.ItemAddViewModel ViewModel { get; }

    public ItemAddWindow(ViewModels.ItemAddViewModel Viewmodels)
    {
        ViewModel = Viewmodels;
        DataContext = this;
        ViewModel.window = this;
        InitializeComponent();
    }


    protected override void OnClosing(CancelEventArgs e)
    {
        ViewModel.window_close?.Invoke();
        Hide();
        e.Cancel = true;
    }
}