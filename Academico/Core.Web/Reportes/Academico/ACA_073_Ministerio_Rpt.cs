using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Bus.General;
using Core.Info.Reportes.Academico;
using Core.Bus.Reportes.Academico;
using System.Collections.Generic;
using Core.Bus.Academico;

namespace Core.Web.Reportes.Academico
{
    public partial class ACA_073_Ministerio_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        public ACA_073_Ministerio_Rpt()
        {
            InitializeComponent();
        }

        private void PageFooter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void ACA_073_Ministerio_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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
            int IdAlumno = string.IsNullOrEmpty(p_IdAlumno.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAlumno.Value);
            bool MostrarRetirados = string.IsNullOrEmpty(p_MostrarRetirados.Value.ToString()) ? false : Convert.ToBoolean(p_MostrarRetirados.Value);

            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            aca_Sede_Bus bus_sede = new aca_Sede_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null)
            {
                if (emp.em_logo != null)
                {
                    ImageConverter obj = new ImageConverter();
                    lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
                }
            }

            var sede = bus_sede.GetInfo(IdEmpresa, IdSede);
            if (sede != null)
            {
                rector.Text = sede.NombreRector;
                secretaria.Text = sede.NombreSecretaria;
            }

            ACA_073_Bus bus_rpt = new ACA_073_Bus();
            List<ACA_073_Info> lst_rpt = new List<ACA_073_Info>();
            lst_rpt = bus_rpt.GetListMinisterio(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo, MostrarRetirados);
            Fecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.DataSource = lst_rpt;
            xrCrossTab1.DataSource = lst_rpt;
        }
    }
}
