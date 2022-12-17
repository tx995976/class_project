using book_manager.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace book_manager.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();

            //binding callback
            App.GetService<UserService>().flush_user += flush_panel;
        }

        #region book_attributes

        [ObservableProperty]
        private string status_log = "登录";

        #endregion

        #region view_generate

        private void InitializeViewModel()
        {
            ApplicationTitle = "book_manager";

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Home",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.DashboardPage)
                },
                new NavigationItem()
                {
                    Content = "Data",
                    PageTag = "data",
                    Icon = SymbolRegular.DataHistogram24,
                    PageType = typeof(Views.Pages.DataPage)
                },
                new NavigationItem()
                {
                    Content = "BookShelf",
                    PageTag = "book",
                    Icon = SymbolRegular.Book24,
                    PageType = typeof(Views.Pages.BookViewPage)
                }
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Settings",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "test",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;
            
        }

        #endregion

        #region login_method

        [RelayCommand]
        private void OnshowLoginWindow(){
            App.GetService<UserLoginWindow>().Show();
        }

        private void flush_panel(Models.User? user){
            Status_log = user?.student?.name ?? "登录";

            //flush_panel
            if(user != null)
                switch(user.accountType){
                    case Models.User.userType.normal:
                        break;
                    case Models.User.userType.book_manager:
                        break;
                    case Models.User.userType.system_manager:
                        break;
                }
            else{
                
            }
        }

        #endregion

    }
}
