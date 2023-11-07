using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Nota
{
    public int LegajoAlumno { get; set; }

    public int IdCursada { get; set; }

    public string TipoNota { get; set; } = null!;

    public double NotaNumerica { get; set; }

    public DateTime? Fecha { get; set; }
}
