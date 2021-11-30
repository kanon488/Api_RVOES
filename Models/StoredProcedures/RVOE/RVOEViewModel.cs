using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.RVOE
{
    public class RVOEViewModel
    {
        public string NumRVOE { get; set; }
        public string ClaveCT { get; set; }
        public string NombreInstitucion { get; set; }
        public string IdSolicitante { get; set; }
        public string NombreSolicitante { get; set; }
        public string Programa { get; set; }
        public string Modalidad { get; set; }
        public string CorreoInstitucion { get; set; }
        public string CorreoAlterno { get; set; }
    }
}
