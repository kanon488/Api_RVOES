using API_RVOES.AppCode.Services;
using API_RVOES.Data;
using API_RVOES.Models.StoredProcedures.AreasOpinion;
using API_RVOES.Models.StoredProcedures.Opinion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        //[HttpGet("[action]/{idSolAsignada}")]
        //public async Task<ActionResult<List<OpinionViewModel>>> Listar(int idSolAsignada)
        //{
        //    try
        //    {
        //        var opiniones = await _rvoerepository.GetOpinionesBySolAsignada(idSolAsignada);
        //        if (opiniones == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(opiniones);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        /// <summary>
        /// Lista todas las opiniones que tienen registradas una solicitud.
        /// </summary>
        /// <returns></returns>
        //GET: api/Opiniones/Listar
        [HttpGet("[action]/{idSolicitud}")]
        public async Task<ActionResult<List<OpinionViewModel>>> Listar(int idSolicitud)
        {
            try
            {
                var opiniones = await _rvoerepository.GetOpinionesByIdSolicitud(idSolicitud);
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
                var sNombreArchivo = "OficioOpinion_" + SolicitudAsignada.AbrevArea + "_" + SolicitudAsignada.idestatusasignado + "_" + EdoOpinion.Abrev;

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
                var sNombreArchivo = "OficioOpinion_" + SolicitudAsignada.AbrevArea + "_" + SolicitudAsignada.idestatusasignado + "_" + EdoOpinion.Abrev;

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

        /// <summary>
        /// Descarga el oficio adjunto a la asignación de una solicitud a un área.
        /// </summary>
        /// <param name="idSolicitudArea"></param>
        /// <returns></returns>
        //[Authorize(Roles = "AdminRVOE,RespRVOE,RespDIEP,CIVE,DEN,DGB,DGEU,DIEP,DJ")]
        [HttpGet("[action]/{IdOpinion}")]
        public async Task<ActionResult> DescargaOficioOpinion([FromRoute] int IdOpinion)
        {
            try
            {
                if (IdOpinion == 0)
                {
                    return BadRequest();
                }

                var opinionSolicitud = await _rvoerepository.GetInfoOpinionById(IdOpinion);

                var NombreArchivo = opinionSolicitud.NumSolicitud + "_" + opinionSolicitud.AbrevAreaOpinion + "_" + opinionSolicitud.DescEdoAsignado.ToString()+"_"+ opinionSolicitud.DescEdoOpinion;
                var sRutaOficioOpinion = await _rvoerepository.GetOficioOpinionById(IdOpinion);
                var ms = new MemoryStream();
                ms = _archivoService.RecuperarArchivo(sRutaOficioOpinion);
                return File(ms, "application/pdf", NombreArchivo + ".pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        ///Carga el archivo de respuesta Semsys de una Opinion y guarda su ruta en BD.
        /// </summary>
        /// <param name="oficioSemsys"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<ActionResult> CargarArchivoSemsys([FromForm] rptaOpSemsysViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

               

                string ExtensionArchivo = System.IO.Path.GetExtension(model.OficioRptaSemsys.FileName);
                var sNombreArchivo = "OficioRptaSemsys_" + model.abrevAreaOpinion + "_" + model.idEdoAsignado + "_" + model.descEdoOpinion;

                var sFinalDestinationPath = _archivoService.AlmacenarArchivo(model.OficioRptaSemsys, model.NumSolicitud.ToString(), sNombreArchivo, ExtensionArchivo);
                string sRutaDestino = sFinalDestinationPath;

                string actualizado = await _rvoerepository.InsertOficioRptaSemsys(model.idOpinion,sRutaDestino);

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
