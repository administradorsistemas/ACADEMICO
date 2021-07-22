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
    public partial class CXC_004_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        CXC_004_Bus bus_rpt = new CXC_004_Bus();
        public List<CXC_004_Info> Lista { get; set; }
        public CXC_004_Rpt()
        {
            InitializeComponent();
        }

        private void CXC_004_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;
            
            this.DataSource = Lista;
        }
    }
}




