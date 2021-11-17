using Core.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Academico
{
    public class aca_Matricula_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public decimal IdMatricula { get; set; }
        [Required(ErrorMessage = "El campo código es obligatorio")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "El campo alumno es obligatorio")]
        public decimal IdAlumno { get; set; }
        [Required(ErrorMessage = "El campo año lectivo es obligatorio")]
        public int IdAnio { get; set; }
        public int IdSede { get; set; }
        public int IdNivel { get; set; }
        public int IdJornada { get; set; }
        public int IdCurso { get; set; }
        [Required(ErrorMessage = "El campo curso es obligatorio")]
        public int IdParalelo { get; set; }
        public decimal IdPersonaF { get; set; }
        public decimal IdPersonaR { get; set; }
        [Required(ErrorMessage = "El campo plantilla es obligatorio")]
        public int IdPlantilla { get; set; }
        [Required(ErrorMessage = "El campo mecanismo de pago es obligatorio")]
        public decimal IdMecanismo { get; set; }
        [Required(ErrorMessage = "El campo fecha es obligatorio")]
        public System.DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string IdUsuarioAnulacion { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        public string MotivoAnulacion { get; set; }
        public Nullable<int> IdEmpresa_rol { get; set; }
        public Nullable<decimal> IdEmpleado { get; set; }
        public Nullable<int> IdCatalogoESTMAT { get; set; }
        public Nullable<bool> EsPatrocinado { get; set; }
        public Nullable<decimal> IdPreMatricula { get; set; }



        #region Campos que no existen en la tabla
        public string IdComboCurso { get; set; }
        public List<aca_Matricula_Info> lst_matricula_curso { get; set; }
        public List<aca_AnioLectivo_Curso_Documento_Info> lst_alumno_documentos { get; set; }
        public List<aca_AlumnoDocumento_Info> lst_documentos { get; set; }
        public string pe_cedulaRuc { get; set; }
        public string pe_nombreCompleto { get; set; }
        public string NomSede { get; set; }
        public string NomNivel { get; set; }
        public string NomJornada { get; set; }
        public string NomParalelo { get; set; }
        public string Descripcion { get; set; }
        public string NomCurso { get; set; }
        public string Validar { get; set; }
        public double ValorPlantilla { get; set; }
        public double ValorPlantillaProntoPago { get; set; }
        public List<aca_Matricula_Rubro_Info> lst_MatriculaRubro { get; set; }
        public bool BloquearMatricula { get; set; }
        [Required(ErrorMessage = "El campo mecanismo de pago del rubro es obligatorio")]
        public decimal IdMecanismoDet { get; set; }
        [Required(ErrorMessage = "El campo forma de pago del rubro es obligatorio")]
        public int IdPuntoVta { get; set; }
        public string IdCatalogo_FormaPago { get; set; }
        public int IdSucursal { get; set; }
        public string vt_serie1 { get; set; }
        public string vt_serie2 { get; set; }
        public string vt_NumFactura { get; set; }

        public string ObservacionCambio { get; set; }
        public string NomCatalogoESTMAT { get; set; }
        public string NomCatalogoESTALU { get; set; }
        public bool PaseAnio { get; set; }

        public bool EsRetirado { get; set; }
        public string EsRetiradoString { get; set; }
        public List<aca_MatriculaCalificacionParcial_Info> lst_calificacion_parcial { get; set; }
        public List<aca_MatriculaCalificacion_Info> lst_calificacion { get; set; }
        public List<aca_MatriculaConducta_Info> lst_conducta { get; set; }

        public List<aca_MatriculaCalificacionParticipacion_Info> lst_participacion { get; set; }
        public aca_MatriculaCambios_Info info_MatriculaCambios { get; set; }
        public Nullable<decimal> PromedioFinal { get; set; }
        public int OrdenJornada { get; set; }
        public int OrdenNivel { get; set; }
        public int OrdenCurso { get; set; }
        public int OrdenParalelo { get; set; }
        public string NomPlantilla { get; set; }

        public List<aca_MatriculaCalificacionParcial_Info> lst_MatriculaCalificacionParcial { get; set; }
        public List<aca_MatriculaCalificacion_Info> lst_MatriculaCalificacion { get; set; }
        public List<aca_MatriculaCalificacionCualitativa_Info> lst_MatriculaCalificacionCualitativa { get; set; }
        public List<aca_MatriculaCalificacionCualitativaPromedio_Info> lst_MatriculaCalificacionCualitativaPromedio { get; set; }
        public List<aca_MatriculaGrado_Info> lst_MatriculaGrado { get; set; }
        public List<aca_MatriculaAsistencia_Info> lst_asistencia { get; set; }
        public string ObservacionCambioPlantilla { get; set; }
        public string ObservacionCambioCurso { get; set; }

        public bool AplicaDescuentoNomina { get; set; }


        public decimal IdAdmision { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal ValorPago { get; set; }
        public string CodigoAlumno { get; set; }
        public bool ValidaImportacionPreMatricula { get; set; }
        public string NomPlantillaTipo { get; set; }
        public int IdSocioEconomico { get; set; }
        public int CantEstudiantes { get; set; }
        #endregion
    }
}
