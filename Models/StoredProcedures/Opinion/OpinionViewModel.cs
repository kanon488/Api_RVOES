using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Models.StoredProcedures.AreasOpinion
{
    public class OpinionViewModel
    {
        public int IdSolicitud { get; set; }
        public int NumSolicitud { get; set; }
        public int IdAreaOpinion { get; set; }
        public string AbrevAreaOpinion { get; set; }        
        public int IdEdoAsignado { get; set; }
        public string DescEdoAsignado { get; set; }
        public int IdOpinion { get; set; }
        public int IdEdoOpinion { get; set; }
        public string DescEdoOpinion { get; set; }
        public string FechaAsginacion { get; set; }
        public string FechaLimOpinion { get; set; }
        public string rutaOficioOpinion { get; set; }
        public string IndOficioOpinion { get; set; }
        public string IndOficioSemsys { get; set; }

    }
}
