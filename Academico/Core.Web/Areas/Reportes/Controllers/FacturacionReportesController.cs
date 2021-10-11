using Core.Bus.Academico;
using Core.Bus.Facturacion;
using Core.Bus.General;
using Core.Bus.Inventario;
using Core.Web.Reportes.Facturacion;
using Core.Info.Academico;
using Core.Info.Facturacion;
using Core.Info.General;
using Core.Info.Helps;
using Core.Info.Inventario;
using Core.Info.Reportes.Facturacion;
using Core.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Reportes.Controllers
{
    public class FacturacionReportesController : Controller
    {
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
        in_Producto_Bus bus_producto = new in_Producto_Bus();
        fa_factura_Bus bus_factura = new fa_factura_Bus();
        fa_catalogo_Bus bus_catalogo = new fa_catalogo_Bus();
        tb_sis_reporte_x_tb_empresa_Bus bus_rep_x_emp = new tb_sis_reporte_x_tb_empresa_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        in_Marca_Bus bus_marca = new in_Marca_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        fa_cliente_contactos_Bus bus_cliente_contacto = new fa_cliente_contactos_Bus();
        string RootReporte = System.IO.Path.GetTempPath() + "Rpt_Facturacion.repx";

        #region Metodos ComboBox bajo demanda
        public ActionResult CmbCliente_Facturacion()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info();
            return PartialView("_CmbCliente_Facturacion", model);
        }
        public ActionResult CmbTipoNota()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info();
            return PartialView("_CmbTipoNota", model);
        }
        
        public List<tb_persona_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }

        public ActionResult CmbProductoHijo_Facturacion()
        {
            SessionFixed.IdProducto_padre_dist = (!string.IsNullOrEmpty(Request.Params["IdProductoPadre"])) ? Request.Params["IdProductoPadre"].ToString() : "-1";
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info();
            return PartialView("_CmbProductoHijo_Facturacion", model);
        }
        public List<in_Producto_Info> get_list_ProductoHijo_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_producto.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoBusquedaProducto.PORSUCURSAL, cl_enumeradores.eModulo.INV, 0, Convert.ToInt32(SessionFixed.IdSucursal));
        }
        public in_Producto_Info get_info_producto_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_producto.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion
        #region Combo Alumno
        public ActionResult CmbAlumno_Facturacion()
        {
            decimal model = new decimal();
            return PartialView("_CmbAlumno_Facturacion", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda_alumno(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda_alumno(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO.ToString());
        }
        #endregion

        public ActionResult FAC_001(int IdSucursal = 0, int IdBodega = 0, decimal IdCbteVta = 0)
        {
            FAC_001_Rpt model = new FAC_001_Rpt();            
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "FAC_001.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "FAC_001");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                model.LoadLayout(RootReporte);
            }
            #endregion
            model.p_IdEmpresa.Value = IdEmpresa;
            model.p_IdBodega.Value = IdBodega;
            model.p_IdSucursal.Value = IdSucursal;
            model.p_IdCbteVta.Value = IdCbteVta;
            model.p_mostrar_cuotas.Value = bus_factura.MostrarCuotasRpt(IdEmpresa, IdSucursal, IdBodega, IdCbteVta);
            model.RequestParameters = false;
            model.DefaultPrinterSettingsUsing.UsePaperKind = false;
            //bus_factura.modificarEstadoImpresion(Convert.ToInt32(SessionFixed.IdEmpresa), IdSucursal, IdBodega, IdCbteVta, true);
            
            return View(model);
        }

        public ActionResult FAC_002(int IdSucursal = 0, int IdBodega = 0, decimal IdCbteVta = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
        
            FAC_002_Info model = new FAC_002_Info();
            model.IdEmpresa = IdEmpresa;
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = new aca_AnioLectivo_Info();
            info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);

            model.IdAnio = info_anio == null ? 0 : info_anio.IdAnio;
            model.fecha_ini = DateTime.Now.Date.AddMonths(-1);
            model.fecha_fin = DateTime.Now.Date;
            
            model.IdSucursal = IdSucursal;
            model.IdBodega = IdBodega;
            model.IdCbteVta = IdCbteVta;

            if (model.IdCbteVta != 0)
            {
                var info_factura = bus_factura.get_info(IdEmpresa, IdSucursal, IdBodega, IdCbteVta);
                var info_cliente = bus_cliente_contacto.get_info(IdEmpresa, info_factura.IdCliente, 1);
                model.Correos = (info_cliente == null ? "" : info_cliente.Correo);
            }

            FAC_002_Rpt report = new FAC_002_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "FAC_002.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "FAC_002");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = IdEmpresa;
            report.p_IdBodega.Value = IdBodega;
            report.p_IdSucursal.Value = IdSucursal;
            report.p_IdCbteVta.Value = IdCbteVta;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.RequestParameters = false;

            ViewBag.Report = report;

            return View(model);
        }
        [HttpPost]
        public ActionResult FAC_002(FAC_002_Info model)
        {
            FAC_002_Rpt report = new FAC_002_Rpt();
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "FAC_002.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "FAC_002");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            if (model.IdCbteVta != 0)
            {
                var info_factura = bus_factura.get_info(model.IdEmpresa, model.IdSucursal, model.IdBodega, model.IdCbteVta);
                var info_cliente = bus_cliente_contacto.get_info(model.IdEmpresa, info_factura.IdCliente, 1);
                model.Correos = (info_cliente == null ? "" : info_cliente.Correo);
            }

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdAnio.Value = model.IdAnio;
            report.p_IdSede.Value = model.IdSede;
            report.p_IdNivel.Value = model.IdNivel;
            report.p_IdJornada.Value = model.IdJornada;
            report.p_IdCurso.Value = model.IdCurso;
            report.p_IdParalelo.Value = model.IdParalelo;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_IdBodega.Value = model.IdBodega;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_IdCbteVta.Value = model.IdCbteVta;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;

            ViewBag.Report = report;

            return View(model);
        }
        public ActionResult FAC_003(int IdSucursal = 0, int IdBodega = 0, decimal IdNota = 0)
        {
            FAC_003_Rpt model = new FAC_003_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "FAC_003.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "FAC_003");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                model.LoadLayout(RootReporte);
            }
            #endregion
            model.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.p_IdBodega.Value = IdBodega;
            model.p_IdSucursal.Value = IdSucursal;
            model.p_IdNota.Value = IdNota;
            model.usuario = SessionFixed.IdUsuario;
            model.empresa = SessionFixed.NomEmpresa;
            model.RequestParameters = false;
            return View(model);
        }

        public ActionResult FAC_004(int IdSucursal = 0, int IdBodega = 0, decimal IdCbteVta = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            FAC_004_Rpt model = new FAC_004_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "FAC_004.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "FAC_004");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                model.LoadLayout(RootReporte);
            }
            #endregion
            model.p_IdEmpresa.Value = IdEmpresa;
            model.p_IdBodega.Value = IdBodega;
            model.p_IdSucursal.Value = IdSucursal;
            model.p_IdCbteVta.Value = IdCbteVta;
            model.RequestParameters = false;

            return View(model);
        }

        #region FAC_005
        private void CargarCombosFAC_005()
        {
            var lstNaturaleza = new Dictionary<string, string>();
            lstNaturaleza.Add("", "TODAS");
            lstNaturaleza.Add("INT", "INTERNAS");
            lstNaturaleza.Add("SRI", "SRI");
            ViewBag.lstNaturaleza = lstNaturaleza;

            var lstCreDeb = new Dictionary<string, string>();
            lstCreDeb.Add("C","CREDITO");
            lstCreDeb.Add("D", "DEBITO");
            ViewBag.lstCreDeb = lstCreDeb;
        }
        public ActionResult FAC_005(int IdEmpresa = 0, int IdTipoNota = 0, string NaturalezaNota = null)
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.fecha_ini = DateTime.Now.AddMonths(-1);
            model.fecha_fin = DateTime.Now;
            model.CreDeb = "C";
            model.NaturalezaNota = null;
            model.IdTipoNota = IdTipoNota;

            FAC_005_Rpt report = new FAC_005_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaDesde.Value = model.fecha_ini;
            report.p_FechaHasta.Value = model.fecha_fin;
            report.p_Naturaleza.Value = model.NaturalezaNota;
            report.p_CreDeb.Value = model.CreDeb;
            report.p_IdTipoNota.Value = model.IdTipoNota ?? 0;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            CargarCombosFAC_005();

            FAC_005_Resumen_Rpt reportResumen = new FAC_005_Resumen_Rpt();
            reportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            reportResumen.p_FechaDesde.Value = model.fecha_ini;
            reportResumen.p_FechaHasta.Value = model.fecha_fin;
            reportResumen.p_Naturaleza.Value = model.NaturalezaNota;
            reportResumen.p_CreDeb.Value = model.CreDeb;
            reportResumen.p_IdTipoNota.Value = model.IdTipoNota ?? 0;
            reportResumen.usuario = SessionFixed.IdUsuario;
            reportResumen.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportResumen = reportResumen;

            return View(model);
        }
        [HttpPost]
        public ActionResult FAC_005(cl_filtros_facturacion_Info model)
        {
            FAC_005_Rpt report = new FAC_005_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaDesde.Value = model.fecha_ini;
            report.p_FechaHasta.Value = model.fecha_fin;
            report.p_Naturaleza.Value = model.NaturalezaNota;
            report.p_IdTipoNota.Value = model.IdTipoNota;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            report.p_CreDeb.Value = model.CreDeb;
            ViewBag.Report = report;
            CargarCombosFAC_005();

            FAC_005_Resumen_Rpt reportResumen = new FAC_005_Resumen_Rpt();
            reportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            reportResumen.p_FechaDesde.Value = model.fecha_ini;
            reportResumen.p_FechaHasta.Value = model.fecha_fin;
            reportResumen.p_Naturaleza.Value = model.NaturalezaNota;
            reportResumen.p_CreDeb.Value = model.CreDeb;
            reportResumen.p_IdTipoNota.Value = model.IdTipoNota ?? 0;
            reportResumen.usuario = SessionFixed.IdUsuario;
            reportResumen.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportResumen = reportResumen;

            return View(model);
        }
        #endregion

        #region FAC_006
        private void cargar_FAC_006(cl_filtros_facturacion_Info model)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            lst_sucursal.Add(new tb_sucursal_Info
            {
                IdSucursal = 0,
                Su_Descripcion = "Todas"
            });
            ViewBag.lst_sucursal = lst_sucursal;

            var lst_formapago = bus_catalogo.get_list((int)cl_enumeradores.eTipoCatalogoFact.FormaDePago, false);
            lst_formapago.Add(new fa_catalogo_Info
            {
                IdCatalogo = "",
                Nombre = "TODAS"
            });

            ViewBag.lst_formapago = lst_formapago;

        }

        public ActionResult FAC_006()
        {

            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdAlumno = 0,
                IdCatalogo_FormaPago = ""
            };


            cargar_FAC_006(model);
            FAC_006_Rpt report = new FAC_006_Rpt();
            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_MostrarAnulados.Value = model.mostrarAnulados;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            FAC_006_Resumen_Rpt reportResumen = new FAC_006_Resumen_Rpt();
            reportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            reportResumen.p_IdSucursal.Value = model.IdSucursal;
            reportResumen.p_IdAlumno.Value = model.IdAlumno;
            reportResumen.p_fecha_ini.Value = model.fecha_ini;
            reportResumen.p_fecha_fin.Value = model.fecha_fin;
            reportResumen.p_MostrarAnulados.Value = model.mostrarAnulados;
            reportResumen.usuario = SessionFixed.IdUsuario;
            reportResumen.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportResumen = reportResumen;

            return View(model);
        }
        [HttpPost]
        public ActionResult FAC_006(cl_filtros_facturacion_Info model)
        {
            FAC_006_Rpt report = new FAC_006_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_IdSucursal.Value = model.IdSucursal;
            report.p_IdAlumno.Value = model.IdAlumno;
            report.p_fecha_ini.Value = model.fecha_ini;
            report.p_fecha_fin.Value = model.fecha_fin;
            report.p_MostrarAnulados.Value = model.mostrarAnulados;
            cargar_FAC_006(model);
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            FAC_006_Resumen_Rpt reportResumen = new FAC_006_Resumen_Rpt();
            reportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            reportResumen.p_IdSucursal.Value = model.IdSucursal;
            reportResumen.p_IdAlumno.Value = model.IdAlumno;
            reportResumen.p_fecha_ini.Value = model.fecha_ini;
            reportResumen.p_fecha_fin.Value = model.fecha_fin;
            reportResumen.p_MostrarAnulados.Value = model.mostrarAnulados;
            reportResumen.usuario = SessionFixed.IdUsuario;
            reportResumen.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportResumen = reportResumen;

            return View(model);
        }
        #endregion

        #region FAC_007
        private void CargarCombosFAC_007()
        {
            var lstEmpresa = bus_empresa.get_list(false);
            lstEmpresa.Add(new tb_empresa_Info
            {
                IdEmpresa = 0,
                em_nombre = "TODAS"
            });
            ViewBag.lstEmpresa = lstEmpresa.OrderBy(q=> q.IdEmpresa).ToList();
        }
        public ActionResult FAC_007()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info();
            model.IdEmpresa = 0;
            FAC_007_Rpt report = new FAC_007_Rpt();

            report.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.p_IdEmpresa_rol.Value = model.IdEmpresa;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            CargarCombosFAC_007();

            FAC_007_Resumen_Rpt reportResumen = new FAC_007_Resumen_Rpt();
            reportResumen.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            reportResumen.p_FechaIni.Value = model.fecha_ini;
            reportResumen.p_FechaFin.Value = model.fecha_fin;
            reportResumen.p_IdEmpresa_rol.Value = model.IdEmpresa;
            reportResumen.usuario = SessionFixed.IdUsuario;
            reportResumen.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportResumen = reportResumen;

            return View(model);
        }
        [HttpPost]
        public ActionResult FAC_007(cl_filtros_facturacion_Info model)
        {
            FAC_007_Rpt report = new FAC_007_Rpt();

            #region Cargo diseño desde base
            var reporte = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "FAC_007");
            if (reporte != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, reporte.ReporteDisenio);
                report.LoadLayout(RootReporte);
            }
            #endregion
            report.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            report.p_FechaIni.Value = model.fecha_ini;
            report.p_FechaFin.Value = model.fecha_fin;
            report.p_IdEmpresa_rol.Value = model.IdEmpresa;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            CargarCombosFAC_007();

            FAC_007_Resumen_Rpt reportResumen = new FAC_007_Resumen_Rpt();
            reportResumen.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            reportResumen.p_FechaIni.Value = model.fecha_ini;
            reportResumen.p_FechaFin.Value = model.fecha_fin;
            reportResumen.p_IdEmpresa_rol.Value = model.IdEmpresa;
            reportResumen.usuario = SessionFixed.IdUsuario;
            reportResumen.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportResumen = reportResumen;

            return View(model);
        }
        #endregion

        #region FAC_008
        public ActionResult FAC_008(int IdEmpresa, int IdSucursal = 0, int IdBodega = 0, decimal IdNota = 0)
        {
            FAC_008_Rpt model = new FAC_008_Rpt();
            #region Cargo diseño desde base
            string RootReporte = System.IO.Path.GetTempPath() + Guid.NewGuid() + "FAC_008.repx";
            var rptFormat = bus_rep_x_emp.GetInfo(Convert.ToInt32(SessionFixed.IdEmpresa), "FAC_008");
            if (rptFormat != null)
            {
                System.IO.File.WriteAllBytes(RootReporte, rptFormat.ReporteDisenio);
                model.LoadLayout(RootReporte);
            }
            #endregion
            model.p_IdEmpresa.Value = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.p_IdSucursal.Value = IdSucursal;
            model.p_IdBodega.Value = IdBodega;
            model.p_IdNota.Value = IdNota;
            model.usuario = SessionFixed.IdUsuario;
            model.empresa = SessionFixed.NomEmpresa;
            return View(model);
        }
        #endregion

        #region FAC_009
        public ActionResult FAC_009()
        {
            cl_filtros_facturacion_Info model = new cl_filtros_facturacion_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            model.fecha_ini = DateTime.Now.AddMonths(-1);
            model.fecha_fin = DateTime.Now;
            model.CreDeb = "C";
            model.NaturalezaNota = null;

            FAC_009_Rpt report = new FAC_009_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaDesde.Value = model.fecha_ini;
            report.p_FechaHasta.Value = model.fecha_fin;
            report.p_IdTipoNota.Value = model.IdTipoNota ?? 0;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;
            
            
            FAC_009_Resumen_Rpt reportResumen = new FAC_009_Resumen_Rpt();
            reportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            reportResumen.p_FechaDesde.Value = model.fecha_ini;
            reportResumen.p_FechaHasta.Value = model.fecha_fin;
            reportResumen.p_IdTipoNota.Value = model.IdTipoNota ?? 0;
            reportResumen.usuario = SessionFixed.IdUsuario;
            reportResumen.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportResumen = reportResumen;
            
            return View(model);
        }
        [HttpPost]
        public ActionResult FAC_009(cl_filtros_facturacion_Info model)
        {
            FAC_009_Rpt report = new FAC_009_Rpt();

            report.p_IdEmpresa.Value = model.IdEmpresa;
            report.p_FechaDesde.Value = model.fecha_ini;
            report.p_FechaHasta.Value = model.fecha_fin;
            report.p_IdTipoNota.Value = model.IdTipoNota;
            report.usuario = SessionFixed.IdUsuario;
            report.empresa = SessionFixed.NomEmpresa;
            ViewBag.Report = report;

            
            FAC_009_Resumen_Rpt reportResumen = new FAC_009_Resumen_Rpt();
            reportResumen.p_IdEmpresa.Value = model.IdEmpresa;
            reportResumen.p_FechaDesde.Value = model.fecha_ini;
            reportResumen.p_FechaHasta.Value = model.fecha_fin;
            reportResumen.p_IdTipoNota.Value = model.IdTipoNota ?? 0;
            reportResumen.usuario = SessionFixed.IdUsuario;
            reportResumen.empresa = SessionFixed.NomEmpresa;
            ViewBag.ReportResumen = reportResumen;
            
            return View(model);
        }
        #endregion
    }
}