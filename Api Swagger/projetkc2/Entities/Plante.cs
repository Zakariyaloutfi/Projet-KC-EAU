using System;
using System.Collections.Generic;

namespace projetkc2.Entities;

public partial class Plante
{
    public int IdPlante { get; set; }

    public string? NomPlante { get; set; }

    public int? IdType { get; set; }

    public virtual Types? IdTypeNavigation { get; set; }

    public virtual ICollection<Information> Information { get; } = new List<Information>();
}
