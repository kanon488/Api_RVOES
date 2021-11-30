using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.Usuarios
{
    public class ActualizarViewModel
    {
        [Required]
        public int idUsuario { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre del usuario debe tener minimo 3 letras y máximo 20.")]
        public string Nombre { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El apellido paterno del usuario debe tener minimo 3 letras y máximo 20.")]
        public string ApellidoPaterno { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El apellido materno del usuario debe tener minimo 3 letras y máximo 20.")]
        public string ApellidoMaterno { get; set; }
        [Required]
        [EmailAddress]
        public string CorreoInstitucional { get; set; }
        [Required]
        [EmailAddress]
        public string CorreoAlterno { get; set; }
        [StringLength(10, ErrorMessage = "El número de contacto del usuario debe tener minimo 10 digitos.")]
        public string TelefonoContacto { get; set; }
        [Required]
        public int Perfil { get; set; }
        public int AreaAsignada { get; set; }

    }
}
