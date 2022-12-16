using SqlSugar;
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


}