using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.Solicitudes
{
    public class SolicitudesViewModel
    {
        public int IdSolicitud { get; set; }
        public string NumSolicitud { get; set; }
        public string IdSolicitante { get; set; }
        public string NombreInstEducativa { get; set; }
        public string Estatus { get; set; }
        public string FechaSolicitud { get; set; }
        public int Nivel { get; set; }
        public string NivelDescripcion { get; set; }
        public string PlanyPrograma { get; set; }
    }
}
