using API_RVOES.Data;
using API_RVOES.Models.StoredProcedures.EstadoAsignacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosAsignacionController : ControllerBase
    {
        private readonly RVOERepository _rvoerepository;

        public EstadosAsignacionController(RVOERepository rvoerepository)
        {
            _rvoerepository = rvoerepository;
        }
        

        /// <summary>
        /// Selecciona toda la información del catalogo de Asignación de Opinión.
        /// </summary>
        /// <returns></returns>
        // GET: api/EstadoOpinion/Select
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<EdoSelectViewModel>>> Select()
        {
            try
            {
                var edoAsignacion = await _rvoerepository.GetEstadosAsignacion();

                return Ok(edoAsignacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Selecciona el edo de Asignación que a los cuales tiene permiso el usuario.
        /// </summary>
        /// <param name="idPerfil"></param>
        /// <returns></returns>
        // GET: api/EstadoAsignacion/Select
        [HttpGet("[action]/{idPerfil}")]
        public async Task<ActionResult<IEnumerable<EdoSelectViewModel>>> Select([FromRoute] int idPerfil)
        {
            try
            {
                List<EdoSelectViewModel> edoAsignacion;

                edoAsignacion = await _rvoerepository.getEdoByPerfil(idPerfil);

                return Ok(edoAsignacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           
        }

        /// <summary>
        /// Selecciona el edo de Asignación que a los cuales tiene permiso el usuario.
        /// </summary>
        /// <param name="idPerfil"></param>
        /// <returns></returns>
        // GET: api/EstadoAsignacion/Select
        [HttpGet("[action]/{idPerfil}")]
        public async Task<ActionResult<IEnumerable<EdoSelectViewModel>>> SelectById([FromRoute] int idPerfil)
        {
            try
            {
                List<EdoSelectViewModel> edoAsignacion;

                edoAsignacion = await _rvoerepository.getEdoByPerfil(idPerfil);

                return Ok(edoAsignacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
