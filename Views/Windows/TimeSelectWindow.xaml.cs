using System.ComponentModel;
using Wpf.Ui.Controls;


namespace book_manager.Views.Windows;


public partial class TimeSelectWindow : UiWindow
{
    public ViewModels.TimeSelectViewModel ViewModel { get; }


    public TimeSelectWindow(ViewModels.TimeSelectViewModel Viewmodels)
    {
        ViewModel = Viewmodels;
        DataContext = this;
        ViewModel.window = this;
        InitializeComponent();
    }


    protected override void OnClosing(CancelEventArgs e)
    {
        ViewModel.clear_deleg();
        Hide();
        e.Cancel = true;
    }
}