using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Bus.Academico;
using Core.Bus.Reportes.Academico;
using Core.Info.Reportes.Academico;
using System.Collections.Generic;
using Core.Bus.General;

namespace Core.Web.Reportes.Academico
{
    public partial class ACA_005_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public ACA_005_Rpt()
        {
            InitializeComponent();
        }

        private void ACA_005_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_usuario.Text = usuario;

            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdAnio = string.IsNullOrEmpty(p_IdAnio.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAnio.Value);
            decimal IdAlumno = string.IsNullOrEmpty(p_IdAlumno.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAlumno.Value);
            int IdSede = string.IsNullOrEmpty(p_IdSede.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSede.Value);

            aca_Sede_Bus bus_sede = new aca_Sede_Bus();
            ACA_005_Bus bus_rpt = new ACA_005_Bus();
            aca_AnioLectivoConductaEquivalencia_Bus bus_conducta = new aca_AnioLectivoConductaEquivalencia_Bus();
            List<ACA_005_Info> lst_rpt = new List<ACA_005_Info>();
            ACA_005_Info info = bus_rpt.GetInfo(IdEmpresa, IdAnio, IdAlumno);

            var AnioLectivo = "";
            var Estudiante = "";
            var Jornada = "";
            var Curso = "";
            var Aprovechamiento = "";
            var Comportamiento = "";
            //int IdAnio = 0;

            if (info!=null)
            {
                IdAnio = info.IdAnio;
                AnioLectivo = info.Descripcion;
                Estudiante = info.NombreAlumno;
                Jornada = info.NomJornada;
                Curso = info.NomCurso;
                Aprovechamiento =  ((info.Promedio==0 || info.Promedio == null) ? "" : Convert.ToDecimal(info.Promedio).ToString("N2"));
                var info_conducta = bus_conducta.GetInfoXPromedioConducta(IdEmpresa, IdAnio, Convert.ToDecimal(info.Conducta));
                Comportamiento = (info.Conducta==null ? null : (info_conducta==null ? "": info_conducta.Letra));

                lst_rpt.Add(info);
            }

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

            txtSolicitud.Rtf = txtSolicitud.Rtf.Replace("[Jornada]", Jornada);
            txtSolicitud.Rtf = txtSolicitud.Rtf.Replace("[AnioLectivo]", AnioLectivo);
            txtSolicitud.Rtf = txtSolicitud.Rtf.Replace("[NombreEstudiante]", Estudiante);
            txtSolicitud.Rtf = txtSolicitud.Rtf.Replace("[Curso]", Curso);
            txtSolicitud.Rtf = txtSolicitud.Rtf.Replace("[Aprovechamiento]", Aprovechamiento);
            txtSolicitud.Rtf = txtSolicitud.Rtf.Replace("[Comportamiento]", Comportamiento);

            this.DataSource = lst_rpt;
        }
    }
}
