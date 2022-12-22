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

    public static int user_max_book_loan = 5;   
    public static bool ban_lose_to_loan = true;    

    public User? user { get; set; }

    public BookService(){
        App.GetService<UserService>().flush_user += user_change;

    }

    public void user_change(User? _user) =>
        user = _user;

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

    public ISugarQueryable<item> get_items() =>
        dbhelper.Db.Queryable<item>().Includes(x => x.title);

    public ISugarQueryable<item> search_items(string word) =>
        dbhelper.Db.Queryable<item>().Includes(x => x.title)
                                    .Where(x => x.title!.name!.Contains(word));


    public Title? get_title(long isbn) =>
        dbhelper.Db.Queryable<Title>().InSingle(isbn);

    public item? get_item(long id) =>
        dbhelper.Db.Queryable<item>().InSingle(id);


    #endregion

    #region book_actions_normal_users

    // return --> reservation_item_snowid
    public long reservation_new(long isbn){
        var db = dbhelper.Db;

        var title = db.Queryable<Title>()
                            .Includes(x => x!.items!.Where(z => z.is_free).ToList())
                            .InSingle(isbn);
        
        var res_item = title!.items!.First();
        var info = new info_reservation(user!.id,res_item.item_id);
        var confim = new waiting_solve(user!.id,res_item.item_id,waiting_solve.solve_type.reservation_to_loan);

        var snowid = db.Insertable(info).ExecuteReturnSnowflakeId();
        db.Insertable(confim).ExecuteReturnSnowflakeId();

        res_item.reservation_id = snowid;
        res_item.is_free = false;
        title.last_num--;

        db.Updateable(title).ExecuteCommand();
        db.Updateable(res_item).ExecuteCommand();

        return res_item.item_id;
    }

    public void lose_new(long item_id){
        var db = dbhelper.Db;

        var item = db.Queryable<item>().Includes(x => x.loan).InSingle(item_id);

        var info = new info_lose(user!.id,item_id);
        var confim = new waiting_solve(user!.id,item_id,waiting_solve.solve_type.lose_solve);

        var snowid = db.Insertable(info).ExecuteReturnSnowflakeId();

        item.loan!.is_complete = true;
        db.UpdateNav(item).Include(x => x.loan).ExecuteCommand();

        item.lose_id = snowid;
        item.loan_id = 0;

        db.Updateable(item).ExecuteCommand();
        db.Insertable(confim).ExecuteReturnSnowflakeId();
    }

    public void ext_new(long item_id){
        var iuser = App.GetService<UserInfoService>();
        var db = dbhelper.Db;

        var item = db.Queryable<item>().Includes(x => x.loan).InSingle(item_id);
        var confim = new waiting_solve(user!.id,item_id,waiting_solve.solve_type.ext_loan);

        db.Insertable(confim).ExecuteReturnSnowflakeId();
    }

    #endregion


    #region book_actions_manager

     /*
        @insert loan_info
        @update item,title
    */
    public void confim_return(long solve_id){
        var db = dbhelper.Db;

        var confim = db.Queryable<waiting_solve>().InSingle(solve_id);
        var item = db.Queryable<item>()
                .Includes(x => x.loan)
                .Includes(x => x.title)
                .InSingle(confim.id_item);

        item.loan!.is_complete = true;
        item.title!.last_num += 1;
        db.UpdateNav(item).Include(x => x.loan).Include(x => x.title).ExecuteCommand();

        item.is_free = true;
        item.loan_id = 0;

        confim.is_complete = true;
        
        db.Updateable(item).ExecuteCommand();
        db.Updateable(confim).ExecuteCommand();
    }

    public void confim_loan(long solve_id,DateTime end_date) {
        var db = dbhelper.Db;

        //execute
        var confim =  db.Queryable<waiting_solve>().InSingle(solve_id);
        var item = db.Queryable<item>().Includes(x => x.reservation).InSingle(confim.id_item);
        var return_confim = new waiting_solve(confim.id_borrower,confim.id_item,waiting_solve.solve_type.loan_end);

        var info = new info_loan(confim.id_borrower,confim.id_item,end_date);
        var res = db.Insertable<info_loan>(info).ExecuteReturnSnowflakeId();
        db.Insertable(return_confim).ExecuteReturnSnowflakeId();

        item.reservation!.is_complete = true;
        db.UpdateNav<item>(item).Include(x => x.reservation).ExecuteCommand();
        item.reservation_id = 0;
        item.loan_id = res;

        confim.is_complete = true;

        db.Updateable<item>(item).ExecuteCommand();
        db.Updateable<waiting_solve>(confim).ExecuteCommand();
    }

    public void confim_lose(long solve_id){
        var db = dbhelper.Db;

        var confim =  db.Queryable<waiting_solve>().InSingle(solve_id);

    }

    public void confim_ext(long solve_id,DateTime end_date){
         var db = dbhelper.Db;

        //execute
        var confim =  db.Queryable<waiting_solve>().InSingle(solve_id);
        var item = db.Queryable<item>().Includes(x => x.reservation).InSingle(confim.id_item);
        


    }


    #endregion

    #region book_manage

    public void add_title(Title book) =>
        dbhelper.Db.Insertable(book).ExecuteCommand();
    
    public void update_title(Title book) =>
        dbhelper.Db.Updateable(book).ExecuteCommand();
    
    public long add_item(item new_item){
        var res = dbhelper.Db.Insertable(new_item).ExecuteReturnSnowflakeId();

        var title = dbhelper.Db.Queryable<Title>().InSingle(new_item.isbn);
        title.total_num += 1;
        title.last_num += 1;
        dbhelper.Db.Updateable(title).ExecuteCommand();
        return res;
    }

    public void delete_item(long item_id){
        var item = dbhelper.Db.Queryable<item>().InSingle(item_id);
        var title = dbhelper.Db.Queryable<Title>().InSingle(item.isbn);

        title.last_num -= 1;
        title.total_num -= 1;
        dbhelper.Db.Updateable(title).ExecuteCommand();
        dbhelper.Db.Deleteable(item).ExecuteCommand();
    }
  

    #endregion

}