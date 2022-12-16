using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using book_manager.Models;
using book_manager.Helpers;
using System.Linq;

namespace book_manager.Services;

public partial class BookService
{
    #region definitions_book_service

    const int user_max_book_loan = 5;  
    const bool ban_lose_to_loan = true;    

    #endregion


    #region DB_Title_page

    public ISugarQueryable<Title> get_book_titles() {
        var res = dbhelper.Db.Queryable<Title>();
        return res;
    }

    public ISugarQueryable<Title> search_book_titles(string word) {
        var res = dbhelper.Db.Queryable<Title>().Where(it => it!.name!.Contains(word));
        return res;
    }

    #endregion

    #region book_actions_normal_users

    /*
        @insert loan_info
        @update item,title
    */
    public int loan_new(long item_id,DateTime end_date) {
        var iuser = App.GetService<UserInfoService>();
        var db = dbhelper.Db;

        //check currentUser
        if((iuser!.user_loans!.Count) >= user_max_book_loan || (ban_lose_to_loan && (iuser!.user_loses!.Count > 0)))
            return -1;
        //

        //execute
        var item =  db.Queryable<item>().Includes(x => x.title).InSingle(item_id);

        var info = new info_loan(iuser!.currentUser!,item_id,end_date);
        var res = db.Insertable<info_loan>(info).ExecuteReturnSnowflakeId();

        item.loan_id = res;
        item!.title!.last_num--;
        item.is_free = false;
        db.UpdateNav<item>(item).Include(x => x.title).ExecuteCommand();

        return 0;
    }

    // return --> reservation_item_snowid
    public long reservation_new(long isbn){
        var iuser = App.GetService<UserInfoService>();
        var db = dbhelper.Db;

        var title = db.Queryable<Title>()
                            .Includes(x => x!.items!.Where(z => z.is_free).ToList())
                            .InSingle(isbn);
        
        var res_item = title!.items!.First();
        var info = new info_reservation(iuser!.currentUser!,res_item.item_id);
        var confim = new waiting_solve(iuser!.currentUser!,res_item.item_id,waiting_solve.solve_type.reservation_to_loan);

        var snowid = db.Insertable(info).ExecuteReturnSnowflakeId();
        db.Insertable(confim).ExecuteCommand();

        res_item.reservation_id = snowid;
        res_item.is_free = false;
        title.last_num--;

        db.Updateable(title).ExecuteCommand();
        db.Updateable(res_item).ExecuteCommand();

        return res_item.item_id;
    }

    public void lose_new(long item_id){
        var iuser = App.GetService<UserInfoService>();
        var db = dbhelper.Db;

        var item = db.Queryable<item>().InSingle(item_id);

        var info = new info_lose(iuser!.currentUser!,item_id);
        



    }

    public int loan_solve(long item_id){
        var iuser = App.GetService<UserInfoService>();
        var db = dbhelper.Db;

        return 0;
    }
    
    #endregion

    #region book_actions_manager


    #endregion




}