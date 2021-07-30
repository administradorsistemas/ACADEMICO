using Core.Bus.Academico;
using Core.Bus.General;
using Core.Info.Academico;
using Core.Info.General;
using Core.Info.Helps;
using Core.Web.Helps;
using DevExpress.Web;
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
    public class CalificacionHistoricoController : Controller
    {
        #region Variables
        aca_AnioLectivoCalificacionHistorico_List Lista_CalificacionHistorico = new aca_AnioLectivoCalificacionHistorico_List();
        aca_AnioLectivoCalificacionHistorico_Bus bus_CalificacionHistorico = new aca_AnioLectivoCalificacionHistorico_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_AnioLectivo_Sede_NivelAcademico_Bus bus_anio_nivel = new aca_AnioLectivo_Sede_NivelAcademico_Bus();
        aca_AnioLectivo_Jornada_Curso_Bus bus_anio_curso = new aca_AnioLectivo_Jornada_Curso_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        aca_AnioLectivoConductaEquivalencia_Bus bus_conducta = new aca_AnioLectivoConductaEquivalencia_Bus();
        aca_AnioLectivoEquivalenciaPromedio_Bus bus_promedio = new aca_AnioLectivoEquivalenciaPromedio_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        #endregion

        #region Metodos ComboBox bajo demanda
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

        #region Combos
        public ActionResult ComboBoxPartial_Anio()
        {
            return PartialView("_ComboBoxPartial_Anio", new aca_AnioLectivo_NivelAcademico_Jornada_Info());
        }
        public ActionResult ComboBoxPartial_Sede()
        {
            int IdAnio = (Request.Params["IdAnio"] != null) ? int.Parse(Request.Params["IdAnio"]) : -1;
            return PartialView("_ComboBoxPartial_Sede", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio });
        }
        public ActionResult ComboBoxPartial_Nivel()
        {
            int IdAnio = (Request.Params["IdAnio"] != null) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = (Request.Params["IdSede"] != null) ? int.Parse(Request.Params["IdSede"]) : -1;
            return PartialView("_ComboBoxPartial_Nivel", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio, IdSede = IdSede });
        }
        public ActionResult ComboBoxPartial_Jornada()
        {
            int IdAnio = (Request.Params["IdAnio"] != null) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = (Request.Params["IdSede"] != null) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = (Request.Params["IdNivel"] != null) ? int.Parse(Request.Params["IdNivel"]) : -1;
            return PartialView("_ComboBoxPartial_Jornada", new aca_AnioLectivo_Jornada_Curso_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel });
        }

        public ActionResult ComboBoxPartial_Curso()
        {
            int IdAnio = (Request.Params["IdAnio"] != null) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = (Request.Params["IdSede"] != null) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = (Request.Params["IdNivel"] != null) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdJornada = (Request.Params["IdJornada"] != null) ? int.Parse(Request.Params["IdJornada"]) : -1;
            return PartialView("_ComboBoxPartial_Curso", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada });
        }
        #endregion

        #region Metodos
        private void cargar_combos(aca_AnioLectivoCalificacionHistorico_Info info)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_conducta = bus_conducta.GetList(info.IdEmpresa, info.IdAnio, false);
            ViewBag.lst_conducta = lst_conducta;
            var lst_nivel = bus_anio_nivel.GetListNivel_x_Anio(info.IdEmpresa, info.IdAnio);
            ViewBag.lst_nivel = lst_nivel;
            var lst_curso = bus_anio_curso.GetListCurso_x_Anio(info.IdEmpresa, info.IdAnio);
            ViewBag.lst_curso = lst_curso;
        }
        #endregion

        #region Index
        public ActionResult Index(decimal IdAlumno=0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            var info_anio = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);
            aca_AnioLectivoCalificacionHistorico_Info model = new aca_AnioLectivoCalificacionHistorico_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdAlumno = IdAlumno,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };
            ViewBag.IdAlumno = model.IdAlumno;
            List<aca_AnioLectivoCalificacionHistorico_Info> lista = bus_CalificacionHistorico.GetList(model.IdEmpresa, model.IdAlumno, true);
            Lista_CalificacionHistorico.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "CalificacionHistorico", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion
            return View(model);
        }

        //[HttpPost]
        //public ActionResult Index(aca_AnioLectivoCalificacionHistorico_Info model)
        //{
        //    List<aca_AnioLectivoCalificacionHistorico_Info> lista = bus_CalificacionHistorico.GetList(model.IdEmpresa, model.IdAlumno, true);
        //    Lista_CalificacionHistorico.set_list(lista, Convert.ToDecimal(model.IdTransaccionSession));

        //    return View(model);
        //}

        [ValidateInput(false)]
        public ActionResult GridViewPartial_CalificacionHistorico(decimal IdAlumno=0, bool Nuevo = false, bool Modificar = false)
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            ViewBag.IdAlumno = IdAlumno;
            List<aca_AnioLectivoCalificacionHistorico_Info> model = Lista_CalificacionHistorico.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            ViewBag.Nuevo = Nuevo;
            ViewBag.Modificar = Modificar;
            
            return PartialView("_GridViewPartial_CalificacionHistorico", model);
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa=0, decimal IdAlumno=0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            var info = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);
            aca_AnioLectivoCalificacionHistorico_Info model = new aca_AnioLectivoCalificacionHistorico_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdAnio = (info == null ? 0 : info.IdAnio),
                IdAlumno = IdAlumno,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };
            ViewBag.IdAlumno = model.IdAlumno;
            cargar_combos(model);
            #region Permisos
            aca_Menu_x_seg_usuario_Info inf = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Alumno", "Index");
            if (!inf.Nuevo)
                return RedirectToAction("Index");
            #endregion
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(aca_AnioLectivoCalificacionHistorico_Info model)
        {
            var info_conducta = bus_conducta.GetInfo(model.IdEmpresa, model.IdAnio, Convert.ToInt32(model.SecuenciaConducta));
            var info_equivalencia = bus_promedio.GetInfo_x_Promedio(model.IdEmpresa, model.IdAnio, model.Promedio);
            model.Conducta = info_conducta.Calificacion;
            model.IdEquivalenciaPromedio = (info_equivalencia==null ? (int?)null : info_equivalencia.IdEquivalenciaPromedio);

            if (!bus_CalificacionHistorico.GuardarDB(model))
            {
                ViewBag.mensaje = "No se ha podido guardar el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdAnio = model.IdAnio, IdAlumno = model.IdAlumno, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdAnio = 0, int IdAlumno = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_AnioLectivoCalificacionHistorico_Info model = bus_CalificacionHistorico.GetInfo(IdEmpresa, IdAnio, IdAlumno);

            if (model == null)
                return RedirectToAction("Index");
            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Alumno", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            cargar_combos(model);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdAnio = 0, int IdAlumno = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_AnioLectivoCalificacionHistorico_Info model = bus_CalificacionHistorico.GetInfo(IdEmpresa, IdAnio, IdAlumno);

            if (model == null)
                return RedirectToAction("Index");
            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Alumno", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            cargar_combos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(aca_AnioLectivoCalificacionHistorico_Info model)
        {
            var info_conducta = bus_conducta.GetInfo(model.IdEmpresa, model.IdAnio, Convert.ToInt32(model.SecuenciaConducta));
            var info_equivalencia = bus_promedio.GetInfo_x_Promedio(model.IdEmpresa, model.IdAnio, model.Promedio);
            model.Conducta = info_conducta.Calificacion;
            model.IdEquivalenciaPromedio = (info_equivalencia == null ? (int?)null : info_equivalencia.IdEquivalenciaPromedio);

            if (!bus_CalificacionHistorico.ModificarDB(model))
            {
                ViewBag.mensaje = "No se ha podido guardar el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdAnio = model.IdAnio, IdAlumno = model.IdAlumno, Exito = true });
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
        public JsonResult EliminarCalificacion(int IdEmpresa = 0, int IdAnio=0, decimal IdAlumno = 0)
        {
            string retorno = string.Empty;
            var info_historico = bus_CalificacionHistorico.GetInfo(IdEmpresa, IdAnio, IdAlumno);
            if (!bus_CalificacionHistorico.EliminarDB(info_historico))
            {
                retorno = "No se ha podido eliminar el registro";
            }
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Importacion
        public ActionResult UploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("UploadControlFile", UploadControlSettings_Calificacion.UploadValidationSettings, UploadControlSettings_Calificacion.FileUploadComplete);
            return null;
        }
        public ActionResult Importar(int IdEmpresa = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_AnioLectivoCalificacionHistorico_Info model = new aca_AnioLectivoCalificacionHistorico_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Importar(aca_AnioLectivoCalificacionHistorico_Info model)
        {
            try
            {
                var Lista_Calificacion_Historico = Lista_CalificacionHistorico.get_list(model.IdTransaccionSession);
                foreach (var item in Lista_Calificacion_Historico)
                {
                    if (!bus_CalificacionHistorico.GuardarDB(item))
                    {
                        ViewBag.mensaje = "Error al importar el archivo";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                //SisLogError.set_list((ex.InnerException) == null ? ex.Message.ToString() : ex.InnerException.ToString());

                ViewBag.error = ex.Message.ToString();
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public ActionResult GridViewPartial_CalificacionHistoricoImportacion()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CalificacionHistorico.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_CalificacionHistoricoImportacion", model);
        }
        #endregion
    }
    public class aca_AnioLectivoCalificacionHistorico_List
    {
        string Variable = "aca_AnioLectivoCalificacionHistorico_Info";
        public List<aca_AnioLectivoCalificacionHistorico_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_AnioLectivoCalificacionHistorico_Info> list = new List<aca_AnioLectivoCalificacionHistorico_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_AnioLectivoCalificacionHistorico_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_AnioLectivoCalificacionHistorico_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class UploadControlSettings_Calificacion
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".xlsx" },
            MaxFileSize = 40000000
        };
        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            #region Variables
            aca_AnioLectivoCalificacionHistorico_List ListaCalificaciones = new aca_AnioLectivoCalificacionHistorico_List();
            List<aca_AnioLectivoCalificacionHistorico_Info> Lista_Calificaciones = new List<aca_AnioLectivoCalificacionHistorico_Info>();
            List<aca_AnioLectivoCalificacionHistorico_Info> Lista_Calificaciones_NoValidos = new List<aca_AnioLectivoCalificacionHistorico_Info>();
            tb_persona_Bus bus_persona = new tb_persona_Bus();
            aca_Alumno_Bus bus_alumno = new aca_Alumno_Bus();
            aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
            aca_AnioLectivoConductaEquivalencia_Bus bus_conducta = new aca_AnioLectivoConductaEquivalencia_Bus();
            aca_AnioLectivo_Jornada_Curso_Bus bus_jornada_curso = new aca_AnioLectivo_Jornada_Curso_Bus();
            int cont = 0;
            decimal IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            #endregion

            Stream stream = new MemoryStream(e.UploadedFile.FileBytes);
            if (stream.Length > 0)
            {
                IExcelDataReader reader = null;
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                #region Calificacion   
                var lst_persona = bus_persona.get_list(false);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && cont > 0)
                    {
                        int anio_inicio = (Convert.ToInt32(reader.GetValue(0)));
                        int anio_fin = (Convert.ToInt32(reader.GetValue(1)));
                        decimal conducta = Convert.ToInt32(reader.GetValue(7));
                        string cedula_ruc_alumno = (Convert.ToString(reader.GetValue(2)).Trim());
                        aca_Alumno_Info info_alumno = bus_alumno.get_info_x_num_cedula(IdEmpresa, cedula_ruc_alumno);
                        aca_AnioLectivo_Info info_anio = bus_anio.GetInfo_x_Anio(IdEmpresa, anio_inicio, anio_fin);
                        aca_AnioLectivoConductaEquivalencia_Info info_conducta = bus_conducta.GetInfo_ImportacionConducta(IdEmpresa, info_anio.IdAnio, conducta);

                        //tb_persona_Info info_persona_alumno = new tb_persona_Info();
                        //info_persona_alumno = lst_persona.Where(q => q.pe_cedulaRuc==cedula_ruc_alumno).FirstOrDefault();

                        aca_AnioLectivoCalificacionHistorico_Info info = new aca_AnioLectivoCalificacionHistorico_Info
                        {
                            IdEmpresa = IdEmpresa,
                            IdAnio = info_anio.IdAnio,
                            pe_nombreCompleto = info_alumno.pe_nombreCompleto,
                            pe_cedulaRuc = cedula_ruc_alumno,
                            IdAlumno = info_alumno.IdAlumno,
                            IdNivel = (Convert.ToInt32(reader.GetValue(3))),
                            IdCurso = (Convert.ToInt32(reader.GetValue(4))),
                            AntiguaInstitucion = (Convert.ToString(reader.GetValue(5)).Trim()),
                            Promedio = (Convert.ToDecimal(reader.GetValue(6))),
                            Conducta = conducta,
                            Letra = (info_conducta== null ? "" :info_conducta.Letra)
                        };

                        if (info.IdAlumno==0 || info.Promedio==0)
                        {
                            Lista_Calificaciones_NoValidos.Add(info);
                        }
                        else
                        {
                            if (Lista_Calificaciones.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdAnio == q.IdAnio && q.IdAlumno == info.IdAlumno && q.IdNivel == info.IdNivel && q.IdCurso == info.IdCurso).Count() == 0)
                                Lista_Calificaciones.Add(info);
                        }
                        
                    }
                    else
                        cont++;
                }
                var ListaNoValida = Lista_Calificaciones_NoValidos;
                ListaCalificaciones.set_list(Lista_Calificaciones, IdTransaccionSession);
                #endregion
            }
        }
    }
}