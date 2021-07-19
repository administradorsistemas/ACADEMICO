using Core.Bus.Academico;
using Core.Bus.General;
using Core.Info.Academico;
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

namespace Core.Web.Areas.Academico.Controllers
{
    public class CorreoMasivoAcademicoController : Controller
    {
        #region Variables
        tb_ColaCorreo_Academico_List ListaCorreo = new tb_ColaCorreo_Academico_List();
        tb_ColaCorreo_Bus bus_cola = new tb_ColaCorreo_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        aca_AnioLectivo_Curso_Paralelo_Bus bus_treelist = new aca_AnioLectivo_Curso_Paralelo_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        TreeList_CorreoMasivoAcademico_List Lista_TreeList = new TreeList_CorreoMasivoAcademico_List();
        aca_Alumno_Bus bus_alumno = new aca_Alumno_Bus();
        aca_AlumnoPeriodoActual_CorreoMasivoAcademico_List List_AlumnosPeriodoActual = new aca_AlumnoPeriodoActual_CorreoMasivoAcademico_List();
        tb_ColaCorreoCodigo_Bus bus_correo_codigo = new tb_ColaCorreoCodigo_Bus();
        aca_Catalogo_Bus bus_catalogotipo = new aca_Catalogo_Bus();
        aca_AnioLectivoParcial_Bus bus_parcial = new aca_AnioLectivoParcial_Bus();
        string mensaje = string.Empty;
        #endregion

        #region Combos bajo demanada
        public ActionResult Cmb_Alumno()
        {
            decimal model = new decimal();
            return PartialView("_CmbAlumno", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda_alumno(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO_MATRICULA.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda_alumno(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO_MATRICULA.ToString());
        }
        #endregion

        #region Metodos
        private void cargar_combos(tb_ColaCorreo_Info model)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            var lst_codigo = bus_correo_codigo.GetList_Academico(IdEmpresa);
            ViewBag.lst_codigo = lst_codigo;

            Dictionary<string, string> lst_quimestres = new Dictionary<string, string>();
            lst_quimestres.Add("6", "QUIMESTRE 1");
            lst_quimestres.Add("7", "QUIMESTRE 2");
            ViewBag.lst_quimestres = lst_quimestres;

            var lst_parcial = bus_parcial.GetList_Reportes(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdCatalogoTipo);
            ViewBag.lst_parcial = lst_parcial;

        }
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

            tb_ColaCorreo_Info model = new tb_ColaCorreo_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdAnio = info_anio.IdAnio,
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdCatalogoTipo = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1),
                IdCatalogoParcial = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoParcial.P1),
                Cuerpo = "Escribe tu mensaje",
                MostrarRetirados = false,
                MostrarPromedios = true,
                RepEconomico = false,
                RepLegal = true,
                CantidadIni = 0,
                CantidadFin = 0,
                lst_correo_masivo = new List<TreeList_Info>()
            };

            model.lst_correo_masivo = bus_treelist.GetList_CorreoMasivoAcademico(model.IdEmpresa, model.IdAnio);
            Lista_TreeList.set_list(model.lst_correo_masivo, model.IdTransaccionSession);

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "CorreoMasivoAcademico", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion
            cargar_combos(model);
            return View(model);
        }
        #endregion

        #region Editor
        public ActionResult HtmlEditorPartial(tb_ColaCorreo_Info model)
        {
            return PartialView("_HtmlEditorPartial", model);
        }
        public ActionResult HtmlEditorPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("HtmlEditorPartial", CorreoMasivoAcademicoControllerHtmlEditorSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult HtmlEditorPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("HtmlEditorPartial", CorreoMasivoAcademicoControllerHtmlEditorSettings.ImageUploadValidationSettings, CorreoMasivoAcademicoControllerHtmlEditorSettings.ImageUploadDirectory);
            return null;
        }
        #endregion

        #region TreeList
        [ValidateInput(false)]
        public ActionResult aca_AnioLectivo_ParaleloTreeList()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<TreeList_Info> model = Lista_TreeList.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ViewData["selectedIDs"] = Request.Params["selectedIDs"];
            if (ViewData["selectedIDs"] == null)
            {
                int x = 0;
                string selectedIDs = "";
                foreach (var item in model.Where(q => q.Seleccionado == true).ToList())
                {
                    if (x == 0)
                        selectedIDs = item.IdString.ToString();
                    else
                        selectedIDs += "," + item.IdString.ToString();
                    x++;
                }
                ViewData["selectedIDs"] = selectedIDs;
            }
            return PartialView("_aca_AnioLectivo_ParaleloTreeList", model);
        }

        #endregion

        #region Json
        public JsonResult guardar(int IdEmpresa = 0, int IdAnio=0, int IdSedeSession=0, int Modificado = 0, string Ids = "", decimal IdAlumno = 0, string Copia = "", string Codigo = "", string Asunto = "", string Cuerpo = "", bool RepLegal = false, bool RepEconomico = false, int IdCatalogoTipo = 0, int IdCatalogoParcial = 0, bool MostrarRetirados=false, bool MostrarPromedios=false, decimal IdTransaccionSession = 0)
        {
            string[] array = Ids.Split(',');
            List<TreeList_Info> lista = new List<TreeList_Info>();
            var lst_TreeList = Lista_TreeList.get_list(IdTransaccionSession);
            if (Ids != "")
            {
                var output = array.GroupBy(q => q).ToList();
                foreach (var item in lst_TreeList)
                {
                    foreach (var item2 in output)
                    {
                        if (item.IdString == Convert.ToString(item2.Key))
                        {
                            lista.Add(item);
                        }
                    }
                }
            }

            var AlumnosPorPeriodoActual = bus_alumno.GetList_PeriodoActual(IdEmpresa);
            List_AlumnosPeriodoActual.set_list(AlumnosPorPeriodoActual, IdTransaccionSession);
            var lstAlumnos = List_AlumnosPeriodoActual.get_list(IdTransaccionSession).ToList();

            var lstParalelos = lista.Where(q => q.IdStringPadre.Count() == 15).ToList();
            var ListaAlumnosCorreo = new List<aca_Alumno_Info>();
            foreach (var item in lstParalelos)
            {
                var lst_x_paralelo = lista.Where(q => q.IdString == item.IdString).ToList();

                foreach (var itemAlu in lst_x_paralelo)
                {
                    var info_alumno = new aca_Alumno_Info
                    {
                        IdEmpresa = IdEmpresa,
                        IdSede = Convert.ToInt32(itemAlu.IdString.Substring(0, 3)),
                        IdJornada = Convert.ToInt32(itemAlu.IdString.Substring(3, 3)),
                        IdNivel = Convert.ToInt32(itemAlu.IdString.Substring(6, 3)),
                        IdCurso = Convert.ToInt32(itemAlu.IdString.Substring(9, 3)),
                        IdParalelo = Convert.ToInt32(itemAlu.IdString.Substring(12, 3)),
                        IdAlumno = Convert.ToInt32(itemAlu.IdString.Substring(15, 6)),
                        correoRepEconomico = itemAlu.CorreoEmiteFactura,
                        CorreoRepLegal = itemAlu.CorreoRepresentante
                    };
                    ListaAlumnosCorreo.Add(info_alumno);
                }

            }

            var CodigoCorreo = "";
            var AsuntoCorreo = "";
            var CuerpoCorreo = "";
            var ParametrosCorreo = "";

            if (string.IsNullOrEmpty(Codigo))
            {
                CodigoCorreo = "";
                AsuntoCorreo = Asunto;
                CuerpoCorreo = Cuerpo;
                ParametrosCorreo = "";
            }
            else
            {
                var info_correo = bus_correo_codigo.GetInfo(IdEmpresa, Codigo);
                if (info_correo != null)
                {
                    CodigoCorreo = info_correo.Codigo;
                    AsuntoCorreo = info_correo.Asunto;
                    CuerpoCorreo = info_correo.Cuerpo;
                }
            }

            if (string.IsNullOrEmpty(AsuntoCorreo) || string.IsNullOrEmpty(CuerpoCorreo))
            {
                mensaje = "Es obligatorio ingresar asunto y cuerpo del correo";
            }
            else if (IdAlumno == 0 && lstParalelos.Count == 0)
            {
                mensaje = "Debe ingresar al menos un destinario";
            }
            else
            {
                foreach (var item in lstParalelos)
                {
                    var IdSede = Convert.ToInt32(item.IdString.Substring(0, 3));
                    var IdJornada = Convert.ToInt32(item.IdString.Substring(3, 3));
                    var IdNivel = Convert.ToInt32(item.IdString.Substring(6, 3));
                    var IdCurso = Convert.ToInt32(item.IdString.Substring(9, 3));
                    var IdParalelo = Convert.ToInt32(item.IdString.Substring(12, 3));
                    var IdAlumnoParalelo = Convert.ToInt32(item.IdString.Substring(15, 6));

                    var DatosAlumno = ListaAlumnosCorreo.Where(q => q.IdEmpresa == IdEmpresa && q.IdSede == IdSede && q.IdJornada == IdJornada && q.IdNivel == IdNivel && q.IdCurso == IdCurso && q.IdParalelo == IdParalelo && q.IdAlumno==IdAlumnoParalelo).FirstOrDefault();
                    //var CorreoXAlumno = lstAlumnosPorParalelos.Where(q => q.IdEmpresa == IdEmpresa && q.IdAlumno == IdAlumnoParalelo).FirstOrDefault();
                    var Destinatarios = (RepLegal == true ? (DatosAlumno == null ? "" : (string.IsNullOrEmpty(DatosAlumno.CorreoRepLegal) ? "" : DatosAlumno.CorreoRepLegal + ";")) : "") + (RepEconomico == true ? (DatosAlumno == null ? "" : (string.IsNullOrEmpty(DatosAlumno.correoRepEconomico) ? "" : DatosAlumno.correoRepEconomico + ";")) : "") + (!string.IsNullOrEmpty(Copia) ? Copia + ";" : "");

                    if (CodigoCorreo == "ACA_013")
                        ParametrosCorreo = IdEmpresa.ToString() + ";" + IdAnio.ToString() + ";" + IdSedeSession.ToString() + ";" + DatosAlumno.IdAlumno.ToString() + ";" + IdCatalogoTipo.ToString() + ";" + IdCatalogoParcial.ToString() + ";" + (MostrarRetirados == true ? "1" : "0") + ";" + (MostrarPromedios == true ? "1" : "0");

                    if (CodigoCorreo == "ACA_014")
                        ParametrosCorreo = IdEmpresa.ToString() + ";" + IdAnio.ToString() + ";" + IdSedeSession.ToString() + ";" + DatosAlumno.IdAlumno.ToString() + ";" + IdCatalogoTipo.ToString() + ";" + (MostrarRetirados == true ? "1" : "0") + ";" + (MostrarPromedios == true ? "1" : "0");

                    if (CodigoCorreo == "ACA_052")
                        ParametrosCorreo = IdEmpresa.ToString() + ";" + IdAnio.ToString() + ";" + IdSedeSession.ToString() + ";" + DatosAlumno.IdAlumno.ToString();

                    var info = new tb_ColaCorreo_Info
                    {
                        IdEmpresa = IdEmpresa,
                        Asunto = AsuntoCorreo,
                        Cuerpo = CuerpoCorreo,
                        Destinatarios = Destinatarios,
                        Codigo = CodigoCorreo,
                        Parametros = ParametrosCorreo,
                        IdUsuarioCreacion = SessionFixed.IdUsuario
                    };

                    if (!bus_cola.GuardarDB(info))
                    {
                        mensaje = "Ha ocurrido un error al guardar los registros";
                    }



                    //var lstAlumnosPorParalelos = ListaAlumnosCorreo.Where(q => q.IdEmpresa == IdEmpresa && q.IdSede == IdSede && q.IdJornada == IdJornada && q.IdNivel == IdNivel && q.IdCurso == IdCurso && q.IdParalelo == IdParalelo).ToList();

                    //foreach (var item1 in lstAlumnosPorParalelos)
                    //{
                    //    var CorreoXAlumno = lstAlumnosPorParalelos.Where(q => q.IdEmpresa == IdEmpresa && q.IdAlumno == item1.IdAlumno).FirstOrDefault();
                    //    var Destinatarios = (RepLegal == true ? (CorreoXAlumno == null ? "" : (string.IsNullOrEmpty(CorreoXAlumno.CorreoRepLegal) ? "" : CorreoXAlumno.CorreoRepLegal + ";")) : "") + (RepEconomico == true ? (CorreoXAlumno == null ? "" : (string.IsNullOrEmpty(CorreoXAlumno.correoRepEconomico) ? "" : CorreoXAlumno.correoRepEconomico + ";")) : "") + (!string.IsNullOrEmpty(Copia) ? Copia + ";" : "");

                    //    if (CodigoCorreo == "ACA_013")
                    //        ParametrosCorreo = IdEmpresa.ToString() + ";" + IdAnio.ToString() + ";" + IdSedeSession.ToString() + ";" + CorreoXAlumno.IdAlumno.ToString() + ";" + IdCatalogoTipo.ToString() + ";" + IdCatalogoParcial.ToString() + ";" + (MostrarRetirados == true ? "1" : "0") + ";" + (MostrarPromedios == true ? "1" : "0");

                    //    if (CodigoCorreo == "ACA_014")
                    //        ParametrosCorreo = IdEmpresa.ToString() + ";" + IdAnio.ToString() + ";" + IdSedeSession.ToString() + ";" + CorreoXAlumno.IdAlumno.ToString() + ";" + IdCatalogoTipo.ToString() + ";" + (MostrarRetirados == true ? "1" : "0") + ";" + (MostrarPromedios == true ? "1" : "0");

                    //    if (CodigoCorreo == "ACA_052")
                    //        ParametrosCorreo = IdEmpresa.ToString() + ";" + IdAnio.ToString() + ";" + IdSedeSession.ToString() + ";" + CorreoXAlumno.IdAlumno.ToString();


                    //    var info = new tb_ColaCorreo_Info
                    //    {
                    //        IdEmpresa = IdEmpresa,
                    //        Asunto = AsuntoCorreo,
                    //        Cuerpo = CuerpoCorreo,
                    //        Destinatarios = Destinatarios,
                    //        Codigo = CodigoCorreo,
                    //        Parametros = ParametrosCorreo,
                    //        IdUsuarioCreacion = SessionFixed.IdUsuario
                    //    };

                    //    if (!bus_cola.GuardarDB(info))
                    //    {
                    //        mensaje = "Ha ocurrido un error al guardar los registros";
                    //    }
                    //}
                }
                if (IdAlumno > 0)
                {
                    var CorreosAlumno = List_AlumnosPeriodoActual.get_list(IdTransaccionSession).Where(q => q.IdAlumno == IdAlumno).FirstOrDefault();
                    var DestinatarioAlumno = (RepLegal == true ? (CorreosAlumno == null ? "" : (string.IsNullOrEmpty(CorreosAlumno.CorreoRepLegal) ? "" : CorreosAlumno.CorreoRepLegal + ";")) : "") + (RepEconomico == true ? (CorreosAlumno == null ? "" : (string.IsNullOrEmpty(CorreosAlumno.correoRepEconomico) ? "" : CorreosAlumno.correoRepEconomico + ";")) : "") + (!string.IsNullOrEmpty(Copia) ? Copia + ";" : "");
                    if (CodigoCorreo == "ACA_013")
                        ParametrosCorreo = IdEmpresa.ToString() + ";" + IdAnio.ToString() + ";" + IdSedeSession.ToString() + ";" + CorreosAlumno.IdAlumno.ToString() + ";" + IdCatalogoTipo.ToString() + ";" + IdCatalogoParcial.ToString() + ";" + (MostrarRetirados == true ? "1" : "0") + ";" + (MostrarPromedios == true ? "1" : "0");

                    if (CodigoCorreo == "ACA_014")
                        ParametrosCorreo = IdEmpresa.ToString() + ";" + IdAnio.ToString() + ";" + IdSedeSession.ToString() + ";" + CorreosAlumno.IdAlumno.ToString() + ";" + IdCatalogoTipo.ToString() + ";" + (MostrarRetirados == true ? "1" : "0") + ";" + (MostrarPromedios == true ? "1" : "0");

                    if (CodigoCorreo == "ACA_052")
                        ParametrosCorreo = IdEmpresa.ToString() + ";" + IdAnio.ToString() + ";" + IdSedeSession.ToString() + ";" + CorreosAlumno.IdAlumno.ToString();

                    var info = new tb_ColaCorreo_Info
                    {
                        IdEmpresa = IdEmpresa,
                        Asunto = AsuntoCorreo,
                        Cuerpo = CuerpoCorreo,
                        Destinatarios = DestinatarioAlumno,
                        Codigo = CodigoCorreo,
                        Parametros = ParametrosCorreo,
                        IdUsuarioCreacion = SessionFixed.IdUsuario
                    };

                    if (!bus_cola.GuardarDB(info))
                    {
                        mensaje = "No se ha podido guardar el registro";
                    }
                }
            }

            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        public JsonResult actualizarTreeList(int IdEmpresa = 0, int IdAnio = 0, decimal IdTransaccionSession = 0)
        {
            var lst_correo_masivo = bus_treelist.GetList_CorreoMasivoAcademico(IdEmpresa, IdAnio);
            Lista_TreeList.set_list(lst_correo_masivo, IdTransaccionSession);

            return Json(lst_correo_masivo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DatosCorreo(int IdEmpresa = 0, string Codigo = "")
        {
            var info_CodigoCorreo = bus_correo_codigo.GetInfo(IdEmpresa, Codigo);
            var Ini = 0;
            var Fin = 0;
            if (info_CodigoCorreo != null)
            {
                Ini = info_CodigoCorreo.CantidadIni ?? 0;
                Fin = info_CodigoCorreo.CantidadFin ?? 0;
            }

            return Json(new { Ini = Ini, Fin = Fin }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarParciales_X_Quimestre(int IdEmpresa = 0, int IdSede = 0, int IdAnio = 0, int IdCatalogoTipo = 0)
        {
            var resultado = bus_parcial.GetList_Reportes(IdEmpresa, IdSede, IdAnio, IdCatalogoTipo);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class CorreoMasivoAcademicoControllerHtmlEditorSettings
    {
        public const string ImageUploadDirectory = "~/Content/imagenes/correos/";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (imageSelectorSettings == null)
                {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "CorreoMasivo", Action = "HtmlEditorPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }

    public class tb_ColaCorreo_Academico_List
    {
        string Variable = "tb_ColaCorreo_Academico_Info";
        public List<tb_ColaCorreo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<tb_ColaCorreo_Info> list = new List<tb_ColaCorreo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<tb_ColaCorreo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<tb_ColaCorreo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
    public class TreeList_CorreoMasivoAcademico_List
    {
        string Variable = "TreeList_CorreoMasivoAcademico_Info";
        public List<TreeList_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<TreeList_Info> list = new List<TreeList_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<TreeList_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<TreeList_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
    public class aca_AlumnoPeriodoActual_CorreoMasivoAcademico_List
    {
        string Variable = "aca_Alumno_CorreoMasivoAcademico_Info";
        public List<aca_Alumno_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_Alumno_Info> list = new List<aca_Alumno_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_Alumno_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_Alumno_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}