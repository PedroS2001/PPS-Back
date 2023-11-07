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
    public class UsuariosController : ControllerBase
    {
        private readonly GestionAcademicaCopiaContext _context;

        public UsuariosController(GestionAcademicaCopiaContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }



        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'GestionAcademicaCopiaContext.Usuarios'  is null.");
            }
            _context.Usuarios.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsuarioExists(usuario.Legajo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUsuario", new { id = usuario.Legajo }, usuario);
        }

        [HttpPost("Login")]
        public ActionResult<Usuario> IniciarSesion(string correo, string clave)
        {
            var user = _context.Usuarios.Where(x => x.Correo == correo && x.Clave == clave).FirstOrDefault();
            if(user == null)
            {
                return Problem("Correo o clave invalida");
            }
            return user;
        }

        [HttpGet("Cursadas/{legajo}")]
        public ActionResult<List<Materia>> GetCursadasActivasAlumno(int legajo)
        {
            var query = from c in _context.Cursadas
                        join um in _context.UsuarioCursada on c.Id equals um.IdCursada
                        join m in _context.Materias on c.IdMateria equals m.Id
                        where um.IdCursada == legajo && c.Activa == 1
                        select new Materia() { Nombre = m.Nombre, Id = m.Id };

            return query.ToList();
        }

        [HttpGet("Notas/{legajo}")]
        public ActionResult<List<Nota>> ObtenerNotasUsuario(int legajo)
        {
            var query = from n in _context.Notas
                        join a in _context.Usuarios on n.LegajoAlumno equals a.Legajo
                        join c in _context.Cursadas on n.IdCursada equals c.Id
                        join m in _context.Materias on c.IdMateria equals m.Id
                        where a.Legajo == legajo
                        select new Nota() { LegajoAlumno = a.Legajo, NotaNumerica = n.NotaNumerica, Fecha = n.Fecha, TipoNota = m.Nombre, IdCursada= c.Id };


            return query.ToList();
        }






        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.Legajo == id)).GetValueOrDefault();
        }
    }
}
