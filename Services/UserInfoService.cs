using SqlSugar;
using System;
using System.Collections.Generic;

using book_manager.Models;
using book_manager.Helpers;

using System.Linq;
using System.Threading.Tasks;

namespace book_manager.Services;

public partial class UserInfoService
{
    public User? currentUser { get; set; }

    public UserInfoService() {
        App.GetService<UserService>().flush_user += user_change;
    }

    async private void user_change(User? user) {
        currentUser = user;
        if(user == null)
            return;
        await Task.Run(() => info_flush());
    }

    async public void info_flush(){
        switch (currentUser!.accountType) {
            case User.userType.normal:
                await Task.Run(() => normal_user_flush());
                break;
            case User.userType.book_manager:
                await Task.Run(() => book_user_flush());
                break;
            case User.userType.system_manager:
                await Task.Run(() => admin_user_flush());
                break;
        }
    }

    #region normal_user_info

    public List<info_loan>? user_loans { get; set; }
    public List<info_lose>? user_loses { get; set; }
    public List<info_reservation>? user_reservations { get; set; }

    async public Task normal_user_flush() {
        user_loans = await dbhelper.Db.Queryable<info_loan>()
                    .Includes(x => x.item)
                    .Where(info => info.id_borrower == currentUser!.id && (!info.is_complete))
                    .ToListAsync();

        user_loses = await dbhelper.Db.Queryable<info_lose>()
                    .Includes(x=> x.item)
                    .Where(info => info.id_borrower == currentUser!.id && (!info.is_complete))
                    .ToListAsync();

        user_reservations = await dbhelper.Db.Queryable<info_reservation>()
                    .Includes(x => x.item)
                    .Where(info => info.id_borrower == currentUser!.id && (!info.is_complete))
                    .ToListAsync();
    }

    #endregion

    #region book_manager_info

    public List<waiting_solve>? confims { get; set; }

    async public Task book_user_flush() {
        confims = await dbhelper.Db.Queryable<waiting_solve>()
                    .Where(x => !x.is_complete)
                    .ToListAsync();
    }

    public async Task<waiting_solve?> get_confimAsync(long solve_id) =>
        await dbhelper.Db.Queryable<waiting_solve>().InSingleAsync(solve_id);


    #endregion

    #region admin_user_info

    public List<User>? users { get; set; }

    async public Task admin_user_flush() {
        users = await dbhelper.Db.Queryable<User>()
                .Where(x => x.accountType != User.userType.system_manager)
                .ToListAsync();
        
    }

    #endregion





}