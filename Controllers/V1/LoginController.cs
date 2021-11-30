using API_RVOES.Data;
using API_RVOES.Models.StoredProcedures.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_RVOES.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly RVOERepository _rvoerepository;
        private readonly IConfiguration _configuration;

        public LoginController(RVOERepository rvoerepository, IConfiguration configuration)
        {
            _rvoerepository = rvoerepository ?? throw new ArgumentNullException("No implementado");
            _configuration = configuration;
        }

        /// <summary>
        /// Autentica si la información del usuario es correcta.
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>Regresa un JWT</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Authenticar([FromBody] UserInfo userInfo)
        {
            try
            {

                var userName = userInfo.UserName.ToLower();

                var usuario = await _rvoerepository.getUserByUsername(userName);
                    
                if (usuario == null)
                {
                    return NotFound();
                }
                if (userInfo.Password != usuario.Password)
                {
                    return NotFound();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.CorreoInstitucional),
                    new Claim(ClaimTypes.Role,usuario.IdPerfil.ToString()),
                    new Claim("idusuario",usuario.IdUsuario.ToString()),
                    new Claim("nombrePerfil",usuario.NombrePerfil),
                    new Claim("nombre",usuario.Nombre+" "+usuario.ApellidoPaterno+" "+usuario.ApellidoMaterno)
                };
                var token = GenerarToken(claims);
                return Ok(new { token });
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        private string GenerarToken(List<Claim> claims)
        {
            try
            {

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expiration = DateTime.UtcNow.AddHours(1);

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Issuer"],
                    claims: claims,
                    expires: expiration,
                    signingCredentials: creds
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
