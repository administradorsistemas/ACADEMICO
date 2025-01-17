﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Bus.Reportes.Academico;
using Core.Info.Reportes.Academico;
using System.Collections.Generic;
using Core.Bus.Academico;
using Core.Bus.General;

namespace Core.Web.Reportes.Academico
{
    public partial class ACA_049_Detalle_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        
        ACA_049_Detalle_Bus bus_rpt = new ACA_049_Detalle_Bus();
        public ACA_049_Detalle_Rpt()
        {
            InitializeComponent();
        }

        private void ACA_049_Detalle_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
                int IdAnio = string.IsNullOrEmpty(p_IdAnio.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAnio.Value);
                decimal IdMatricula = string.IsNullOrEmpty(p_IdMatricula.Value.ToString()) ? 0 : Convert.ToInt32(p_IdMatricula.Value);
                List<ACA_049_Detalle_Info> Lista = bus_rpt.GetList(IdEmpresa, IdAnio, IdMatricula);
                this.DataSource = Lista;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void Promedio_SubRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.Parameters["p_IdEmpresa"].Value = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdAnio"].Value = p_IdAnio.Value == null ? 0 : Convert.ToInt32(p_IdAnio.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdMatricula"].Value = p_IdMatricula.Value == null ? 0 : Convert.ToDecimal(p_IdMatricula.Value);
        }
    }
}
