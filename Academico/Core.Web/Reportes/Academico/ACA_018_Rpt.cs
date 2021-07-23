using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Bus.Academico;
using Core.Bus.General;
using Core.Bus.Reportes.Academico;
using System.Collections.Generic;
using Core.Info.Reportes.Academico;

namespace Core.Web.Reportes
{
    public partial class ACA_018_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        public ACA_018_Rpt()
        {
            InitializeComponent();
        }

        private void ACA_018_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_usuario.Text = usuario;

            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSede = string.IsNullOrEmpty(p_IdSede.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSede.Value);
            int IdAnio = string.IsNullOrEmpty(p_IdAnio.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAnio.Value);
            int IdNivel = string.IsNullOrEmpty(p_IdNivel.Value.ToString()) ? 0 : Convert.ToInt32(p_IdNivel.Value);
            int IdJornada = string.IsNullOrEmpty(p_IdJornada.Value.ToString()) ? 0 : Convert.ToInt32(p_IdJornada.Value);
            int IdCurso = string.IsNullOrEmpty(p_IdCurso.Value.ToString()) ? 0 : Convert.ToInt32(p_IdCurso.Value);
            int IdParalelo = string.IsNullOrEmpty(p_IdParalelo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdParalelo.Value);
            decimal IdAlumno = string.IsNullOrEmpty(p_IdAlumno.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAlumno.Value);
            bool MostrarRetirados = string.IsNullOrEmpty(p_MostrarRetirados.Value.ToString()) ? false : Convert.ToBoolean(p_MostrarRetirados.Value);

            ACA_018_Bus bus_rpt = new ACA_018_Bus();
            List<ACA_018_Info> lst_rpt = new List<ACA_018_Info>();
            lst_rpt = bus_rpt.GetList(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo, IdAlumno, MostrarRetirados);

            this.DataSource = lst_rpt;

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

            aca_Sede_Bus bus_sede = new aca_Sede_Bus();
            var sede = bus_sede.GetInfo(IdEmpresa, IdSede);
            if (sede != null)
            {
                //Rector.Text = sede.NombreRector;
                //Secretaria.Text = sede.NombreSecretaria;
            }
        }

        private void fotoAlumno_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            aca_parametro_Bus bus_parametro = new aca_parametro_Bus();
            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            decimal IdAlumno = string.IsNullOrEmpty(p_IdAlumno.Value.ToString()) ? 0 : Convert.ToDecimal(p_IdAlumno.Value);
            //var NombreImagen = IdEmpresa + IdAlumno;
            //var URLString = "C:/Users/Wendy Pc/Documents/GitHub/ACADEMICO/Academico/Core.Web/Content/imagenes/alumnos/"+NombreImagen + ".png";
            var info_parametros = bus_parametro.get_info(IdEmpresa);
            var URLImagen = (info_parametros == null ? "" : info_parametros.RutaImagen_Alumno) + IdAlumno + ".jpg";
            //var URLSitio = "C:/Users/Wendy Pc/Documents/GitHub/ACADEMICO/Academico/Core.Web";
            var URL = "http://sistema.liceocristiano.edu.ec/lcg";
            string[] subs = URLImagen.Split('~');
            var URLString = URL + subs[1];
            //var Imagen = new Bitmap(URLString);
            fotoAlumno.ImageUrl = URLString.ToString();
        }
    }
}
