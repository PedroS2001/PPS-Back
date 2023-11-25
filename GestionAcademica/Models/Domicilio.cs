using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Domicilio
{
    public int Id { get; set; }

    public int IdPais { get; set; }

    public string Provincia { get; set; } = null!;

    public string Localidad { get; set; } = null!;

    public int CodigoPostal { get; set; }

    public string Calle { get; set; } = null!;

    public int? Altura { get; set; }

    public int? Piso { get; set; }

    public string? Departamento { get; set; }

}
