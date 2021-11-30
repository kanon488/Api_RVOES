using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.Solicitudes
{
    public class EditarAsignarAreaViewModel
    {
        [Required]
        public int idSolArea { get; set; }
        [Required(ErrorMessage = "El campo Id Solicitud es requerido")]
        public int idsolicitud { get; set; }
        public string numsolicitud { get; set; }
        [Required(ErrorMessage = "El campo Id Area es requerido")]
        public int idarea { get; set; }
        public string rutaOficioAsignacion { get; set; }
        [Required(ErrorMessage = "El campo Id Estatus asignado es requerido")]
        public int idestatusasignado { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Asignación es requerido")]
        public string fechaAsignacion { get; set; }
        [Required(ErrorMessage = "El campo Fecha Limite es requerido")]
        public string fechalimiteopinion { get; set; }
        [Required(ErrorMessage = "El Oficio asignado a esta solicitud  es requerido")]
        public IFormFile OficioAsignado { get; set; }
    }
}
