﻿using System;
using System.Collections.Generic;

namespace GestionAcademica.Models;

public partial class Cursada
{
    public int Id { get; set; }

    public int Cuatrimestre { get; set; }

    public int IdMateria { get; set; }

    public int? Anio { get; set; }

    public int Activa { get; set; }

    public int? Dia { get; set; }

    public int? Turno { get; set; }

    public int? IdProfesor { get; set; }

}
