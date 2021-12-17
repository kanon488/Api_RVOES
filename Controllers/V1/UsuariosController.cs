using API_RVOES.AppCode.Generales;
using API_RVOES.Data;
using API_RVOES.Models.StoredProcedures.Usuarios;
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
    public class UsuariosController : ControllerBase
    {
        private readonly RVOERepository _rvoerepository;

        public UsuariosController(RVOERepository rvoerepository)
        {
            _rvoerepository = rvoerepository ?? throw new ArgumentNullException("No implementado");
        }

        /// <summary>
        /// Lista todos los usuarios registrados.
        /// </summary>
        /// <returns></returns>
        //GET: api/Usuarios/Listar
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<UsuarioViewModel>>> Listar()
        {
            try
            {
                var listaUsuarios = await _rvoerepository.GetAllUsers();

                return Ok(listaUsuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<UsuarioViewModel>> Mostrar([FromRoute] int id) 
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

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Crea un Usuario dentro del sistema.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int ExisteUsuario = await _rvoerepository.ExisteUsuario(model.ClaveUsuario);

                if (ExisteUsuario > 0)
                {
                    return BadRequest("El usuario "+model.ClaveUsuario +" ya se encuentra previamente registrado.");
                }

                string password = GeneratePassword();

                string sIdUsuario = await _rvoerepository.CreateUsuario(model, password);

                if (String.IsNullOrEmpty(sIdUsuario))
                {
                    return BadRequest("No se pudo crear el Usuario.");
                }

               
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        /// <summary>
        /// Actualiza la información de un usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string actualizado = await _rvoerepository.UpdateUsuario(model);

                if (String.IsNullOrEmpty(actualizado) || Int32.Parse(actualizado) <= 0)
                {
                    return BadRequest("No se pudo actualizar la información del Usuario.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        /// <summary>
        /// Cambia el indicador de Activo a Inactivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var actualizado = await _rvoerepository.DesactivaUsuario(id);

                if (String.IsNullOrEmpty(actualizado) || Int32.Parse(actualizado) <= 0)
                {
                    return BadRequest("No se pudo actualizar la información.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        /// <summary>
        /// Cambia el indicador de Inactivo a Activo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }
                var actualizado = await _rvoerepository.ActivaUsuario(id);

                if (String.IsNullOrEmpty(actualizado) || Int32.Parse(actualizado) <= 0)
                {
                    return BadRequest("No se pudo actualizar la información.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        private string GeneratePassword()
        {
            bool lowercase = true;
            bool uppercase = true;
            bool digit = true;
            bool special = false;
            int length = 10;

            clsPassword oPassword = new clsPassword();
            string sPassword = oPassword.GenerarPassword(lowercase, uppercase, digit, special, length);

            return sPassword;
        }

    }
}
