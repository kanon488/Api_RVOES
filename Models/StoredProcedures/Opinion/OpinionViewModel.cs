using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.AreasOpinion
{
    public class OpinionViewModel
    {
        public int IdOpinion { get; set; }
        public int IdSolicitudArea { get; set; }
        public int IdEdoOpinion { get; set; }
        public string  EdoOpDescripcion { get; set; }
        public string RutaOficioNotificacion { get; set; }
        public string RutaOficioSemsys { get; set; }
        public string FechaRegistro { get; set; }
    }
}
