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
    public class UsuariosController : ControllerBase
    {
        private readonly GestionAcademicaCopiaContext _context;

        public UsuariosController(GestionAcademicaCopiaContext context)
        {
            _context = context;
        }

        #region Administrador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            return await _context.Usuarios.ToListAsync();
        }

        /// <summary>
        /// Dar de alta a un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Usuario>> CrearUsuario(UsuarioDomicilioDTO usuario)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'GestionAcademicaCopiaContext.Usuarios'  is null.");
            }
            try
            {
                usuario.User.Estado = 1;
                usuario.User.Clave = "123123";//usuario.User.Dni;
                usuario.User.FechaRegistro = DateTime.Now;
                usuario.Domicilio.IdPais = 1;

                _context.Domicilios.Add(usuario.Domicilio);
                _context.SaveChanges();

                usuario.User.IdDomicilio = usuario.Domicilio.Id;
                _context.Usuarios.Add(usuario.User);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction("GetUsuario", new { id = usuario.User.Legajo }, usuario.User);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDomicilioDTO>> GetUsuario(int id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            var domicilio = _context.Domicilios.FirstOrDefault(x => x.Id == (int)usuario.IdDomicilio);
            UsuarioDomicilioDTO rta = new UsuarioDomicilioDTO();
            rta.User = usuario;
            rta.Domicilio = domicilio;

            if (usuario == null)
            {
                return NotFound();
            }

            return rta;
        }

        [HttpPut("ModificarEstado")]
        public IActionResult ModificarEstado(Usuario usuario)
        {
            var user = _context.Usuarios.Find(usuario.Legajo);

            if (user == null)
            {
                return BadRequest("No se encontro el usuario");
            }
            try
            {
                if (user.Estado == 0)
                {
                    user.Estado = 1;
                }
                else if (user.Estado == 1)
                {
                    user.Estado = 0;
                }
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return Ok(user);
        }

        [HttpPut("")]
        public IActionResult ModificarEstado(UsuarioDomicilioDTO usuario)
        {
            var user = _context.Usuarios.Find(usuario.User.Legajo);
            var domicilio = _context.Domicilios.Find(usuario.Domicilio.Id);

            if (user == null || domicilio == null)
            {
                return BadRequest("No se encontro el usuario");
            }
            user.Correo = usuario.User.Correo;
            domicilio.Provincia = usuario.Domicilio.Provincia;
            domicilio.Localidad = usuario.Domicilio.Localidad;
            domicilio.CodigoPostal = usuario.Domicilio.CodigoPostal;
            domicilio.Altura = usuario.Domicilio.Altura;
            domicilio.Calle = usuario.Domicilio.Calle;
            domicilio.Piso = usuario.Domicilio.Piso;
            domicilio.Departamento = usuario.Domicilio.Departamento;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return Ok(user);
        }

        #endregion

        [HttpPost("Login")]
        public ActionResult<Usuario> IniciarSesion(Usuario usuario)
        {
            var user = _context.Usuarios.Where(x => x.Correo == usuario.Correo && x.Clave == usuario.Clave).FirstOrDefault();
            if (user == null)
            {
                return Problem("Correo o clave invalida");
            }
            return user;
        }

        /// <summary>
        /// Trae todas las notas de un alumno
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>
        [HttpGet("Notas/{legajo}")]
        public ActionResult<List<NotaDTO>> GetNotasUsuario(int legajo)
        {
            var query = from n in _context.Notas
                        join a in _context.Usuarios on n.LegajoAlumno equals a.Legajo
                        join c in _context.Cursadas on n.IdCursada equals c.Id
                        join m in _context.Materias on c.IdMateria equals m.Id
                        where a.Legajo == legajo && n.TipoNota == (int)ETipoNota.Final
                        select new NotaDTO() { NombreMateria = m.Nombre, Fecha = (DateTime)n.Fecha, NotaNumerica = n.NotaNumerica };

            return query.ToList();
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.Legajo == id)).GetValueOrDefault();
        }
    }
}
