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
        /// Trae todas las carreras y agrupadas por facultades
        /// </summary>
        /// <returns></returns>
        [HttpGet("Carreras")]
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

        /// <summary>
        /// Trae todas las materias
        /// </summary>
        /// <param name="idCarrera"></param>
        /// <returns></returns>
        [HttpGet("Materias")]
        public async Task<ActionResult<IEnumerable<Materia>>> GetMateriasCarrera()
        {
            if (_context.Materias == null)
            {
                return NotFound();
            }

            return _context.Materias.ToList();
        }

        /// <summary>
        /// Trae todas las materias de una carrera
        /// </summary>
        /// <param name="idCarrera"></param>
        /// <returns></returns>
        [HttpGet("Materias/{idCarrera}")]
        public async Task<ActionResult<IEnumerable<MateriasDTO>>> GetMateriasCarrera(int idCarrera)
        {
            if (_context.Materias == null)
            {
                return NotFound();
            }
            var query = from cm in _context.CarrerasMaterias
                        join m in _context.Materias on cm.IdMateria equals m.Id
                        join co in _context.Correlativas on m.Id equals co.IdMateria into com
                        from res in com.DefaultIfEmpty()
                        where cm.IdCarrera == idCarrera
                        select new MateriasDTO { IdMateria = m.Id, Nombre = m.Nombre, Cuatrimestre = cm.Cuatrimestre, Correlativas = res.IdMateriaCorrelativa.GetValueOrDefault(), CargaHoraria = m.CargaHoraria };

            var pepe = query.ToList();

            return query.OrderBy(x => x.Cuatrimestre).ToList();
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
