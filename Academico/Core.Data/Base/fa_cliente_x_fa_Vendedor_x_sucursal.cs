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
    
    public partial class fa_cliente_x_fa_Vendedor_x_sucursal
    {
        public int IdEmpresa { get; set; }
        public decimal IdCliente { get; set; }
        public int IdSucursal { get; set; }
        public int IdVendedor { get; set; }
        public string observacion { get; set; }
    
        public virtual fa_cliente fa_cliente { get; set; }
        public virtual fa_Vendedor fa_Vendedor { get; set; }
    }
}
