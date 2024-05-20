using System;
using System.Collections.Generic;

namespace projetkc2.Entities;

public class WateringAutonomyRequest
{
    public double Kc { get; set; }
    public double WaterReserveVolume { get; set; }
    public double CultureSurface { get; set; }
}
