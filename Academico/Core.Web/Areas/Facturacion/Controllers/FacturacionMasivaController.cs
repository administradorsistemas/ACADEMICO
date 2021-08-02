using Core.Bus.Academico;
using Core.Bus.Facturacion;
using Core.Bus.General;
using Core.Info.Academico;
using Core.Info.General;
using Core.Info.Helps;
using Core.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Facturacion.Controllers
{
    public class FacturacionMasivaController : Controller
    {
        #region Variables
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        fa_PuntoVta_Bus bus_punto_venta = new fa_PuntoVta_Bus();
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        aca_AnioLectivo_Periodo_Bus bus_periodo = new aca_AnioLectivo_Periodo_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        aca_AnioLectivo_Periodo_FactMasiva_List Lista_FactMasiva = new aca_AnioLectivo_Periodo_FactMasiva_List();
        aca_Matricula_Rubro_FactMasiva_List Lista_FactMasivaDet = new aca_Matricula_Rubro_FactMasiva_List();
        aca_Matricula_Rubro_Bus bus_MatRubro = new aca_Matricula_Rubro_Bus();
        tb_ColaCorreo_Bus busCorreo = new tb_ColaCorreo_Bus();
        tb_ColaCorreoCodigo_Bus busCorreoCodigo = new tb_ColaCorreoCodigo_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
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
            var info_anio = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);

            aca_AnioLectivo_Periodo_Info model = new aca_AnioLectivo_Periodo_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<aca_AnioLectivo_Periodo_Info> lista = bus_periodo.getList_AnioCurso(model.IdEmpresa);
            Lista_FactMasiva.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "FacturacionMasiva", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(aca_AnioLectivo_Periodo_Info model)
        {
            List<aca_AnioLectivo_Periodo_Info> lista = bus_periodo.getList_AnioCurso(model.IdEmpresa);
            Lista_FactMasiva.set_list(lista, Convert.ToDecimal(model.IdTransaccionSession));

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "FacturacionMasiva", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion
            cargar_combos();
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_FacturacionMasiva()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<aca_AnioLectivo_Periodo_Info> model = Lista_FactMasiva.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_FacturacionMasiva", model);
        }
        #endregion

        #region Combos
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
            return PartialView("_CmbNivel", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio, IdSede = IdSede, IdJornada = IdJornada });
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

        #region Detalle
        [ValidateInput(false)]
        public ActionResult GridViewPartial_FacturacionMasivaDet()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_FactMasivaDet.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_FacturacionMasivaDet", model);
        }
        #endregion

        #region Metodos
        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            bool EsContador = Convert.ToBoolean(SessionFixed.EsContador);

            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            ViewBag.lst_sucursal = lst_sucursal;

            var lst_ptoventa = bus_punto_venta.GetListUsuario(IdEmpresa, IdSucursal, false, SessionFixed.IdUsuario, EsContador, "FACT");
            ViewBag.lst_ptoventa = lst_ptoventa;
        }
        #endregion

        #region Acciones
        public ActionResult Consultar(int IdEmpresa = 0, int IdAnio=0, int IdPeriodo = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            aca_Menu_x_seg_usuario_Info inf = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "FacturacionMasiva", "Index");
            ViewBag.Nuevo = inf.Nuevo;
            ViewBag.Modificar = inf.Modificar;
            ViewBag.Anular = inf.Anular;
            #endregion

            var info_anio = bus_anio.GetInfo_AnioEnCurso(IdEmpresa, 0);
            aca_AnioLectivo_Periodo_Info model = bus_periodo.GetInfo(IdEmpresa, IdAnio, IdPeriodo);
            model.IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdJornada = 0;
            model.IdNivel = 0;
            model.IdCurso = 0;
            model.IdParalelo = 0;

            //model.lst_det_fact_masiva = bus_MatRubro.getList_FactMasiva(IdEmpresa, IdAnio, IdPeriodo);
            model.lst_det_fact_masiva = bus_MatRubro.getList_FactMasiva(model.IdEmpresa, model.IdAnio, model.IdPeriodo, model.IdSede??0, model.IdJornada??0, model.IdNivel??0, model.IdCurso??0, model.IdParalelo??0);
            Lista_FactMasivaDet.set_list(model.lst_det_fact_masiva, model.IdTransaccionSession);
            ViewBag.MostrarBoton = true;
            if (model.Procesado==true)
            {
                ViewBag.MostrarBoton = false;
            }

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Consultar(aca_AnioLectivo_Periodo_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;

            model.lst_det_fact_masiva = Lista_FactMasivaDet.get_list(model.IdTransaccionSession).Where(q=> q.Procesado == false).ToList();
            
            var info_periodo_anterior = new aca_AnioLectivo_Periodo_Info();
            var periodo_anterior = model.IdPeriodo - 1;
            if (periodo_anterior!=0)
            {
                info_periodo_anterior = bus_periodo.GetInfo(model.IdEmpresa, model.IdAnio, periodo_anterior);
            }

            if (info_periodo_anterior!=null && (info_periodo_anterior.Procesado==false || info_periodo_anterior.Procesado ==null))
            {
                ViewBag.mensaje = "No se puede procesar este periodo, tiene pendiente procesar el periodo anterior";
                cargar_combos();
                return View(model);
            }
            if (model.lst_det_fact_masiva.Count > 0)
            {
                if (!bus_periodo.Modificar_FacturacionMasiva(model))
                {
                    ViewBag.mensaje = "No se ha podido guardar el registro";
                    cargar_combos();
                    return View(model);
                }
            }
            //Ponga aqui el metodo de cerrar periodo
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdAnio = model.IdAnio, IdPeriodo=model.IdPeriodo, Exito = true });
        }
        #endregion

        #region Json
        public JsonResult EnviarCorreoAutorizados(int IdEmpresa, decimal IdTransaccionSession)
        {
            string Mensaje = string.Empty;
            var lst = Lista_FactMasivaDet.get_list(IdTransaccionSession).Where(q=> q.Procesado == true && !string.IsNullOrEmpty(q.vt_autorizacion));
            var Codigo = busCorreoCodigo.GetInfo(IdEmpresa, "FAC_002");
            if (Codigo != null)
            {
                foreach (var item in lst)
                {
                    busCorreo.GuardarDB(new tb_ColaCorreo_Info
                    {
                        IdEmpresa = IdEmpresa,
                        Codigo = "FAC_002",
                        Destinatarios = item.Correo,
                        Asunto = Codigo.Asunto,
                        Cuerpo = Codigo.Cuerpo,
                        Parametros = item.IdEmpresa.ToString()+";"+item.IdSucursal.ToString()+";"+item.IdBodega.ToString()+";"+item.IdCbteVta.ToString(),
                        IdUsuarioCreacion = SessionFixed.IdUsuario
                    });
                }
            }

            return Json(Mensaje,JsonRequestBehavior.AllowGet);
        }

        public JsonResult EnviarCorreo(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta, string Correos)
        {
            string Mensaje = string.Empty;
            var Codigo = busCorreoCodigo.GetInfo(IdEmpresa, "FAC_002");
            if (Codigo != null)
            {
                busCorreo.GuardarDB(new tb_ColaCorreo_Info
                {
                    IdEmpresa = IdEmpresa,
                    Codigo = "FAC_002",
                    Destinatarios = Correos,
                    Asunto = Codigo.Asunto,
                    Cuerpo = Codigo.Cuerpo,
                    Parametros = IdEmpresa.ToString() + ";" + IdSucursal.ToString() + ";" + IdBodega.ToString() + ";" + IdCbteVta.ToString(),
                    IdUsuarioCreacion = SessionFixed.IdUsuario
                });
            }

            return Json(Mensaje, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Buscar(int IdEmpresa, int IdPeriodo, int IdAnio = 0, int IdSede = 0, int IdJornada = 0, int IdNivel = 0, int IdCurso = 0, int IdParalelo = 0, decimal IdTransaccionSession = 0)
        {
            string Mensaje = string.Empty;
            var lst_det_fact_masiva = bus_MatRubro.getList_FactMasiva(IdEmpresa, IdAnio, IdPeriodo, IdSede, IdJornada, IdNivel, IdCurso, IdParalelo);
            Lista_FactMasivaDet.set_list(lst_det_fact_masiva, IdTransaccionSession);

            return Json(Mensaje, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class aca_AnioLectivo_Periodo_FactMasiva_List
    {
        string Variable = "aca_AnioLectivo_Periodo_FactMasiva_Info";
        public List<aca_AnioLectivo_Periodo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_AnioLectivo_Periodo_Info> list = new List<aca_AnioLectivo_Periodo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_AnioLectivo_Periodo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_AnioLectivo_Periodo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class aca_Matricula_Rubro_FactMasiva_List
    {
        string Variable = "aca_Matricula_Rubro_FactMasiva_Info";
        public List<aca_Matricula_Rubro_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_Matricula_Rubro_Info> list = new List<aca_Matricula_Rubro_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_Matricula_Rubro_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_Matricula_Rubro_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}