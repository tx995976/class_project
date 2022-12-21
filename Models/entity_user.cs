using SqlSugar;
using System.Collections.Generic;

namespace book_manager.Models;

[SugarTable("Borrower")]
public class Borrower
{

    [SugarColumn(IsPrimaryKey = true)]
    public int id { get; set; }  //学号

    public string? name { get; set; } //名字

    public string? cardno { get; set; } //班级

    public string? department { get; set; } //学院

    public string? type { get; set; }

    public string? major { get; set; }

    public bool Isgraduate { get; set; } //


}

[SugarTable("User")]
public class User
{
    public int id { get; set; }  //学号

    [SugarColumn(IsPrimaryKey = true)]
    public string? Account { get; set; }

    [SugarColumn(IsNullable = false)]
    public string? Password { get; set; }

    [SugarColumn(IsNullable = false)]
    public userType accountType { get; set; }

    public enum userType
    {
        normal,
        book_manager,
        system_manager
    }

    [Navigate(NavigateType.OneToOne, nameof(id))]
    public Borrower? student { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(id),nameof(info_loan.id_borrower))]
    public List<info_loan>? loans { get; set; }

    [Navigate(NavigateType.OneToMany,nameof(id),nameof(info_lose.id_borrower))]
    public List<info_lose>? loses { get; set; }

    [Navigate(NavigateType.OneToMany,nameof(id),nameof(info_reservation.id_borrower))]
    public List<info_reservation>? reservations { get; set; }

}