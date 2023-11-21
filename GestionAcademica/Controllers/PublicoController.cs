using System;
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

        [HttpGet("Carreras")]
        public async Task<ActionResult<IEnumerable<Carrera>>> GetCarreras()
        {
            if (_context.Carreras == null)
            {
                return NotFound();
            }

            return await _context.Carreras.ToListAsync();
        }

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
        /// DOCUMENTACION NOVEDADES
        /// </summary>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <returns>pedro</returns>
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
