using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Nota
{
    public int? Id_nota { get; set; }

    public int LegajoAlumno { get; set; }

    public int IdCursada { get; set; }

    public int TipoNota { get; set; } 

    public int NotaNumerica { get; set; }

    public DateTime? Fecha { get; set; }
}
