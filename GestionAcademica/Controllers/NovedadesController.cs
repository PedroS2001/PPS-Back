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

        [HttpPost]
        public ActionResult PostNovedad([FromForm] IFormFile file, [FromForm] string titulo, [FromForm] string copete, [FromForm] string texto, [FromForm] bool snMostrar)
        {
            try
            {
                if (_context.Novedades == null)
                {
                    return Problem("Entity set 'GestionAcademicaCopiaContext.Novedades'  is null.");
                }
                MemoryStream memoryStream = new MemoryStream();

                var novedad = new Novedad();
                novedad.Titulo = titulo;
                novedad.FechaPublicacion = DateTime.Now;
                novedad.Copete = copete;
                novedad.Texto = texto;
                novedad.SnMostrar = (snMostrar) ? 1 : 0;

                file.CopyTo(memoryStream);

                novedad.Imagen = Convert.ToBase64String(memoryStream.ToArray());
                _context.Novedades.Add(novedad);
                _context.SaveChanges();

                ResponseDTO rta = new ResponseDTO() { Status = 200, Message = "Novedad Cargada con exito" };
                return Ok(rta);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
