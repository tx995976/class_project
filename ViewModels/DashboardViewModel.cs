using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;

using book_manager.Helpers;
using book_manager.Models;
using System;

namespace book_manager.ViewModels
{
    public partial class DashboardViewModel :ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private int _counter = 0;

        public void OnNavigatedTo() {
        }

        public void OnNavigatedFrom() {
        }

        [RelayCommand]
        private void OnCounterIncrement() {
            Counter++;
            //for testing
            Helpers.dbhelper.table_test();
        }

    }
}
