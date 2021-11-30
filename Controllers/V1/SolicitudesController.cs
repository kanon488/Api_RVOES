using API_RVOES.AppCode.Services;
using API_RVOES.Data;
using API_RVOES.Models.StoredProcedures.Solicitudes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesController : ControllerBase
    {
        private readonly RVOERepository _rvoerepository;
        private readonly IConfiguration _configuration;
        private readonly IArchivoService _archivoService;

        public SolicitudesController(RVOERepository rvoerepository, IConfiguration configuration, IArchivoService archivoService)
        {
            _rvoerepository = rvoerepository ?? throw new ArgumentNullException("No implementado");
            _configuration = configuration;
            _archivoService = archivoService;
        }
        /// <summary>
        /// Lista todas las solicitudes que sean de nivel superior o superior del año 2020.
        /// </summary>
        /// <returns></returns>
        //GET: api/Solicitudes/Listar
        [HttpGet("[action]")]
        public async Task<ActionResult<List<SolicitudesViewModel>>> Listar()
        {
            try
            {
                var solicitudes = await  _rvoerepository.GetAllSolicitudes();
                if (solicitudes == null) 
                {
                    return NotFound();
                }
                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Muestra la informacion especifica de una solicitud.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Solicitudes/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<SolicitudViewModel>> Mostrar(int id) 
        {
            try
            {
                var solicitud = await _rvoerepository.GetSolicitud(id);
                if (solicitud == null)
                {
                    return NotFound();
                }
                return Ok(solicitud);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Lista todas las solicitudes por nivel educativo.
        /// </summary>
        /// <param name="nivel"></param>
        /// <returns></returns>
        // GET: api/Solicitudes/ListarPorNivel/6
        [HttpGet("[action]/{nivel}")]
        public async Task<ActionResult<List<SolicitudesViewModel>>> ListarPorNivel(int nivel)
        {
            try
            {
                var solicitudes = await _rvoerepository.GetSolicitudesNivel(nivel);
                if (solicitudes == null)
                {
                    return NotFound();
                }

                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Muestra el detalle de una solicitud.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Solicitudes/DetalleSolicitud/425
        [HttpGet("[action]/{idsolicitud}")]
        public async Task<ActionResult<DetalleSolicitudViewModel>> DetalleSolicitud(int idsolicitud)
        {
            try
            {
                var detalle = await _rvoerepository.ObtDetalleSolicitud(idsolicitud);
                if (detalle == null)
                {
                    return NotFound();
                }
                return Ok(detalle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Lista todas las solicitudes asignadas a un area.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<SolicitudAsignadaViewModel>>> ListarAllAsignadas()
        {
            try
            {
                var solicitudesAsignadas = await _rvoerepository.GetAllSolicitudesAsignadas();

                if (solicitudesAsignadas == null) 
                {
                    return NotFound();
                }

                return Ok(solicitudesAsignadas);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Muestra las areas asignadas  de una solicitud.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Solicitudes/ListarAreasAsignadas/425
        [HttpGet("[action]/{idsolicitud}")]
        public async Task<ActionResult<IEnumerable<SolicitudAsignadaViewModel>>> ListarAreasAsignadas(int idsolicitud) 
        {
            try
            {
                var AreasAsignadas = await _rvoerepository.GetSolicituAreasAsignadas(idsolicitud);
                if(AreasAsignadas == null)
                {
                    return NotFound();
                }

                return Ok(AreasAsignadas);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        /// <summary>
        /// Lista todas las solicitudes que tiene una area asignada.
        /// </summary>
        /// <param name="idarea"></param>
        /// <returns></returns>
        /// 
        //[Authorize(Roles = "AdminRVOE,RespRVOE,RespDIEP,CIVE,DEN,DGB,DGEU,DIEP,DJ")]
        [HttpGet("[action]/{idarea}")]
        public async Task<ActionResult<IEnumerable<SolicitudAsignadaViewModel>>> ListarxArea(int idarea)
        {
            try
            {
                var solicitudesAsignadas = await _rvoerepository.GetSolicitudesAsignadasByArea(idarea);

                if (solicitudesAsignadas == null)
                {
                    return NotFound();
                }

                return Ok(solicitudesAsignadas);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Asigna una solicitud a un area especifica.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[Authorize(Roles = "AdminRVOE,RespRVOE,RespDIEP")]
        [Consumes("multipart/form-data")]
        [HttpPost("[action]")]
        public async Task<ActionResult> AsignaraArea([FromForm] AsignarAreaViewModel model)
        {
            try
            {
                if (model.OficioAsignado == null)
                {
                    return BadRequest("Debe seleccionar un archivo para el oficio de asignación.");
                }
                else if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else if (System.IO.Path.GetExtension(model.OficioAsignado.FileName) != ".pdf")
                {
                    return BadRequest();
                }
                else
                {
                    var sAreaAsigada = await _rvoerepository.GetAreaOpinionById(model.idarea);
                    var sEdoAsignado = await _rvoerepository.GetEstatusasignadoById(model.idestatusasignado);
                    var Solicitud = await _rvoerepository.GetSolicitud(model.idsolicitud);
                    model.numsolicitud = Solicitud.NumSolicitud;
                    string sFolderPath = model.numsolicitud;
                    string ExtensionArchivo = System.IO.Path.GetExtension(model.OficioAsignado.FileName);
                    var sNombreArchivo = "OficioAsignado_" + sAreaAsigada + "_" + sEdoAsignado;

                    var sFinalDestinationPath = _archivoService.AlmacenarArchivo(model.OficioAsignado, sFolderPath, sNombreArchivo, ExtensionArchivo);
                    model.rutaOficioAsignacion = sFinalDestinationPath;
                    string sIdSolArea = await _rvoerepository.AsignarSolicitudArea(model);

                    if (String.IsNullOrEmpty(sIdSolArea))
                    {
                        return BadRequest("No se pudo guardar el registro.");
                    }

                    return Ok(sIdSolArea);
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Modifica una la informácion de una solicitud asignada a un área.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[Authorize(Roles = "AdminRVOE,RespRVOE,RespDIEP")]
        [Consumes("multipart/form-data")]
        [HttpPut("[action]")]
        public async Task<ActionResult> EditarAsignacionArea([FromForm] EditarAsignarAreaViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else if (System.IO.Path.GetExtension(model.OficioAsignado.FileName) != ".pdf")
                {
                    return BadRequest();
                }
                else
                {
                    //var solicitudAsignada = await _rvoerepository.GetSolicitudAsignadaById(model.idSolArea);

                    var sFinalDestinationPath = "";
                    if (model.OficioAsignado != null)
                    {
                        var sAreaAsigada = await _rvoerepository.GetAreaOpinionById(model.idarea);
                        var sEdoAsignado = await _rvoerepository.GetEstatusasignadoById(model.idestatusasignado);
                        var Solicitud = await _rvoerepository.GetSolicitud(model.idsolicitud);
                        model.numsolicitud = Solicitud.NumSolicitud;
                        string sFolderPath = model.numsolicitud;
                        string ExtensionArchivo = System.IO.Path.GetExtension(model.OficioAsignado.FileName);
                        var sNombreArchivo = "OficioAsignado_" + sAreaAsigada + "_" + sEdoAsignado;
                        sFinalDestinationPath = _archivoService.AlmacenarArchivo(model.OficioAsignado, sFolderPath, sNombreArchivo, ExtensionArchivo);
                        model.rutaOficioAsignacion = sFinalDestinationPath;
                    }

                    if (model.OficioAsignado != null && String.IsNullOrEmpty(sFinalDestinationPath))
                    {
                        return BadRequest("No se pudo subir el archivo seleccionado");
                    }
                    else
                    {
                        string actualizado = await _rvoerepository.EditarSolicitudAreaAsignada(model);

                        if (String.IsNullOrEmpty(actualizado) || Int32.Parse(actualizado) <= 0)
                        {
                            return BadRequest("No se pudo actualizar la información.");
                        }
                    }


                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Calcula los días que se tiene como límite para emitir una opinión.
        /// </summary>
        /// <param name="sFechaAsignacion"></param>
        /// <returns></returns>
        //[Authorize(Roles = "AdminRVOE,RespRVOE,RespDIEP")]
        [HttpGet("[action]/{sFechaAsignacion}")]
        public async Task<ActionResult> CalcularFechaOpinion([FromRoute] string sFechaAsignacion)
        {
            try
            {
                //DateTime FechaAsignacion = DateTime.Parse(sFechaAsignacion);
                var sFechaLimite = await _rvoerepository.ObtSigDiaHabil(sFechaAsignacion);
                return Ok(sFechaLimite);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Descarga el oficio adjunto a la asignación de una solicitud a un área.
        /// </summary>
        /// <param name="idSolicitudArea"></param>
        /// <returns></returns>
        //[Authorize(Roles = "AdminRVOE,RespRVOE,RespDIEP,CIVE,DEN,DGB,DGEU,DIEP,DJ")]
        [HttpGet("[action]/{IdSolicitudArea}")]
        public async Task<ActionResult> DescargaOficioAsignacion([FromRoute] int IdSolicitudArea)
        {
            try
            {
                if (IdSolicitudArea == 0)
                {
                    return BadRequest();
                }

                var solicitudAsignada = await _rvoerepository.GetSolicitudAsignadaById(IdSolicitudArea);

                //var NombreArchivo = solicitudAsignada.idsolicitud.ToString() + "_" + solicitudAsignada.AbrevArea + "_" + solicitudAsignada.descripcionEstatus;
                var NombreArchivo = solicitudAsignada.numsolicitud + "_" + solicitudAsignada.AbrevArea + "_" + solicitudAsignada.idestatusasignado.ToString();
                var ms = new MemoryStream();
                ms = _archivoService.RecuperarArchivo(solicitudAsignada.rutaOficioAsignacion);
                return File(ms, "application/pdf", NombreArchivo + ".pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Elimina de manera logica el registro de una solicitud asignada a área y todas las opiniones registradas correspondientes.
        /// </summary>
        /// <param name="IdSolicitudArea"></param>
        /// <returns></returns>
        [HttpPut("[action]/{IdSolicitudArea}")]
        public async Task<ActionResult> EliminarSolicitudAsignada([FromRoute] int IdSolicitudArea) 
        {
            try
            {
                if (IdSolicitudArea <= 0 )
                {
                    return NotFound();
                }
                string actualizado = await _rvoerepository.EliminarAsignacionSolicitud(IdSolicitudArea);
                if (String.IsNullOrEmpty(actualizado) || Int32.Parse(actualizado) <= 0)
                {
                    return BadRequest("No se pudo realizar la operación.");
                }
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
