using Core.Bus.Academico;
using Core.Bus.General;
using Core.Info.Academico;
using Core.Info.General;
using Core.Info.Helps;
using Core.Web.Helps;
using Core.Web.Reportes;
using Core.Web.Reportes.Academico;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Reportes.Controllers
{
    public class AcademicoReportesController : Controller
    {
        #region Variables
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        tb_sis_reporte_x_tb_empresa_Bus bus_rep_x_emp = new tb_sis_reporte_x_tb_empresa_Bus();
        
        aca_Profesor_Bus bus_profesor = new aca_Profesor_Bus();
        aca_MatriculaCalificacion_Bus bus_calificacion = new aca_MatriculaCalificacion_Bus();
        aca_MatriculaCalificacionParticipacion_Bus bus_calificacion_participacion = new aca_MatriculaCalificacionParticipacion_Bus();
        aca_MatriculaCalificacionCualitativa_Bus bus_calificacion_cualitativa = new aca_MatriculaCalificacionCualitativa_Bus();
        aca_ReporteCalificacion_Combos_List Lista_CombosCalificaciones = new aca_ReporteCalificacion_Combos_List();
        aca_ReporteCalificacionCualitativa_Combos_List Lista_CombosCalificacionesCualitativas = new aca_ReporteCalificacionCualitativa_Combos_List();
        aca_ReporteCalificacionParticipacion_Combos_List Lista_CombosCalificacionesParticipacion = new aca_ReporteCalificacionParticipacion_Combos_List();
        aca_AnioLectivoParcial_Bus bus_parcial = new aca_AnioLectivoParcial_Bus();
        aca_Rubro_Bus bus_rubro = new aca_Rubro_Bus();
        #endregion

        #region Combos
        public ActionResult Cmb_Alumno()
        {
            decimal model = new decimal();
            return PartialView("_CmbAlumno", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda_alumno(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda_alumno(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO.ToString());
        }

        public ActionResult ComboBoxPartial_Anio()
        {
            return PartialView("_ComboBoxPartial_Anio", new aca_AnioLectivo_NivelAcademico_Jornada_Info());
        }
        public ActionResult CmbSede()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            return PartialView("_CmbSede", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio });
        }
        public ActionResult CmbJornada()
        {
            
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            //int IdNivel = (Request.Params["IdNivel"] != null) ? int.Parse(Request.Params["IdNivel"]) : -1;
            return PartialView("_CmbJornada", new aca_AnioLectivo_Jornada_Curso_Info { IdAnio = IdAnio, IdSede = IdSede });
        }
        public ActionResult CmbNivel()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            return PartialView("_CmbNivel", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada= IdJornada });
        }
        public ActionResult CmbCurso()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            
            return PartialView("_CmbCurso", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada = IdJornada, IdNivel = IdNivel });
        }
        public ActionResult ComboBoxPartial_Paralelo()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdCurso = !string.IsNullOrEmpty(Request.Params["IdCurso"]) ? int.Parse(Request.Params["IdCurso"]) : -1;

            return PartialView("_ComboBoxPartial_Paralelo", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada = IdJornada, IdNivel = IdNivel, IdCurso = IdCurso });
        }

        #endregion

        #region Combos ACA_013 ACA_014
        public ActionResult ComboBoxPartial_AnioAlumno()
        {
            return PartialView("_ComboBoxPartial_AnioAlumno", new aca_AnioLectivo_NivelAcademico_Jornada_Info());
        }
        public ActionResult CmbSedeAlumno()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            return PartialView("_CmbSedeAlumno", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio });
        }
        public ActionResult CmbJornadaAlumno()
        {

            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            //int IdNivel = (Request.Params["IdNivel"] != null) ? int.Parse(Request.Params["IdNivel"]) : -1;
            return PartialView("_CmbJornadaAlumno", new aca_AnioLectivo_Jornada_Curso_Info { IdAnio = IdAnio, IdSede = IdSede });
        }
        public ActionResult CmbNivelAlumno()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            return PartialView("_CmbNivel", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada = IdJornada });
        }
        public ActionResult CmbCursoAlumno()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;

            return PartialView("_CmbCursoAlumno", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada = IdJornada, IdNivel = IdNivel });
        }
        public ActionResult ComboBoxPartial_ParaleloAlumno()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdCurso = !string.IsNullOrEmpty(Request.Params["IdCurso"]) ? int.Parse(Request.Params["IdCurso"]) : -1;

            return PartialView("_ComboBoxPartial_ParaleloAlumno", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada = IdJornada, IdNivel = IdNivel, IdCurso = IdCurso });
        }
        public ActionResult ComboBoxPartial_Alumno()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdCurso = !string.IsNullOrEmpty(Request.Params["IdCurso"]) ? int.Parse(Request.Params["IdCurso"]) : -1;
            var IdParalelo = !string.IsNullOrEmpty(Request.Params["IdParalelo"]) ? int.Parse(Request.Params["IdParalelo"]) : -1;
            return PartialView("_ComboBoxPartial_Alumno", new aca_Matricula_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada, IdCurso = IdCurso, IdParalelo = IdParalelo });
        }
        #endregion

        #region ACA_001
        public ActionResult ACA_001(int IdEmpresa = 0, decimal IdAlumno = 0, int IdAnio = 0)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            var info_anio = new aca_AnioLectivo_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            if (IdAnio == 0)
            {
                info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            }

            model.IdAnio = (IdAnio == 0 ? info_anio.IdAnio : IdAnio);
            model.IdAlumno = IdAlumno;

            ACA_001_Rpt Report = new ACA_001_Rpt();

            Report.p_IdEmpresa.Value = model.IdEmpresa;
            Report.p_IdSede.Value = model.IdSede;
            Report.p_IdAnio.Value = model.IdAnio;
            Report.p_IdAlumno.Value = IdAlumno;
            Report.usuario = SessionFixed.IdUsuario;
            Report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = Report;

            ACA_002_Rpt ReportSolicitud = new ACA_002_Rpt();

            ReportSolicitud.p_IdEmpresa.Value = model.IdEmpresa;
            ReportSolicitud.p_IdAlumno.Value = model.IdAlumno;
            ReportSolicitud.p_IdAnio.Value = model.IdAnio;
            ReportSolicitud.usuario = SessionFixed.IdUsuario;
            ReportSolicitud.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportSolicitud = ReportSolicitud;

            ACA_063_Rpt ReportAutorizacion= new ACA_063_Rpt();

            ReportAutorizacion.p_IdEmpresa.Value = model.IdEmpresa;
            ReportAutorizacion.p_IdAlumno.Value = model.IdAlumno;
            ReportAutorizacion.p_IdAnio.Value = model.IdAnio;
            ReportAutorizacion.p_IdSede.Value = model.IdSede;
            ReportAutorizacion.usuario = SessionFixed.IdUsuario;
            ReportAutorizacion.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportAutorizacion = ReportAutorizacion;

            ACA_003_Rpt ReportContrato = new ACA_003_Rpt();

            ReportContrato.p_IdEmpresa.Value = model.IdEmpresa;
            ReportContrato.p_IdAnio.Value = model.IdAnio;
            ReportContrato.p_IdAlumno.Value = model.IdAlumno;
            ReportContrato.p_IdSede.Value = model.IdSede;
            ReportContrato.usuario = SessionFixed.IdUsuario;
            ReportContrato.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportContrato = ReportContrato;

            ACA_005_Rpt ReportSocioEconomica = new ACA_005_Rpt();

            ReportSocioEconomica.p_IdEmpresa.Value = model.IdEmpresa;
            ReportSocioEconomica.p_IdAlumno.Value = model.IdAlumno;
            ReportSocioEconomica.p_IdSede.Value = model.IdSede;
            ReportSocioEconomica.p_IdAnio.Value = model.IdAnio;
            ReportSocioEconomica.usuario = SessionFixed.IdUsuario;
            ReportSocioEconomica.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportSocioEconomica = ReportSocioEconomica;

            return View(model);
        }

        [HttpPost]
        public ActionResult ACA_001(cl_filtros_Info model)
        {
            ACA_001_Rpt Report = new ACA_001_Rpt();

            Report.p_IdEmpresa.Value = model.IdEmpresa;
            Report.p_IdAlumno.Value = model.IdAlumno;
            Report.p_IdAnio.Value = model.IdAnio;
            Report.p_IdSede.Value = model.IdSede;
            Report.usuario = SessionFixed.IdUsuario;
            Report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = Report;

            ACA_002_Rpt ReportSolicitud = new ACA_002_Rpt();

            ReportSolicitud.p_IdEmpresa.Value = model.IdEmpresa;
            ReportSolicitud.p_IdAlumno.Value = model.IdAlumno;
            ReportSolicitud.p_IdAnio.Value = model.IdAnio;
            ReportSolicitud.usuario = SessionFixed.IdUsuario;
            ReportSolicitud.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportSolicitud = ReportSolicitud;

            ACA_063_Rpt ReportAutorizacion = new ACA_063_Rpt();

            ReportAutorizacion.p_IdEmpresa.Value = model.IdEmpresa;
            ReportAutorizacion.p_IdAlumno.Value = model.IdAlumno;
            ReportAutorizacion.p_IdAnio.Value = model.IdAnio;
            ReportAutorizacion.p_IdSede.Value = model.IdSede;
            ReportAutorizacion.usuario = SessionFixed.IdUsuario;
            ReportAutorizacion.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportAutorizacion = ReportAutorizacion;

            ACA_003_Rpt ReportContrato = new ACA_003_Rpt();

            ReportContrato.p_IdEmpresa.Value = model.IdEmpresa;
            ReportContrato.p_IdAnio.Value = model.IdAnio;
            ReportContrato.p_IdAlumno.Value = model.IdAlumno;
            ReportContrato.p_IdSede.Value = model.IdSede;
            ReportContrato.usuario = SessionFixed.IdUsuario;
            ReportContrato.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportContrato = ReportContrato;


            ACA_005_Rpt ReportSocioEconomica = new ACA_005_Rpt();

            ReportSocioEconomica.p_IdEmpresa.Value = model.IdEmpresa;
            ReportSocioEconomica.p_IdAlumno.Value = model.IdAlumno;
            ReportSocioEconomica.p_IdSede.Value = model.IdSede;
            ReportSocioEconomica.p_IdAnio.Value = model.IdAnio;
            ReportSocioEconomica.usuario = SessionFixed.IdUsuario;
            ReportSocioEconomica.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportSocioEconomica = ReportSocioEconomica;

            return View(model);
        }

        #endregion

        #region ACA_002
        public ActionResult ACA_002(int IdEmpresa = 0, decimal IdAlumno = 0, int IdAnio = 0)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var info_anio = new aca_AnioLectivo_Info();

            if (IdAnio == 0)
            {
                info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            }

            model.IdAnio = (IdAnio == 0 ? info_anio.IdAnio : IdAnio);
            model.IdAlumno = IdAlumno;
            ACA_002_Rpt report = new ACA_002_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdAlumno.Value = IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }

        [HttpPost]
        public ActionResult ACA_002(cl_filtros_Info model)
        {
            ACA_002_Rpt report = new ACA_002_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdAnio.Value = model.IdAnio;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }

        #endregion

        #region ACA_003
        public ActionResult ACA_003(int IdEmpresa = 0, decimal IdAlumno = 0)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);

            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdAlumno = IdAlumno;

            ACA_003_Rpt report = new ACA_003_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAlumno.Value = IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }

        [HttpPost]
        public ActionResult ACA_003(cl_filtros_Info model)
        {
            ACA_003_Rpt report = new ACA_003_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdSede.Value = model.IdSede;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }

        #endregion

        #region ACA_004
        public ActionResult ACA_004()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);

            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);

            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;

            ACA_004_Rpt report = new ACA_004_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_004.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_004");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_004(cl_filtros_Info model)
        {
            ACA_004_Rpt report = new ACA_004_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_004.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_004");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }

        #endregion

        #region ACA_005
        public ActionResult ACA_005(int IdEmpresa = 0, int IdAnio=0, decimal IdAlumno = 0)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);

            model.IdAnio = IdAnio;
            model.IdAlumno = IdAlumno;

            ACA_005_Rpt report = new ACA_005_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdAlumno.Value = IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }

        [HttpPost]
        public ActionResult ACA_005(cl_filtros_Info model)
        {
            ACA_005_Rpt report = new ACA_005_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        #endregion

        #region ACA_006
        public ActionResult ACA_006()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            var info_anio = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);

            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.fecha_ini = DateTime.Now.AddMonths(-1);
            model.fecha_fin = DateTime.Now;
            model.IdJornada = 0;
            model.IdNivel = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_006_Rpt report = new ACA_006_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_006.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_006");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostarAlumnosRetirados.Value = model.mostrarAnulados;

            ACA_006_Resumen_Rpt ReportResumen = new ACA_006_Resumen_Rpt();

            ReportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            ReportResumen.p_IdSede.Value = model.IdSede;
            ReportResumen.p_fecha_fin.Value = model.fecha_fin;
            ReportResumen.p_fecha_ini.Value = model.fecha_ini;
            ReportResumen.p_IdAnio.Value = model.IdAnio;
            ReportResumen.p_IdJornada.Value = model.IdJornada;
            ReportResumen.p_IdNivel.Value = model.IdNivel;
            ReportResumen.p_IdCurso.Value = model.IdCurso;
            ReportResumen.p_IdParalelo.Value = model.IdParalelo;
            ReportResumen.p_MostarAlumnosRetirados.Value = model.mostrarAnulados;

            ViewBag.ReportResumen = ReportResumen;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_006(cl_filtros_Info model)
        {
            ACA_006_Rpt report = new ACA_006_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_006.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_006");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostarAlumnosRetirados.Value = model.mostrarAnulados;

            ACA_006_Resumen_Rpt ReportResumen = new ACA_006_Resumen_Rpt();

            ReportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            ReportResumen.p_IdSede.Value = model.IdSede;
            ReportResumen.p_fecha_fin.Value = model.fecha_fin;
            ReportResumen.p_fecha_ini.Value = model.fecha_ini;
            ReportResumen.p_IdAnio.Value = model.IdAnio;
            ReportResumen.p_IdJornada.Value = model.IdJornada;
            ReportResumen.p_IdNivel.Value = model.IdNivel;
            ReportResumen.p_IdCurso.Value = model.IdCurso;
            ReportResumen.p_IdParalelo.Value = model.IdParalelo;
            ReportResumen.p_MostarAlumnosRetirados.Value = model.mostrarAnulados;

            ViewBag.ReportResumen = ReportResumen;
            ViewBag.Report = report;

            return View(model);
        }


        #endregion

        #region ACA_007
        public ActionResult ACA_007()
        {
            var info_anio = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.fecha_ini = DateTime.Now.AddMonths(-1);
            model.fecha_fin = DateTime.Now;
            model.IdAnio = info_anio.IdAnio;
            model.IdJornada = 0;
            model.IdNivel = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_007_Rpt report = new ACA_007_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_007.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_007");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostarAlumnosRetirados.Value = model.mostrarAnulados;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_007(cl_filtros_Info model)
        {
            ACA_007_Rpt report = new ACA_007_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_007.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_007");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostarAlumnosRetirados.Value = model.mostrarAnulados;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_008
        public ActionResult ACA_008()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrar_observacion_completa = false;
            model.mostrarAnulados = false;

            ACA_008_Rpt report = new ACA_008_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_008.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_008");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarPlantilla.Value = model.mostrar_observacion_completa;
            report.p_MostarAlumnosRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            ACA_008_Resumen_Rpt ReportResumen = new ACA_008_Resumen_Rpt();

            ReportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            ReportResumen.p_IdAnio.Value = model.IdAnio;
            ReportResumen.p_IdSede.Value = model.IdSede;
            ReportResumen.p_IdNivel.Value = model.IdNivel;
            ReportResumen.p_IdJornada.Value = model.IdJornada;
            ReportResumen.p_IdCurso.Value = model.IdCurso;
            ReportResumen.p_IdParalelo.Value = model.IdParalelo;
            ReportResumen.p_MostarAlumnosRetirados.Value = model.mostrarAnulados;
            ReportResumen.usuario = SessionFixed.IdUsuario;
            ReportResumen.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportResumen = ReportResumen;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_008(cl_filtros_Info model)
        {
            ACA_008_Rpt report = new ACA_008_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_008.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_008");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarPlantilla.Value = model.mostrar_observacion_completa;
            report.p_MostarAlumnosRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            ACA_008_Resumen_Rpt ReportResumen = new ACA_008_Resumen_Rpt();

            ReportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            ReportResumen.p_IdAnio.Value = model.IdAnio;
            ReportResumen.p_IdSede.Value = model.IdSede;
            ReportResumen.p_IdNivel.Value = model.IdNivel;
            ReportResumen.p_IdJornada.Value = model.IdJornada;
            ReportResumen.p_IdCurso.Value = model.IdCurso;
            ReportResumen.p_IdParalelo.Value = model.IdParalelo;
            ReportResumen.p_MostarAlumnosRetirados.Value = model.mostrarAnulados;
            ReportResumen.usuario = SessionFixed.IdUsuario;
            ReportResumen.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportResumen = ReportResumen;

            return View(model);
        }
        #endregion

        #region ACA_009
        public ActionResult ACA_009(int IdEmpresa = 0, decimal IdAlumno = 0)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.fecha_ini = DateTime.Now.AddMonths(-1);
            model.fecha_fin = DateTime.Now;
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);

            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdAlumno = IdAlumno;

            ACA_009_Rpt report = new ACA_009_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_009.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_009");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_IdAnio.Value = model.IdAnio;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_009(cl_filtros_Info model)
        {
            ACA_009_Rpt report = new ACA_009_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_009.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_009");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_IdAnio.Value = model.IdAnio;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region Combos
        public ActionResult ComboBoxPartial_Anio_Cal()
        {
            return PartialView("_ComboBoxPartial_Anio_Cal", new aca_AnioLectivo_NivelAcademico_Jornada_Info());
        }
        public ActionResult ComboBoxPartial_Sede_Cal()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            return PartialView("_ComboBoxPartial_Sede_Cal", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio });
        }
        public ActionResult ComboBoxPartial_Jornada_Cal()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;

            return PartialView("_ComboBoxPartial_Jornada_Cal", new aca_AnioLectivo_Jornada_Curso_Info { IdAnio = IdAnio, IdSede = IdSede });
        }
        public ActionResult ComboBoxPartial_Nivel_Cal()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            return PartialView("_ComboBoxPartial_Nivel_Cal", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada= IdJornada });
        }
        public ActionResult ComboBoxPartial_Curso_Cal()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            
            return PartialView("_ComboBoxPartial_Curso_Cal", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada = IdJornada, IdNivel = IdNivel });
        }
        public ActionResult ComboBoxPartial_Paralelo_Cal()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdCurso = !string.IsNullOrEmpty(Request.Params["IdCurso"]) ? int.Parse(Request.Params["IdCurso"]) : -1;

            return PartialView("_ComboBoxPartial_Paralelo_Cal", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada = IdJornada, IdNivel = IdNivel, IdCurso = IdCurso });
        }
        public ActionResult ComboBoxPartial_Materia_Cal()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdCurso = !string.IsNullOrEmpty(Request.Params["IdCurso"]) ? int.Parse(Request.Params["IdCurso"]) : -1;
            var IdParalelo = !string.IsNullOrEmpty(Request.Params["IdParalelo"]) ? int.Parse(Request.Params["IdParalelo"]) : -1;

            return PartialView("_ComboBoxPartial_Materia_Cal", new aca_AnioLectivo_Paralelo_Profesor_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada = IdJornada, IdNivel = IdNivel, IdCurso = IdCurso, IdParalelo = IdParalelo });
        }
        #endregion

        #region Combos Funciones Agrupados
        public List<aca_AnioLectivo_Info> CargarAnio(int IdEmpresa = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = Lista_CombosCalificaciones.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q => q.IdEmpresa == IdEmpresa).ToList();
            var lst_anio = (from q in lst_combos
                            group q by new
                            {
                                q.IdEmpresa,
                                q.IdAnio,
                                q.Descripcion
                            } into a
                            select new aca_AnioLectivo_Info
                            {
                                IdEmpresa = a.Key.IdEmpresa,
                                IdAnio = a.Key.IdAnio,
                                Descripcion = a.Key.Descripcion
                            }).OrderBy(q => q.Descripcion).ToList();

            var ListaAnio = new List<aca_AnioLectivo_Info>();

            foreach (var item in lst_anio)
            {
                ListaAnio.Add(new aca_AnioLectivo_Info
                {
                    IdAnio = item.IdAnio,
                    Descripcion = item.Descripcion
                });
            }

            return ListaAnio;
        }

        public List<aca_Sede_Info> CargarSede(int IdEmpresa = 0, int IdAnio = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = Lista_CombosCalificaciones.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio).ToList();

            var lst_sede = (from q in lst_combos
                            group q by new
                            {
                                q.IdEmpresa,
                                q.IdAnio,
                                q.IdSede,
                                q.NomSede
                            } into a
                            select new aca_Sede_Info
                            {
                                IdEmpresa = a.Key.IdEmpresa,
                                IdSede = a.Key.IdSede,
                                NomSede = a.Key.NomSede
                            }).OrderBy(q => q.NomSede).ToList();

            var ListaSede = new List<aca_Sede_Info>();

            foreach (var item in lst_sede)
            {
                ListaSede.Add(new aca_Sede_Info
                {
                    IdSede = item.IdSede,
                    NomSede = item.NomSede
                });
            }

            return ListaSede;
        }

        public List<aca_Jornada_Info> CargarJornada(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = Lista_CombosCalificaciones.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede).ToList();

            var lst_jornada = (from q in lst_combos
                               group q by new
                               {
                                   q.IdEmpresa,
                                   q.IdAnio,
                                   q.IdSede,
                                   q.IdJornada,
                                   q.NomJornada,
                                   q.OrdenJornada
                               } into a
                               select new aca_Jornada_Info
                               {
                                   IdEmpresa = a.Key.IdEmpresa,
                                   IdJornada = a.Key.IdJornada,
                                   NomJornada = a.Key.NomJornada,
                                   OrdenJornada = a.Key.OrdenJornada
                               }).OrderBy(q => q.OrdenJornada).ToList();

            var ListaJornada = new List<aca_Jornada_Info>();

            foreach (var item in lst_jornada)
            {
                ListaJornada.Add(new aca_Jornada_Info
                {
                    IdJornada = item.IdJornada,
                    NomJornada = item.NomJornada
                });
            }

            return ListaJornada;
        }

        public List<aca_NivelAcademico_Info> CargarNivel(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0, int IdJornada=0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = Lista_CombosCalificaciones.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede).ToList();

            var lst_nivel = (from q in lst_combos
                             group q by new
                             {
                                 q.IdEmpresa,
                                 q.IdAnio,
                                 q.IdSede,
                                 q.IdNivel,
                                 q.NomNivel,
                                 q.OrdenNivel
                             } into a
                             select new aca_NivelAcademico_Info
                             {
                                 IdEmpresa = a.Key.IdEmpresa,
                                 IdNivel = a.Key.IdNivel,
                                 NomNivel = a.Key.NomNivel,
                                 Orden = a.Key.OrdenNivel
                             }).OrderBy(q => q.Orden).ToList();

            var ListaNivel = new List<aca_NivelAcademico_Info>();

            foreach (var item in lst_nivel)
            {
                ListaNivel.Add(new aca_NivelAcademico_Info
                {
                    IdNivel = item.IdNivel,
                    NomNivel = item.NomNivel
                });
            }

            return ListaNivel;
        }

        public List<aca_Curso_Info> CargarCurso(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0, int IdJornada = 0, int IdNivel = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = Lista_CombosCalificaciones.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede && q.IdNivel == IdNivel && q.IdJornada == IdJornada).ToList();

            var lst_curso = (from q in lst_combos
                             group q by new
                             {
                                 q.IdEmpresa,
                                 q.IdAnio,
                                 q.IdSede,
                                 q.IdNivel,
                                 q.IdJornada,
                                 q.IdCurso,
                                 q.NomCurso,
                                 q.OrdenCurso
                             } into a
                             select new aca_Curso_Info
                             {
                                 IdEmpresa = a.Key.IdEmpresa,
                                 IdCurso = a.Key.IdCurso,
                                 NomCurso = a.Key.NomCurso,
                                 OrdenCurso = a.Key.OrdenCurso
                             }).OrderBy(q => q.OrdenCurso).ToList();

            var ListaCurso = new List<aca_Curso_Info>();

            foreach (var item in lst_curso)
            {
                ListaCurso.Add(new aca_Curso_Info
                {
                    IdCurso = item.IdCurso,
                    NomCurso = item.NomCurso
                });
            }

            return ListaCurso;
        }

        public List<aca_Paralelo_Info> CargarParalelo(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0, int IdJornada = 0, int IdNivel = 0,  int IdCurso = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = Lista_CombosCalificaciones.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede && q.IdNivel == IdNivel && q.IdJornada == IdJornada && q.IdCurso == IdCurso).ToList();

            var lst_paralelo = (from q in lst_combos
                                group q by new
                                {
                                    q.IdEmpresa,
                                    q.IdAnio,
                                    q.IdSede,
                                    q.IdNivel,
                                    q.IdJornada,
                                    q.IdCurso,
                                    q.IdParalelo,
                                    q.NomParalelo,
                                    q.OrdenParalelo
                                } into a
                                select new aca_Paralelo_Info
                                {
                                    IdEmpresa = a.Key.IdEmpresa,
                                    IdParalelo = a.Key.IdParalelo,
                                    NomParalelo = a.Key.NomParalelo,
                                    OrdenParalelo = a.Key.OrdenParalelo,
                                }).OrderBy(q => q.OrdenParalelo).ToList();

            var ListaParalelo = new List<aca_Paralelo_Info>();

            foreach (var item in lst_paralelo)
            {
                ListaParalelo.Add(new aca_Paralelo_Info
                {
                    IdParalelo = item.IdParalelo,
                    NomParalelo = item.NomParalelo
                });
            }

            return ListaParalelo;
        }

        public List<aca_Materia_Info> CargarMateria(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0, int IdJornada = 0, int IdNivel = 0, int IdCurso = 0, int IdParalelo = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = Lista_CombosCalificaciones.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede && q.IdNivel == IdNivel && q.IdJornada == IdJornada && q.IdCurso == IdCurso && q.IdParalelo == IdParalelo).ToList();

            var lst_materia = (from q in lst_combos
                               group q by new
                               {
                                   q.IdEmpresa,
                                   q.IdAnio,
                                   q.IdSede,
                                   q.IdNivel,
                                   q.IdJornada,
                                   q.IdCurso,
                                   q.IdParalelo,
                                   q.IdMateria,
                                   q.NomMateria,
                                   q.OrdenMateria
                               } into a
                               select new aca_Materia_Info
                               {
                                   IdEmpresa = a.Key.IdEmpresa,
                                   IdMateria = a.Key.IdMateria,
                                   NomMateria = a.Key.NomMateria,
                                   OrdenMateria = a.Key.OrdenMateria
                               }).OrderBy(q => q.OrdenMateria).ToList();

            var ListaMateria = new List<aca_Materia_Info>();

            foreach (var item in lst_materia)
            {
                ListaMateria.Add(new aca_Materia_Info
                {
                    IdMateria = item.IdMateria,
                    NomMateria = item.NomMateria
                });
            }

            return ListaMateria;
        }
        #endregion

        #region ACA_010
        private void cargar_combos_ACA_010(aca_MatriculaCalificacionParcial_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            //var lst_parcial = new List<aca_AnioLectivoParcial_Info>();
            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
            //var lst_parcial = new List<aca_AnioLectivoParcial_Info>();
            //var lst_quim1 = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1));
            //var lst_quim2 = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2));
            //lst_parcial.AddRange(lst_quim1);
            //lst_parcial.AddRange(lst_quim2);

            //ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_010(int IdEmpresa = 0, decimal IdAlumno = 0)
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_CombosCalificaciones(model.IdEmpresa, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_010_Rpt report = new ACA_010_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_010.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_010");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_010(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_010(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_010_Rpt report = new ACA_010_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_010.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_010");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_010(model);
            return View(model);
        }

        #endregion

        #region ACA_011
        private void cargar_combos_ACA_011(aca_MatriculaCalificacion_Info model)
        {
            var lst_parcial = new List<aca_AnioLectivoParcial_Info>();
            var lst_quim1 = bus_parcial.GetList(model.IdEmpresa, model.IdSede, model.IdAnio, Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1), DateTime.Now.Date);
            var lst_quim2 = bus_parcial.GetList(model.IdEmpresa, model.IdSede, model.IdAnio, Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2), DateTime.Now.Date);
            lst_parcial.AddRange(lst_quim1);
            lst_parcial.AddRange(lst_quim2);

            ViewBag.lst_parcial = lst_parcial;
        }

        public ActionResult ACA_011()
        {
            aca_MatriculaCalificacion_Info model = new aca_MatriculaCalificacion_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);

            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_Combos_TutorCalificacion(model.IdEmpresa,model.IdAnio, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_011_Rpt report = new ACA_011_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_011.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_011");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_011(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_011(aca_MatriculaCalificacion_Info model)
        {
            ACA_011_Rpt report = new ACA_011_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_011.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_011");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_011(model);
            return View(model);
        }

        #endregion

        #region ACA_012
        private void cargar_combos_ACA_012(cl_filtros_Info model)
        {
            var lst_rubro = bus_rubro.GetList(model.IdEmpresa, false);
            ViewBag.lst_rubro = lst_rubro;
        }
        public ActionResult ACA_012()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = new aca_AnioLectivo_Info();
            info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);

            model.IdAnio = info_anio == null ? 0 : info_anio.IdAnio;
            model.mostrarAnulados = true;
            model.IdRubro = 3;
            ACA_012_Rpt report = new ACA_012_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_012.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_012");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdRubro.Value = model.IdRubro;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.p_QuebrarPorParalelo.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            cargar_combos_ACA_012(model);
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_012(cl_filtros_Info model)
        {
            ACA_012_Rpt report = new ACA_012_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_012.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_012");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdRubro.Value = model.IdRubro;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.p_QuebrarPorParalelo.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            cargar_combos_ACA_012(model);
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        public JsonResult CargarParciales_X_Quimestre(int IdEmpresa=0, int IdSede=0, int IdAnio = 0, int IdCatalogoTipo=0)
        {
            var resultado = bus_parcial.GetList_Reportes(IdEmpresa, IdSede, IdAnio, IdCatalogoTipo);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #region ACA_013
        private void cargar_combos_ACA_013(aca_MatriculaCalificacionParcial_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            //var lst_parcial = new List<aca_AnioLectivoParcial_Info>();
            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;

        }
        public ActionResult ACA_013(int IdEmpresa = 0, decimal IdAlumno = 0, int IdCatalogoTipo = 0, int IdCatalogoParcial = 0, bool MostrarRetirados = false, bool MostrarPromedios = true)
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdAlumno = IdAlumno;
            model.IdCatalogoTipo = IdCatalogoTipo;
            model.IdCatalogoParcial = IdCatalogoParcial;
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.MostrarRetirados = MostrarRetirados;
            model.MostrarPromedios = MostrarPromedios;
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            //List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_Combos(model.IdEmpresa, IdProfesor, EsSuperAdmin);
            //Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_013_Rpt report = new ACA_013_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_013.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_013");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.p_MostrarComportamiento.Value = model.MostrarPromedios;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_013(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_013(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_013_Rpt report = new ACA_013_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_013.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_013");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.p_MostrarComportamiento.Value = model.MostrarPromedios;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_013(model);
            return View(model);
        }
        #endregion

        #region ACA_014
        private void cargar_combos_ACA_014(cl_filtros_Info model)
        {
            aca_CatalogoTipo_Bus bus_catalogo = new aca_CatalogoTipo_Bus();
            var lst_quimestre = new List<aca_CatalogoTipo_Info>();
            var quim1 = bus_catalogo.GetInfo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1));
            lst_quimestre.Add(quim1);
            var quim2 = bus_catalogo.GetInfo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2));
            lst_quimestre.Add(quim2);
            ViewBag.lst_quimestre = lst_quimestre;
        }
        public ActionResult ACA_014(int IdEmpresa = 0, decimal IdAlumno = 0, int IdCatalogoTipo = 0, bool MostrarRetirados = false, bool MostrarPromedios = true)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdAlumno = IdAlumno;
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoParcial = IdCatalogoTipo;
            model.mostrarAnulados = MostrarRetirados;
            model.MostrarPromedios = MostrarPromedios;
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_Combos(model.IdEmpresa, model.IdAnio, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_014_Rpt report = new ACA_014_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_014.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_014");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.p_MostrarPromedios.Value = model.MostrarPromedios;

            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_014(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_014(cl_filtros_Info model)
        {
            ACA_014_Rpt report = new ACA_014_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_014.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_014");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.p_MostrarPromedios.Value = model.MostrarPromedios;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_014(model);
            return View(model);
        }
        #endregion

        #region ACA_015
        public ActionResult ACA_015()
        {
            aca_Matricula_Info model = new aca_Matricula_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);

            ACA_015_Rpt report = new ACA_015_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_015.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_015");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_015(aca_Matricula_Info model)
        {
            ACA_015_Rpt report = new ACA_015_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_015.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_015");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }

        #endregion

        #region ACA_016
        private void cargar_combos_ACA_016(aca_MatriculaCalificacionCualitativa_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_016()
        {
            aca_MatriculaCalificacionCualitativa_Info model = new aca_MatriculaCalificacionCualitativa_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion_cualitativa.getList_CombosCualitativa(model.IdEmpresa, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_016_Rpt report = new ACA_016_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_016.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_016");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_016(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_016(aca_MatriculaCalificacionCualitativa_Info model)
        {
            ACA_016_Rpt report = new ACA_016_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_016.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_016");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_016(model);
            return View(model);
        }

        #endregion

        #region ACA_017
        public ActionResult ACA_017()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);

            ACA_017_Rpt report = new ACA_017_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_017.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_017");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = true;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_017(cl_filtros_Info model)
        {
            ACA_017_Rpt report = new ACA_017_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_017.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_017");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_018
        public ActionResult ACA_018()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);

            ACA_018_Rpt report = new ACA_018_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_018.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_018");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = true;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_018(cl_filtros_Info model)
        {
            ACA_018_Rpt report = new ACA_018_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_018.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_018");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_019
        public ActionResult ACA_019()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);

            ACA_019_Rpt report = new ACA_019_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_019.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_019");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = true;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_019(cl_filtros_Info model)
        {
            ACA_019_Rpt report = new ACA_019_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_019.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_019");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_020
        public ActionResult ACA_020()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;

            ACA_020_Rpt report = new ACA_020_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_020.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_020");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_020(cl_filtros_Info model)
        {
            ACA_020_Rpt report = new ACA_020_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_020.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_020");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_021
        public ActionResult ACA_021()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;
            ACA_021_Rpt report = new ACA_021_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_021.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_021");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_021(cl_filtros_Info model)
        {
            ACA_021_Rpt report = new ACA_021_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_021.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_021");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_022
        private void cargar_combos_ACA_022(aca_MatriculaAsistencia_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_022()
        {
            aca_MatriculaAsistencia_Info model = new aca_MatriculaAsistencia_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.getList_Combos_InspectorCalificacion(model.IdEmpresa, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_022_Rpt report = new ACA_022_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_022.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_022");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_022(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_022(aca_MatriculaAsistencia_Info model)
        {
            ACA_022_Rpt report = new ACA_022_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_022.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_022");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_022(model);
            return View(model);
        }

        #endregion

        #region ACA_023
        private void cargar_combos_ACA_023(aca_MatriculaConducta_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_023()
        {
            aca_MatriculaConducta_Info model = new aca_MatriculaConducta_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.getList_Combos_InspectorCalificacion(model.IdEmpresa, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_023_Rpt report = new ACA_023_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_023.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_023");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_023(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_023(aca_MatriculaConducta_Info model)
        {
            ACA_023_Rpt report = new ACA_023_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_023.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_023");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_023(model);
            return View(model);
        }

        #endregion

        #region ACA_024
        private void cargar_combos_ACA_024(aca_MatriculaCalificacionParcial_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            //var lst_parcial = new List<aca_AnioLectivoParcial_Info>();
            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;

        }
        public ActionResult ACA_024()
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);

            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);

            ACA_024_Rpt report = new ACA_024_Rpt();

            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_024.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_024");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_024(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_024(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_024_Rpt report = new ACA_024_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_024.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_024");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_024(model);
            return View(model);
        }
        #endregion

        #region ACA_025
        private void cargar_combos_ACA_025(aca_MatriculaCalificacionParcial_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_025(int IdEmpresa = 0, decimal IdAlumno = 0)
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            model.MostrarRetirados = false;
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_CombosCalificaciones(model.IdEmpresa, model.IdSede);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_025_Rpt report = new ACA_025_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_025.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_025");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_025(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_025(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_025_Rpt report = new ACA_025_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_025.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_025");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_025(model);
            return View(model);
        }

        #endregion

        #region ACA_026
        private void cargar_combos_ACA_026(aca_MatriculaCalificacionCualitativa_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_026()
        {
            aca_MatriculaCalificacionCualitativa_Info model = new aca_MatriculaCalificacionCualitativa_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            model.MostrarRetirados = false;
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion_cualitativa.GetList_CombosCualitativa(model.IdEmpresa, model.IdSede);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_026_Rpt report = new ACA_026_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_026.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_026");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_026(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_026(aca_MatriculaCalificacionCualitativa_Info model)
        {
            ACA_026_Rpt report = new ACA_026_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_026.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_026");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_026(model);
            return View(model);
        }

        #endregion

        #region ACA_027
        private void cargar_combos_ACA_027(aca_MatriculaConducta_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_027()
        {
            aca_MatriculaConducta_Info model = new aca_MatriculaConducta_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            model.MostrarRetirados = false;
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_CombosCalificaciones(model.IdEmpresa, model.IdSede);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_027_Rpt report = new ACA_027_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_027.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_027");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_027(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_027(aca_MatriculaConducta_Info model)
        {
            ACA_027_Rpt report = new ACA_027_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_027.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_027");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_027(model);
            return View(model);
        }

        #endregion

        #region ACA_028
        private void cargar_combos_ACA_028(aca_MatriculaCalificacion_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;
        }

        public ActionResult ACA_028()
        {
            aca_MatriculaCalificacion_Info model = new aca_MatriculaCalificacion_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);

            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_Combos_TutorCalificacion(model.IdEmpresa, model.IdAnio, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_028_Rpt report = new ACA_028_Rpt();
            #region Cargo diseño desde base
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_028.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(IdEmpresa, "ACA_028");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_028(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_028(aca_MatriculaCalificacion_Info model)
        {
            ACA_028_Rpt report = new ACA_028_Rpt();
            #region Cargo diseño desde base
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_028.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(IdEmpresa, "ACA_028");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_028(model);
            return View(model);
        }

        #endregion

        #region ACA_029
        private void cargar_combos_ACA_029(aca_MatriculaCalificacion_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_029()
        {
            aca_MatriculaCalificacion_Info model = new aca_MatriculaCalificacion_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_CombosCalificaciones(model.IdEmpresa, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_029_Rpt report = new ACA_029_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_029.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_029");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_029(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_029(aca_MatriculaCalificacion_Info model)
        {
            ACA_029_Rpt report = new ACA_029_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_029.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_029");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_029(model);
            return View(model);
        }

        #endregion

        #region ACA_030
        private void cargar_combos_ACA_030(aca_MatriculaCalificacion_Info model)
        {
            aca_CatalogoTipo_Bus bus_catalogo = new aca_CatalogoTipo_Bus();
            var lst_quimestre = new List<aca_CatalogoTipo_Info>();
            var quim1 = bus_catalogo.GetInfo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1));
            lst_quimestre.Add(quim1);
            var quim2 = bus_catalogo.GetInfo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2));
            lst_quimestre.Add(quim2);
            ViewBag.lst_quimestre = lst_quimestre;
        }
        public ActionResult ACA_030()
        {
            aca_MatriculaCalificacion_Info model = new aca_MatriculaCalificacion_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            model.MostrarRetirados = false;
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_Combos(model.IdEmpresa, model.IdAnio, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_030_Rpt report = new ACA_030_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_030.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_030");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdCatalogoParcialTipo.Value = model.IdCatalogoTipo;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;

            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_030(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_030(aca_MatriculaCalificacion_Info model)
        {
            ACA_030_Rpt report = new ACA_030_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_030.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_030");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_IdCatalogoParcialTipo.Value = model.IdCatalogoTipo;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_030(model);
            return View(model);
        }
        #endregion

        #region ACA_031
        private void cargar_combos_ACA_031(aca_MatriculaConducta_Info model)
        {
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add(cl_enumeradores.eTipoCatalogoAcademicoConductaFinal.QUIMESTRE_1.ToString(), "QUIMESTRE 1");
            lst_quimestres.Add(cl_enumeradores.eTipoCatalogoAcademicoConductaFinal.QUIMESTRE_2.ToString(), "QUIMESTRE 2");
            lst_quimestres.Add(cl_enumeradores.eTipoCatalogoAcademicoConductaFinal.PROMEDIOFINAL.ToString(), "PROMEDIO FINAL");
            ViewBag.lst_quimestres = lst_quimestres;
        }
        public ActionResult ACA_031()
        {
            aca_MatriculaConducta_Info model = new aca_MatriculaConducta_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdPromedioFinal = cl_enumeradores.eTipoCatalogoAcademicoConductaFinal.QUIMESTRE_1.ToString();
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.getList_Combos_InspectorCalificacion(model.IdEmpresa, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_031_Rpt report = new ACA_031_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_031.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_031");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoParcialTipo.Value = model.IdPromedioFinal;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_031(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_031(aca_MatriculaConducta_Info model)
        {
            ACA_031_Rpt report = new ACA_031_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_031.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_031");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoParcialTipo.Value = model.IdPromedioFinal;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_031(model);
            return View(model);
        }

        #endregion

        #region ACA_032
        private void cargar_combos_ACA_032(aca_MatriculaConducta_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add(cl_enumeradores.eTipoCatalogoAcademicoConductaFinal.QUIMESTRE_1.ToString(), "QUIMESTRE 1");
            lst_quimestres.Add(cl_enumeradores.eTipoCatalogoAcademicoConductaFinal.QUIMESTRE_2.ToString(), "QUIMESTRE 2");
            lst_quimestres.Add(cl_enumeradores.eTipoCatalogoAcademicoConductaFinal.PROMEDIOFINAL.ToString(), "PROMEDIO FINAL");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_032()
        {
            aca_MatriculaConducta_Info model = new aca_MatriculaConducta_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdPromedioFinal = cl_enumeradores.eTipoCatalogoAcademicoConductaFinal.QUIMESTRE_1.ToString();
            model.MostrarRetirados = false;
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_CombosCalificaciones(model.IdEmpresa, model.IdSede);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_032_Rpt report = new ACA_032_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_032.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_032");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoParcialTipo.Value = model.IdPromedioFinal;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_032(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_032(aca_MatriculaConducta_Info model)
        {
            ACA_032_Rpt report = new ACA_032_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_032.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_032");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoParcialTipo.Value = model.IdPromedioFinal;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_032(model);
            return View(model);
        }

        #endregion

        #region ACA_033
        private void cargar_combos_ACA_033(aca_MatriculaCalificacionParcial_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_033(int IdEmpresa = 0, decimal IdAlumno = 0)
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_CombosCalificaciones(model.IdEmpresa, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_033_Rpt report = new ACA_033_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_033.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_033");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_033(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_033(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_033_Rpt report = new ACA_033_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_033.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_033");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_033(model);
            return View(model);
        }

        #endregion

        #region ACA_034
        public ActionResult ACA_034()
        {
            aca_MatriculaCalificacion_Info model = new aca_MatriculaCalificacion_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            model.MostrarRetirados = false;
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_Combos(model.IdEmpresa, model.IdAnio, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_034_Rpt report = new ACA_034_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_034.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_034");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;

            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            ACA_034_General_Rpt reportGeneral = new ACA_034_General_Rpt();
            reportGeneral.p_IdEmpresa.Value = model.IdEmpresa;
            reportGeneral.p_IdSede.Value = model.IdSede;
            reportGeneral.p_IdAnio.Value = model.IdAnio;
            reportGeneral.p_IdNivel.Value = model.IdNivel;
            reportGeneral.p_IdJornada.Value = model.IdJornada;
            reportGeneral.p_IdCurso.Value = model.IdCurso;
            reportGeneral.p_IdParalelo.Value = model.IdParalelo;
            reportGeneral.p_IdAlumno.Value = model.IdAlumno;
            reportGeneral.p_MostrarRetirados.Value = model.MostrarRetirados;
            reportGeneral.usuario = SessionFixed.IdUsuario;
            reportGeneral.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportGeneral = reportGeneral;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_034(aca_MatriculaCalificacion_Info model)
        {
            ACA_034_Rpt report = new ACA_034_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_034.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_034");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            ACA_034_General_Rpt reportGeneral = new ACA_034_General_Rpt();
            reportGeneral.p_IdEmpresa.Value = model.IdEmpresa;
            reportGeneral.p_IdSede.Value = model.IdSede;
            reportGeneral.p_IdAnio.Value = model.IdAnio;
            reportGeneral.p_IdNivel.Value = model.IdNivel;
            reportGeneral.p_IdJornada.Value = model.IdJornada;
            reportGeneral.p_IdCurso.Value = model.IdCurso;
            reportGeneral.p_IdParalelo.Value = model.IdParalelo;
            reportGeneral.p_IdAlumno.Value = model.IdAlumno;
            reportGeneral.p_MostrarRetirados.Value = model.MostrarRetirados;
            reportGeneral.usuario = SessionFixed.IdUsuario;
            reportGeneral.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportGeneral = reportGeneral;

            return View(model);
        }
        #endregion

        #region ACA_035
        public ActionResult ACA_035()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            ACA_035_Rpt report = new ACA_035_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_035.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_035");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_035(cl_filtros_Info model)
        {
            ACA_035_Rpt report = new ACA_035_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_035.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_035");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_036
        public ActionResult ACA_036()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            ACA_036_Rpt report = new ACA_036_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_036.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_036");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_036(cl_filtros_Info model)
        {
            ACA_036_Rpt report = new ACA_036_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_036.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_036");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_037
        public ActionResult ACA_037()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;

            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_CombosParticipacion(model.IdEmpresa, model.IdAnio, model.IdSede);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_037_Rpt report = new ACA_037_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_037.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_037");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_037(cl_filtros_Info model)
        {
            ACA_037_Rpt report = new ACA_037_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_037.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_037");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_CombosParticipacion(model.IdEmpresa, model.IdAnio, model.IdSede);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return View(model);
        }

        #endregion

        #region ACA_038
        public ActionResult ACA_038()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            ACA_038_Rpt report = new ACA_038_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_038.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_038");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_038(cl_filtros_Info model)
        {
            ACA_038_Rpt report = new ACA_038_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_038.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_038");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_039
        public ActionResult ACA_039()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;
            ACA_039_Rpt report = new ACA_039_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_039.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_039");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_039(cl_filtros_Info model)
        {
            ACA_039_Rpt report = new ACA_039_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_039.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_039");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_040
        public JsonResult CargarParciales_X_Quimestre_ACA_040(int IdEmpresa = 0, int IdSede = 0, int IdAnio = 0, int IdCatalogoTipo = 0)
        {
            var resultado = new List<aca_AnioLectivoParcial_Info>();
            resultado.Add(new aca_AnioLectivoParcial_Info
            {
                IdEmpresa = IdEmpresa,
                IdSede = IdSede,
                IdAnio = IdAnio,
                IdCatalogoParcial = 0,
                NomCatalogo = "",
                Orden = 0
            });

            resultado.AddRange(bus_parcial.GetList_Reportes(IdEmpresa, IdSede, IdAnio, IdCatalogoTipo));
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        private void cargar_combos_ACA_040(aca_MatriculaCalificacionParcial_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            lst_quimestres.Add("0", "PROMEDIO FINAL");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = new List<aca_AnioLectivoParcial_Info>();
            lst_parcial.Add(
                new aca_AnioLectivoParcial_Info
                {
                    IdEmpresa = model.IdEmpresa,
                    IdSede = model.IdSede,
                    IdAnio = model.IdAnio,
                    IdCatalogoParcial = 0,
                    NomCatalogo = "",
                    Orden = 0
                });
            lst_parcial.AddRange(bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo));
            
            ViewBag.lst_parcial = lst_parcial;

        }
        public ActionResult ACA_040()
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.MostrarRetirados = false;
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            ACA_040_Rpt report = new ACA_040_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_040.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_040");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.p_IdCatalogoParcialTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_040(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_040(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_040_Rpt report = new ACA_040_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_040.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_040");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.p_IdCatalogoParcialTipo.Value = model.IdCatalogoTipo;
            report.p_IdCatalogoParcial.Value = model.IdCatalogoParcial;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_040(model);
            return View(model);
        }
        #endregion

        #region ACA_041
        public ActionResult ACA_041()
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.MostrarRetirados = false;
            ACA_041_Rpt report = new ACA_041_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_041.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_041");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_041(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_041_Rpt report = new ACA_041_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_041.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_041");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_042
        public ActionResult ACA_042()
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.MostrarRetirados = false;
            ACA_042_Rpt report = new ACA_042_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_042.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_042");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_042(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_042_Rpt report = new ACA_042_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_042.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_042");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_043
        public ActionResult ACA_043()
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.MostrarRetirados = false;
            ACA_043_Rpt report = new ACA_043_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_043.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_043");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_043(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_043_Rpt report = new ACA_043_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_043.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_043");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_044
        public ActionResult ACA_044()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;
            ACA_044_Rpt report = new ACA_044_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_044.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_044");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_044(cl_filtros_Info model)
        {
            ACA_044_Rpt report = new ACA_044_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_044.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_044");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_045
        public ActionResult ACA_045()
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.MostrarRetirados = false;
            ACA_045_Rpt report = new ACA_045_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_045.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_045");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_045(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_045_Rpt report = new ACA_045_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_045.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_045");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_046
        public ActionResult ACA_046()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;
            ACA_046_Rpt report = new ACA_046_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_046.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_046");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_046(cl_filtros_Info model)
        {
            ACA_046_Rpt report = new ACA_046_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_046.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_046");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_047
        private void cargar_combos_ACA_047(aca_MatriculaCalificacionCualitativa_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_047()
        {
            aca_MatriculaCalificacionCualitativa_Info model = new aca_MatriculaCalificacionCualitativa_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion_cualitativa.getList_CombosCualitativa(model.IdEmpresa, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_047_Rpt report = new ACA_047_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_047.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_047");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoParcialTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_047(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_047(aca_MatriculaCalificacionCualitativa_Info model)
        {
            ACA_047_Rpt report = new ACA_047_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_047.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_047");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoParcialTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_047(model);
            return View(model);
        }

        #endregion

        #region ACA_048
        private void cargar_combos_ACA_048(aca_MatriculaCalificacion_Info model)
        {
            aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;
        }
        public ActionResult ACA_048()
        {
            aca_MatriculaCalificacion_Info model = new aca_MatriculaCalificacion_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion_cualitativa.getList_CombosCualitativa(model.IdEmpresa, model.IdSede, IdProfesor, EsSuperAdmin);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_048_Rpt report = new ACA_048_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_048.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_048");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoParcialTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_048(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_048(aca_MatriculaCalificacion_Info model)
        {
            ACA_048_Rpt report = new ACA_048_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_048.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_048");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdMateria.Value = model.IdMateria;
            report.p_IdCatalogoParcialTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_048(model);
            return View(model);
        }

        #endregion

        #region ACA_049
        public ActionResult ACA_049()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.fecha_ini = DateTime.Now;
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;
            ACA_049_Rpt report = new ACA_049_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_049.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_049");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            ACA_049_General_Rpt reportGeneral = new ACA_049_General_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.p_Fecha.Value = model.fecha_ini;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            reportGeneral.p_IdEmpresa.Value = model.IdEmpresa;
            reportGeneral.p_IdSede.Value = model.IdSede;
            reportGeneral.p_IdAnio.Value = model.IdAnio;
            reportGeneral.p_IdNivel.Value = model.IdNivel;
            reportGeneral.p_IdJornada.Value = model.IdJornada;
            reportGeneral.p_IdCurso.Value = model.IdCurso;
            reportGeneral.p_IdParalelo.Value = model.IdParalelo;
            reportGeneral.p_IdAlumno.Value = model.IdAlumno;
            reportGeneral.p_MostrarRetirados.Value = model.mostrarAnulados;
            reportGeneral.p_Fecha.Value = model.fecha_ini;
            reportGeneral.usuario = SessionFixed.IdUsuario;
            reportGeneral.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportGeneral = reportGeneral;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_049(cl_filtros_Info model)
        {
            ACA_049_Rpt report = new ACA_049_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_049.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_049");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            ACA_049_General_Rpt reportGeneral = new ACA_049_General_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.p_Fecha.Value = model.fecha_ini;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            reportGeneral.p_IdEmpresa.Value = model.IdEmpresa;
            reportGeneral.p_IdSede.Value = model.IdSede;
            reportGeneral.p_IdAnio.Value = model.IdAnio;
            reportGeneral.p_IdNivel.Value = model.IdNivel;
            reportGeneral.p_IdJornada.Value = model.IdJornada;
            reportGeneral.p_IdCurso.Value = model.IdCurso;
            reportGeneral.p_IdParalelo.Value = model.IdParalelo;
            reportGeneral.p_IdAlumno.Value = model.IdAlumno;
            reportGeneral.p_MostrarRetirados.Value = model.mostrarAnulados;
            reportGeneral.p_Fecha.Value = model.fecha_ini;
            reportGeneral.usuario = SessionFixed.IdUsuario;
            reportGeneral.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportGeneral = reportGeneral;
            return View(model);
        }
        #endregion

        #region ACA_050
        public ActionResult ACA_050()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;
            ACA_050_Rpt report = new ACA_050_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_050.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_050");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_050(cl_filtros_Info model)
        {
            ACA_050_Rpt report = new ACA_050_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_050.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_050");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_051
        public ActionResult ACA_051()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrar_observacion_completa = false;
            model.mostrarAnulados = false;

            ACA_051_Rpt report = new ACA_051_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_051.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_051");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_051(cl_filtros_Info model)
        {
            ACA_051_Rpt report = new ACA_051_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_051.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_051");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_052
        public ActionResult ACA_052(int IdEmpresa=0, decimal IdAlumno=0)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.IdAlumno = IdAlumno;
            model.mostrar_observacion_completa = false;
            model.mostrarAnulados = false;

            ACA_052_Rpt report = new ACA_052_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_052.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_052");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_052(cl_filtros_Info model)
        {
            ACA_052_Rpt report = new ACA_052_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_052.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_052");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_053
        public ActionResult ACA_053()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_053_Rpt report = new ACA_053_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_053.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_053");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_053(cl_filtros_Info model)
        {
            ACA_053_Rpt report = new ACA_053_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_053.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_053");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_054
        public ActionResult ACA_054()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_054_Rpt report = new ACA_054_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_054.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_054");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_054(cl_filtros_Info model)
        {
            ACA_054_Rpt report = new ACA_054_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_054.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_054");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_055
        public ActionResult ACA_055()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_055_Rpt report = new ACA_055_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_055.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_055");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_055(cl_filtros_Info model)
        {
            ACA_055_Rpt report = new ACA_055_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_055.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_055");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_056
        public ActionResult ACA_056()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            var info_anio = new aca_AnioLectivo_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);

            ACA_056_Rpt Report = new ACA_056_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_056.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_056");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                Report.LoadLayout(RootReporte);
            }
            #endregion
            Report.p_IdEmpresa.Value = model.IdEmpresa;
            Report.p_IdSede.Value = model.IdSede;
            Report.usuario = SessionFixed.IdUsuario;
            Report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = Report;

            return View(model);
        }

        #endregion

        #region ACA_057
        public ActionResult ACA_057()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;

            ACA_057_Rpt report = new ACA_057_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_057.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_057");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_057(cl_filtros_Info model)
        {
            ACA_057_Rpt report = new ACA_057_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_057.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_057");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_058
        public ActionResult ACA_058()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;

            ACA_058_Rpt report = new ACA_058_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_058.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_058");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_058(cl_filtros_Info model)
        {
            ACA_058_Rpt report = new ACA_058_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_058.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_058");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_059
        public ActionResult ACA_059()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_059_Rpt report = new ACA_059_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_059.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_059");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_059(cl_filtros_Info model)
        {
            ACA_059_Rpt report = new ACA_059_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_059.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_059");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_060
        public ActionResult ACA_060()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_060_Rpt report = new ACA_060_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_060.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_060");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_060(cl_filtros_Info model)
        {
            ACA_060_Rpt report = new ACA_060_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_060.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_060");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_061
        public ActionResult ACA_061()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;
            ACA_061_Rpt report = new ACA_061_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_061.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_061");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_061(cl_filtros_Info model)
        {
            ACA_061_Rpt report = new ACA_061_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_061.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_061");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_062
        public ActionResult ACA_062()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;
            ACA_062_Rpt report = new ACA_062_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_062.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_062");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_062(cl_filtros_Info model)
        {
            ACA_062_Rpt report = new ACA_062_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_062.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_062");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_064
        public ActionResult ACA_064()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_064_Rpt report = new ACA_064_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_064.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_064");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_064(cl_filtros_Info model)
        {
            ACA_064_Rpt report = new ACA_064_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_064.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_064");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_065
        public ActionResult ACA_065()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_065_Rpt report = new ACA_065_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_065.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_065");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_065(cl_filtros_Info model)
        {
            ACA_065_Rpt report = new ACA_065_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_065.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_065");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_066
        public ActionResult ACA_066()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);

            ACA_066_Rpt report = new ACA_066_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_066.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_066");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_066(cl_filtros_Info model)
        {
            ACA_066_Rpt report = new ACA_066_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_066.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_066");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        #endregion

        #region ACA_067
        public ActionResult ACA_067()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_067_Rpt reportMatutina = new ACA_067_Rpt();
            ACA_067_Vespertina_Rpt reportVespertina = new ACA_067_Vespertina_Rpt();

            reportMatutina.p_IdEmpresa.Value = model.IdEmpresa;
            reportMatutina.p_IdAnio.Value = model.IdAnio;
            reportMatutina.p_IdSede.Value = model.IdSede;
            reportMatutina.p_IdNivel.Value = model.IdNivel;
            reportMatutina.p_IdJornada.Value = model.IdJornada;
            reportMatutina.p_IdCurso.Value = model.IdCurso;
            reportMatutina.p_IdParalelo.Value = model.IdParalelo;
            reportMatutina.p_MostrarRetirados.Value = model.mostrarAnulados;
            reportMatutina.usuario = SessionFixed.IdUsuario;
            reportMatutina.empresa = SessionFixed.NomEmpresa;

            reportVespertina.p_IdEmpresa.Value = model.IdEmpresa;
            reportVespertina.p_IdAnio.Value = model.IdAnio;
            reportVespertina.p_IdSede.Value = model.IdSede;
            reportVespertina.p_IdNivel.Value = model.IdNivel;
            reportVespertina.p_IdJornada.Value = model.IdJornada;
            reportVespertina.p_IdCurso.Value = model.IdCurso;
            reportVespertina.p_IdParalelo.Value = model.IdParalelo;
            reportVespertina.p_MostrarRetirados.Value = model.mostrarAnulados;
            reportVespertina.usuario = SessionFixed.IdUsuario;
            reportVespertina.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportMatutina = reportMatutina;
            ViewBag.ReportVespertina = reportVespertina;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_067(cl_filtros_Info model)
        {
            ACA_067_Rpt reportMatutina = new ACA_067_Rpt();
            ACA_067_Vespertina_Rpt reportVespertina = new ACA_067_Vespertina_Rpt();

            reportMatutina.p_IdEmpresa.Value = model.IdEmpresa;
            reportMatutina.p_IdAnio.Value = model.IdAnio;
            reportMatutina.p_IdSede.Value = model.IdSede;
            reportMatutina.p_IdNivel.Value = model.IdNivel;
            reportMatutina.p_IdJornada.Value = model.IdJornada;
            reportMatutina.p_IdCurso.Value = model.IdCurso;
            reportMatutina.p_IdParalelo.Value = model.IdParalelo;
            reportMatutina.p_MostrarRetirados.Value = model.mostrarAnulados;
            reportMatutina.usuario = SessionFixed.IdUsuario;
            reportMatutina.empresa = SessionFixed.NomEmpresa;

            reportVespertina.p_IdEmpresa.Value = model.IdEmpresa;
            reportVespertina.p_IdAnio.Value = model.IdAnio;
            reportVespertina.p_IdSede.Value = model.IdSede;
            reportVespertina.p_IdNivel.Value = model.IdNivel;
            reportVespertina.p_IdJornada.Value = model.IdJornada;
            reportVespertina.p_IdCurso.Value = model.IdCurso;
            reportVespertina.p_IdParalelo.Value = model.IdParalelo;
            reportVespertina.p_MostrarRetirados.Value = model.mostrarAnulados;
            reportVespertina.usuario = SessionFixed.IdUsuario;
            reportVespertina.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportMatutina = reportMatutina;
            ViewBag.ReportVespertina = reportVespertina;

            return View(model);
        }
        #endregion

        #region ACA_068
        public ActionResult ACA_068()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_068_Rpt report = new ACA_068_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_068.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_068");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_068(cl_filtros_Info model)
        {
            ACA_068_Rpt report = new ACA_068_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_068.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_068");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;


            return View(model);
        }
        #endregion

        #region ACA_069
        public ActionResult ACA_069()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_069_Rpt report = new ACA_069_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_069.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_069");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_069(cl_filtros_Info model)
        {
            ACA_069_Rpt report = new ACA_069_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_069.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_069");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;


            return View(model);
        }
        #endregion

        #region ACA_070
        public ActionResult ACA_070()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.mostrarAnulados = false;

            ACA_070_Rpt report = new ACA_070_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_070.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_070");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_070(cl_filtros_Info model)
        {
            ACA_070_Rpt report = new ACA_070_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_070.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_070");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;


            return View(model);
        }
        #endregion

        #region ACA_071
        public ActionResult ACA_071()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdNivel = 0;
            model.IdJornada = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;
            model.IdAlumno = 0;
            model.mostrarAnulados = false;

            ACA_071_Rpt report = new ACA_071_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_071.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_071");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_071(cl_filtros_Info model)
        {
            ACA_071_Rpt report = new ACA_071_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_071.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_071");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_MostrarRetirado.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;


            return View(model);
        }
        #endregion

        #region ACA_072
        public ActionResult ACA_072()
        {
            aca_MatriculaCalificacion_Info model = new aca_MatriculaCalificacion_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);

            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacion.GetList_Combos_TutorCalificacion(model.IdEmpresa, model.IdAnio, model.IdSede, IdProfesor, true);
            Lista_CombosCalificaciones.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ACA_028_Rpt report = new ACA_028_Rpt();

            #region Cargo diseño desde base
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_072.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(IdEmpresa, "ACA_072");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_028(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_072(aca_MatriculaCalificacion_Info model)
        {
            ACA_028_Rpt report = new ACA_028_Rpt();

            #region Cargo diseño desde base
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_072.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(IdEmpresa, "ACA_072");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            cargar_combos_ACA_028(model);
            return View(model);
        }

        #endregion

        #region ACA_073
        public ActionResult ACA_073()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;

            ACA_073_Rpt report = new ACA_073_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            ACA_073_Ministerio_Rpt reportMinisterio = new ACA_073_Ministerio_Rpt();
            reportMinisterio.p_IdEmpresa.Value = model.IdEmpresa;
            reportMinisterio.p_IdSede.Value = model.IdSede;
            reportMinisterio.p_IdAnio.Value = model.IdAnio;
            reportMinisterio.p_IdNivel.Value = model.IdNivel;
            reportMinisterio.p_IdJornada.Value = model.IdJornada;
            reportMinisterio.p_IdCurso.Value = model.IdCurso;
            reportMinisterio.p_IdParalelo.Value = model.IdParalelo;
            reportMinisterio.p_MostrarRetirados.Value = model.mostrarAnulados;
            reportMinisterio.usuario = SessionFixed.IdUsuario;
            reportMinisterio.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportMinisterio = reportMinisterio;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_073(cl_filtros_Info model)
        {
            ACA_073_Rpt report = new ACA_073_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            ACA_073_Ministerio_Rpt reportMinisterio = new ACA_073_Ministerio_Rpt();
            reportMinisterio.p_IdEmpresa.Value = model.IdEmpresa;
            reportMinisterio.p_IdSede.Value = model.IdSede;
            reportMinisterio.p_IdAnio.Value = model.IdAnio;
            reportMinisterio.p_IdNivel.Value = model.IdNivel;
            reportMinisterio.p_IdJornada.Value = model.IdJornada;
            reportMinisterio.p_IdCurso.Value = model.IdCurso;
            reportMinisterio.p_IdParalelo.Value = model.IdParalelo;
            reportMinisterio.p_MostrarRetirados.Value = model.mostrarAnulados;
            reportMinisterio.usuario = SessionFixed.IdUsuario;
            reportMinisterio.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportMinisterio = reportMinisterio;

            return View(model);
        }

        #endregion

        #region ACA_074
        public ActionResult ACA_074()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;
            ACA_074_Rpt report = new ACA_074_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_074.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_074");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_074(cl_filtros_Info model)
        {
            ACA_074_Rpt report = new ACA_074_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_074.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_074");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            return View(model);
        }

        #endregion

        #region ACA_075
        public ActionResult ACA_075()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.mostrarAnulados = false;

            ACA_075_Rpt report = new ACA_075_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            ACA_075_Ministerio_Rpt reportMinisterio = new ACA_075_Ministerio_Rpt();
            reportMinisterio.p_IdEmpresa.Value = model.IdEmpresa;
            reportMinisterio.p_IdSede.Value = model.IdSede;
            reportMinisterio.p_IdAnio.Value = model.IdAnio;
            reportMinisterio.p_IdNivel.Value = model.IdNivel;
            reportMinisterio.p_IdJornada.Value = model.IdJornada;
            reportMinisterio.p_IdCurso.Value = model.IdCurso;
            reportMinisterio.p_IdParalelo.Value = model.IdParalelo;
            reportMinisterio.p_MostrarRetirados.Value = model.mostrarAnulados;
            reportMinisterio.usuario = SessionFixed.IdUsuario;
            reportMinisterio.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportMinisterio = reportMinisterio;

            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_075(cl_filtros_Info model)
        {
            ACA_075_Rpt report = new ACA_075_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_MostrarRetirados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            ACA_075_Ministerio_Rpt reportMinisterio = new ACA_075_Ministerio_Rpt();
            reportMinisterio.p_IdEmpresa.Value = model.IdEmpresa;
            reportMinisterio.p_IdSede.Value = model.IdSede;
            reportMinisterio.p_IdAnio.Value = model.IdAnio;
            reportMinisterio.p_IdNivel.Value = model.IdNivel;
            reportMinisterio.p_IdJornada.Value = model.IdJornada;
            reportMinisterio.p_IdCurso.Value = model.IdCurso;
            reportMinisterio.p_IdParalelo.Value = model.IdParalelo;
            reportMinisterio.p_MostrarRetirados.Value = model.mostrarAnulados;
            reportMinisterio.usuario = SessionFixed.IdUsuario;
            reportMinisterio.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportMinisterio = reportMinisterio;

            return View(model);
        }

        #endregion

        #region ACA_076
        private void cargar_combos_ACA_046(aca_MatriculaCalificacionParcial_Info model)
        {
            aca_CatalogoTipo_Bus bus_catalogo = new aca_CatalogoTipo_Bus();
            var lst_quimestre = new List<aca_CatalogoTipo_Info>();
            var quim1 = bus_catalogo.GetInfo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1));
            lst_quimestre.Add(quim1);
            var quim2 = bus_catalogo.GetInfo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2));
            lst_quimestre.Add(quim2);
            ViewBag.lst_quimestre = lst_quimestre;
        }
        public ActionResult ACA_076()
        {
            aca_MatriculaCalificacionParcial_Info model = new aca_MatriculaCalificacionParcial_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
            model.IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1);
            model.MostrarRetirados = false;
            ACA_076_Rpt report = new ACA_076_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_076.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_076");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_046(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ACA_076(aca_MatriculaCalificacionParcial_Info model)
        {
            ACA_076_Rpt report = new ACA_076_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "ACA_076.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "ACA_076");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdCatalogoTipo.Value = model.IdCatalogoTipo;
            report.p_MostrarRetirados.Value = model.MostrarRetirados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            cargar_combos_ACA_046(model);
            return View(model);
        }
        #endregion
    }

    public class aca_ReporteCalificacion_Combos_List
    {
        string Variable = "aca_ReporteCalificacion_Combos_Info";
        public List<aca_MatriculaCalificacion_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_MatriculaCalificacion_Info> list = new List<aca_MatriculaCalificacion_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_MatriculaCalificacion_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_MatriculaCalificacion_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
    
    public class aca_ReporteCalificacionCualitativa_Combos_List
    {
        string Variable = "aca_ReporteCalificacionCualitativa_Info";
        public List<aca_MatriculaCalificacionCualitativa_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_MatriculaCalificacionCualitativa_Info> list = new List<aca_MatriculaCalificacionCualitativa_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_MatriculaCalificacionCualitativa_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_MatriculaCalificacionCualitativa_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class aca_ReporteCalificacionParticipacion_Combos_List
    {
        string Variable = "aca_ReporteCalificacionParticipacion_Info";
        public List<aca_MatriculaCalificacion_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_MatriculaCalificacion_Info> list = new List<aca_MatriculaCalificacion_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_MatriculaCalificacion_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_MatriculaCalificacion_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}