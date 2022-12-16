using SqlSugar;
using System;
using System.Collections.Generic;

using book_manager.Models;
using book_manager.Helpers;

using System.Linq;

namespace book_manager.Services;

public partial class UserInfoService
{

    #region normal_user_info

    public User? currentUser { get; set; }

    public List<info_loan>? user_loans { get; set; }
    public List<info_lose>? user_loses { get; set; }
    public List<info_reservation>? user_reservations { get; set; }

    #endregion

    public UserInfoService(){
        App.GetService<UserService>().flush_user += user_change;


    }

    private void user_change(User? user) => 
        currentUser = user;


    async public void user_flush(){
        if(currentUser == null || currentUser.accountType != User.userType.normal)
            return;

        user_loans = await dbhelper.Db.Queryable<info_loan>()
                    .Where(info => info.id_borrower == currentUser!.id && (!info.is_complete))
                    .ToListAsync();

        user_loses = await dbhelper.Db.Queryable<info_lose>()
                    .Where(info => info.id_borrower == currentUser!.id && (!info.is_complete))
                    .ToListAsync();

        user_reservations = await dbhelper.Db.Queryable<info_reservation>()
                    .Where(info => info.id_borrower == currentUser!.id && (!info.is_complete))
                    .ToListAsync();
    }





}