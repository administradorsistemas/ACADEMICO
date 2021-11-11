using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Reportes.Academico
{
    public class ACA_075_Info
    {
        public int IdEmpresa { get; set; }
        public decimal IdMatricula { get; set; }
        public int IdAnio { get; set; }
        public int IdSede { get; set; }
        public int IdNivel { get; set; }
        public int IdJornada { get; set; }
        public int IdCurso { get; set; }
        public int IdParalelo { get; set; }
        public decimal IdAlumno { get; set; }
        public string Codigo { get; set; }
        public string pe_nombreCompleto { get; set; }
        public string pe_cedulaRuc { get; set; }
        public string Descripcion { get; set; }
        public string NomSede { get; set; }
        public string NomNivel { get; set; }
        public int OrdenNivel { get; set; }
        public string NomJornada { get; set; }
        public int OrdenJornada { get; set; }
        public string NomCurso { get; set; }
        public int OrdenCurso { get; set; }  
        public string NomParalelo { get; set; }
        public int OrdenParalelo { get; set; }
        public string AnioCal { get; set; }
        public string NivelCal { get; set; }
        public int OrdenNivelCal { get; set; }
        public string CursoCal { get; set; }
        public int OrdenCursoCal { get; set; }
        public Nullable<decimal> Promedio { get; set; }


        public int Secuencial { get; set; }
        public int CalificacionNull { get; set; }
        public Nullable<decimal> SumaGeneral { get; set; }
        public Nullable<decimal> PromedioCalculado { get; set; }
        public Nullable<decimal> PromedioFinalCalculado { get; set; }
        public string PromedioString { get; set; }
        public int NumCalificaciones { get; set; }
        public Nullable<decimal> Prom1 { get; set; }
        public Nullable<decimal> Prom2 { get; set; }
        public Nullable<decimal> Prom3 { get; set; }
    }
}
