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
using Newtonsoft.Json.Linq;

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


        /// <summary>
        /// Trae todas las novedades
        /// </summary>
        /// <returns></returns>
        [HttpGet("Novedades")]
        public async Task<ActionResult<IEnumerable<Novedad>>> GetNovedades()
        {
            if (_context.Novedades == null)
            {
                return NotFound();
            }

            return _context.Novedades.ToList();

        }

        /// <summary>
        /// Trae todas las novedades
        /// </summary>
        /// <returns></returns>
        [HttpGet("Novedad/{idNovedad}")]
        public async Task<ActionResult<Novedad>> GetNovedad(int idNovedad)
        {
            if (_context.Novedades == null)
            {
                return NotFound();
            }

            return _context.Novedades.Where(x => x.Id == idNovedad).FirstOrDefault();
        }


    }
}
