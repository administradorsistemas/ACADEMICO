using Core.Bus.Academico;
using Core.Bus.CuentasPorCobrar;
using Core.Bus.Facturacion;
using Core.Bus.General;
using Core.Bus.Reportes.CuentasPorCobrar;
using Core.Bus.SeguridadAcceso;
using Core.Info.Academico;
using Core.Info.CuentasPorCobrar;
using Core.Info.General;
using Core.Info.Helps;
using Core.Info.SeguridadAcceso;
using Core.Web.Helps;
using Core.Web.Reportes.CuentasPorCobrar;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Reportes.Controllers
{
    public class CuentasPorCobrarReportesController : Controller
    {
        tb_sis_reporte_x_tb_empresa_Bus bus_rep_x_emp = new tb_sis_reporte_x_tb_empresa_Bus();
        tb_persona_Bus BusPersona = new tb_persona_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        fa_cliente_contactos_Bus bus_cliente_contacto = new fa_cliente_contactos_Bus();
        fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        cxc_cobro_Bus bus_cobro = new cxc_cobro_Bus();

        #region Combos
        public ActionResult Cmb_Alumno()
        {
            decimal model = new decimal();
            return PartialView("_CmbAlumno", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda_alumno(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return BusPersona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda_alumno(ListEditItemRequestedByValueEventArgs args)
        {
            return BusPersona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO.ToString());
        }        
        #endregion

        #region CargarCombos
        private void CargarSucursal(cl_filtros_facturacion_Info model)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(model.IdEmpresa, false);
            lst_sucursal.Add(new Info.General.tb_sucursal_Info
            {
                IdSucursal = 0,
                Su_Descripcion = "TODAS"
            });
            ViewBag.lst_sucursal = lst_sucursal;
        }
        private void cargar_sucursal_check(int IdEmpresa, int[] intArray)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            if (intArray == null || intArray.Count() == 0)
            {
                lst_sucursal.Where(q => q.IdSucursal == Convert.ToInt32(SessionFixed.IdSucursal)).FirstOrDefault().Seleccionado = true;
            }
            else
                foreach (var item in lst_sucursal)
                {
                    item.Seleccionado = (intArray.Where(q => q == item.IdSucursal).Count() > 0 ? true : false);
                }
            ViewBag.lst_sucursal = lst_sucursal;
        }

        private void cargar_usuario_check(int IdEmpresa, string[] StringArray)
        {
            seg_usuario_Bus bus_usuario = new seg_usuario_Bus();
            List<seg_usuario_Info> lst_usuario = new List<seg_usuario_Info>();
           var infoUsuario = bus_usuario.get_info(SessionFixed.IdUsuario);

            if (infoUsuario !=null && infoUsuario.es_super_admin==true)
            {
                lst_usuario = bus_usuario.GetListCobradores(IdEmpresa);
            }
            else
            {
                lst_usuario.Add(infoUsuario);
            }
            
            if (StringArray == null || StringArray.Count() == 0)
            {
                lst_usuario.Where(q => q.IdUsuario == SessionFixed.IdUsuario).FirstOrDefault().Seleccionado = true;
            }
            else
                foreach (var item in lst_usuario)
                {
                    item.Seleccionado = (StringArray.Where(q => q == item.IdUsuario).Count() > 0 ? true : false);
                }
            ViewBag.lst_usuario = lst_usuario;
        }
        #endregion

        public ActionResult CXC_001()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdCliente = 0,
                IntArray = new int[] { Convert.ToInt32(SessionFixed.IdSucursal) }
            };
            CargarSucursal(model);
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            CXC_001_Rpt report = new CXC_001_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.IntArray = model.IntArray;
            report.p_IdCliente.Value = model.IdAlumno == null ? 0 : Convert.ToDecimal(model.IdAlumno);
            report.p_fecha_corte.Value = model.fecha_corte;
            report.p_mostrarSaldo0.Value = model.mostrarSaldo0;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_001(cl_filtros_facturacion_Info model)
        {
            CXC_001_Rpt report = new CXC_001_Rpt();
            CargarSucursal(model);
            report.IntArray = model.IntArray;
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdCliente.Value = model.IdAlumno == null ? 0 : Convert.ToDecimal(model.IdAlumno);
            report.p_fecha_corte.Value = model.fecha_corte;
            report.p_mostrarSaldo0.Value = model.mostrarSaldo0;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            cargar_sucursal_check(model.IdEmpresa, model.IntArray);
            ViewBag.Report = report;

            return View(model);
        }

        public ActionResult CXC_002(int IdSucursal = 0, decimal IdCobro = 0)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var info_cobro = bus_cobro.get_info(IdEmpresa, IdSucursal,IdCobro);
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = IdEmpresa,
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdSucursal = IdSucursal,
                IdAlumno = Convert.ToInt32(info_cobro.IdAlumno),
                IdCbteCble = IdCobro

            };

            tb_empresa_Bus busEmpresa = new tb_empresa_Bus();
            CXC_002_Bus bus_rpt = new CXC_002_Bus();
            CXC_002_Rpt Report = new CXC_002_Rpt();
            cxc_cobro_Bus busCobro = new cxc_cobro_Bus();
            CXC_002_Aplicaciones_Bus busRptDet = new CXC_002_Aplicaciones_Bus();

            Report.lst_rpt = bus_rpt.get_list(IdEmpresa, IdSucursal, IdCobro);
            Report.lst_det = busRptDet.get_list(IdEmpresa, IdSucursal, IdCobro);
            Report.infoEmpresa = busEmpresa.get_info(IdEmpresa);
            Report.Primero = Report.lst_rpt.FirstOrDefault();
            Report.Saldo = busCobro.GetSaldoAlumno(IdEmpresa, (Report.Primero == null ? 0 : Report.Primero.IdAlumno ?? 0), false).ToString("c2");
            Report.SaldoConDscto = busCobro.GetSaldoAlumno(IdEmpresa, (Report.Primero == null ? 0 : Report.Primero.IdAlumno ?? 0), true).ToString("c2");


            Report.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            Report.p_IdSucursal.Value = IdSucursal;
            Report.p_IdCobro.Value = IdCobro;
            Report.usuario = SessionFixed.IdUsuario;
            ViewBag.Report = Report;

            return View(model);
        }

        #region CXC_003
        private void CargarCombos_CXC_003(int IdEmpresa, string[] StringArray)
        {
            seg_usuario_Bus bus_usuario = new seg_usuario_Bus();
            List<seg_usuario_Info> lst_usuario = new List<seg_usuario_Info>();
            var infoUsuario = bus_usuario.get_info(SessionFixed.IdUsuario);

            if (infoUsuario != null && infoUsuario.es_super_admin == true)
            {
                lst_usuario = bus_usuario.GetListCobradores(IdEmpresa);
            }
            else
            {
                lst_usuario.Add(infoUsuario);
            }

            if (StringArray == null || StringArray.Count() == 0)
            {
                lst_usuario.Where(q => q.IdUsuario == SessionFixed.IdUsuario).FirstOrDefault().Seleccionado = true;
            }
            else
                foreach (var item in lst_usuario)
                {
                    item.Seleccionado = true;
                    // (StringArray.Where(q => q == item.IdUsuario).Count() > 0 ? true : false);
                }
            ViewBag.lst_usuario = lst_usuario;
        }

        public ActionResult CXC_003()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                fecha_ini = DateTime.Now.AddMonths(-1),
                fecha_fin = DateTime.Now,
                StringArray = new string[] { SessionFixed.IdUsuario }
            };

            CargarCombos_CXC_003(model.IdEmpresa, model.StringArray);
            CXC_003_Rpt report = new CXC_003_Rpt();
            
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.StringArray = model.StringArray;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_003(cl_filtros_Info model)
        {
            CXC_003_Rpt report = new CXC_003_Rpt();
          
            //CargarSucursal(model);

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.StringArray = model.StringArray;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            CargarCombos_CXC_003(model.IdEmpresa, model.StringArray);
            ViewBag.Report = report;

            return View(model);
        }

        #endregion

        #region CXC_004
        public ActionResult CXC_004()
        {
            CXC_004_Bus busrpt = new CXC_004_Bus();
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdCliente = 0,
                fecha_corte = DateTime.Now
            };
            var Lista = busrpt.Getlist_Reporte(model.IdEmpresa, SessionFixed.IdUsuario, model.fecha_corte);

            CXC_004_Rpt report = new CXC_004_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_FechaCorte.Value = model.fecha_corte;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.Lista = Lista;
            ViewBag.Report = report;

            CXC_004_Resumen_Rpt report2 = new CXC_004_Resumen_Rpt();

            report2.p_IdEmpresa.Value = model.IdEmpresa;
            report2.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report2.usuario = SessionFixed.IdUsuario;
            report2.empresa = SessionFixed.NomEmpresa;
            report2.p_FechaCorte.Value = model.fecha_corte;
            report2.Lista = Lista;
            ViewBag.ReportResumen = report2;


            CXC_004_Saldo_Rpt report3 = new CXC_004_Saldo_Rpt();

            report3.p_IdEmpresa.Value = model.IdEmpresa;
            report3.usuario = SessionFixed.IdUsuario;
            report3.empresa = SessionFixed.NomEmpresa;
            report3.p_FechaCorte.Value = model.fecha_corte;
            ViewBag.ReportSaldoEsperado = report3;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_004(cl_filtros_facturacion_Info model)
        {
            CXC_004_Bus busrpt = new CXC_004_Bus();
            var Lista = busrpt.Getlist_Reporte(model.IdEmpresa, SessionFixed.IdUsuario, model.fecha_corte);
            CXC_004_Rpt report = new CXC_004_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_FechaCorte.Value = model.fecha_corte;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.Lista = Lista;
            ViewBag.Report = report;

            CXC_004_Resumen_Rpt report2 = new CXC_004_Resumen_Rpt();

            report2.p_IdEmpresa.Value = model.IdEmpresa;
            report2.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report2.usuario = SessionFixed.IdUsuario;
            report2.p_FechaCorte.Value = model.fecha_corte;
            report2.empresa = SessionFixed.NomEmpresa;
            report2.Lista = Lista;
            ViewBag.ReportResumen = report2;

            CXC_004_Saldo_Rpt report3 = new CXC_004_Saldo_Rpt();

            report3.p_IdEmpresa.Value = model.IdEmpresa;
            report3.usuario = SessionFixed.IdUsuario;
            report3.empresa = SessionFixed.NomEmpresa;
            report3.p_FechaCorte.Value = model.fecha_corte;
            ViewBag.ReportSaldoEsperado = report3;

            return View(model);
        }
        #endregion

        public ActionResult CXC_005(int IdSucursal = 0, decimal IdLiquidacion = 0)
        {
            CXC_005_Rpt report = new CXC_005_Rpt();
            
            report.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            report.p_IdSucursal.Value = IdSucursal;
            report.p_IdLiquidacion.Value = IdLiquidacion;
            report.usuario = SessionFixed.IdUsuario.ToString();
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            return View(report);
        }

        #region CXC_006
        private void CargarCombos_CXC_006(int IdEmpresa, int[] intArray)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            if (intArray == null || intArray.Count() == 0)
            {
                lst_sucursal.Where(q => q.IdSucursal == Convert.ToInt32(SessionFixed.IdSucursal)).FirstOrDefault().Seleccionado = true;
            }
            else
                foreach (var item in lst_sucursal)
                {
                    item.Seleccionado = (intArray.Where(q => q == item.IdSucursal).Count() > 0 ? true : false);
                }
            ViewBag.lst_sucursal = lst_sucursal;
        }
        
        public ActionResult CXC_006()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdCliente = 0,

            };

            CXC_006_Rpt report = new CXC_006_Rpt();
            
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.IntArray = model.IntArray;
            report.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa ?? "0");
            ViewBag.Report = report;
            CargarCombos_CXC_006(model.IdEmpresa,model.IntArray);
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_006(cl_filtros_facturacion_Info model)
        {
            CXC_006_Rpt report = new CXC_006_Rpt();
            
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.IntArray = model.IntArray;
            report.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa ?? "0");
            ViewBag.Report = report;
            CargarCombos_CXC_006(model.IdEmpresa, model.IntArray);
            return View(model);
        }
        #endregion

        #region CXC_007
        public ActionResult CXC_007()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
            };

            CXC_007_Rpt report = new CXC_007_Rpt();
          
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaCorte.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;
            ViewBag.Report = report;

           CXC_007_Resumen_Rpt ReportResumen = new CXC_007_Resumen_Rpt();
            
            ReportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            ReportResumen.p_fechaCorte.Value = model.fecha_fin;
            ReportResumen.usuario = SessionFixed.IdUsuario;
            ReportResumen.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportResumen = ReportResumen;
            return View(model);
        }

        [HttpPost]
        public ActionResult CXC_007(cl_filtros_facturacion_Info model)
        {
            CXC_007_Rpt report = new CXC_007_Rpt();
            
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fechaCorte.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            report.RequestParameters = false;
            ViewBag.Report = report;

            CXC_007_Resumen_Rpt ReportResumen = new CXC_007_Resumen_Rpt();
            
            ReportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            ReportResumen.p_fechaCorte.Value = model.fecha_fin;
            ReportResumen.usuario = SessionFixed.IdUsuario;
            ReportResumen.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportResumen = ReportResumen;
            return View(model);
        }
        #endregion

        #region CXC_008
        public ActionResult CXC_008()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = new aca_AnioLectivo_Info();
            info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);

            model.IdAnio = info_anio == null ? 0 : info_anio.IdAnio;
            model.mostrarAnulados = true;
            model.CantMinima = 0;
            model.CantMaxima = 9999;
            CXC_008_Rpt report = new CXC_008_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_FechaCorte.Value = model.fecha_fin;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_CantMinima.Value = model.CantMinima;
            report.p_CantMaxima.Value = model.CantMaxima;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            CXC_008_Resumen_Rpt ReportResumen = new CXC_008_Resumen_Rpt();
            ReportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            ReportResumen.p_IdAnio.Value = model.IdAnio;
            ReportResumen.p_IdSede.Value = model.IdSede;
            ReportResumen.p_IdNivel.Value = model.IdNivel;
            ReportResumen.p_IdJornada.Value = model.IdJornada;
            ReportResumen.p_IdCurso.Value = model.IdCurso;
            ReportResumen.p_IdParalelo.Value = model.IdParalelo;
            ReportResumen.p_FechaCorte.Value = model.fecha_fin;
            ReportResumen.p_IdAlumno.Value = model.IdAlumno;
            ReportResumen.p_CantMinima.Value = model.CantMinima;
            ReportResumen.p_CantMaxima.Value = model.CantMaxima;
            ReportResumen.usuario = SessionFixed.IdUsuario;
            ReportResumen.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportResumen = ReportResumen;

            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_008(cl_filtros_Info model)
        {
            CXC_008_Rpt report = new CXC_008_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_FechaCorte.Value = model.fecha_fin;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_CantMinima.Value = model.CantMinima;
            report.p_CantMaxima.Value = model.CantMaxima;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            CXC_008_Resumen_Rpt ReportResumen = new CXC_008_Resumen_Rpt();
            ReportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            ReportResumen.p_IdAnio.Value = model.IdAnio;
            ReportResumen.p_IdSede.Value = model.IdSede;
            ReportResumen.p_IdNivel.Value = model.IdNivel;
            ReportResumen.p_IdJornada.Value = model.IdJornada;
            ReportResumen.p_IdCurso.Value = model.IdCurso;
            ReportResumen.p_IdParalelo.Value = model.IdParalelo;
            ReportResumen.p_FechaCorte.Value = model.fecha_fin;
            ReportResumen.p_IdAlumno.Value = model.IdAlumno;
            ReportResumen.p_CantMinima.Value = model.CantMinima;
            ReportResumen.p_CantMaxima.Value = model.CantMaxima;
            ReportResumen.usuario = SessionFixed.IdUsuario;
            ReportResumen.empresa = SessionFixed.NomEmpresa;

            ViewBag.ReportResumen = ReportResumen;

            return View(model);
        }
        #endregion

        #region CXC_009
        public ActionResult CXC_009()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = new aca_AnioLectivo_Info();
            info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);

            model.IdAnio = info_anio == null ? 0 : info_anio.IdAnio;
            model.mostrarAnulados = true;
            CXC_009_Rpt report = new CXC_009_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaIni.Value = model.fecha_ini.Date;
            report.p_FechaFin.Value = model.fecha_fin.Date;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;


            CXC_009_Resumen_Rpt ReportResumen = new CXC_009_Resumen_Rpt();

            ReportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            ReportResumen.p_FechaIni.Value = model.fecha_ini.Date;
            ReportResumen.p_FechaFin.Value = model.fecha_fin.Date;
            ReportResumen.usuario = SessionFixed.IdUsuario;
            ReportResumen.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportResumen = ReportResumen;

            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_009(cl_filtros_Info model)
        {
            CXC_009_Rpt report = new CXC_009_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaIni.Value = model.fecha_ini.Date;
            report.p_FechaFin.Value = model.fecha_fin.Date;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;


            CXC_009_Resumen_Rpt ReportResumen = new CXC_009_Resumen_Rpt();

            ReportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            ReportResumen.p_FechaIni.Value = model.fecha_ini.Date;
            ReportResumen.p_FechaFin.Value = model.fecha_fin.Date;
            ReportResumen.usuario = SessionFixed.IdUsuario;
            ReportResumen.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportResumen = ReportResumen;
            return View(model);
        }
        #endregion

        #region CXC_010
        public ActionResult CXC_010()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            CXC_010_Rpt report = new CXC_010_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_010(cl_filtros_Info model)
        {
            CXC_010_Rpt report = new CXC_010_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_fechaIni.Value = model.fecha_ini;
            report.p_fechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        #endregion

        #region CXC_011
        public ActionResult CXC_011(decimal IdAlumno = 0)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdAlumno = IdAlumno;
            var info_anio = new aca_AnioLectivo_Info();
            info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = info_anio == null ? 0 : info_anio.IdAnio;

            CXC_011_Rpt report = new CXC_011_Rpt();

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
            ViewBag.MostrarCorreo = false;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_011(cl_filtros_Info model)
        {
            CXC_011_Rpt report = new CXC_011_Rpt();

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
            ViewBag.MostrarCorreo = true;
            return View(model);
        }
        #endregion

        #region CXC_012
        public ActionResult CXC_012()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            CXC_012_Rpt report = new CXC_012_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_012(cl_filtros_Info model)
        {
            CXC_012_Rpt report = new CXC_012_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        #endregion

        #region CXC_013
        public ActionResult CXC_013(decimal IdAlumno = 0)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdAlumno = IdAlumno;
            var info_anio = new aca_AnioLectivo_Info();
            info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = info_anio == null ? 0 : info_anio.IdAnio;

            CXC_013_Rpt report = new CXC_013_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_FechaCorte.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            ViewBag.MostrarCorreo = false;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_013(cl_filtros_Info model)
        {
            CXC_013_Rpt report = new CXC_013_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_FechaCorte.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            ViewBag.MostrarCorreo = true;
            return View(model);
        }
        #endregion

        #region CXC_014
        public ActionResult CXC_014(decimal IdAlumno = 0)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdAlumno = IdAlumno;
            var info_anio = new aca_AnioLectivo_Info();
            info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = info_anio == null ? 0 : info_anio.IdAnio;

            CXC_014_Rpt report = new CXC_014_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_FechaCorte.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            ViewBag.MostrarCorreo = false;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_014(cl_filtros_Info model)
        {
            CXC_014_Rpt report = new CXC_014_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_FechaCorte.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            ViewBag.MostrarCorreo = true;
            return View(model);
        }
        #endregion

        #region CXC_015
        public ActionResult CXC_015(decimal IdAlumno = 0)
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdAlumno = IdAlumno;
            var info_anio = new aca_AnioLectivo_Info();
            info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            model.IdAnio = info_anio == null ? 0 : info_anio.IdAnio;

            CXC_015_Rpt report = new CXC_015_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_FechaCorte.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            ViewBag.MostrarCorreo = false;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_015(cl_filtros_Info model)
        {
            CXC_015_Rpt report = new CXC_015_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_FechaCorte.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            ViewBag.MostrarCorreo = true;
            return View(model);
        }
        #endregion

        #region CXC_016
        public ActionResult CXC_016(int IdEmpresa = 0, int IdPagare = 0)
        {
            cxc_Pagare_Info model = new cxc_Pagare_Info();
            model.IdEmpresa = (IdEmpresa == 0 ? Convert.ToInt32(SessionFixed.IdEmpresa) : IdEmpresa);
            model.IdPagare = IdPagare;

            CXC_016_Rpt report = new CXC_016_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdPagare.Value = model.IdPagare;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            ViewBag.MostrarCorreo = false;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_016(cl_filtros_Info model)
        {
            CXC_016_Rpt report = new CXC_016_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdPagare.Value = model.IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            ViewBag.MostrarCorreo = true;
            return View(model);
        }
        #endregion

        #region CXC_017
        public ActionResult CXC_017(int IdEmpresa = 0, int IdConvenio = 0)
        {
            cxc_Convenio_Info model = new cxc_Convenio_Info();
            model.IdEmpresa = (IdEmpresa==0 ? Convert.ToInt32(SessionFixed.IdEmpresa) : IdEmpresa);
            model.IdConvenio = IdConvenio;

            CXC_017_Rpt report = new CXC_017_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdConvenio.Value = model.IdConvenio;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            ViewBag.MostrarCorreo = false;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_017(cl_filtros_Info model)
        {
            CXC_017_Rpt report = new CXC_017_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdConvenio.Value = model.IdAlumno;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            ViewBag.MostrarCorreo = true;
            return View(model);
        }
        #endregion

        #region CXC_018
        public ActionResult CXC_018()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdAlumno = 0;
            model.fecha_ini = DateTime.Now.AddMonths(-1);
            model.fecha_fin = DateTime.Now;
            model.mostrarAnulados = false;

            CXC_018_Rpt report = new CXC_018_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_mostrarAnulados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_018(cl_filtros_Info model)
        {
            CXC_018_Rpt report = new CXC_018_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_mostrarAnulados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        #endregion

        #region CXC_019
        public ActionResult CXC_019()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdAlumno = 0;
            model.fecha_ini = DateTime.Now.AddMonths(-1);
            model.fecha_fin = DateTime.Now;
            model.mostrarAnulados = false;

            CXC_019_Rpt report = new CXC_019_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_mostrarAnulados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_019(cl_filtros_Info model)
        {
            CXC_019_Rpt report = new CXC_019_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_mostrarAnulados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        #endregion

        #region CXC_020
        public ActionResult CXC_020()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.IdAlumno = 0;
            model.fecha_ini = DateTime.Now.AddMonths(-1);
            model.fecha_fin = DateTime.Now;
            model.mostrarAnulados = false;

            CXC_020_Rpt report = new CXC_020_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_020(cl_filtros_Info model)
        {
            CXC_020_Rpt report = new CXC_020_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        #endregion

        #region CXC_021
        public ActionResult ComboBoxPartial_Anio()
        {
            return PartialView("_ComboBoxPartial_Anio", new aca_AnioLectivo_NivelAcademico_Jornada_Info());
        }
        public ActionResult ComboBoxPartial_Sede()
        {
            int IdAnio = (Request.Params["IdAnio"] != null) ? int.Parse(Request.Params["IdAnio"]) : -1;
            return PartialView("_ComboBoxPartial_Sede", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio });
        }

        public ActionResult CXC_021()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var info_AnioAdmision = bus_anio.GetInfo_AnioAdmision(model.IdEmpresa);
            model.IdAnio = (info_AnioAdmision == null ? 0 : info_AnioAdmision.IdAnio);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdAlumno = 0;
            model.fecha_ini = DateTime.Now.AddMonths(-1);
            model.fecha_fin = DateTime.Now;
            model.mostrarAnulados = false;

            CXC_021_Rpt report = new CXC_021_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_021(cl_filtros_Info model)
        {
            CXC_021_Rpt report = new CXC_021_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;
            return View(model);
        }
        #endregion

        #region CXC_022
        public ActionResult CXC_022()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdCliente = 0,
                fecha_corte = DateTime.Now
            };

            CXC_022_Rpt report = new CXC_022_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_FechaCorte.Value = model.fecha_corte;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            
            return View(model);
        }
        [HttpPost]
        public ActionResult CXC_022(cl_filtros_facturacion_Info model)
        {
            CXC_022_Rpt report = new CXC_022_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdUsuario.Value = SessionFixed.IdUsuario;
            report.p_FechaCorte.Value = model.fecha_corte;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            return View(model);
        }
        #endregion
    }
}