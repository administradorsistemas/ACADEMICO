using Core.Bus.Academico;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Admision.Controllers
{
    public class ConsultaController : Controller
    {
        #region Variables
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_Admision_Bus bus_admision = new aca_Admision_Bus();
        #endregion
        public ActionResult Index()
        {
            int IdEmpresa = 1;
            //var info_anio = bus_anio.GetInfo_AnioEnCurso(IdEmpresa, 0);
            var info_anio = bus_anio.GetList_Admision(IdEmpresa).FirstOrDefault();
            var model = new aca_Admision_Info
            {
                IdEmpresa = IdEmpresa,
                IdAnio = (info_anio == null ? 0 : info_anio.IdAnio),
            };

            return View(model);
        }

        #region JSON
        public JsonResult ConsultaAdmision(int IdEmpresa = 0, int IdAnio = 0, string CedulaRuc_Aspirante="")
        {
            var info_aspirante = bus_admision.ConsultaAdmision(IdEmpresa, IdAnio, CedulaRuc_Aspirante);
            if (info_aspirante==null)
            {
                info_aspirante = new aca_Admision_Info();
            }
            else
            {
                info_aspirante.FechaString = info_aspirante.FechaIngreso_Aspirante.ToString("dd-MM-yyyy");
            }
            var resultado = info_aspirante;
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}