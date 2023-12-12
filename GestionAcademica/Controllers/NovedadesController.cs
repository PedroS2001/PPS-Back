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
    public class NovedadesController : ControllerBase
    {
        private readonly GestionAcademicaCopiaContext _context;

        public NovedadesController(GestionAcademicaCopiaContext context)
        {
            _context = context;
        }

        // GET: api/Novedades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Novedad>>> GetNovedades()
        {
            if (_context.Novedades == null)
            {
                return NotFound();
            }
            return await _context.Novedades.ToListAsync();
        }

        // GET: api/Novedades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Novedad>> GetNovedad(int id)
        {
            if (_context.Novedades == null)
            {
                return NotFound();
            }
            var novedad = await _context.Novedades.FindAsync(id);

            if (novedad == null)
            {
                return NotFound();
            }

            return novedad;
        }

        // PUT: api/Novedades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNovedad(int id, Novedad novedad)
        {
            if (id != novedad.Id)
            {
                return BadRequest();
            }

            _context.Entry(novedad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NovedadExists(id))
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

        // POST: api/Novedades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Novedad>> PostNovedad([FromForm] IFormFile file, [FromForm] Novedad novedad)
        {
            if (_context.Novedades == null)
            {
                return Problem("Entity set 'GestionAcademicaCopiaContext.Novedades'  is null.");
            }
            _context.Novedades.Add(novedad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNovedad", new { id = novedad.Id }, novedad);
        }

        // DELETE: api/Novedades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNovedad(int id)
        {
            if (_context.Novedades == null)
            {
                return NotFound();
            }
            var novedad = await _context.Novedades.FindAsync(id);
            if (novedad == null)
            {
                return NotFound();
            }

            _context.Novedades.Remove(novedad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NovedadExists(int id)
        {
            return (_context.Novedades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
