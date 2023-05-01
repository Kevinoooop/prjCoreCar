using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CarMaintenance;

public partial class Bill
{
    
    public string CarId { get; set; } = null!;

    public int Id { get; set; }

    [DisplayName("日期")]
    public DateTime Date { get; set; }

    [DisplayName("項目")]
    public string Project { get; set; } = null!;

    public bool IsDelete { get; set; }

    [DisplayName("價格")]
    public decimal Price { get; set; }

    [DisplayName("車牌號碼")]
    public virtual Car Car { get; set; } = null!;
}
