using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CarMaintenance;

public partial class Customer
{
    public int Id { get; set; }

    [DisplayName("姓名")]
    public string Name { get; set; } = null!;

    [DisplayName("電話")]
    public string Phone { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<Car> Cars { get; } = new List<Car>();
}
