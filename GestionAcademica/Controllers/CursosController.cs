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
    public class CursosController : ControllerBase
    {
        private readonly GestionAcademicaCopiaContext _context;

        public CursosController(GestionAcademicaCopiaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Trae todas los cursos
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpGet("")]
        public ActionResult<List<Cursada>> GetCursos()
        {
            return _context.Cursadas.ToList();
        }

        /// <summary>
        /// Trae todas las Materias con cursadas activas de un usuario
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpGet("{legajo}")]
        public ActionResult<List<Cursada>> GetCursadasActivasAlumno(int legajo)
        {
            var query = from c in _context.Cursadas
                        join um in _context.UsuarioCursada on c.Id equals um.IdCursada
                        join m in _context.Materias on c.IdMateria equals m.Id
                        where um.LegajoAlumno == legajo && c.Estado == (int)EEstadoCursada.Activa
                        select c;

            return query.ToList();
        }

        /// <summary>
        /// Inscribe a un alumno a una cursada
        /// </summary>
        /// <param name="legajo"></param>
        /// <param name="idCursada"></param>
        /// <returns></returns>
        [HttpPost("Inscribir/{legajo}/{idCursada}")]
        public async Task<ActionResult<UsuarioCursada>> InscribirACursada(int legajo, int idCursada)
        {
            UsuarioCursada usuarioCursada = new UsuarioCursada() { IdCursada = idCursada, LegajoAlumno = legajo, Activa = 1 };
            try
            {
                //TODO:  Validar que el alumno tenga las cuotas al dia 

                //TODO:  Validar correlativas.

                #region  Valido que el alumno no tenga otras cursadas en ese horario
                var query = from c in _context.Cursadas
                            join uc in _context.UsuarioCursada on c.Id equals uc.IdCursada
                            where uc.LegajoAlumno == legajo
                            select c;

                var cursadasAlumno = query.ToList();

                var laCursada = _context.Cursadas.Find(idCursada);
                if (laCursada == null)
                {
                    return BadRequest("La cursada no existe");
                }

                foreach (var cursada in cursadasAlumno)
                {
                    if (cursada.Estado == (int)EEstadoCursada.Activa && cursada.Anio == laCursada.Anio && cursada.Cuatrimestre == laCursada.Cuatrimestre && cursada.Dia == laCursada.Dia && cursada.Turno == laCursada.Turno)
                    {
                        return BadRequest("El usuario ya esta inscrito a una cursada en ese horario");
                    }
                }
                #endregion

                //Inscribo al alumno en la cursada
                var user = _context.Usuarios.Find(legajo);
                _context.UsuarioCursada.Add(usuarioCursada);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Ha ocurrido un error al inscribir al alumno a la cursada");
            }

            return Created("Creada", usuarioCursada);
        }

        /// <summary>
        /// Da de baja a un alumno de una cursada
        /// </summary>
        /// <param name="legajo"></param>
        /// <param name="idCursada"></param>
        /// <returns></returns>
        [HttpPut("Baja/{legajo}/{idCursada}")]
        public ActionResult<UsuarioCursada> DarDeBaja(int legajo, int idCursada)
        {
            var rta = _context.UsuarioCursada.FirstOrDefault(x => x.IdCursada == idCursada && x.LegajoAlumno == legajo);
            if (rta == null)
            {
                return NotFound();
            }
            rta.Activa = 0;
            _context.SaveChanges();

            return rta;
        }

        /// <summary>
        /// Trae todos los materiasles de una cursada
        /// </summary>
        /// <param name="idCursada"></param>
        /// <returns></returns>
        [HttpGet("Material/{idCursada}")]
        public ActionResult<List<Material>> GetMaterialCursada(int idCursada)
        {
            var rta = _context.Materiales.Where(x => x.IdCursada == idCursada);

            return rta.ToList();
        }

        /// <summary>
        /// Trae todos los temas de una Materia
        /// </summary>
        /// <param name="idCursada"></param>
        /// <returns></returns>
        [HttpGet("Temario/{idMateria}")]
        public ActionResult<List<Temario>> GetTemasMateria(int idMateria)
        {
            var rta = _context.Temarios.Where(x => x.IdMateria == idMateria);

            return rta.ToList();
        }

        /// <summary>
        /// Dar de alta una cursada
        /// </summary>
        /// <param name="cursada"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Cursada>> CrearCursada(Cursada cursada)
        {

            if (_context.Cursadas == null)
            {
                return Problem("Entity set 'GestionAcademicaCopiaContext.Cursadas'  is null.");
            }
            try
            {
                _context.Cursadas.Add(cursada);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }

            return Created("cursada", cursada);
        }





    }
}
