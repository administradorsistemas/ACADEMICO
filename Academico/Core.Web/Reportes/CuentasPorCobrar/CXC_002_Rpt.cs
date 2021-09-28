using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Info.Reportes.CuentasPorCobrar;
using Core.Bus.Reportes.CuentasPorCobrar;
using System.Collections.Generic;
using Core.Bus.General;
using Core.Bus.CuentasPorCobrar;
using System.Linq;
using Core.Info.General;

namespace Core.Web.Reportes.CuentasPorCobrar
{
    public partial class CXC_002_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        
        cxc_cobro_Bus busCobro = new cxc_cobro_Bus();
        public tb_empresa_Info infoEmpresa { get; set; }
        public List<CXC_002_Info> lst_rpt { get; set; }
        public List<CXC_002_Aplicaciones_Info> lst_det { get; set; }
        public CXC_002_Info Primero { get; set; }
        public string Saldo { get; set; }
        public string SaldoConDscto { get; set; }
        public int MyProperty { get; set; }
        public CXC_002_Rpt()
        {
            InitializeComponent();
        }

        private void CXC_002_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_usuario.Text = usuario;

            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            decimal IdCobro = string.IsNullOrEmpty(p_IdCobro.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdCobro.Value);

            CXC_002_Bus bus_rpt = new CXC_002_Bus();
             
            this.DataSource = lst_rpt;
            
            if (infoEmpresa != null)
            {
                lbl_empresa.Text = infoEmpresa.em_nombre;
                lblDireccion.Text = infoEmpresa.em_direccion;
                if (infoEmpresa.em_logo != null)
                {
                    ImageConverter obj = new ImageConverter();
                    logo.Image = (Image)obj.ConvertFrom(infoEmpresa.em_logo);
                }
            }
            lblSaldo.Text = Saldo;
            lblSaldoConDscto.Text = SaldoConDscto;
            if (Primero != null)
            {
                string Cadena = lblReemplaza.Text;
                Cadena =  Cadena.Replace("{0}", Primero.CedulaCliente).Replace("{1}", Primero.NomCliente);
                lblReemplaza.Text = Cadena;
            }
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.DataSource = lst_det;
        }
    }
}
