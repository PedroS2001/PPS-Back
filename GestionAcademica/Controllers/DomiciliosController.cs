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
    public class DomiciliosController : ControllerBase
    {
        private readonly GestionAcademicaCopiaContext _context;

        public DomiciliosController(GestionAcademicaCopiaContext context)
        {
            _context = context;
        }

        // GET: api/Domicilios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domicilio>>> GetDomicilios()
        {
          if (_context.Domicilios == null)
          {
              return NotFound();
          }
            return await _context.Domicilios.ToListAsync();
        }

        // GET: api/Domicilios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Domicilio>> GetDomicilio(int id)
        {
          if (_context.Domicilios == null)
          {
              return NotFound();
          }
            var domicilio = await _context.Domicilios.FindAsync(id);

            if (domicilio == null)
            {
                return NotFound();
            }

            return domicilio;
        }

        // PUT: api/Domicilios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDomicilio(int id, Domicilio domicilio)
        {
            if (id != domicilio.Id)
            {
                return BadRequest();
            }

            _context.Entry(domicilio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DomicilioExists(id))
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

        // POST: api/Domicilios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Domicilio>> PostDomicilio(Domicilio domicilio)
        {
          if (_context.Domicilios == null)
          {
              return Problem("Entity set 'GestionAcademicaCopiaContext.Domicilios'  is null.");
          }
            _context.Domicilios.Add(domicilio);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DomicilioExists(domicilio.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDomicilio", new { id = domicilio.Id }, domicilio);
        }

        // DELETE: api/Domicilios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDomicilio(int id)
        {
            if (_context.Domicilios == null)
            {
                return NotFound();
            }
            var domicilio = await _context.Domicilios.FindAsync(id);
            if (domicilio == null)
            {
                return NotFound();
            }

            _context.Domicilios.Remove(domicilio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DomicilioExists(int id)
        {
            return (_context.Domicilios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
