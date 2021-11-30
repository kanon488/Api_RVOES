using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.AreasOpinion
{
    public class CrearOpinionViewModel
    {
        [Required(ErrorMessage = "El campo IdSolicitudArea es requerido.")]
        public int IdSolicitudArea { get; set; }
        [Required(ErrorMessage = "El campo IdEdoOpinion es requerido.")]
        public int IdEdoOpinion { get; set; }
        public string RutaOficioNotificacion { get; set; }
        [Required(ErrorMessage = "El campo UsuarioRegistra es requerido.")]
        public string UsuarioRegistra { get; set; }
        [Required(ErrorMessage = "El Oficio de Notificacion a esta opinión  es requerido")]
        public IFormFile OficioNotificacion { get; set; }
    }
}
