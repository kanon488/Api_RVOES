using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.Solicitudes
{
    public class SolicitudAsignadaViewModel
    {
        public int idsolicitud { get; set; }
        public int idsolicitudArea { get; set; }
        public string numsolicitud { get; set; }
        public int idarea { get; set; }
        public string AbrevArea { get; set; }
        public string rutaOficioAsignacion { get; set; }
        public int idestatusasignado { get; set; }
        public string descripcionEstatus { get; set; }
        public string fechaasignacion { get; set; }
        public string fechalimiteopinion { get; set; }
        public string IndOpinionReg { get; set; }
    }
}
