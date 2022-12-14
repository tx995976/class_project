using SqlSugar;
using System;

namespace book_manager.Helpers;

public class dbhelper
{
    public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig()
    {
        ConnectionString = "Server=192.168.41.3;Database=Bookshelf;Uid=tx995975;Pwd=020926",
        DbType = DbType.MySqlConnector,
        IsAutoCloseConnection = true
    },
    (db) =>
    {


    });


    public static void table_test(){
        //init tables
        var tables = Db.DbMaintenance.GetTableInfoList(false);
        foreach (var table in tables){
            Console.WriteLine(table.Name);
        }

        Db.CodeFirst.InitTables<Models.Borrower>();
        Db.CodeFirst.InitTables<Models.User>();
        Db.CodeFirst.InitTables<Models.Title>();
        Db.CodeFirst.InitTables<Models.item>();
        Db.CodeFirst.InitTables<Models.info_loan>();
        Db.CodeFirst.InitTables<Models.info_lose>();
        Db.CodeFirst.InitTables<Models.info_reservation>();
    }
}

