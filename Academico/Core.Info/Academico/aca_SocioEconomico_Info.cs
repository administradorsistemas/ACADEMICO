using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Academico
{
    public class aca_SocioEconomico_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int IdSocioEconomico { get; set; }
        [Required(ErrorMessage = "El campo alumno es obligatorio")]
        public decimal IdAlumno { get; set; }
        [Required(ErrorMessage = "El campo vivienda es obligatorio")]
        public int IdCatalogoFichaVi { get; set; }
        [Required(ErrorMessage = "El campo tipo de vivienda es obligatorio")]
        public int IdCatalogoFichaTVi { get; set; }
        [Required(ErrorMessage = "El campo agua es obligatorio")]
        public int IdCatalogoFichaAg { get; set; }
        [Required(ErrorMessage = "El campo energia electrica es obligatorio")]
        public bool TieneElectricidad { get; set; }
        [Required(ErrorMessage = "El campo tiene hermanos es obligatorio")]
        public bool TieneHermanos { get; set; }
        [Required(ErrorMessage = "El campo sueldo del padre es obligatorio")]
        public double SueldoPadre { get; set; }
        [Required(ErrorMessage = "El campo sueldo de la madre es obligatorio")]
        public double SueldoMadre { get; set; }

        [Required(ErrorMessage = "El campo otros ingresos madre es obligatorio")]
        public double OtroIngresoMadre { get; set; }
        [Required(ErrorMessage = "El campo otros ingresos padre es obligatorio")]
        public double OtroIngresoPadre { get; set; }

        [Required(ErrorMessage = "El campo gastos de alimentación es obligatorio")]
        public double GastoAlimentacion { get; set; }
        [Required(ErrorMessage = "El campo gastos de educación es obligatorio")]
        public double GastoEducacion { get; set; }
        [Required(ErrorMessage = "El campo gastos de servicio basico es obligatorio")]
        public double GastoServicioBasico { get; set; }
        [Required(ErrorMessage = "El campo gastos de salud es obligatorio")]
        public double GastoSalud { get; set; }
        [Required(ErrorMessage = "El campo gastos de arriendo es obligatorio")]
        public double GastoArriendo { get; set; }
        [Required(ErrorMessage = "El campo gastos por prestamos es obligatorio")]
        public double GastoPrestamo { get; set; }
        [Required(ErrorMessage = "El campo otros gastos es obligatorio")]
        public double OtroGasto { get; set; }
        [Required(ErrorMessage = "El campo motivo de ingreso es obligatorio")]
        public int IdCatalogoFichaMot { get; set; }
        [Required(ErrorMessage = "El campo como se informó de la institución es obligatorio")]
        public int IdCatalogoFichaIns { get; set; }
        [Required(ErrorMessage = "El campo como financiara los estudios es obligatorio")]
        public int IdCatalogoFichaFin { get; set; }
        [Required(ErrorMessage = "El campo vive con es obligatorio")]
        public int IdCatalogoFichaVive { get; set; }
        public string OtroMotivoIngreso { get; set; }
        public string OtroInformacionInst { get; set; }
        public string OtroFinanciamiento { get; set; }
        public Nullable<int> CantidadHermanos { get; set; }
        public bool Estado { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }

        #region Campos que no existen en la tabla
        public bool SI_HERM { get; set; }
        public bool NO_HERM { get; set; }
        public bool SI_ENERG { get; set; }
        public bool NO_ENERG { get; set; }
        public string Descripcion { get; set; }
        public double TotalIngreso { get; set; }
        public double TotalGasto { get; set; }
        public double Saldo { get; set; }
        public List<aca_Matricula_Info> lst_hermanos { get; set; }
        public int IdAnio { get; set; }

        #endregion
    }
}
