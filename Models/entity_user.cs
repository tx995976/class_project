using SqlSugar;

namespace book_manager.Models;

[SugarTable("Borrower")]
public class Borrower
{
    [SugarColumn(IsPrimaryKey =true)]
    public int id { get; set; }  //学号

    [SugarColumn]
    public string? name { get; set; } //名字

    [SugarColumn]
    public string? cardno { get; set; } //班级

    [SugarColumn]
    public string? department { get; set; } //学院

    [SugarColumn]
    public string? type { get; set; }

    [SugarColumn]
    public string? major { get; set; }

    [SugarColumn]
    public bool Isgraduate { get; set; } //
}

[SugarTable("User")]
public class User
{
    [SugarColumn]
    public int id { get; set; }  //学号

    [SugarColumn(IsPrimaryKey =true)]
    public string Account { get; set;} 

    [SugarColumn(IsNullable =false)]
    public string Password { get; set;} 

}