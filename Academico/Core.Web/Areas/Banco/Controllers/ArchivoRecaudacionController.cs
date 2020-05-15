﻿using Core.Bus.Academico;
using Core.Bus.Banco;
using Core.Bus.Contabilidad;
using Core.Bus.General;
using Core.Bus.SeguridadAcceso;
using Core.Info.Academico;
using Core.Info.Banco;
using Core.Info.General;
using Core.Info.Helps;
using Core.Web.Helps;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Banco.Controllers
{
    public class ArchivoRecaudacionController : Controller
    {
        #region Variables
        string rutafile = System.IO.Path.GetTempPath();
        ba_ArchivoRecaudacion_Bus bus_archivo = new ba_ArchivoRecaudacion_Bus();
        ba_ArchivoRecaudacionDet_Bus bus_archivo_det = new ba_ArchivoRecaudacionDet_Bus();
        ba_ArchivoRecaudacion_List Lista = new ba_ArchivoRecaudacion_List();
        ba_ArchivoRecaudacionDet_List Lista_det = new ba_ArchivoRecaudacionDet_List();
        ba_ArchivoRecaudacionDet_Saldo_List Lista_det_Saldo = new ba_ArchivoRecaudacionDet_Saldo_List();
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        tb_banco_procesos_bancarios_x_empresa_Bus bus_procesos_bancarios = new tb_banco_procesos_bancarios_x_empresa_Bus();
        ba_Banco_Cuenta_Bus bus_cuentas_bancarias = new ba_Banco_Cuenta_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        string mensaje = string.Empty;

        ba_Banco_Cuenta_Bus bus_banco_cuenta = new ba_Banco_Cuenta_Bus();
        ba_parametros_Bus bus_param = new ba_parametros_Bus();
        ba_TipoFlujo_Bus bus_flujo = new ba_TipoFlujo_Bus();

        string MensajeSuccess = "La transacción se ha realizado con éxito";
        tb_ColaImpresionDirecta_Bus bus_impresion = new tb_ColaImpresionDirecta_Bus();
        seg_usuario_Bus bus_usuario = new seg_usuario_Bus();
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        #endregion

        #region Index

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                fecha_ini = DateTime.Now.AddMonths(-1),
                fecha_fin = DateTime.Now
            };

            List<ba_ArchivoRecaudacion_Info> lista = bus_archivo.GetList(model.IdEmpresa, model.fecha_ini, model.fecha_fin, true);
            Lista.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Banco", "ArchivoRecaudacion", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            List<ba_ArchivoRecaudacion_Info> lista = bus_archivo.GetList(model.IdEmpresa, model.fecha_ini, model.fecha_fin, true);
            Lista.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Banco", "ArchivoRecaudacion", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_ArchivoRecaudacion(DateTime? fecha_ini, DateTime? fecha_fin, bool Nuevo = false, bool Modificar = false, bool Anular = false)
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            ViewBag.Nuevo = Nuevo;
            ViewBag.Modificar = Modificar;
            ViewBag.Anular = Anular;

            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.fecha_ini = fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(fecha_ini);
            ViewBag.fecha_fin = fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(fecha_fin);

            List<ba_ArchivoRecaudacion_Info> model = Lista.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_ArchivoRecaudacion", model);

        }
        #endregion
        #region Metodos
        private bool validar(ba_ArchivoRecaudacion_Info i_validar, ref string msg)
        {
            if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.Fecha, cl_enumeradores.eModulo.BANCO, i_validar.IdSucursal, ref msg))
            {
                return false;
            }

            var pro = bus_procesos_bancarios.get_info(i_validar.IdEmpresa, i_validar.IdProceso_bancario);
            //i_validar.Cod_Empresa = pro.Codigo_Empresa;

            return true;
        }
        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            var lst_cuenta_bancarias = bus_cuentas_bancarias.get_list(IdEmpresa, Convert.ToInt32(SessionFixed.IdSucursal), false);
            ViewBag.lst_cuenta_bancarias = lst_cuenta_bancarias;

            var lst_proceso = bus_procesos_bancarios.get_list(IdEmpresa, false);
            ViewBag.lst_proceso = lst_proceso;
        }

        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            ba_ArchivoRecaudacion_Info model = new ba_ArchivoRecaudacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                Fecha = DateTime.Now,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                Lst_det = new List<ba_ArchivoRecaudacionDet_Info>(),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal)
            };
            Lista_det.set_list(model.Lst_det, model.IdTransaccionSession);

            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(ba_ArchivoRecaudacion_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            model.Lst_det = Lista_det.get_list(model.IdTransaccionSession);

            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos();
                return View(model);
            }
            if (!bus_archivo.GuardarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdArchivo = model.IdArchivo, Exito = true });
        }
        public ActionResult Modificar(int IdEmpresa = 0, decimal IdArchivo = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            ba_ArchivoRecaudacion_Info model = bus_archivo.GetInfo(IdEmpresa, IdArchivo);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.Lst_det = bus_archivo_det.GetList(model.IdEmpresa, model.IdArchivo);
            Lista_det.set_list(model.Lst_det, model.IdTransaccionSession);

            cargar_combos();

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ba_ArchivoRecaudacion_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            model.Lst_det = Lista_det.get_list(model.IdTransaccionSession);

            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos();
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                return View(model);
            }
            if (!bus_archivo.ModificarDB(model))
            {
                cargar_combos();
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                return View(model);
            }

            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdArchivo = model.IdArchivo, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, decimal IdArchivo = 0)
        {
            ba_ArchivoRecaudacion_Info model = bus_archivo.GetInfo(IdEmpresa, IdArchivo);
            if (model == null)
                return RedirectToAction("Index");

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.Lst_det = bus_archivo_det.GetList(model.IdEmpresa, model.IdArchivo);
            Lista_det.set_list(model.Lst_det, model.IdTransaccionSession);
            cargar_combos();

            #region Validacion Periodo CXC
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.Fecha, cl_enumeradores.eModulo.BANCO, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ba_ArchivoRecaudacion_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            if (!bus_archivo.AnularDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Detalle Archivo
        [ValidateInput(false)]
        public ActionResult GridViewPartial_ArchivoRecaudacionDet()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var model = Lista_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_ArchivoRecaudacionDet", model);
        }

        public ActionResult GridViewPartial_ArchivoRecaudacion_Saldo()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var model = Lista_det_Saldo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_ArchivoRecaudacion_Saldo", model);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult EditingAddNew(string IDs = "", decimal IdTransaccionSession = 0, int IdEmpresa = 0)
        {
            if (IDs != "")
            {
                string[] array = IDs.Split(',');
                var Lista = Lista_det_Saldo.get_list(IdTransaccionSession);
                foreach (var item in array)
                {
                    var info_det = Lista.Where(q => q.IdEmpresa==IdEmpresa && q.IdMatricula == Convert.ToInt32(item)).FirstOrDefault();
                    if (info_det != null)
                    {
                        Lista_det.AddRow(info_det, IdTransaccionSession);
                    }
                }
            }
            
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ba_ArchivoRecaudacionDet_Info info_det)
        {

            if (ModelState.IsValid)
                Lista_det.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = Lista_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_ArchivoRecaudacionDet", model);
        }
        public ActionResult EditingDelete(decimal IdOrdenPago)
        {
            Lista_det.DeleteRow(IdOrdenPago, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = Lista_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_ArchivoRecaudacionDet", model);
        }
        #endregion

        #region Json
        public JsonResult GetListPorCruzar(int IdEmpresa = 0, decimal IdTransaccionSession = 0, int IdSucursal = 0)
        {
            //var lst = bus_archivo_det.get_list_con_saldo(IdEmpresa, 0, "PROVEE", 0, "APRO", SessionFixed.IdUsuario ?? " ", IdSucursal, false);
            var lst = new List<ba_ArchivoRecaudacionDet_Info>();
            Lista_det_Saldo.set_list(lst, IdTransaccionSession);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetValor(decimal IdTransaccionSession = 0)
        {
            double Valor = Math.Round(Lista_det.get_list(IdTransaccionSession).Sum(q => q.Valor), 2, MidpointRounding.AwayFromZero);
            double ValorProntoPago = Math.Round(Lista_det.get_list(IdTransaccionSession).Sum(q => q.Valor), 2, MidpointRounding.AwayFromZero);
            return Json(new { Valor = Valor, ValorProntoPago= ValorProntoPago }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Archivo

        public FileResult get_archivo(int IdEmpresa = 0, int IdArchivo = 0)
        {
            byte[] archivo;
            ba_ArchivoRecaudacion_Bus bus_tipo_file = new ba_ArchivoRecaudacion_Bus();

            var info_archivo = bus_archivo.GetInfo(IdEmpresa, IdArchivo);
            info_archivo.Lst_det = bus_archivo_det.GetList(IdEmpresa, IdArchivo);

            archivo = GetArchivo(info_archivo, info_archivo.Nom_Archivo);
            return File(archivo, "application/xml", info_archivo.Nom_Archivo + ".txt");
        }

        private byte[] GetMulticash(ba_ArchivoRecaudacion_Info info, string NombreArchivo)
        {
            try
            {
                System.IO.File.Delete(rutafile + NombreArchivo + ".txt");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(rutafile + NombreArchivo + ".txt", true))
                {
                    var ListaA = info.Lst_det.Where(v => v.Valor > 0).GroupBy(q => new { q.IdMatricula }).Select(q => new
                    {
                        Valor = q.Sum(g => g.Valor)
                    }).ToList();

                    var banco = bus_banco_cuenta.get_info(info.IdEmpresa, info.IdBanco);
                    //foreach (var item in info.Lst_det.Where(v => v.Valor > 0).ToList())
                    foreach (var item in ListaA)
                    {
                        string linea = "";
                        double valor = Convert.ToDouble(item.Valor);
                        double valorEntero = Math.Floor(valor);
                        double valorDecimal = Convert.ToDouble((valor - valorEntero).ToString("N2")) * 100;

                        linea += "PA\t";
                        linea += string.IsNullOrEmpty(banco.ba_Num_Cuenta) ? "" : banco.ba_Num_Cuenta.PadLeft(10, '0') + "\t";
                        /*linea += item.Secuencial_reg_x_proceso.ToString().PadLeft(7, ' ') + "\t";
                        linea += "\t";//COMPROBANTE DE PAGO
                        linea += (string.IsNullOrEmpty(item.num_cta_acreditacion) ? item.pe_cedulaRuc.Trim() : item.num_cta_acreditacion.Trim()) + "\t";
                        linea += "USD\t";
                        linea += (valorEntero.ToString() + valorDecimal.ToString().PadLeft(2, '0')).PadLeft(13, '0') + "\t";
                        linea += (string.IsNullOrEmpty(item.num_cta_acreditacion) ? "EFE" : "CTA") + "\t";
                        linea += (string.IsNullOrEmpty(item.num_cta_acreditacion) ? "0017" : item.CodigoLegalBanco.ToString().PadLeft(4, '0')) + "\t";
                        linea += (string.IsNullOrEmpty(item.num_cta_acreditacion) || string.IsNullOrEmpty(item.IdTipoCta_acreditacion_cat) ? "" : (item.IdTipoCta_acreditacion_cat.Trim() == "COR" ? "CTE" : item.IdTipoCta_acreditacion_cat)) + "\t";
                        linea += string.IsNullOrEmpty(item.num_cta_acreditacion) ? "" : item.num_cta_acreditacion.PadLeft(10, '0') + "\t";
                        linea += (item.IdTipoDocumento == "CED" ? "C" : (item.IdTipoDocumento == "RUC" ? "R" : "P")) + "\t";
                        linea += item.pe_cedulaRuc.Trim() + "\t";
                        linea += (string.IsNullOrEmpty(item.Nom_Beneficiario) ? "" : (item.Nom_Beneficiario.Length > 40 ? item.Nom_Beneficiario.Substring(0, 40) : item.Nom_Beneficiario.Trim())) + "\t";
                        linea += "\t";//(string.IsNullOrEmpty(item.pr_direccion) ? "" : (item.pr_direccion.Length > 40 ? item.pr_direccion.Substring(0, 40) : item.pr_direccion.Trim())) + "\t";
                        linea += "\t";//Ciudad
                        linea += "\t";//Telefono
                        linea += "\t";//Localidad
                        var Referencia = string.Empty;
                        foreach (var refe in info.Lst_det.Where(q => q.pe_cedulaRuc == item.pe_cedulaRuc).ToList())
                        {
                            if (!string.IsNullOrEmpty(refe.Referencia))
                                Referencia += ((string.IsNullOrEmpty(refe.Referencia) ? "" : "/") + refe.Referencia);
                        }
                        linea += (string.IsNullOrEmpty(Referencia) ? "" : (Referencia.Length > 200 ? Referencia.Substring(0, 200) : Referencia.Trim())) + "\t";
                        //linea += (string.IsNullOrEmpty(item.Referencia) ? "" : (item.Referencia.Length > 200 ? item.Referencia.Substring(0, 200) : item.Referencia.Trim())) + "\t";
                        linea += "|" + (string.IsNullOrEmpty(item.pr_correo) ? "" : (item.pr_correo.Trim().Length > 100 ? item.pr_correo.Trim().Substring(0, 100) : item.pr_correo.Trim())) + "\t";//Ref adicional
                        */
                        file.WriteLine(linea);
                    }
                }
                byte[] filebyte = System.IO.File.ReadAllBytes(rutafile + NombreArchivo + ".txt");
                return filebyte;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public byte[] GetArchivo(ba_ArchivoRecaudacion_Info info, string nombre_file)
        {
            try
            {
                return GetMulticash(info, nombre_file);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }

    public class ba_ArchivoRecaudacion_List
    {
        string Variable = "ba_ArchivoRecaudacion_Info";
        public List<    ba_ArchivoRecaudacion_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_ArchivoRecaudacion_Info> list = new List<ba_ArchivoRecaudacion_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_ArchivoRecaudacion_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ba_ArchivoRecaudacion_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class ba_ArchivoRecaudacionDet_Saldo_List
    {
        string Variable = "ba_ArchivoRecaudacionDet_Saldo_Info";
        public List<ba_ArchivoRecaudacionDet_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_ArchivoRecaudacionDet_Info> list = new List<ba_ArchivoRecaudacionDet_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_ArchivoRecaudacionDet_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }
        public void set_list(List<ba_ArchivoRecaudacionDet_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
    public class ba_ArchivoRecaudacionDet_List
    {
        string Variable = "ba_ArchivoRecaudacionDet_Info";
        public List<ba_ArchivoRecaudacionDet_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_ArchivoRecaudacionDet_Info> list = new List<ba_ArchivoRecaudacionDet_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_ArchivoRecaudacionDet_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ba_ArchivoRecaudacionDet_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(ba_ArchivoRecaudacionDet_Info info_det, decimal IdTransaccionSession)
        {
            List<ba_ArchivoRecaudacionDet_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            if (list.Where(q => q.IdMatricula == info_det.IdMatricula).Count() == 0)
                list.Add(info_det);
        }

        public void UpdateRow(ba_ArchivoRecaudacionDet_Info info_det, decimal IdTransaccionSession)
        {
            ba_ArchivoRecaudacionDet_Info edited_info = get_list(IdTransaccionSession).Where(m => m.IdMatricula == info_det.IdMatricula).First();
            if (edited_info != null)
            {
                edited_info.Valor = info_det.Valor;
            }
        }

        public void DeleteRow(decimal IdOrdenPago, decimal IdTransaccionSession)
        {
            List<ba_ArchivoRecaudacionDet_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.IdMatricula == IdOrdenPago).First());
        }
    }
}