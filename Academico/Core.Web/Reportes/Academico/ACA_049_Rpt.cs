﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Bus.Reportes.Academico;
using Core.Info.Reportes.Academico;
using System.Collections.Generic;
using Core.Bus.General;
using Core.Bus.Academico;

namespace Core.Web.Reportes.Academico
{
    public partial class ACA_049_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public ACA_049_Rpt()
        {
            InitializeComponent();
        }

        private void ACA_049_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSede = string.IsNullOrEmpty(p_IdSede.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSede.Value);
            int IdAnio = string.IsNullOrEmpty(p_IdAnio.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAnio.Value);
            int IdNivel = string.IsNullOrEmpty(p_IdNivel.Value.ToString()) ? 0 : Convert.ToInt32(p_IdNivel.Value);
            int IdJornada = string.IsNullOrEmpty(p_IdJornada.Value.ToString()) ? 0 : Convert.ToInt32(p_IdJornada.Value);
            int IdCurso = string.IsNullOrEmpty(p_IdCurso.Value.ToString()) ? 0 : Convert.ToInt32(p_IdCurso.Value);
            int IdParalelo = string.IsNullOrEmpty(p_IdParalelo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdParalelo.Value);
            decimal IdAlumno = string.IsNullOrEmpty(p_IdAlumno.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAlumno.Value);
            bool MostrarRetirados = string.IsNullOrEmpty(p_MostrarRetirados.Value.ToString()) ? false : Convert.ToBoolean(p_MostrarRetirados.Value);
            DateTime Fecha = string.IsNullOrEmpty(p_Fecha.Value.ToString()) ? DateTime.Now : Convert.ToDateTime(p_Fecha.Value);

            ACA_049_Bus bus_rpt = new ACA_049_Bus();
            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            List<ACA_049_Info> lst_rpt = new List<ACA_049_Info>();
            lst_rpt = bus_rpt.GetList(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo, IdAlumno, MostrarRetirados);
            Usuario.Text = "Elaborado por: "+usuario;
            this.DataSource = lst_rpt;
            lst_rpt.ForEach(q => q.FechaActual = Fecha.ToString("d' de 'MMMM' de 'yyyy"));

            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null)
            {
                if (emp.em_logo != null)
                {
                    ImageConverter obj = new ImageConverter();
                    lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
                }
            }

            aca_Sede_Bus bus_sede = new aca_Sede_Bus();
            var sede = bus_sede.GetInfo(IdEmpresa, IdSede);
            if (sede != null)
            {
                Rector.Text = sede.NombreRector;
                Secretaria.Text = sede.NombreSecretaria;
            }

        }

        private void PromedioMaterias_SubRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.Parameters["p_IdEmpresa"].Value = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdAnio"].Value = p_IdAnio.Value == null ? 0 : Convert.ToInt32(p_IdAnio.Value);
            ((XRSubreport)sender).ReportSource.Parameters["p_IdMatricula"].Value = IdMatricula.Value == null ? 0 : Convert.ToDecimal(IdMatricula.Value);
        }
    }
}
