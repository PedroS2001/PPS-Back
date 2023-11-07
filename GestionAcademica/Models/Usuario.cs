using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Usuario
{
    public int Legajo { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }

    public string? Dni { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public int? IdCarrera { get; set; }

    public int? IdDomicilio { get; set; }

    public int? Sexo { get; set; }

    public int? TipoUsuario { get; set; }

}
