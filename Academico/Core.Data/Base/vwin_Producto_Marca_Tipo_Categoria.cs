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
    
    public partial class vwin_Producto_Marca_Tipo_Categoria
    {
        public int IdEmpresa { get; set; }
        public decimal IdProducto { get; set; }
        public string pr_codigo { get; set; }
        public string pr_codigo_barra { get; set; }
        public int IdProductoTipo { get; set; }
        public string IdPresentacion { get; set; }
        public string IdCategoria { get; set; }
        public string pr_descripcion { get; set; }
        public string pr_observacion { get; set; }
        public string IdUnidadMedida { get; set; }
        public int pr_precio_publico { get; set; }
        public int pr_stock { get; set; }
        public int pr_pedidos { get; set; }
        public int IdMarca { get; set; }
        public int pr_stock_minimo { get; set; }
        public string IdUsuario { get; set; }
        public Nullable<System.DateTime> Fecha_Transac { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltAnu { get; set; }
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
        public string pr_motivoAnulacion { get; set; }
        public string nom_pc { get; set; }
        public string ip { get; set; }
        public string Estado { get; set; }
        public string pr_descripcion_2 { get; set; }
        public int IdLinea { get; set; }
        public int IdGrupo { get; set; }
        public int IdSubGrupo { get; set; }
        public string Descripcion { get; set; }
        public string ca_Categoria { get; set; }
        public string tp_descripcion { get; set; }
        public string IdUnidadMedida_Consumo { get; set; }
    }
}