using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Reportes.Academico
{
    public class ACA_014_Info
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
        public Nullable<bool> PromediarGrupo { get; set; }
        public Nullable<int> OrdenMateria { get; set; }
        public Nullable<int> IdCatalogoTipoCalificacion { get; set; }
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
        public string NombreInspector { get; set; }
        public string NombreRepresentante { get; set; }
        public string CalificacionP1 { get; set; }
        public string EquivalenciaPromedioP1 { get; set; }
        public string CalificacionP2 { get; set; }
        public string EquivalenciaPromedioP2 { get; set; }
        public string CalificacionP3 { get; set; }
        public string EquivalenciaPromedioP3 { get; set; }
        public string PorcentajePromedioQ1 { get; set; }
        public string PorcentajeExamenQ1 { get; set; }
        public string ExamenQ1 { get; set; }
        public string EquivalenciaPromedioEQ1 { get; set; }
        public string PromedioFinalQ1 { get; set; }
        public string EquivalenciaPromedioQ1 { get; set; }
        public string CalificacionP4 { get; set; }
        public string EquivalenciaPromedioP4 { get; set; }
        public string CalificacionP5 { get; set; }
        public string EquivalenciaPromedioP5 { get; set; }
        public string CalificacionP6 { get; set; }
        public string EquivalenciaPromedioP6 { get; set; }
        public string PorcentajePromedioQ2 { get; set; }
        public string PorcentajeExamenQ2 { get; set; }
        public string ExamenQ2 { get; set; }
        public string EquivalenciaPromedioEQ2 { get; set; }
        public string PromedioFinalQ2 { get; set; }
        public string EquivalenciaPromedioQ2 { get; set; }
        public string PromedioQuimestres_PF { get; set; }
        public string Promedio_PR { get; set; }
        public string ExamenMejoramiento { get; set; }
        public string CampoMejoramiento { get; set; }
        public string ExamenSupletorio { get; set; }
        public string ExamenRemedial { get; set; }
        public string ExamenGracia { get; set; }
        public string PromedioFinal { get; set; }
        public Nullable<int> IdEquivalenciaPromedioPF { get; set; }
        public string EquivalenciaPromedioPF { get; set; }

        #region Campos que no existen en la tabla
        public decimal? PromedioFinalQ1Double { get; set; }
        public decimal? PromedioFinalQ2Double { get; set; }
        public decimal? PromedioFinalDouble { get; set; }
        public decimal? PromedioQuimestresDouble { get; set; }
        public decimal? PromedioGrupoQ1Double { get; set; }
        public decimal? PromedioGrupoQ2Double { get; set; }
        public decimal? PromedioFinalGrupoDouble { get; set; }
        public decimal? PromedioQuimestresGrupoDouble { get; set; }
        public decimal? PFQ1 { get; set; }
        public decimal? PFQ2 { get; set; }
        public decimal? PFQuim { get; set; }
        public decimal? PF { get; set; }
        public int NoMostrarPromedioQ1 { get; set; }
        public int NoMostrarPromedioQ2 { get; set; }
        public int NoMostrarPromedioQuim { get; set; }
        public int NoMostrarPromedioFinal { get; set; }
        public int NoMostrarPF { get; set; }
        #endregion
    }
}
