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
    
    public partial class tb_region
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_region()
        {
            this.tb_provincia = new HashSet<tb_provincia>();
        }
    
        public string Cod_Region { get; set; }
        public string Nom_region { get; set; }
        public string codigo { get; set; }
        public bool estado { get; set; }
        public string IdPais { get; set; }
    
        public virtual tb_pais tb_pais { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_provincia> tb_provincia { get; set; }
    }
}