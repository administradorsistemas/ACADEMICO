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
    
    public partial class tb_sis_Impuesto_x_ctacble
    {
        public string IdCod_Impuesto { get; set; }
        public int IdEmpresa_cta { get; set; }
        public string IdCtaCble { get; set; }
        public string observacion { get; set; }
        public string IdCtaCble_vta { get; set; }
    
        public virtual tb_sis_Impuesto tb_sis_Impuesto { get; set; }
    }
}
