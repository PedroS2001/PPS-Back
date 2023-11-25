﻿using System;
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
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioDomicilioDTO usuario)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'GestionAcademicaCopiaContext.Usuarios'  is null.");
            }
            try
            {
                _context.Domicilios.Add(usuario.Domicilio);
                _context.SaveChanges();
                _context.Usuarios.Add(usuario.User);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }

            return CreatedAtAction("GetUsuario", new { id = usuario.User.Legajo }, usuario.User);
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
        #endregion

        [HttpPost("Login")]
        public ActionResult<Usuario> IniciarSesion(string correo, string clave)
        {
            var user = _context.Usuarios.Where(x => x.Correo == correo && x.Clave == clave).FirstOrDefault();
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
        public ActionResult<List<Nota>> GetNotasUsuario(int legajo)
        {
            var query = from n in _context.Notas
                        join a in _context.Usuarios on n.LegajoAlumno equals a.Legajo
                        join c in _context.Cursadas on n.IdCursada equals c.Id
                        join m in _context.Materias on c.IdMateria equals m.Id
                        where a.Legajo == legajo
                        select new Nota() { LegajoAlumno = a.Legajo, NotaNumerica = n.NotaNumerica, Fecha = n.Fecha, TipoNota = m.Nombre, IdCursada = c.Id };

            return query.ToList();
        }






        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.Legajo == id)).GetValueOrDefault();
        }
    }
}
