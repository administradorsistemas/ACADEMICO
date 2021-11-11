using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Academico
{
    public class aca_AnioLectivoCalificacionParticipacionHistorico_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int IdAnio { get; set; }
        public decimal IdAlumno { get; set; }
        public int IdSede { get; set; }
        public int IdNivel { get; set; }
        public int IdJornada { get; set; }
        public int IdCurso { get; set; }
        public int IdTematica { get; set; }
        public int IdCampoAccion { get; set; }
        public Nullable<decimal> PromedioFinal { get; set; }
        public bool RegistroValido { get; set; }
        public string pe_nombreCompleto { get; set; }
    }
}
