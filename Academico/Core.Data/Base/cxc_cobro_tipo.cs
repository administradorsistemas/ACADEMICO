//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Data.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class cxc_cobro_tipo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cxc_cobro_tipo()
        {
            this.cxc_cobro = new HashSet<cxc_cobro>();
            this.cxc_cobro_det = new HashSet<cxc_cobro_det>();
            this.cxc_cobro_tipo_Param_conta_x_sucursal = new HashSet<cxc_cobro_tipo_Param_conta_x_sucursal>();
        }
    
        public string IdCobro_tipo { get; set; }
        public string tc_descripcion { get; set; }
        public string Estado { get; set; }
        public string tc_abreviatura { get; set; }
        public string tc_Tomar_Cta_Cble_De { get; set; }
        public string ESRetenIVA { get; set; }
        public string ESRetenFTE { get; set; }
        public double PorcentajeRet { get; set; }
        public string IdUsuario { get; set; }
        public Nullable<System.DateTime> Fecha_Transac { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltAnu { get; set; }
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
        public string IdMotivo_tipo_cobro { get; set; }
        public bool EsTarjetaCredito { get; set; }
        public bool SeDeposita { get; set; }
        public double PorcentajeDescuento { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cxc_cobro> cxc_cobro { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cxc_cobro_det> cxc_cobro_det { get; set; }
        public virtual cxc_cobro_tipo_motivo cxc_cobro_tipo_motivo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cxc_cobro_tipo_Param_conta_x_sucursal> cxc_cobro_tipo_Param_conta_x_sucursal { get; set; }
    }
}