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
    
    public partial class cp_orden_giro_x_in_Ing_Egr_Inven
    {
        public int og_IdEmpresa { get; set; }
        public decimal og_IdCbteCble_Ogiro { get; set; }
        public int og_IdTipoCbte_Ogiro { get; set; }
        public int inv_IdEmpresa { get; set; }
        public int inv_IdSucursal { get; set; }
        public int inv_IdMovi_inven_tipo { get; set; }
        public decimal inv_IdNumMovi { get; set; }
    
        public virtual cp_orden_giro cp_orden_giro { get; set; }
    }
}
