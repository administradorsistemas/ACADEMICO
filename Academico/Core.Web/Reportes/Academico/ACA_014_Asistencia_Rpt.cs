using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Info.Reportes.Academico;
using Core.Bus.Reportes.Academico;
using System.Collections.Generic;

namespace Core.Web.Reportes.Academico
{
    public partial class ACA_014_Asistencia_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ACA_014_Asistencia_Rpt()
        {
            InitializeComponent();
        }

        private void ACA_014_Conducta_Rpt_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdMatricula = string.IsNullOrEmpty(p_IdMatricula.Value.ToString()) ? 0 : Convert.ToInt32(p_IdMatricula.Value);
            int IdCatalogoParcial = string.IsNullOrEmpty(p_IdCatalogoParcial.Value.ToString()) ? 0 : Convert.ToInt32(p_IdCatalogoParcial.Value);

            ACA_014_Asistencia_Bus bus_rpt = new ACA_014_Asistencia_Bus();
            List<ACA_014_Asistencia_Info> lst_rpt = new List<ACA_014_Asistencia_Info>();
            lst_rpt = bus_rpt.GetList(IdEmpresa, IdMatricula, IdCatalogoParcial);

            this.DataSource = lst_rpt;
        }
    }
}
