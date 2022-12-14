using SqlSugar;
using System;
using System.Collections.Generic;

using book_manager.Models;
using book_manager.Helpers;

namespace book_manager.Services;

public partial class BookService{
    
    #region Title page

    public ISugarQueryable<Title> get_book_titles(){
        var res = dbhelper.Db.Queryable<Title>();
        return res;
    }

    public ISugarQueryable<Title> search_book_titles(string word){
        var res = dbhelper.Db.Queryable<Title>().Where(it => it!.name!.Contains(word));
        return res;
    }

    #endregion




}