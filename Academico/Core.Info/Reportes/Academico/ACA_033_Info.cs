﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Reportes.Academico
{
    public class ACA_033_Info
    {
        public int IdEmpresa { get; set; }
        public int IdAnio { get; set; }
        public int IdSede { get; set; }
        public int IdNivel { get; set; }
        public int IdJornada { get; set; }
        public int IdCurso { get; set; }
        public int IdParalelo { get; set; }
        public decimal IdAlumno { get; set; }
        public decimal IdMatricula { get; set; }
        public int IdMateria { get; set; }
        public string NomMateria { get; set; }
        public string NomMateriaArea { get; set; }
        public string NomMateriaGrupo { get; set; }
        public Nullable<bool> EsObligatorio { get; set; }
        public Nullable<int> OrdenMateriaArea { get; set; }
        public Nullable<int> OrdenMateriaGrupo { get; set; }
        public Nullable<int> OrdenMateria { get; set; }
        public string Codigo { get; set; }
        public string pe_nombreCompleto { get; set; }
        public string Descripcion { get; set; }
        public string NomSede { get; set; }
        public string NomNivel { get; set; }
        public Nullable<int> OrdenNivel { get; set; }
        public string NomJornada { get; set; }
        public Nullable<int> OrdenJornada { get; set; }
        public string NomCurso { get; set; }
        public Nullable<int> OrdenCurso { get; set; }
        public string CodigoParalelo { get; set; }
        public string NomParalelo { get; set; }
        public Nullable<int> OrdenParalelo { get; set; }
        public string NombreProfesor { get; set; }
        public Nullable<decimal> CalificacionP1 { get; set; }
        public string EquivalenciaPromedioP1 { get; set; }
        public Nullable<decimal> CalificacionP2 { get; set; }
        public string EquivalenciaPromedioP2 { get; set; }
        public Nullable<decimal> CalificacionP3 { get; set; }
        public string EquivalenciaPromedioP3 { get; set; }
        public Nullable<decimal> PromedioQ1 { get; set; }
        public Nullable<decimal> ExamenQ1 { get; set; }
        public string EquivalenciaPromedioEQ1 { get; set; }
        public Nullable<decimal> PromedioFinalQ1 { get; set; }
        public string EquivalenciaPromedioQ1 { get; set; }
        public Nullable<decimal> CalificacionP4 { get; set; }
        public string EquivalenciaPromedioP4 { get; set; }
        public Nullable<decimal> CalificacionP5 { get; set; }
        public string EquivalenciaPromedioP5 { get; set; }
        public Nullable<decimal> CalificacionP6 { get; set; }
        public string EquivalenciaPromedioP6 { get; set; }
        public Nullable<decimal> PromedioQ2 { get; set; }
        public Nullable<decimal> ExamenQ2 { get; set; }
        public string EquivalenciaPromedioEQ2 { get; set; }
        public Nullable<decimal> PromedioFinalQ2 { get; set; }
        public string EquivalenciaPromedioQ2 { get; set; }
        public Nullable<decimal> PromedioQuimestres_PF { get; set; }
        public Nullable<decimal> Promedio_PR { get; set; }
        public Nullable<decimal> ExamenMejoramiento { get; set; }
        public string CampoMejoramiento { get; set; }
        public Nullable<decimal> ExamenSupletorio { get; set; }
        public Nullable<decimal> ExamenRemedial { get; set; }
        public Nullable<decimal> ExamenGracia { get; set; }
        public Nullable<decimal> PromedioFinal { get; set; }
        public Nullable<int> IdEquivalenciaPromedioPF { get; set; }
        public string EquivalenciaPromedioPF { get; set; }
    }
}
