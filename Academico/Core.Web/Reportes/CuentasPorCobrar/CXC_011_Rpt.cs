﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Bus.General;
using Core.Info.Reportes.CuentasPorCobrar;
using Core.Bus.Reportes.CuentasPorCobrar;
using System.Collections.Generic;
using Core.Bus.Academico;
using System.Linq;

namespace Core.Web.Reportes.CuentasPorCobrar
{
    public partial class CXC_011_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        public CXC_011_Rpt()
        {
            InitializeComponent();
        }

        private void CXC_011_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_sede.Text = empresa;
            lbl_usuario.Text = usuario;
            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSede = string.IsNullOrEmpty(p_IdSede.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSede.Value);
            int IdAnio = string.IsNullOrEmpty(p_IdAnio.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAnio.Value);
            int IdJornada = string.IsNullOrEmpty(p_IdJornada.Value.ToString()) ? 0 : Convert.ToInt32(p_IdJornada.Value);
            int IdNivel = string.IsNullOrEmpty(p_IdNivel.Value.ToString()) ? 0 : Convert.ToInt32(p_IdNivel.Value);
            int IdCurso = string.IsNullOrEmpty(p_IdCurso.Value.ToString()) ? 0 : Convert.ToInt32(p_IdCurso.Value);
            int IdParalelo = string.IsNullOrEmpty(p_IdParalelo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdParalelo.Value);
            int IdAlumno = string.IsNullOrEmpty(p_IdAlumno.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAlumno.Value);

            CXC_011_Bus bus_rpt = new CXC_011_Bus();
            List<CXC_011_Info> lst_rpt = new List<CXC_011_Info>();

            lst_rpt.AddRange(bus_rpt.get_list(IdEmpresa,IdAnio, IdSede, IdJornada, IdNivel,IdCurso, IdParalelo, IdAlumno));
            this.DataSource = lst_rpt;

            if (lst_rpt.Count > 0)
            {
                if (lst_rpt.First().MostrarValoresDesdeHasta == true)
                {
                    ValoresDesdeHasta.Visible = true;
                }
                else
                {
                    ValoresDesdeHasta.Visible = false;
                }
            }
            else
            {
                ValoresDesdeHasta.Visible = false;
            }

            tb_mes_Bus bus_mes = new tb_mes_Bus();
            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null && emp.em_logo != null)
            {
                ImageConverter obj = new ImageConverter();
                lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
                lbl_telefono.Text = "TELEFONO " + emp.em_telefonos;
            }

            DateTime fecha = DateTime.Now;
            var mes = fecha.Month;
            var lst_mes = bus_mes.get_list();
            var descripcion_mes = "";
            foreach (var item in lst_mes)
            {
                if (item.idMes==mes)
                {
                    descripcion_mes = item.smes;
                }
            }

            Fecha.Text = "Guayaquil, " + fecha.Day.ToString() + " de " + descripcion_mes + " de " + fecha.Year.ToString();
            lbl_texto.Text = "Mediante reporte generado con corte al " + fecha.ToString("d 'de' MMMM 'de' yyyy") + ", el departamento de Cobranzas informa el detalle de su estado de cuenta, considerando las facturas pendientes de pago:";

            aca_Sede_Bus bus_sede = new aca_Sede_Bus();
            var sede = bus_sede.GetInfo(IdEmpresa, IdSede);
            if (sede!=null)
            {
                lbl_sede.Text =  sede.NomSede.ToUpper();
                lbl_direccion.Text = sede.Direccion.ToUpper();
            }
        }
    }
}
