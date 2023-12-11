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
    public class CarrerasController : ControllerBase
    {
        private readonly GestionAcademicaCopiaContext _context;

        public CarrerasController(GestionAcademicaCopiaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Trae todas las carreras y agrupadas por facultades
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public ActionResult<IEnumerable<FacultadCarrerasDTO>> GetCarreras()
        {
            if (_context.Carreras == null)
            {
                return NotFound();
            }
            var carreras = _context.Carreras.ToList();

            //Las Agrupo por Facultades
            List<FacultadCarrerasDTO> resultados = new List<FacultadCarrerasDTO>();

            foreach (var carrera in carreras)
            {
                int indice = resultados.FindIndex(x => x.NombreFacultad == carrera.Facultad);
                if (indice >= 0)
                {
                    resultados[indice].Carreras.Add(carrera);
                }
                else
                {
                    FacultadCarrerasDTO aux = new FacultadCarrerasDTO();
                    aux.NombreFacultad = carrera.Facultad;
                    aux.Carreras.Add(carrera);
                    resultados.Add(aux);
                }
            }

            return resultados;
        }

        // GET: api/Carreras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Carrera>> GetCarrera(int id)
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

            return carrera;
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
        public ActionResult<AltaCarreraDTO> PostCarrera(AltaCarreraDTO carrera)
        {
            if (_context.Carreras == null)
            {
                return Problem("Entity set 'GestionAcademicaCopiaContext.Carreras'  is null.");
            }
            Carrera c = new Carrera() { Facultad = carrera.Facultad, Nombre = carrera.Nombre };
            _context.Carreras.Add(c);

            try
            {
                _context.SaveChanges();

                carrera.Materias.ForEach(car =>
                {
                    car.IdCarrera = c.Id;
                });

                _context.CarrerasMaterias.AddRange(carrera.Materias);
                _context.SaveChanges();

            }
            catch (DbUpdateException)
            {

            }

            return Created("Carrera", carrera);
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
