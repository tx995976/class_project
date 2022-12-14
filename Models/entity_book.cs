using SqlSugar;
using System.Collections.Generic;

namespace book_manager.Models;

[SugarTable("Title")]
public class Title
{

    [SugarColumn(IsPrimaryKey = true)]
    public string? isbn { get; set; }
    
    [SugarColumn]
    public string? name { get; set; }

    [SugarColumn]
    public string? description { get; set; }

    [SugarColumn]
    public string? author { get; set; }

    [SugarColumn]
    public double price { get; set; }

    [SugarColumn]
    public int total_num { get; set; }

    [SugarColumn]
    public int last_num { get; set; }

    [SugarColumn]
    public string? type { get; set; }

    [Navigate(NavigateType.OneToMany,nameof(isbn),nameof(item.isbn))]
    public List<item>? items { get; set; }

}

[SugarTable("item")]
public class item
{

    [SugarColumn(IsPrimaryKey = true,IsIdentity =true)]
    public int item_id { get; set; }

    [SugarColumn]
    public string? isbn { get; set; }

    [SugarColumn]
    public int loan_id { get; set;}

    [SugarColumn]
    public int lose_id { get; set;}

    [SugarColumn]
    public int reservation_id { get; set;}

    #region fk

    [Navigate(NavigateType.OneToOne,nameof(loan_id))]
    public info_loan? loan { get; set;}

    [Navigate(NavigateType.OneToOne, nameof(lose_id))]
    public info_lose? lose { get; set;}

    [Navigate(NavigateType.OneToOne, nameof(reservation_id))]
    public info_reservation? reservation{ get; set;}

    #endregion
}