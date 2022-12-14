using SqlSugar;
using System;

namespace book_manager.Models;

[SugarTable("info_loan")]
public class info_loan
{

    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int loan_id { get; set; }

    [SugarColumn]
    public DateTime loan_date { get; set; }

    [SugarColumn]
    public DateTime end_date { get; set; }

    [SugarColumn]
    public int id { get; set; }

    [Navigate(NavigateType.OneToOne,nameof(id))]
    public Borrower? student { get; set; }
}

[SugarTable("info_lose")]
public class info_lose
{

    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int lose_id { get; set; }

    [SugarColumn]
    public int id_borrower { get; set; }

    [SugarColumn]
    public DateTime lose_date { get; set; }

}

[SugarTable("info_reservation")]
public class info_reservation
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]    
    public int reservation_id { get; set; }

    [SugarColumn]        
    public int id_borrower { get; set; }

    [SugarColumn]        
    public DateTime reser_date { get; set;}

    [SugarColumn]        
    public int title_id { get; set;}

}