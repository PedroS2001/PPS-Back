using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Material
{
    public int IdMaterial { get; set; }

    public int IdCursada { get; set; }

    public DateTime FechaPublicacion { get; set; }

    public int Tipo { get; set; }
    public string? Titulo { get; set; }
    public string? Texto { get; set; }
    public string? FilePath { get; set; }
}
