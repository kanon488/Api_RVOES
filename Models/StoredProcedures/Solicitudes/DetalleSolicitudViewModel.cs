using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.Solicitudes
{
    public class DetalleSolicitudViewModel
    {
        public string IdSolicitud { get; set; }
        public string fechaSolicitud { get; set; }
        public string NumSolicitud { get; set; }
        public string TipoPersona { get; set; }
        public string Solicitante { get; set; }
        public string RepresentanteLegal { get; set; }
        public string TipoEducativo { get; set; }
        public string NivelEducativo { get; set; }
        public string Modalidad { get; set; }
        public string PlanDeEstudios { get; set; }
        public string Turno { get; set; }
        public string DatosdelInmueble { get; set; }
        public string Localidad { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string PersonasAuthorizadas { get; set; }
        public string DomicilioRecibieNotificaciones { get; set; }
        public string OpcionEducativa { get; set; }
    }
}
