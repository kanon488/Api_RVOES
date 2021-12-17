using API_RVOES.AppCode.Services;
using API_RVOES.Data;
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
    public class CorreosController : ControllerBase
    {
        private readonly RVOERepository _rvoerepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public CorreosController(RVOERepository rvoerepository, IConfiguration configuration, IEmailService emailService)
        {
            _rvoerepository = rvoerepository ?? throw new ArgumentNullException("No implementado");
            _configuration = configuration;
            _emailService = emailService;
        }

        /// <summary>
        /// Envia el correo con el usuairo de la contraseña al usuario.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> EnviarCorreoAcceso([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }
                var usuario = await _rvoerepository.GetUsuario(id);
                if (usuario == null)
                {
                    return NotFound();
                }
                string sNombreCompleto = usuario.Nombre + " " + usuario.ApellidoPaterno + " " + usuario.ApellidoMaterno;
                string sCuentaOrigen = _configuration["EmailData:Account"];
                string sDestino = usuario.CorreoInstitucional;
                string sSubject = _configuration["EmailData:SubjectNewUser"];
                string sUrlRvoe = _configuration["UrlAccesoSistema"];
                string sCuerpoMensaje = _emailService.getBodyNuevoUsuario(usuario.ClaveUsuario, usuario.Password, sUrlRvoe, sNombreCompleto);
                _emailService.Send(sCuentaOrigen, sDestino, sSubject, sCuerpoMensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        public async Task<IActionResult> NotifRptCargadaParticular([FromRoute] int idOpinion) 
        {
            try
            {
                if (idOpinion <= 0)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
