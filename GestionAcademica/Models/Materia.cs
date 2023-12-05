using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Materia
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int CargaHoraria { get; set; }

}
