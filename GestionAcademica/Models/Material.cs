using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Material
{
    public int IdMaterial { get; set; }

    public int IdCursada { get; set; }

    public DateTime FechaPublicacion { get; set; }

    public int Tipo { get; set; }
}
