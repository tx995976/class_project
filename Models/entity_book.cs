using SqlSugar;
using System.Collections.Generic;

namespace book_manager.Models;

[SugarTable("Title")]
public class Title
{

    [SugarColumn(IsPrimaryKey = true)]
    public long isbn { get; set; }
    
    public string? name { get; set; }

    public string? description { get; set; }

    public string? author { get; set; }

    public double price { get; set; }

    public int total_num { get; set; }

    public int last_num { get; set; }

    public string? type { get; set; }

    [Navigate(NavigateType.OneToMany,nameof(isbn),nameof(item.isbn))]
    public List<item>? items { get; set; }

}

[SugarTable("item")]
public class item
{

    [SugarColumn(IsPrimaryKey = true)]
    public long item_id { get; set; }

    public long isbn { get; set; }

    public long loan_id { get; set;}

    public long lose_id { get; set;}

    public long reservation_id { get; set;}

    public bool is_free { get; set; }

    #region fk

    [Navigate(NavigateType.OneToOne,nameof(loan_id))]
    public info_loan? loan { get; set;}

    [Navigate(NavigateType.OneToOne, nameof(lose_id))]
    public info_lose? lose { get; set;}

    [Navigate(NavigateType.OneToOne, nameof(reservation_id))]
    public info_reservation? reservation{ get; set;}

    [Navigate(NavigateType.OneToOne, nameof(isbn))]
    public Title? title { get; set;}

    #endregion
}