using Core.Bus.Academico;
using Core.Bus.General;
using Core.Info.Academico;
using Core.Info.General;
using Core.Info.Helps;
using Core.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Academico.Controllers
{
    public class SocioEconomicoController : Controller
    {
        #region Variables
        aca_SocioEconomico_Bus bus_socio_economico = new aca_SocioEconomico_Bus();
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        aca_Matricula_List Lista_Matricula = new aca_Matricula_List();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        aca_SocioEconomico_List Lista_SocioEconomica = new aca_SocioEconomico_List();
        aca_CatalogoFicha_Bus bus_catalogo_socioeconomico = new aca_CatalogoFicha_Bus();
        aca_SocioEconomico_Hermanos_List Lista_Hermanos = new aca_SocioEconomico_Hermanos_List();
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        aca_MatriculaSocioEconomico_List Lista_MatriculaSocioEconomico = new aca_MatriculaSocioEconomico_List();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
        #endregion

        #region Index
        public ActionResult Index(decimal IdAdlumno=0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            var info_anio = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);

            aca_Matricula_Info model = new aca_Matricula_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdAlumno = IdAdlumno,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<aca_Matricula_Info> lista = bus_matricula.GetListAlumno(model.IdEmpresa, model.IdSede, model.IdAlumno, true);
            Lista_MatriculaSocioEconomico.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Alumno", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_SocioEconomico()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<aca_Matricula_Info> model = Lista_MatriculaSocioEconomico.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_SocioEconomico", model);
        }
        #endregion

        #region Combos bajo demanada
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
        #endregion

        #region GridDetalle alumno hermanos
        [ValidateInput(false)]
        public ActionResult GridViewPartial_AlumnoHermanos()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            List<aca_Matricula_Info> model = Lista_Hermanos.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_AlumnoHermanos", model);
        }
        #endregion

        #region Metodos
        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_vivienda = bus_catalogo_socioeconomico.GetList_x_Tipo( Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.VIVIENDA), false);
            ViewBag.lst_vivienda = lst_vivienda;
            var lst_tipo_vivienda = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.TIPOVIVIENDA), false);
            ViewBag.lst_tipo_vivienda = lst_tipo_vivienda;
            var lst_agua = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.AGUA), false);
            ViewBag.lst_agua = lst_agua;
            var lst_ing_institucion = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.MOTIVOING), false);
            ViewBag.lst_ing_institucion = lst_ing_institucion;
            var lst_institucion = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.INSTITUCION), false);
            ViewBag.lst_institucion = lst_institucion;
            var lst_financiamiento = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.ESTUDIOS), false);
            ViewBag.lst_financiamiento = lst_financiamiento;
            var lst_vivecon = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.VIVECON), false);
            ViewBag.lst_vivecon = lst_vivecon;
        }

        private bool validar(aca_SocioEconomico_Info info, ref string msg)
        {
            if (info.IdCatalogoFichaMot == Convert.ToDecimal(cl_enumeradores.eCatalogoSocioEconomico_OtrodMotivos.OTROS_MOTIVOING))
            {
                if (info.OtroMotivoIngreso == "" || info.OtroMotivoIngreso == null)
                {
                    msg = "Debe de ingresar el motivo de ingreso";
                    return false;
                }
            }

            if (info.IdCatalogoFichaIns == Convert.ToDecimal(cl_enumeradores.eCatalogoSocioEconomico_OtrodMotivos.OTROS_INSTITUCION))
            {
                if (info.OtroInformacionInst == "" || info.OtroInformacionInst==null)
                {
                    msg = "Debe de ingresar como conoció nuestra institución";
                    return false;
                }
            }

            if (info.IdCatalogoFichaFin == Convert.ToDecimal(cl_enumeradores.eCatalogoSocioEconomico_OtrodMotivos.OTROS_MEDIOS))
            {
                if (info.OtroFinanciamiento == "" || info.OtroFinanciamiento == null)
                {
                    msg = "Debe de ingresar el medio de financiamiento de los estudios";
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0, decimal IdAlumno = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_SocioEconomico_Info model = new aca_SocioEconomico_Info
            {
                IdTransaccionSession = Convert.ToInt32(SessionFixed.IdTransaccionSessionActual),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdAlumno = IdAlumno,
                SI_HERM = false,
                NO_HERM = true,
                SI_ENERG = true,
                NO_ENERG = false,
                lst_hermanos = new List<aca_Matricula_Info>()
            };

            model.lst_hermanos = bus_socio_economico.GetListHermanos(model.IdEmpresa, model.IdAlumno);
            Lista_Hermanos.set_list(model.lst_hermanos, model.IdTransaccionSession);
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(aca_SocioEconomico_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;

            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos();
                return View(model);
            }

            if (!bus_socio_economico.GuardarDB(model))
            {
                ViewBag.mensaje = "No se ha podido guardar el registro";
                cargar_combos();
                return View(model);
            }

            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdSocioEconomico = model.IdSocioEconomico, Exito=true });
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdAnio= 0, int IdSocioEconomico = 0, bool Exito=false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_SocioEconomico_Info model = bus_socio_economico.GetInfo(IdEmpresa, IdSocioEconomico);
            model.IdTransaccionSession = Convert.ToInt32(SessionFixed.IdTransaccionSessionActual);
            if (model == null)
                return RedirectToAction("Index", "Matricula");

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            model.lst_hermanos = bus_socio_economico.GetListHermanos(model.IdEmpresa, model.IdAlumno);
            Lista_Hermanos.set_list(model.lst_hermanos, model.IdTransaccionSession);
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(aca_SocioEconomico_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;

            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos();
                return View(model);
            }

            if (!bus_socio_economico.ModificarDB(model))
            {
                ViewBag.mensaje = "No se ha podido modificar el registro";
                cargar_combos();
                return View(model);
            }

            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdSocioEconomico = model.IdSocioEconomico, Exito=true });
        }
        #endregion
    }

    public class aca_SocioEconomico_List
    {
        string Variable = "aca_SocioEconomico_Info";
        public List<aca_SocioEconomico_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_SocioEconomico_Info> list = new List<aca_SocioEconomico_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_SocioEconomico_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_SocioEconomico_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class aca_MatriculaSocioEconomico_List
    {
        string Variable = "aca_MatriculaSocioEconomico_Info";
        public List<aca_Matricula_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_Matricula_Info> list = new List<aca_Matricula_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_Matricula_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_Matricula_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class aca_SocioEconomico_Hermanos_List
    {
        string Variable = "aca_Matricula_Hermanos_Info";
        public List<aca_Matricula_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_Matricula_Info> list = new List<aca_Matricula_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_Matricula_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_Matricula_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}