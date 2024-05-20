using System;
using System.Collections.Generic;

namespace projetkc2.Entities;

public partial class Reserve
{
    public int IdReserve { get; set; }

    public int? CodePostale { get; set; }

    public int? ReserveDeau { get; set; }
}
