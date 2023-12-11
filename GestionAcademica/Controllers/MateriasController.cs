using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionAcademica.Data;
using GestionAcademica.Models;
using GestionAcademica.DTO;

namespace GestionAcademica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriasController : ControllerBase
    {
        private readonly GestionAcademicaCopiaContext _context;

        public MateriasController(GestionAcademicaCopiaContext context)
        {
            _context = context;
        }

        // GET: api/Materias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materia>>> GetMaterias()
        {
          if (_context.Materias == null)
          {
              return NotFound();
          }
            return await _context.Materias.ToListAsync();
        }

        /// <summary>
        /// Trae todas las materias de una carrera
        /// </summary>
        /// <param name="idCarrera"></param>
        /// <returns></returns>
        [HttpGet("Carrera/{idCarrera}")]
        public async Task<ActionResult<IEnumerable<MateriasDTO>>> GetMateriasCarrera(int idCarrera)
        {
            if (_context.Materias == null)
            {
                return NotFound();
            }
            var query = from cm in _context.CarrerasMaterias
                        join m in _context.Materias on cm.IdMateria equals m.Id
                        join co in _context.Correlativas on m.Id equals co.IdMateria into com
                        from res in com.DefaultIfEmpty()
                        where cm.IdCarrera == idCarrera
                        select new MateriasDTO { IdMateria = m.Id, Nombre = m.Nombre, Cuatrimestre = cm.Cuatrimestre, Correlativas = res.IdMateriaCorrelativa.GetValueOrDefault(), CargaHoraria = m.CargaHoraria };

            var pepe = query.ToList();

            return query.OrderBy(x => x.Cuatrimestre).ToList();
        }

        // GET: api/Materias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Materia>> GetMateria(int id)
        {
          if (_context.Materias == null)
          {
              return NotFound();
          }
            var materia = await _context.Materias.FindAsync(id);

            if (materia == null)
            {
                return NotFound();
            }

            return materia;
        }

        // PUT: api/Materias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMateria(int id, Materia materia)
        {
            if (id != materia.Id)
            {
                return BadRequest();
            }

            _context.Entry(materia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Materias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Materia>> PostMateria(Materia materia)
        {
          if (_context.Materias == null)
          {
              return Problem("Entity set 'GestionAcademicaCopiaContext.Materias'  is null.");
          }
            _context.Materias.Add(materia);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MateriaExists(materia.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMateria", new { id = materia.Id }, materia);
        }

        // DELETE: api/Materias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMateria(int id)
        {
            if (_context.Materias == null)
            {
                return NotFound();
            }
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
            {
                return NotFound();
            }

            _context.Materias.Remove(materia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MateriaExists(int id)
        {
            return (_context.Materias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
