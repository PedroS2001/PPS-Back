using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Novedad
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Texto { get; set; }
    public string? Imagen { get; set; }

    public DateTime? FechaPublicacion { get; set; }
}
