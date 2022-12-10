using SqlSugar;
using System;

namespace book_manager.Helpers;

public class dbhelper
{
    public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig()
    {
        ConnectionString = "Server=localhost;Database=Bookshelf;Uid=tx995975;Pwd=020926",
        DbType = DbType.MySqlConnector,
        IsAutoCloseConnection = true
    },
    (db) =>
    {


    });

}