﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Bus.Reportes.CuentasPorCobrar;
using System.Collections.Generic;
using Core.Info.Reportes.CuentasPorCobrar;
using Core.Bus.General;

namespace Core.Web.Reportes.CuentasPorCobrar
{
    public partial class CXC_008_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        CXC_008_Bus bus_rpt = new CXC_008_Bus();
        public CXC_008_Rpt()
        {
            InitializeComponent();
        }

        private void CXC_008_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                lbl_empresa.Text = empresa;
                lbl_usuario.Text = usuario;
                int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
                int IdAnio = string.IsNullOrEmpty(p_IdAnio.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAnio.Value);
                int IdSede = string.IsNullOrEmpty(p_IdSede.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSede.Value);
                int IdNivel = string.IsNullOrEmpty(p_IdNivel.Value.ToString()) ? 0 : Convert.ToInt32(p_IdNivel.Value);
                int IdJornada = string.IsNullOrEmpty(p_IdJornada.Value.ToString()) ? 0 : Convert.ToInt32(p_IdJornada.Value);
                int IdCurso = string.IsNullOrEmpty(p_IdCurso.Value.ToString()) ? 0 : Convert.ToInt32(p_IdCurso.Value);
                int IdParalelo = string.IsNullOrEmpty(p_IdParalelo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdParalelo.Value);
                decimal IdAlumno = string.IsNullOrEmpty(p_IdAlumno.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdAlumno.Value);
                int CantMin = string.IsNullOrEmpty(p_CantMinima.Value.ToString()) ? 0 : Convert.ToInt32(p_CantMinima.Value);
                int CantMax = string.IsNullOrEmpty(p_CantMaxima.Value.ToString()) ? 0 : Convert.ToInt32(p_CantMaxima.Value);
                DateTime FechaFin = string.IsNullOrEmpty(p_FechaCorte.Value.ToString()) ? DateTime.Now.Date : Convert.ToDateTime(p_FechaCorte.Value);
                List<CXC_008_Info> Lista = bus_rpt.GetList(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo,IdAlumno,FechaFin, CantMin, CantMax);
                this.DataSource = Lista;

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
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
