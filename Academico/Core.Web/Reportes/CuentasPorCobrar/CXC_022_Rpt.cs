using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Linq;
using Core.Bus.Reportes.CuentasPorCobrar;
using Core.Info.Reportes.CuentasPorCobrar;

namespace Core.Web.Reportes.CuentasPorCobrar
{
    public partial class CXC_022_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        CXC_022_Bus bus_rpt = new CXC_022_Bus();
        public CXC_022_Rpt()
        {
            InitializeComponent();
        }

        private void CXC_022_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;

            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            string IdUsuario = string.IsNullOrEmpty(p_IdUsuario.Value.ToString()) ? null : Convert.ToString(p_IdUsuario.Value);
            DateTime FechaCorte = string.IsNullOrEmpty(p_FechaCorte.Value.ToString()) ? DateTime.Now.Date : Convert.ToDateTime(p_FechaCorte.Value);
            List<CXC_022_Info> Lista = new List<CXC_022_Info>();
            Lista = bus_rpt.GetList(IdEmpresa, FechaCorte);
            this.DataSource = Lista;
        }
    }
}




