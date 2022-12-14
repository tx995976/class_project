
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;
using System.Windows;
using System.Windows.Controls;
using System;
using System.ComponentModel;

namespace book_manager.Views.Windows
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class UserLoginWindow : UiWindow
    {

        public ViewModels.UserLoginViewModel ViewModel { get; }

        public UserLoginWindow(ViewModels.UserLoginViewModel Viewmodels) {
            ViewModel = Viewmodels;
            DataContext = this;
            ViewModel.window = this;
            
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

    }
}
