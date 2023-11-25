﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionAcademica.Data;
using GestionAcademica.Models;

namespace GestionAcademica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicoController : ControllerBase
    {
        private readonly GestionAcademicaCopiaContext _context;

        public PublicoController(GestionAcademicaCopiaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Trae todas las carreras y su ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("Carreras")]
        public async Task<ActionResult<IEnumerable<Carrera>>> GetCarreras()
        {
            if (_context.Carreras == null)
            {
                return NotFound();
            }

            return await _context.Carreras.ToListAsync();
        }

        /// <summary>
        /// Trae todas las materias de una carrera
        /// </summary>
        /// <param name="idCarrera"></param>
        /// <returns></returns>
        [HttpGet("Materias/{idCarrera}")]
        public async Task<ActionResult<IEnumerable<Materia>>> GetMateriasCarrera(int idCarrera)
        {
            if (_context.Materias == null)
            {
                return NotFound();
            }
            var query = from cm in _context.CarrerasMaterias
                        join m in _context.Materias on cm.IdMateria equals m.Id
                        where cm.IdCarrera == idCarrera
                        select m;

            return query.ToList();
        }

        /// <summary>
        /// Trae todas las novedades
        /// </summary>
        /// <returns></returns>
        [HttpGet("Novedades")]
        public async Task<ActionResult<IEnumerable<Novedad>>> GetNovedades()
        {
            if (_context.Novedades == null)
            {
                return NotFound();
            }

            return _context.Novedades.ToList();

        }





    }
}
