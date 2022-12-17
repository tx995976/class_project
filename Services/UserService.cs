using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using book_manager.Models;
using book_manager.Helpers;

public partial class UserService{

    public User? currentUser { get; set;}

    public int login(string? username, string? password){
        var res = dbhelper.Db.Queryable<User>().Includes(x => x.student).InSingle(username);
        
        if(res == null)
            return -1;
        else if(!password!.Equals(res.Password))
            return -2;
        
        currentUser = res;
        flush_user?.Invoke(currentUser);
        return 0;
    }

    public void logout(){
        currentUser = null;
        flush_user?.Invoke(currentUser);
    }
    
    //triggered when user login
    public Action<User?>? flush_user;

    #region normal_actions

    public int add_borrower(Borrower user) =>
        dbhelper.Db.Insertable(user).ExecuteCommand();

    public int add_user(User user) =>
        dbhelper.Db.Insertable(user).ExecuteCommand();

    #endregion


    #region admin_actions

    async public Task<List<User>> get_users() =>
       await dbhelper.Db.Queryable<User>().ToListAsync();

    async public Task<List<User>> get_users(string para) =>
       await dbhelper.Db.Queryable<User>().Where(x => x.Account!.Contains(para)).ToListAsync();

    public void change_user_type(User user,User.userType type){

        
    }

    #endregion


}