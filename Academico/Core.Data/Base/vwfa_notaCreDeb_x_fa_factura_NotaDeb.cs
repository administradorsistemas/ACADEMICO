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
    
    public partial class vwfa_notaCreDeb_x_fa_factura_NotaDeb
    {
        public int IdEmpresa_nt { get; set; }
        public int IdSucursal_nt { get; set; }
        public int IdBodega_nt { get; set; }
        public decimal IdNota_nt { get; set; }
        public int secuencia { get; set; }
        public string vt_tipoDoc { get; set; }
        public int IdEmpresa_fac_nd_doc_mod { get; set; }
        public int IdSucursal_fac_nd_doc_mod { get; set; }
        public int IdBodega_fac_nd_doc_mod { get; set; }
        public decimal IdCbteVta_fac_nd_doc_mod { get; set; }
        public double Valor_Aplicado { get; set; }
        public string vt_NumFactura { get; set; }
        public string vt_Observacion { get; set; }
        public string CodDoc { get; set; }
        public Nullable<double> vt_Subtotal { get; set; }
        public Nullable<double> vt_iva { get; set; }
        public Nullable<double> vt_total { get; set; }
        public double ValorCobrado { get; set; }
        public double saldo { get; set; }
        public Nullable<double> saldo_sin_cobro { get; set; }
        public Nullable<System.DateTime> vt_fecha { get; set; }
        public Nullable<System.DateTime> vt_fech_venc { get; set; }
        public Nullable<decimal> IdCliente { get; set; }
        public string pe_nombreCompleto { get; set; }
        public System.DateTime fecha_cruce { get; set; }
        public Nullable<double> ValorProntoPago { get; set; }
    }
}
