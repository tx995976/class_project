using SqlSugar;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;


namespace book_manager.Helpers;

public class dbhelper
{
    public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig()
    {
        ConnectionString = Configread(),
        DbType = DbType.MySqlConnector,
        IsAutoCloseConnection = true
    },
    (db) =>
    {
        SnowFlakeSingle.WorkId = 14;

    });

     public static string Configread(){
        var str = new StreamReader("./Config/db.json");
        var ini = JsonNode.Parse(str.ReadToEnd());
        var db_config = ini!["db"]!;
        string server = (string)db_config["server"]!;
        string database = (string)db_config["Database"]!;
        string id = (string)db_config["uid"]!;
        string passwd = (string)db_config["upass"]!;

        return $"Server={server};Database={database};Uid={id};Pwd={passwd}";
    }




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
        Db.CodeFirst.InitTables<Models.waiting_solve>();

    }
}

