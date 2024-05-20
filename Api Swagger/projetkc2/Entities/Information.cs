using System;
using System.Collections.Generic;

namespace projetkc2.Entities;

public partial class Information
{
    public int IdInformation { get; set; }

    public string? Stades { get; set; }

    public string? Kc { get; set; }

    public string? Periode { get; set; }

    public string? Vergers { get; set; }

    public string? Irrigation { get; set; }

    public int? IdPlante { get; set; }

    public virtual Plante? IdPlanteNavigation { get; set; }
}
