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
        /// ADMINISTRADOR
        /// Crea una cursada en estado SIN ASIGNAR (para que los profesores se postulen)
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
                cursada.IdProfesor = -1;
                _context.Cursadas.Add(cursada);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }

            return Created("cursada", cursada);
        }


        /// <summary>
        /// ALUMNO - PROFESOR - ADMINISTRADOR
        /// Trae todos los alumnos de una cursada
        /// </summary>
        /// <param name="idCursada"></param>
        /// <returns></returns>
        [HttpGet("Alumnos/{idCursada}")]
        public ActionResult<List<Usuario>> GetAlumnosCursada(int idCursada)
        {
            var query = from c in _context.UsuarioCursada
                        join a in _context.Usuarios on c.LegajoAlumno equals a.Legajo
                        where c.IdCursada == idCursada && a.TipoUsuario == 1
                        select a;

            return query.ToList();
        }


        #region GetCursos

        /// <summary>
        /// ADMINISTRADOR
        /// Trae todas los cursos
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpGet("")]
        public ActionResult<List<CursadaDTO>> GetCursos()
        {

            var query = from a in _context.Cursadas
                        join b in _context.Usuarios on a.IdProfesor equals b.Legajo into bo
                        from res in bo.DefaultIfEmpty()
                        join c in _context.Materias on a.IdMateria equals c.Id
                        select new CursadaDTO { Id = a.Id, Materia = c.Nombre, Turno = ((ETurno)a.Turno).ToString(), Dia = ((EDias)a.Dia).ToString(), Profesor = res.Apellido, Estado = ((EEstadoCursada)a.Estado).ToString() };
            var rta = query.ToList();

            return rta;
        }

        /// <summary>
        /// ALUMNO
        /// Trae todas las Cursadas activas del alumno
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpGet("{legajo}")]
        public ActionResult<List<CursadaDTO>> GetCursadasActivasAlumno(int legajo)
        {
            var query = from c in _context.Cursadas
                        join um in _context.UsuarioCursada on c.Id equals um.IdCursada
                        join m in _context.Materias on c.IdMateria equals m.Id
                        join p in _context.Usuarios on c.IdProfesor equals p.Legajo
                        where um.LegajoAlumno == legajo && um.Activa == 1
                        select new CursadaDTO { Id = c.Id, Materia = m.Nombre, Turno = ((ETurno)c.Turno).ToString(), Dia = ((EDias)c.Dia).ToString(), Profesor = p.Apellido, Estado = ((EEstadoCursada)c.Estado).ToString() };

            return query.ToList();
        }

        /// <summary>
        /// ALUMNO
        /// Trae todas las materias en las cuales se puede inscribir el alumno
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpGet("ParaInscribir/{idCarrera}")]
        public ActionResult<List<CursadaDTO>> GetCursadasParaInscribir(int idCarrera)
        {
            var query = from c in _context.Cursadas
                        join m in _context.Materias on c.IdMateria equals m.Id
                        join a in _context.CarrerasMaterias on m.Id equals a.IdMateria
                        join p in _context.Usuarios on c.IdProfesor equals p.Legajo
                        where c.Estado == (int)EEstadoCursada.Asignada && a.IdCarrera == idCarrera
                        select new CursadaDTO { Id = c.Id, Materia = m.Nombre, Turno = ((ETurno)c.Turno).ToString(), Dia = ((EDias)c.Dia).ToString(), Profesor = p.Apellido, Estado = ((EEstadoCursada)c.Estado).ToString() };

            return query.ToList();
        }

        /// <summary>
        /// PROFESOR
        /// Trae todas las cursadas de la que es profesor
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpGet("profesor/{legajo}")]
        public ActionResult<List<CursadaDTO>> GetCursadasActivasProfesor(int legajo)
        {
            var query = from c in _context.Cursadas
                        join m in _context.Materias on c.IdMateria equals m.Id
                        join p in _context.Usuarios on c.IdProfesor equals p.Legajo
                        where c.IdProfesor == legajo
                        select new CursadaDTO { Id = c.Id, Materia = m.Nombre, Turno = ((ETurno)c.Turno).ToString(), Dia = ((EDias)c.Dia).ToString(), Profesor = p.Apellido, Estado = ((EEstadoCursada)c.Estado).ToString() };

            return query.ToList();
        }

        /// <summary>
        /// PROFESOR
        /// Trae todas los cursos a los que se puede postular
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpGet("Vacantes")]
        public ActionResult<List<CursadaDTO>> GetCursadasVacantesProfesor()
        {
            var query = from a in _context.Cursadas
                        join c in _context.Materias on a.IdMateria equals c.Id
                        where a.Estado == 0
                        select new CursadaDTO { Id = a.Id, Materia = c.Nombre, Turno = ((ETurno)a.Turno).ToString(), Dia = ((EDias)a.Dia).ToString(), Estado = ((EEstadoCursada)a.Estado).ToString() };

            return query.ToList();

        }



        #endregion

        #region POSTULACIONES
        /// <summary>
        /// ADMINISTRADOR
        /// Trae todos los profesores postulados a esa materia
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpGet("postulaciones/{idCursada}")]
        public ActionResult<List<PostulacionDTO>> GetPostulaciones(int idCursada)
        {
            var query = from a in _context.Postulaciones
                        join b in _context.Usuarios on a.LegajoProfesor equals b.Legajo
                        where a.IdCursada == idCursada
                        select new PostulacionDTO { IdCursada = a.IdCursada, Estado = a.Estado, LegajoProfesor = a.LegajoProfesor, Profesor = b.Apellido };
            return query.ToList();
        }

        /// <summary>
        /// PROFESOR
        /// Se postula como profesor a una cursada.
        /// </summary>
        /// <param name="postulacion"></param>
        /// <returns></returns>
        [HttpPost("Postulacion")]
        public ActionResult<Postulacion> AgregarPostulacion(Postulacion postulacion)
        {
            var laCursada = (from a in _context.Cursadas
                             where a.Id == postulacion.IdCursada
                             select a).FirstOrDefault();

            var postulacionesAnteriores = from a in _context.Postulaciones
                                          join b in _context.Cursadas on a.IdCursada equals b.Id
                                          where a.LegajoProfesor == postulacion.LegajoProfesor && a.Estado != 2
                                          select b;


            if (laCursada != null)
            {
                foreach (var item in postulacionesAnteriores)
                {
                    if (item.Anio == laCursada.Anio && item.Cuatrimestre == laCursada.Cuatrimestre && item.Dia == laCursada.Dia && item.Turno == laCursada.Turno)
                    {
                        return BadRequest("Ya tienes una postulacion en el mimso turno");
                    }
                }
            }

            postulacion.Estado = 3;
            var existe = _context.Postulaciones.FirstOrDefault(x => x.IdCursada == postulacion.IdCursada && x.LegajoProfesor == postulacion.LegajoProfesor);
            if (existe != null)
            {
                return BadRequest("Ya esta postulado a este curso");
            }
            var rta = _context.Postulaciones.Add(postulacion);
            _context.SaveChanges();

            return Created("Postulacion", postulacion);
        }

        /// <summary>
        /// ADMINISTRADOR
        /// Asigna un profesor (de una postulacion) a una cursada
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpPut("AsignarProfesor")]
        public ActionResult<Cursada> AsignarProfesor(Postulacion postulacion)
        {
            var cursada = _context.Cursadas.FirstOrDefault(x => x.Id == postulacion.IdCursada);
            if (cursada == null)
            {
                return NotFound();
            }
            cursada.Estado = 1;
            cursada.IdProfesor = postulacion.LegajoProfesor;

            //Marco la postulacion como ganadora y las otras como rechazadas
            var postuGanadora = _context.Postulaciones.Where(x => x.IdCursada == postulacion.IdCursada && x.LegajoProfesor == postulacion.LegajoProfesor).FirstOrDefault();
            var postusNoGanadoras = _context.Postulaciones.Where(x => x.IdCursada == postulacion.IdCursada && x.LegajoProfesor != postulacion.LegajoProfesor).ToList();

            if (postuGanadora != null)
            {
                postuGanadora.Estado = 1;
            }
            if (postusNoGanadoras != null && postusNoGanadoras.Count > 0)
            {
                foreach (var item in postusNoGanadoras)
                {
                    item.Estado = 2;
                }
            }

            _context.SaveChanges();

            return cursada;
        }

        /// <summary>
        /// ADMINISTRADOR
        /// Asigna un profesor (de una postulacion) a una cursada
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpPut("ModificarEstado")]
        public ActionResult<Cursada> ModificarEstado(Postulacion postulacion)
        {
            var cursada = _context.Cursadas.FirstOrDefault(x => x.Id == postulacion.IdCursada);
            if (cursada == null)
            {
                return NotFound();
            }
            cursada.Estado = postulacion.Estado;
            _context.SaveChanges();

            return cursada;
        }

        #endregion

        #region ALUMNO-CURSADA
        /// <summary>
        /// ALUMNO
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
                var laCursada = _context.Cursadas.Find(idCursada);
                if (laCursada == null)
                {
                    return BadRequest("La cursada no existe");
                }

                #region Valido que haya lugar
                int inscriptos = _context.UsuarioCursada.Select(x => x.IdCursada == idCursada).Count();

                if (laCursada.MaxAlumnos <= inscriptos)
                {
                    return BadRequest("Las cursada ya esta completa");
                }
                #endregion

                #region Valido Correlativas
                var querymateriaCorrelativa = from a in _context.Cursadas
                                              join b in _context.Materias on a.IdMateria equals b.Id
                                              join c in _context.Correlativas on a.IdMateria equals c.IdMateria
                                              where a.Id == idCursada
                                              select c.IdMateriaCorrelativa;
                var materiaCorrelativa = querymateriaCorrelativa.FirstOrDefault();

                if (materiaCorrelativa != null)
                {
                    var queryApruebaCorrelativas = from a in _context.Notas
                                                   join b in _context.Cursadas on a.IdCursada equals b.Id
                                                   join c in _context.Materias on b.IdMateria equals c.Id
                                                   where a.LegajoAlumno == legajo && b.IdMateria == materiaCorrelativa && a.TipoNota == (int)ETipoNota.Final
                                                   select a.NotaNumerica;
                    var apruebaCorrelativas = queryApruebaCorrelativas.FirstOrDefault();
                    if (apruebaCorrelativas < 4)
                    {
                        return BadRequest("El alumno no aprueba correlativas.");
                    }
                }
                #endregion

                #region  Valido que el alumno no tenga otras cursadas en ese horario
                var query = from c in _context.Cursadas
                            join uc in _context.UsuarioCursada on c.Id equals uc.IdCursada
                            where uc.LegajoAlumno == legajo && uc.Activa == 1
                            select c;

                var cursadasAlumno = query.ToList();


                foreach (var cursada in cursadasAlumno)
                {
                    if (cursada.Estado == (int)EEstadoCursada.Activa && cursada.Anio == laCursada.Anio && cursada.Cuatrimestre == laCursada.Cuatrimestre && cursada.Dia == laCursada.Dia && cursada.Turno == laCursada.Turno)
                    {
                        return BadRequest("El alumno ya esta inscrito a una cursada en ese horario");
                    }
                }
                #endregion

                //Inscribo al alumno en la cursada
                var user = _context.Usuarios.Find(legajo);
                _context.UsuarioCursada.Add(usuarioCursada);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("El alumno ya esta inscrito a esta materia.");
            }
            catch (Exception ex)
            {
                return BadRequest("Ha ocurrido un error al inscribir al alumno a la cursada");
            }

            return Created("Creada", usuarioCursada);
        }

        /// <summary>
        /// ALUMNO
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

        #endregion

        #region MATERIAL
        /// <summary>
        /// ALUMNO - PROFESOR - ADMINISTRADOR
        /// Trae todos los materiales de una cursada
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
        /// PROFESOR
        /// Agrega un material a una cursada
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        [HttpPost("Material")]
        public ActionResult<Material> AgregarMaterialCursada([FromForm] string titulo, [FromForm] string texto, [FromForm] int tipo, [FromForm] int idCursada, [FromForm] IFormFile? file)
        {

            var material = new Material();

            if (file != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                material.FilePath = Convert.ToBase64String(memoryStream.ToArray());
            }

            material.Titulo = titulo;
            material.IdCursada = idCursada;
            material.Texto = texto;
            material.Tipo = tipo;
            material.FechaPublicacion = DateTime.Now;
            var rta = _context.Materiales.Add(material);
            _context.SaveChanges();

            return Created("Material", material);
        }
        #endregion


        #region NO DEFINIDO
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

        #endregion

        #region NOTAS
        /// <summary>
        /// PROFESOR
        /// Carga las notas de los alumnos de una cursada
        /// </summary>
        /// <param name="notas"></param>
        /// <returns></returns>
        [HttpPost("CargarNotas")]
        public ActionResult<List<Nota>> CargarNotas(List<Nota> notas)
        {
            try
            {
                foreach (var item in notas)
                {
                    item.Fecha = DateTime.Now;
                }
                _context.Notas.AddRange(notas);
                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return notas;
        }

        /// <summary>
        /// PROFESOR
        /// Trae todos las notas de los alumnos de una cursada
        /// </summary>
        /// <param name="idCursada"></param>
        /// <returns></returns>
        [HttpGet("Notas/{idCursada}")]
        public ActionResult<object> GetNotasAlumnosCursada(int idCursada)
        {
            return _context.Notas.Where(x => x.IdCursada == idCursada).ToList();
        }
        #endregion

        #region Asistencias
        [HttpPost("CargarAsistencias")]
        public ActionResult<List<Asistencia>> CargarAsistencias(List<Asistencia> asistencias)

        {
            try
            {
                foreach (var item in asistencias)
                {
                    item.Fecha = DateTime.Now;
                }
                _context.Asistencias.AddRange(asistencias);
                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return asistencias;
        }

        #endregion

    }
}
