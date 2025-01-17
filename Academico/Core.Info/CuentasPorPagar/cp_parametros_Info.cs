﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.CuentasPorPagar
{
   public class cp_parametros_Info
    {
        public int IdEmpresa { get; set; }
        public Nullable<int> pa_TipoCbte_OG { get; set; }
        public string pa_ctacble_deudora { get; set; }
        public string pa_ctacble_iva { get; set; }
        public string pa_ctacble_Proveedores_default { get; set; }
        public Nullable<int> pa_IdTipoCbte_x_Retencion { get; set; }
        public Nullable<int> IdTipoMoviCaja { get; set; }
        public Nullable<int> pa_TipoEgrMoviCaja_Conciliacion { get; set; }
        public string IdUsuario { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> FechaUltMod { get; set; }
        public Nullable<int> pa_TipoCbte_NC { get; set; }
        public Nullable<int> pa_TipoCbte_ND { get; set; }
        public Nullable<int> pa_TipoCbte_para_conci_x_antcipo { get; set; }
        public string pa_ctacble_x_RetIva_default { get; set; }
        public string pa_ctacble_x_RetFte_default { get; set; }
        public int DiasTransaccionesAFuturo { get; set; }
        public Nullable<int> DiasTransaccionesAPasado { get; set; }
    }
}
