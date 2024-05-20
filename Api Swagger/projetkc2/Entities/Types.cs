using System;
using System.Collections.Generic;

namespace projetkc2.Entities;

public partial class Types
{
    public int IdType { get; set; }

    public string? NomType { get; set; }

    public virtual ICollection<Plante> Plantes { get; } = new List<Plante>();
}
