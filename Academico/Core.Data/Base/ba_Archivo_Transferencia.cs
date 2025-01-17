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
    
    public partial class ba_Archivo_Transferencia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ba_Archivo_Transferencia()
        {
            this.ba_Archivo_Transferencia_Det = new HashSet<ba_Archivo_Transferencia_Det>();
            this.ba_archivo_transferencia_x_ba_tipo_flujo = new HashSet<ba_archivo_transferencia_x_ba_tipo_flujo>();
        }
    
        public int IdEmpresa { get; set; }
        public decimal IdArchivo { get; set; }
        public string cod_archivo { get; set; }
        public int IdBanco { get; set; }
        public int IdProceso_bancario { get; set; }
        public string Cod_Empresa { get; set; }
        public string Nom_Archivo { get; set; }
        public System.DateTime Fecha { get; set; }
        public bool Estado { get; set; }
        public string Observacion { get; set; }
        public string IdUsuario { get; set; }
        public Nullable<System.DateTime> Fecha_Transac { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltAnu { get; set; }
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
        public string Motivo_anulacion { get; set; }
        public string IdUsuarioContabiliza { get; set; }
        public Nullable<System.DateTime> Fecha_Proceso { get; set; }
        public bool Contabilizado { get; set; }
        public int IdSucursal { get; set; }
        public decimal SecuencialInicial { get; set; }
        public Nullable<int> IdTipoCbte { get; set; }
        public Nullable<decimal> IdCbteCble { get; set; }
        public double cb_Valor { get; set; }
    
        public virtual ba_Banco_Cuenta ba_Banco_Cuenta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ba_Archivo_Transferencia_Det> ba_Archivo_Transferencia_Det { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ba_archivo_transferencia_x_ba_tipo_flujo> ba_archivo_transferencia_x_ba_tipo_flujo { get; set; }
    }
}
