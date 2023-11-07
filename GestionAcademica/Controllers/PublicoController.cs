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

        // GET: api/Carreras
        [HttpGet("Carreras")]
        public async Task<ActionResult<IEnumerable<Object>>> GetCarreras()
        {
            if (_context.Carreras == null)
            {
                return NotFound();
            }

            return await _context.Carreras.ToListAsync();
        }

        // GET: api/Carreras/5
        [HttpGet("Materias/{idCarrera}")]
        public async Task<ActionResult<IEnumerable<Materia>>> GetCarrera(int idCarrera)
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

        // PUT: api/Carreras/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarrera(int id, Carrera carrera)
        {
            if (id != carrera.Id)
            {
                return BadRequest();
            }

            _context.Entry(carrera).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarreraExists(id))
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

        // POST: api/Carreras
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Carrera>> PostCarrera(Carrera carrera)
        {
            if (_context.Carreras == null)
            {
                return Problem("Entity set 'GestionAcademicaCopiaContext.Carreras'  is null.");
            }
            _context.Carreras.Add(carrera);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CarreraExists(carrera.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCarrera", new { id = carrera.Id }, carrera);
        }

        // DELETE: api/Carreras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrera(int id)
        {
            if (_context.Carreras == null)
            {
                return NotFound();
            }
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null)
            {
                return NotFound();
            }

            _context.Carreras.Remove(carrera);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarreraExists(int id)
        {
            return (_context.Carreras?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
