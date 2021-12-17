using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.Opinion
{
    public class rptaOpSemsysViewModel
    {
        [Required(ErrorMessage = "La propiedad idOpinion es requerida")]
        public int idOpinion { get; set; }
        [Required(ErrorMessage = "La propiedad número solicitud es requerida")]
        public int NumSolicitud { get; set; }
        [Required(ErrorMessage = "La propiedad abrevAreaOpinion es requerida")]
        public string abrevAreaOpinion { get; set; }
        [Required(ErrorMessage = "La propiedad idEdoAsignado es requerida")]
        public int idEdoAsignado { get; set; }
        [Required(ErrorMessage = "La propiedad descEdoOpinion es requerida")]
        public string descEdoOpinion { get; set; }
        [Required(ErrorMessage = "El Oficio de Respuesta Semsys es requerido")]
        public IFormFile OficioRptaSemsys { get; set; }
        
    }
}
