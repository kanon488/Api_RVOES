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
    public class OpinionesController : ControllerBase
    {
        private readonly RVOERepository _rvoerepository;
        private readonly IArchivoService _archivoService;
        public OpinionesController(RVOERepository rvoerepository, IArchivoService archivoService)
        {
            _rvoerepository = rvoerepository ?? throw new ArgumentNullException("No implementado");
            _archivoService = archivoService;
        }

        /// <summary>
        /// Lista todas las opiniones que tienen registradas una solicitud.
        /// </summary>
        /// <returns></returns>
        //GET: api/Opiniones/Listar
        [HttpGet("[action]/{idSolAsignada}")]
        public async Task<ActionResult<List<OpinionViewModel>>> Listar(int idSolAsignada)
        {
            try
            {
                var opiniones = await _rvoerepository.GetOpinionesBySolAsignada(idSolAsignada);
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

        /// <summary>
        /// Registra una opinión para una solicitud asignada a un area de opinión.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> Crear([FromForm] CrearOpinionViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var SolicitudAsignada = await _rvoerepository.GetSolicitudAsignadaById(model.IdSolicitudArea);
                var EdoOpinion = await _rvoerepository.GetEdoOpinionById(model.IdEdoOpinion);
                var sFolderPath = SolicitudAsignada.numsolicitud;

                string ExtensionArchivo = System.IO.Path.GetExtension(model.OficioNotificacion.FileName);
                var sNombreArchivo = "OficioNotificacion_" + SolicitudAsignada.AbrevArea + "_" + SolicitudAsignada.idestatusasignado + "_" + EdoOpinion.Abrev;

                var sFinalDestinationPath = _archivoService.AlmacenarArchivo(model.OficioNotificacion, sFolderPath, sNombreArchivo, ExtensionArchivo);
                model.RutaOficioNotificacion = sFinalDestinationPath;

                string sIdOp = await _rvoerepository.InsertarOpinion(model);

                if (String.IsNullOrEmpty(sIdOp))
                {
                    return BadRequest("No se pudo guardar la información.");
                }

                return Ok(sIdOp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza la información de una opinión registrada.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<ActionResult> Actualizar([FromForm] ActualizarOpinionViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var SolicitudAsignada = await _rvoerepository.GetSolicitudAsignadaById(model.IdSolicitudArea);
                var EdoOpinion = await _rvoerepository.GetEdoOpinionById(model.IdEdoOpinion);
                var sFolderPath = SolicitudAsignada.numsolicitud;

                string ExtensionArchivo = System.IO.Path.GetExtension(model.OficioNotificacion.FileName);
                var sNombreArchivo = "OficioNotificacion_" + SolicitudAsignada.AbrevArea + "_" + SolicitudAsignada.idestatusasignado + "_" + EdoOpinion.Abrev;

                var sFinalDestinationPath = _archivoService.AlmacenarArchivo(model.OficioNotificacion, sFolderPath, sNombreArchivo, ExtensionArchivo);
                model.RutaOficioNotificacion = sFinalDestinationPath;

                string actualizado = await _rvoerepository.UpdateOpinionRegistrada(model);

                if (String.IsNullOrEmpty(actualizado) || Int32.Parse(actualizado) <= 0)
                {
                    return BadRequest("No se pudo actualizar la información.");
                }
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
