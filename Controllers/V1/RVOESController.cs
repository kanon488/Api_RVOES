using API_RVOES.Data;
using API_RVOES.Models.StoredProcedures.RVOE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class RVOESController : ControllerBase
    {
        private readonly RVOERepository _rvoerepository;
        private readonly IConfiguration _configuration;

        public RVOESController(RVOERepository rvoerepository, IConfiguration configuration)
        {
            _rvoerepository = rvoerepository ?? throw new ArgumentNullException("No implementado");
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todos las RVOES.
        /// </summary>
        /// <returns></returns>
        //GET: api/Solicitudes/Listar
        [HttpGet("[action]/{IdNivel}/{IdMunicipio}")]
        public async Task<ActionResult<List<RVOEViewModel>>> Listar([FromRoute] int IdNivel,int IdMunicipio)
        {
            try
            {
                var rvoesOtorgados = await _rvoerepository.GetRVOESByNivelMunicipio(IdNivel, IdMunicipio);
                if (rvoesOtorgados == null)
                {
                    return NotFound();
                }
                return Ok(rvoesOtorgados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
