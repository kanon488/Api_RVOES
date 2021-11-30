using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.Usuarios
{
    public class UsuarioViewModel
    {
        public int IdUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string CorreoInstitucional { get; set; }
        public string TelefonoContacto { get; set; }
        public string Password { get; set; }
        public int Activo { get; set; }
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public string AreaAsignada { get; set; }
    }
}
