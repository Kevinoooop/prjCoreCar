using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CarMaintenance;

public partial class Car
{

    public int CustomerId { get; set; }

    [DisplayName("車牌號碼")]
    public string Id { get; set; } = null!;

    [DisplayName("廠牌")]
    public string Brand { get; set; } = null!;

    [DisplayName("型號")]
    public string Model { get; set; } = null!;

    [DisplayName("年式")]
    public string Year { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<Bill> Bills { get; } = new List<Bill>();

    public virtual Customer Customer { get; set; } = null!;
}
