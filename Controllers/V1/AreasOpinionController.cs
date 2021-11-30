using API_RVOES.AppCode.Services;
using API_RVOES.Data;
using API_RVOES.Models.StoredProcedures.AreasOpinion;
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
    public class AreasOpinionController : ControllerBase
    {
        private readonly RVOERepository _rvoerepository;

        public AreasOpinionController(RVOERepository rvoerepository)
        {
            _rvoerepository = rvoerepository;
        }

        /// <summary>
        /// Selecciona todas las áreas de opinión.
        /// </summary>
        /// <returns></returns>
        // GET: api/AreasOpinion/Select
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<SelectViewModel>>> Select()
        {
            try
            {
                var areasOpinion = await _rvoerepository.GetAreasOpinion();

                return Ok(areasOpinion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Regresa todas las opiniones registradas por las áreas de opinión
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<OpinionViewModel>>> Listar()
        {
            try
            {
                var opiniones = await  _rvoerepository.GetAllOpiniones();
                if (opiniones == null) 
                {
                    return NotFound();
                }

                return Ok(opiniones);

            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        /// <summary>
        /// Regresa todas las opiniones registradas que tiene una solicitud asignada a un área de opinión.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]/{IdSolicitudArea}")]
        public async Task<ActionResult<IEnumerable<OpinionViewModel>>> ListarPorSolicitudAsignada(int IdSolicitudArea) 
        {
            try
            {
                var opiniones = await _rvoerepository.GetOpinionesBySolAsignada(IdSolicitudArea);
                if (opiniones == null)
                {
                    return NotFound();
                }

                return Ok(opiniones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
