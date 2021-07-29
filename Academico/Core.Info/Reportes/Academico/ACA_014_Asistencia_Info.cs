using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Reportes.Academico
{
    public class ACA_014_Asistencia_Info
    {
        public int IdEmpresa { get; set; }
        public decimal IdMatricula { get; set; }
        public Nullable<int> FInjustificadaP1 { get; set; }
        public Nullable<int> FJustificadaP1 { get; set; }
        public Nullable<int> AtrasosP1 { get; set; }
        public Nullable<int> FInjustificadaP2 { get; set; }
        public Nullable<int> FJustificadaP2 { get; set; }
        public Nullable<int> AtrasosP2 { get; set; }
        public Nullable<int> FInjustificadaP3 { get; set; }
        public Nullable<int> FJustificadaP3 { get; set; }
        public Nullable<int> AtrasosP3 { get; set; }
        public Nullable<int> FInjustificadaP4 { get; set; }
        public Nullable<int> FJustificadaP4 { get; set; }
        public Nullable<int> AtrasosP4 { get; set; }
        public Nullable<int> FInjustificadaP5 { get; set; }
        public Nullable<int> FJustificadaP5 { get; set; }
        public Nullable<int> AtrasosP5 { get; set; }
        public Nullable<int> FInjustificadaP6 { get; set; }
        public Nullable<int> FJustificadaP6 { get; set; }
        public Nullable<int> AtrasosP6 { get; set; }
        public string FJustificadaQ1 { get; set; }
        public string FInJustificadaQ1 { get; set; }
        public string AtrasoQ1 { get; set; }
        public string FJustificadaQ2 { get; set; }
        public string FInJustificadaQ2 { get; set; }
        public string AtrasoQ2 { get; set; }
        public string FJustificada { get; set; }
        public string FInJustificada { get; set; }
        public string Atrasos { get; set; }
    }
}
