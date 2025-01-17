﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Reportes.Facturacion
{
    public class FAC_004_Info
    {
        public int IdEmpresa { get; set; }
        public string Su_CodigoEstablecimiento { get; set; }
        public string Su_Descripcion { get; set; }
        public string Su_Direccion { get; set; }
        public string Su_Telefonos { get; set; }
        public int IdSucursal { get; set; }
        public decimal IdCliente { get; set; }
        public string nombre_cliente { get; set; }
        public string ced_ruc_cliente { get; set; }
        public string direccion_cliente { get; set; }
        public string celular_cliente { get; set; }
        public string telefono_cliente { get; set; }
        public Nullable<decimal> IdProforma { get; set; }
        public int Secuencia { get; set; }
        public string nom_TerminoPago { get; set; }
        public decimal vt_plazo { get; set; }
        public string CodCbteVta { get; set; }
        public System.DateTime vt_fecha { get; set; }
        public string Estado { get; set; }
        public string Codigo { get; set; }
        public string Ve_Vendedor { get; set; }
        public double vt_cantidad { get; set; }
        public double vt_Precio { get; set; }
        public double vt_PorDescUnitario { get; set; }
        public double vt_DescUnitario { get; set; }
        public double vt_PrecioFinal { get; set; }
        public double vt_Subtotal { get; set; }
        public double vt_por_iva { get; set; }
        public double vt_iva { get; set; }
        public double pd_total { get; set; }
        public string pr_observacion { get; set; }
        public decimal IdProducto { get; set; }
        public string vt_Observacion { get; set; }
        public int IdBodega { get; set; }
        public decimal IdCbteVta { get; set; }
        public string pr_descripcion { get; set; }
        public string vt_NumFactura { get; set; }
        public Nullable<decimal> SubtotalIVASinDscto { get; set; }
        public Nullable<decimal> SubtotalSinIVASinDscto { get; set; }
        public Nullable<decimal> SubtotalSinDscto { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> SubtotalIVAConDscto { get; set; }
        public Nullable<decimal> SubtotalSinIVAConDscto { get; set; }
        public Nullable<decimal> SubtotalConDscto { get; set; }
        public Nullable<decimal> ValorIVA { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> ValorEfectivo { get; set; }
        public Nullable<decimal> Cambio { get; set; }
    }
}
