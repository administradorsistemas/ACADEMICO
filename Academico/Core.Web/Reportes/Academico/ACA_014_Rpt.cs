using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Bus.Reportes.Academico;
using System.Collections.Generic;
using Core.Info.Reportes.Academico;
using Core.Bus.General;
using System.Linq;
using Core.Info.Helps;

namespace Core.Web.Reportes.Academico
{
    public partial class ACA_014_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public ACA_014_Rpt()
        {
            InitializeComponent();
        }

        private void ACA_014_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_usuario.Text = usuario;

            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdAnio = string.IsNullOrEmpty(p_IdAnio.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAnio.Value);
            int IdSede = string.IsNullOrEmpty(p_IdSede.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSede.Value);
            int IdNivel = string.IsNullOrEmpty(p_IdNivel.Value.ToString()) ? 0 : Convert.ToInt32(p_IdNivel.Value);
            int IdJornada = string.IsNullOrEmpty(p_IdJornada.Value.ToString()) ? 0 : Convert.ToInt32(p_IdJornada.Value);
            int IdCurso = string.IsNullOrEmpty(p_IdCurso.Value.ToString()) ? 0 : Convert.ToInt32(p_IdCurso.Value);
            int IdParalelo = string.IsNullOrEmpty(p_IdParalelo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdParalelo.Value);
            int IdCatalogoParcial = string.IsNullOrEmpty(p_IdCatalogoParcial.Value.ToString()) ? 0 : Convert.ToInt32(p_IdCatalogoParcial.Value);
            int IdAlumno = string.IsNullOrEmpty(p_IdAlumno.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAlumno.Value);
            bool MostrarRetirados = string.IsNullOrEmpty(p_MostrarRetirados.Value.ToString()) ? false : Convert.ToBoolean(p_MostrarRetirados.Value);
            bool MostrarPromedios = string.IsNullOrEmpty(p_MostrarPromedios.Value.ToString()) ? false : Convert.ToBoolean(p_MostrarPromedios.Value);

            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null)
            {
                if (emp.em_logo != null)
                {
                    ImageConverter obj = new ImageConverter();
                    lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
                }
            }

            ACA_014_Bus bus_rpt = new ACA_014_Bus();
            List<ACA_014_Info> lst_rpt = new List<ACA_014_Info>();
            lst_rpt = bus_rpt.GetList(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo, IdCatalogoParcial, IdAlumno, MostrarRetirados, MostrarPromedios);
            this.DataSource = lst_rpt;

        }

        private void xrSubreport2_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.Parameters["p_IdEmpresa"].Value = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdMatricula"].Value = IdMatricula.Value == null ? 0 : Convert.ToInt32(IdMatricula.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdCatalogoParcial"].Value = p_IdCatalogoParcial.Value == null ? 0 : Convert.ToInt32(p_IdCatalogoParcial.Value);

            ((XRSubreport)sender).ReportSource.RequestParameters = false;
        }

        private void xrSubreport1_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.Parameters["p_IdEmpresa"].Value = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdAnio"].Value = p_IdAnio.Value == null ? 0 : Convert.ToInt32(p_IdAnio.Value);
            
            ((XRSubreport)sender).ReportSource.RequestParameters = false;
        }

        private void Subreport_Conducta_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.Parameters["p_IdEmpresa"].Value = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdAnio"].Value = p_IdAnio.Value == null ? 0 : Convert.ToInt32(p_IdAnio.Value);
            ((XRSubreport)sender).ReportSource.RequestParameters = false;
        }

        private void Cualitativa_Subreporte_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.Parameters["p_IdEmpresa"].Value = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdAnio"].Value = p_IdAnio.Value == null ? 0 : Convert.ToInt32(p_IdAnio.Value);
            ((XRSubreport)sender).ReportSource.RequestParameters = false;
        }

        private void Asistencia_Subreporte_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.Parameters["p_IdEmpresa"].Value = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdMatricula"].Value = IdMatricula.Value == null ? 0 : Convert.ToInt32(IdMatricula.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdCatalogoParcial"].Value = p_IdCatalogoParcial.Value == null ? 0 : Convert.ToInt32(p_IdCatalogoParcial.Value);

            ((XRSubreport)sender).ReportSource.RequestParameters = false;
        }
    }
}
