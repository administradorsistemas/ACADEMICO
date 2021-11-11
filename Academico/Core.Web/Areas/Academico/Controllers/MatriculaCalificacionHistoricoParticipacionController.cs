using Core.Bus;
using Core.Bus.Academico;
using Core.Info.Academico;
using Core.Web.Helps;
using DevExpress.Web.Mvc;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Academico.Controllers
{
    public class MatriculaCalificacionHistoricoParticipacionController : Controller
    {
        #region Variables
        aca_Sede_Bus bus_sede = new aca_Sede_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_NivelAcademico_Bus bus_nivel = new aca_NivelAcademico_Bus();
        aca_Jornada_Bus bus_jornada = new aca_Jornada_Bus();
        aca_Curso_Bus bus_curso = new aca_Curso_Bus();
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        aca_AnioLectivoCalificacionParticipacionHistorico_Bus bus_CalificacionHistorico = new aca_AnioLectivoCalificacionParticipacionHistorico_Bus();
        aca_MatriculaCalificacion_Bus bus_calificacionCombo = new aca_MatriculaCalificacion_Bus();
        CalificacionParticipacionHistorico_List Lista_CalificacionHistorico = new CalificacionParticipacionHistorico_List();
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        string mensaje = string.Empty;
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        #endregion

        #region Combos bajo demanada
        public ActionResult ComboBoxPartial_Anio()
        {
            return PartialView("_ComboBoxPartial_Anio", new aca_AnioLectivoCalificacionParticipacionHistorico_Info());
        }
        public ActionResult ComboBoxPartial_Sede()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            return PartialView("_ComboBoxPartial_Sede", new aca_AnioLectivoCalificacionParticipacionHistorico_Info { IdAnio = IdAnio });
        }
        public ActionResult ComboBoxPartial_Nivel()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            return PartialView("_ComboBoxPartial_Nivel", new aca_AnioLectivoCalificacionParticipacionHistorico_Info { IdAnio = IdAnio, IdSede = IdSede });
        }
        public ActionResult ComboBoxPartial_Jornada()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            return PartialView("_ComboBoxPartial_Jornada", new aca_AnioLectivoCalificacionParticipacionHistorico_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel });
        }

        public ActionResult ComboBoxPartial_Curso()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            return PartialView("_ComboBoxPartial_Curso", new aca_AnioLectivoCalificacionParticipacionHistorico_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada });
        }

        public ActionResult ComboBoxPartial_CampoAccion()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdCurso = !string.IsNullOrEmpty(Request.Params["IdCurso"]) ? int.Parse(Request.Params["IdCurso"]) : -1;
            return PartialView("_ComboBoxPartial_CampoAccion", new aca_AnioLectivoCalificacionParticipacionHistorico_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada, IdCurso = IdCurso });
        }

        public ActionResult ComboBoxPartial_Tematica()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdCurso = !string.IsNullOrEmpty(Request.Params["IdCurso"]) ? int.Parse(Request.Params["IdCurso"]) : -1;
            int IdCampoAccion = !string.IsNullOrEmpty(Request.Params["IdCampoAccion"]) ? int.Parse(Request.Params["IdCampoAccion"]) : -1;
            return PartialView("_ComboBoxPartial_Tematica", new aca_AnioLectivoCalificacionParticipacionHistorico_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada, IdCurso=IdCurso, IdCampoAccion=IdCampoAccion });
        }

        #endregion

        #region Importacion/Index
        public ActionResult UploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("UploadControlFile", UploadControlSettings_CalificacionParticipacionHistorico.UploadValidationSettings, UploadControlSettings_CalificacionParticipacionHistorico.FileUploadComplete);
            return null;
        }
        public ActionResult Index(bool isSuccess = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            var info_anio = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);
            aca_AnioLectivoCalificacionParticipacionHistorico_Info model = new aca_AnioLectivoCalificacionParticipacionHistorico_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSede = 0,
                IdAnio = 0,
                IdNivel = 0,
                IdJornada = 0,
                IdCurso = 0,
                IdTematica = 0,
                IdCampoAccion=0,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            ViewBag.MensajeSuccess = (isSuccess == false ? null : MensajeSuccess);
            string IdUsuario = SessionFixed.IdUsuario;
            List<aca_AnioLectivoCalificacionParticipacionHistorico_Info> ListaHistorico = new List<aca_AnioLectivoCalificacionParticipacionHistorico_Info>();

            Lista_CalificacionHistorico.set_list(ListaHistorico, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(aca_AnioLectivoCalificacionParticipacionHistorico_Info model)
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                var Lista_Calificaciones = Lista_CalificacionHistorico.get_list(model.IdTransaccionSession);
                var Lista_CalificacionesGuardar = new List<aca_AnioLectivoCalificacionParticipacionHistorico_Info>();
                Lista_CalificacionesGuardar = Lista_Calificaciones.Where(q => q.RegistroValido == true).ToList();
                ViewBag.mensaje = null;
                ViewBag.MensajeSuccess = null;

                foreach (var item in Lista_CalificacionesGuardar)
                {
                    item.IdSede = model.IdSede;
                    item.IdNivel = model.IdNivel;
                    item.IdJornada = model.IdJornada;
                    item.IdCurso = model.IdCurso;
                    item.IdTematica = model.IdTematica;
                    item.IdCampoAccion = model.IdCampoAccion;
                }
                ViewBag.mensaje = null;

                foreach (var item in Lista_CalificacionesGuardar)
                {
                    var info_existe = bus_CalificacionHistorico.GetInfo(item.IdEmpresa, item.IdAnio, item.IdAlumno);
                    if (info_existe==null)
                    {
                        if (!bus_CalificacionHistorico.GuardarDB(item))
                        {
                            ViewBag.mensaje = "Error al importar el archivo";
                            return View(model);
                        }
                    }
                    else
                    {
                        if (!bus_CalificacionHistorico.ModificarDB(item))
                        {
                            ViewBag.mensaje = "Error al importar el archivo";
                            return View(model);
                        }
                    }

                }
                ViewBag.MensajeSuccess = MensajeSuccess;

                //return RedirectToAction("Importar", new { IdEmpresa = model.IdEmpresa, IdSede = model.IdSede, IdAnio = model.IdAnio, IdNivel = model.IdNivel, IdJornada = model.IdJornada, IdCurso = model.IdCurso, IdParalelo = model.IdParalelo, isSuccess = true });
                return RedirectToAction("Index", new { isSuccess = true });

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message.ToString();
                return View(model);
            }
        }

        public ActionResult GridViewPartial_CalificacionParticipacionHistorico()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<aca_AnioLectivoCalificacionParticipacionHistorico_Info> model = Lista_CalificacionHistorico.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_CalificacionParticipacionHistorico", model);
        }
        #endregion

        #region Json
        public JsonResult ActualizarVariablesSession(int IdEmpresa = 0, decimal IdTransaccionSession = 0)
        {
            string retorno = string.Empty;
            SessionFixed.IdEmpresa = IdEmpresa.ToString();
            SessionFixed.IdTransaccionSession = IdTransaccionSession.ToString();
            SessionFixed.IdTransaccionSessionActual = IdTransaccionSession.ToString();
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LimpiarLista(int IdEmpresa = 0, decimal IdTransaccionSession = 0)
        {
            List<aca_AnioLectivoCalificacionParticipacionHistorico_Info> Lista = new List<aca_AnioLectivoCalificacionParticipacionHistorico_Info>();
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);

            Lista_CalificacionHistorico.set_list(Lista, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return Json(EsSuperAdmin, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
public class CalificacionParticipacionHistorico_List
    {
    aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
    aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
    aca_MatriculaGrado_Bus bus_calificacion = new aca_MatriculaGrado_Bus();

    string Variable = "aca_AnioLectivoCalificacionParticipacionHistorico_Info";
    public List<aca_AnioLectivoCalificacionParticipacionHistorico_Info> get_list(decimal IdTransaccionSession)
    {
        if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
        {
            List<aca_AnioLectivoCalificacionParticipacionHistorico_Info> list = new List<aca_AnioLectivoCalificacionParticipacionHistorico_Info>();

            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
        return (List<aca_AnioLectivoCalificacionParticipacionHistorico_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
    }

    public void set_list(List<aca_AnioLectivoCalificacionParticipacionHistorico_Info> list, decimal IdTransaccionSession)
    {
        HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
    }
}

public class UploadControlSettings_CalificacionParticipacionHistorico
    {
    public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
    {
        AllowedFileExtensions = new string[] { ".xlsx" },
        MaxFileSize = 40000000
    };

    public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
    {
        #region Variables
        CalificacionParticipacionHistorico_List Lista_Historico = new CalificacionParticipacionHistorico_List();
        List<aca_AnioLectivoCalificacionParticipacionHistorico_Info> Lista_Calificacion = new List<aca_AnioLectivoCalificacionParticipacionHistorico_Info>();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_Alumno_Bus bus_alumno = new aca_Alumno_Bus();
        int cont = 0;
        decimal IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
        #endregion

        Stream stream = new MemoryStream(e.UploadedFile.FileBytes);
        if (stream.Length > 0)
        {
            IExcelDataReader reader = null;
            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            while (reader.Read())
            {
                if (!reader.IsDBNull(0) && cont > 0)
                {
                    var IdEmpresa = (Convert.ToInt32(reader.GetValue(0)));
                    var IdAnio = (Convert.ToInt32(reader.GetValue(1)));
                    var IdAlumno = (Convert.ToInt32(reader.GetValue(2)));
                    var Promedio = (reader.GetValue(4)==null || reader.GetValue(4) == "") ? (decimal?)null : (Convert.ToDecimal(reader.GetValue(4)));

                    var info_alumno = bus_alumno.GetInfo(IdEmpresa, IdAlumno);
                    var info_anio = bus_anio.GetInfo(IdEmpresa, IdAnio);
                    aca_AnioLectivoCalificacionParticipacionHistorico_Info info = new aca_AnioLectivoCalificacionParticipacionHistorico_Info
                    {
                    IdEmpresa = IdEmpresa,
                    IdAnio = IdAnio,
                    IdAlumno = IdAlumno,
                    pe_nombreCompleto = info_alumno.pe_nombreCompleto,
                    PromedioFinal = Promedio,
                    RegistroValido = (info_anio == null ? false : ((Promedio==null ||Promedio <= Convert.ToDecimal(info_anio.CalificacionMaxima)) ? true : false)),
                    };

                    Lista_Calificacion.Add(info);
                }
                else
                    cont++;
            }

            Lista_Historico.set_list(Lista_Calificacion, IdTransaccionSession);
        }
    }
}
}