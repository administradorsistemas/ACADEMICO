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
    
    public partial class tb_sis_Documento_Tipo_x_Empresa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_sis_Documento_Tipo_x_Empresa()
        {
            this.tb_sis_Documento_Tipo_Talonario = new HashSet<tb_sis_Documento_Tipo_Talonario>();
        }
    
        public int IdEmpresa { get; set; }
        public string codDocumentoTipo { get; set; }
        public string ApareceComboFac_TipoFact { get; set; }
        public string ApareceComboFac_Import { get; set; }
        public string ApareceTalonario { get; set; }
        public string Descripcion { get; set; }
        public int Posicion { get; set; }
        public string ApareceCombo_FileReporte { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_sis_Documento_Tipo_Talonario> tb_sis_Documento_Tipo_Talonario { get; set; }
        public virtual tb_empresa tb_empresa { get; set; }
    }
}
