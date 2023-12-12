using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Correlativa
{

    public int IdTablaCorrelativas { get; set; }

    public int? IdMateria { get; set; }

    public int? IdMateriaCorrelativa { get; set; }

}
