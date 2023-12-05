using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Cuota
{
    public int IdCuota { get; set; }

    public int LegajoAlumno { get; set; }

    public int Mes { get; set; }

    public int Anio { get; set; }

    public double Monto { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public DateTime FechaPago { get; set; }


}
