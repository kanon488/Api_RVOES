using API_RVOES.Data;
using API_RVOES.Models.StoredProcedures.EstadoOpinion;
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
    public class EstadosOpinionController : ControllerBase
    {
        private readonly RVOERepository _rvoerepository;

        public EstadosOpinionController(RVOERepository rvoerepository)
        {
            _rvoerepository = rvoerepository;
        }

        /// <summary>
        /// Selecciona todas las áreas de opinión.
        /// </summary>
        /// <returns></returns>
        // GET: api/AreasOpinion/Select
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<EdoOpinionSelectViewModel>>> Select()
        {
            try
            {
                var estadosDeOpinion = await _rvoerepository.GetEstadosDeOpinion();

                return Ok(estadosDeOpinion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
