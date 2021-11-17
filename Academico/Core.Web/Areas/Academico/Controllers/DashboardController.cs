using Core.Bus.Academico;
using Core.Info.Academico;
using Core.Info.Helps;
using Core.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Academico.Controllers
{
    public class DashboardController : Controller
    {
        #region Variables
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        string mensaje = string.Empty;
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        #endregion

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            # endregion

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Curso", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            var info_anio = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa),0);
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdAnio = (info_anio==null ? 0 : info_anio.IdAnio)
            };

           return View(model);
        }

        #region JSON
        public JsonResult CantEstudiantesGeneral(int IdEmpresa = 0, int IdAnio=0, int IdSede=0)
        {
            var lstEstudiantes = bus_matricula.Dashboard_EstudiantesGeneral(IdEmpresa, IdAnio, IdSede);
            return Json(lstEstudiantes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CantEstudiantesActual(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0)
        {
            var lstEstudiantes = bus_matricula.CantEstudiantesActual(IdEmpresa, IdAnio, IdSede);
            return Json(lstEstudiantes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CantEstudiantesJornada(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0)
        {
            var lstEstudiantes = bus_matricula.CantEstudiantesJornada(IdEmpresa, IdAnio, IdSede);
            return Json(lstEstudiantes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CantEstudiantesNivel(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0)
        {
            var lstEstudiantes = bus_matricula.CantEstudiantesNivel(IdEmpresa, IdAnio, IdSede);
            return Json(lstEstudiantes, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}