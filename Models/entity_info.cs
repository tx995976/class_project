using SqlSugar;
using System;

namespace book_manager.Models;

[SugarTable("info_loan")]
public class info_loan
{
    public info_loan() {}
    public info_loan(int user_id, long item, DateTime end) {
        id_item = item;
        loan_date = DateTime.Now;
        end_date = end;
        id_borrower = user_id;
        ext_num = 0;
        is_complete = false;
    }



    [SugarColumn(IsPrimaryKey = true)]
    public long loan_id { get; set; }

    public long id_item { get; set; }

    public DateTime loan_date { get; set; }

    public DateTime end_date { get; set; }

    public int ext_num { get; set; }

    public int id_borrower { get; set; }

    public bool is_complete { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(id_borrower))]
    public Borrower? student { get; set; }

}

[SugarTable("info_lose")]
public class info_lose
{
    public info_lose() { }
    public info_lose(int user_id, long item) {
        id_item = item;
        id_borrower = user_id;
        is_complete = false;
        lose_date = DateTime.Now;
    }

    [SugarColumn(IsPrimaryKey = true)]
    public long lose_id { get; set; }

    public long id_item { get; set; }

    public int id_borrower { get; set; }

    public bool is_complete { get; set; }

    public DateTime lose_date { get; set; }

}

[SugarTable("info_reservation")]
public class info_reservation
{
    public info_reservation() { }
    public info_reservation(int user_id, long item) {
        id_item = item;
        id_borrower = user_id;
        reser_date = DateTime.Now;
        is_complete = false;
    }


    [SugarColumn(IsPrimaryKey = true)]
    public long reservation_id { get; set; }

    public long id_item { get; set; }

    public int id_borrower { get; set; }

    public DateTime reser_date { get; set; }

    public bool is_complete { get; set; }

}

//for book_manager to solve
[SugarTable("waiting_solve")]
public class waiting_solve
{
    public waiting_solve(){}
    public waiting_solve(int user_id, long item,solve_type _type){
        id_borrower = user_id;
        id_item = item;
        _type = type;
        is_complete = false;
    }

    public enum solve_type
    {
        reservation_to_loan,
        lose_solve,
        ext_loan
    }

    [SugarColumn(IsPrimaryKey = true)]
    public long id_solve { get; set; }

    public int id_borrower { get; set; }

    public long id_item { get; set; }

    public solve_type type { get; set; }

    public bool is_complete { get; set; }
}