using Core.Bus.Academico;
using Core.Info.Academico;
using Core.Web.Areas.Academico.Controllers;
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
    public class MatriculaGradoController : Controller
    {
        #region Variables
        aca_Sede_Bus bus_sede = new aca_Sede_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_NivelAcademico_Bus bus_nivel = new aca_NivelAcademico_Bus();
        aca_Jornada_Bus bus_jornada = new aca_Jornada_Bus();
        aca_Curso_Bus bus_curso = new aca_Curso_Bus();
        aca_Paralelo_Bus bus_paralelo = new aca_Paralelo_Bus(); 
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        aca_MatriculaGrado_Bus bus_calificacion = new aca_MatriculaGrado_Bus();
        aca_MatriculaCalificacion_Bus bus_calificacionCombo = new aca_MatriculaCalificacion_Bus();
        aca_MatriculaGrado_List Lista_CalificacionGrado = new aca_MatriculaGrado_List();      
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        aca_MatriculaCalificacion_Combos_List ListaCombos = new aca_MatriculaCalificacion_Combos_List();
        aca_Profesor_Bus bus_profesor = new aca_Profesor_Bus();
        aca_Catalogo_Bus bus_catalogo = new aca_Catalogo_Bus();
        aca_AnioLectivoEquivalenciaPromedio_Bus bus_equivalencia = new aca_AnioLectivoEquivalenciaPromedio_Bus();
        string mensaje = string.Empty;
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
            aca_MatriculaGrado_Info model = new aca_MatriculaGrado_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdAnio = (info_anio == null ? 0 : info_anio.IdAnio),
                IdNivel = 0,
                IdJornada = 0,
                IdCurso = 0,
                IdParalelo = 0,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            string IdUsuario = SessionFixed.IdUsuario;
            var info_anioGrado = bus_anio.GetInfo(model.IdEmpresa, model.IdAnio);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacionCombo.GetList_CombosCalificacionesGrado(model.IdEmpresa, model.IdSede, Convert.ToInt32(info_anioGrado.IdCursoBachiller));
            ListaCombos.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_MatriculaGrado()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<aca_MatriculaGrado_Info> model = Lista_CalificacionGrado.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_MatriculaGrado", model);
        }
        #endregion

        #region Combos
        public ActionResult ComboBoxPartial_Anio()
        {
            return PartialView("_ComboBoxPartial_Anio", new aca_AnioLectivo_NivelAcademico_Jornada_Info());
        }
        public ActionResult ComboBoxPartial_Sede()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            return PartialView("_ComboBoxPartial_Sede", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio });
        }
        public ActionResult ComboBoxPartial_Nivel()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            return PartialView("_ComboBoxPartial_Nivel", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio, IdSede = IdSede });
        }
        public ActionResult ComboBoxPartial_Jornada()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            return PartialView("_ComboBoxPartial_Jornada", new aca_AnioLectivo_Jornada_Curso_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel });
        }

        public ActionResult ComboBoxPartial_Curso()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            return PartialView("_ComboBoxPartial_Curso", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada });
        }

        public ActionResult ComboBoxPartial_Paralelo()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdCurso = !string.IsNullOrEmpty(Request.Params["IdCurso"]) ? int.Parse(Request.Params["IdCurso"]) : -1;
            return PartialView("_ComboBoxPartial_Paralelo", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada, IdCurso = IdCurso });
        }

        public ActionResult ComboBoxPartial_Materia()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdCurso = !string.IsNullOrEmpty(Request.Params["IdCurso"]) ? int.Parse(Request.Params["IdCurso"]) : -1;
            var IdParalelo = !string.IsNullOrEmpty(Request.Params["IdParalelo"]) ? int.Parse(Request.Params["IdParalelo"]) : -1;
            return PartialView("_ComboBoxPartial_Materia", new aca_AnioLectivo_Paralelo_Profesor_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada, IdCurso = IdCurso, IdParalelo = IdParalelo });
        }
        #endregion

        #region FuncionesCombos
        public List<aca_AnioLectivo_Info> CargarAnio(int IdEmpresa = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = ListaCombos.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q => q.IdEmpresa == IdEmpresa).ToList();
            var lst_anio = (from q in lst_combos
                            group q by new
                            {
                                q.IdEmpresa,
                                q.IdAnio,
                                q.Descripcion
                            } into a
                            select new aca_AnioLectivo_Info
                            {
                                IdEmpresa = a.Key.IdEmpresa,
                                IdAnio = a.Key.IdAnio,
                                Descripcion = a.Key.Descripcion
                            }).OrderBy(q => q.Descripcion).ToList();

            var ListaAnio = new List<aca_AnioLectivo_Info>();

            foreach (var item in lst_anio)
            {
                ListaAnio.Add(new aca_AnioLectivo_Info
                {
                    IdAnio = item.IdAnio,
                    Descripcion = item.Descripcion
                });
            }

            return ListaAnio;
        }

        public List<aca_Sede_Info> CargarSede(int IdEmpresa = 0, int IdAnio = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = ListaCombos.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio).ToList();

            var lst_sede = (from q in lst_combos
                            group q by new
                            {
                                q.IdEmpresa,
                                q.IdAnio,
                                q.IdSede,
                                q.NomSede
                            } into a
                            select new aca_Sede_Info
                            {
                                IdEmpresa = a.Key.IdEmpresa,
                                IdSede = a.Key.IdSede,
                                NomSede = a.Key.NomSede
                            }).OrderBy(q => q.NomSede).ToList();

            var ListaSede = new List<aca_Sede_Info>();

            foreach (var item in lst_sede)
            {
                ListaSede.Add(new aca_Sede_Info
                {
                    IdSede = item.IdSede,
                    NomSede = item.NomSede
                });
            }

            return ListaSede;
        }

        public List<aca_Jornada_Info> CargarJornada(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = ListaCombos.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede).ToList();

            var lst_jornada = (from q in lst_combos
                               group q by new
                               {
                                   q.IdEmpresa,
                                   q.IdAnio,
                                   q.IdSede,
                                   q.IdJornada,
                                   q.NomJornada,
                                   q.OrdenJornada
                               } into a
                               select new aca_Jornada_Info
                               {
                                   IdEmpresa = a.Key.IdEmpresa,
                                   IdJornada = a.Key.IdJornada,
                                   NomJornada = a.Key.NomJornada,
                                   OrdenJornada = a.Key.OrdenJornada
                               }).OrderBy(q => q.OrdenJornada).ToList();

            var ListaJornada = new List<aca_Jornada_Info>();

            foreach (var item in lst_jornada)
            {
                ListaJornada.Add(new aca_Jornada_Info
                {
                    IdJornada = item.IdJornada,
                    NomJornada = item.NomJornada
                });
            }

            return ListaJornada;
        }

        public List<aca_NivelAcademico_Info> CargarNivel(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = ListaCombos.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede).ToList();

            var lst_nivel = (from q in lst_combos
                             group q by new
                             {
                                 q.IdEmpresa,
                                 q.IdAnio,
                                 q.IdSede,
                                 q.IdNivel,
                                 q.NomNivel,
                                 q.OrdenNivel
                             } into a
                             select new aca_NivelAcademico_Info
                             {
                                 IdEmpresa = a.Key.IdEmpresa,
                                 IdNivel = a.Key.IdNivel,
                                 NomNivel = a.Key.NomNivel,
                                 Orden = a.Key.OrdenNivel
                             }).OrderBy(q => q.Orden).ToList();

            var ListaNivel = new List<aca_NivelAcademico_Info>();

            foreach (var item in lst_nivel)
            {
                ListaNivel.Add(new aca_NivelAcademico_Info
                {
                    IdNivel = item.IdNivel,
                    NomNivel = item.NomNivel
                });
            }

            return ListaNivel;
        }

        public List<aca_Curso_Info> CargarCurso(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0, int IdJornada = 0, int IdNivel = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = ListaCombos.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede && q.IdNivel == IdNivel && q.IdJornada == IdJornada).ToList();

            var lst_curso = (from q in lst_combos
                             group q by new
                             {
                                 q.IdEmpresa,
                                 q.IdAnio,
                                 q.IdSede,
                                 q.IdNivel,
                                 q.IdJornada,
                                 q.IdCurso,
                                 q.NomCurso,
                                 q.OrdenCurso
                             } into a
                             select new aca_Curso_Info
                             {
                                 IdEmpresa = a.Key.IdEmpresa,
                                 IdCurso = a.Key.IdCurso,
                                 NomCurso = a.Key.NomCurso,
                                 OrdenCurso = a.Key.OrdenCurso
                             }).OrderBy(q => q.OrdenCurso).ToList();

            var ListaCurso = new List<aca_Curso_Info>();

            foreach (var item in lst_curso)
            {
                ListaCurso.Add(new aca_Curso_Info
                {
                    IdCurso = item.IdCurso,
                    NomCurso = item.NomCurso
                });
            }

            return ListaCurso;
        }

        public List<aca_Paralelo_Info> CargarParalelo(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0, int IdJornada = 0, int IdNivel = 0, int IdCurso = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = ListaCombos.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede && q.IdNivel == IdNivel && q.IdJornada == IdJornada && q.IdCurso == IdCurso).ToList();

            var lst_paralelo = (from q in lst_combos
                                group q by new
                                {
                                    q.IdEmpresa,
                                    q.IdAnio,
                                    q.IdSede,
                                    q.IdNivel,
                                    q.IdJornada,
                                    q.IdCurso,
                                    q.IdParalelo,
                                    q.NomParalelo,
                                    q.OrdenParalelo
                                } into a
                                select new aca_Paralelo_Info
                                {
                                    IdEmpresa = a.Key.IdEmpresa,
                                    IdParalelo = a.Key.IdParalelo,
                                    NomParalelo = a.Key.NomParalelo,
                                    OrdenParalelo = a.Key.OrdenParalelo,
                                }).OrderBy(q => q.OrdenParalelo).ToList();

            var ListaParalelo = new List<aca_Paralelo_Info>();

            foreach (var item in lst_paralelo)
            {
                ListaParalelo.Add(new aca_Paralelo_Info
                {
                    IdParalelo = item.IdParalelo,
                    NomParalelo = item.NomParalelo
                });
            }

            return ListaParalelo;
        }

        public List<aca_Materia_Info> CargarMateria(int IdEmpresa = 0, int IdAnio = 0, int IdSede = 0, int IdJornada = 0, int IdNivel = 0, int IdCurso = 0, int IdParalelo = 0)
        {
            List<aca_MatriculaCalificacion_Info> lst_combos = ListaCombos.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)).Where(q =>
            q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede && q.IdNivel == IdNivel && q.IdJornada == IdJornada && q.IdCurso == IdCurso && q.IdParalelo == IdParalelo).ToList();

            var lst_materia = (from q in lst_combos
                               group q by new
                               {
                                   q.IdEmpresa,
                                   q.IdAnio,
                                   q.IdSede,
                                   q.IdNivel,
                                   q.IdJornada,
                                   q.IdCurso,
                                   q.IdParalelo,
                                   q.IdMateria,
                                   q.NomMateria,
                                   q.OrdenMateria
                               } into a
                               select new aca_Materia_Info
                               {
                                   IdEmpresa = a.Key.IdEmpresa,
                                   IdMateria = a.Key.IdMateria,
                                   NomMateria = a.Key.NomMateria,
                                   OrdenMateria = a.Key.OrdenMateria
                               }).OrderBy(q => q.OrdenMateria).ToList();

            var ListaMateria = new List<aca_Materia_Info>();

            foreach (var item in lst_materia)
            {
                ListaMateria.Add(new aca_Materia_Info
                {
                    IdMateria = item.IdMateria,
                    NomMateria = item.NomMateria
                });
            }

            return ListaMateria;
        }
        #endregion

        #region Funciones del Grid
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] aca_MatriculaGrado_Info info_det)
        {
            if (ModelState.IsValid)
            {
                int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);
                var info_matricula = bus_matricula.GetInfo(IdEmpresa, info_det.IdMatricula);
                var info_anio = bus_anio.GetInfo(IdEmpresa, info_matricula.IdAnio);
                info_det.RegistroValido = (string.IsNullOrEmpty(info_det.CalificacionGrado.ToString()) || (Convert.ToDecimal(info_det.CalificacionGrado) <= Convert.ToDecimal(info_anio.CalificacionMaxima)) ? true : false);
                if (info_det != null && info_det.RegistroValido==true)
                {
                    Lista_CalificacionGrado.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                }
            }

            List<aca_MatriculaGrado_Info> model = new List<aca_MatriculaGrado_Info>();
            model = Lista_CalificacionGrado.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_MatriculaGrado", model);
        }
        #endregion

        #region Importacion
        public ActionResult UploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("UploadControlFile", UploadControlSettings_Grado.UploadValidationSettings, UploadControlSettings_Grado.FileUploadComplete);
            return null;
        }
        public ActionResult Importar(int IdEmpresa = 0, int IdSede = 0, int IdAnio = 0, int IdNivel = 0, int IdJornada = 0, int IdCurso = 0, int IdParalelo = 0, int IdMateria = 0, int IdCatalogoParcial = 0, bool isSuccess = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            var info_anio = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);
            aca_MatriculaGrado_Info model = new aca_MatriculaGrado_Info
            {
                IdEmpresa = IdEmpresa,
                IdSede = IdSede,
                IdAnio = IdAnio,
                IdNivel = IdNivel,
                IdJornada = IdJornada,
                IdCurso = IdCurso,
                IdParalelo = IdParalelo,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            ViewBag.MensajeSuccess = (isSuccess == false ? null : MensajeSuccess);
            string IdUsuario = SessionFixed.IdUsuario;
            var info_anioGrado = bus_anio.GetInfo(model.IdEmpresa, model.IdAnio);
            List<aca_MatriculaCalificacion_Info> lst_combos = bus_calificacionCombo.GetList_CombosCalificacionesGrado(model.IdEmpresa, model.IdSede, Convert.ToInt32(info_anioGrado.IdCursoBachiller));
            ListaCombos.set_list(lst_combos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            List<aca_MatriculaGrado_Info> ListaCalificacion = new List<aca_MatriculaGrado_Info>();
            ListaCalificacion = bus_calificacion.getList(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo);

            var info_catalogo = bus_catalogo.GetInfo(IdCatalogoParcial);
            var IdCatalogoParcialImportacion = info_catalogo == null ? null : info_catalogo.Codigo;
            foreach (var item in ListaCalificacion)
            {
                item.CalificacionGrado = item.CalificacionGrado == null ? (decimal?)null : Convert.ToDecimal(item.CalificacionGrado);
            }

            Lista_CalificacionGrado.set_list(ListaCalificacion, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return View(model);
        }
        [HttpPost]
        public ActionResult Importar(aca_MatriculaGrado_Info model)
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                string IdUsuario = SessionFixed.IdUsuario;
                bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
                var info_profesor = bus_profesor.GetInfo_x_Usuario(model.IdEmpresa, IdUsuario);
                var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
                var Lista_Calificaciones = Lista_CalificacionGrado.get_list(model.IdTransaccionSession);
                var Lista_CalificacionesGuardar = new List<aca_MatriculaGrado_Info>();
                Lista_CalificacionesGuardar = Lista_Calificaciones.Where(q => q.RegistroValido == true).ToList();
                ViewBag.mensaje = null;
                ViewBag.MensajeSuccess = null;

                foreach (var item in Lista_CalificacionesGuardar)
                {
                    item.IdAnio = model.IdAnio;
                    item.IdSede = model.IdSede;
                    item.IdNivel = model.IdNivel;
                    item.IdJornada = model.IdJornada;
                    item.IdCurso = model.IdCurso;
                    item.IdParalelo = model.IdParalelo;
                }
                ViewBag.mensaje = null;

                foreach (var item in Lista_CalificacionesGuardar)
                {
                    if (!bus_calificacion.modificarDB(item))
                    {
                        ViewBag.mensaje = "Error al importar el archivo";
                        return View(model);
                    }
                }
                ViewBag.MensajeSuccess = MensajeSuccess;
            
                return RedirectToAction("Importar", new { IdEmpresa = model.IdEmpresa, IdSede = model.IdSede, IdAnio = model.IdAnio, IdNivel = model.IdNivel, IdJornada = model.IdJornada, IdCurso = model.IdCurso, IdParalelo = model.IdParalelo, isSuccess = true });
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message.ToString();
                return View(model);
            }
        }

        public ActionResult GridViewPartial_MatriculaGradoImportacion()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CalificacionGrado.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_MatriculaGradoImportacion", model);
        }
        #endregion

        #region Json
        public JsonResult cargar_calificaciones(int IdEmpresa = 0, int IdSede = 0, int IdAnio = 0, int IdNivel = 0, int IdJornada = 0, int IdCurso = 0, int IdParalelo = 0, int IdMateria = 0, int IdCatalogoParcial = 0)
        {
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);
            var info_profesor = bus_profesor.GetInfo_x_Usuario(IdEmpresa, IdUsuario);
            var IdProfesor = (info_profesor == null ? 0 : info_profesor.IdProfesor);
            var info_anio = bus_anio.GetInfo(IdEmpresa, IdAnio);
            List<aca_MatriculaGrado_Info> ListaCalificacion = new List<aca_MatriculaGrado_Info>();

            ListaCalificacion = bus_calificacion.getList(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo);
            Lista_CalificacionGrado.set_list(ListaCalificacion, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return Json(ListaCalificacion, JsonRequestBehavior.AllowGet);
        }

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
            List<aca_MatriculaGrado_Info> ListaCalificacionExamen = new List<aca_MatriculaGrado_Info>();
            string IdUsuario = SessionFixed.IdUsuario;
            bool EsSuperAdmin = Convert.ToBoolean(SessionFixed.EsSuperAdmin);

            Lista_CalificacionGrado.set_list(ListaCalificacionExamen, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return Json(EsSuperAdmin, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class aca_MatriculaGrado_List
    {
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_MatriculaGrado_Bus bus_calificacion = new aca_MatriculaGrado_Bus();

        string Variable = "aca_MatriculaGrado_Info";
        public List<aca_MatriculaGrado_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_MatriculaGrado_Info> list = new List<aca_MatriculaGrado_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_MatriculaGrado_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_MatriculaGrado_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void UpdateRow(aca_MatriculaGrado_Info info_det, decimal IdTransaccionSession)
        {
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);
            var info_matricula = bus_matricula.GetInfo(IdEmpresa, info_det.IdMatricula);
            var info_anio = bus_anio.GetInfo(IdEmpresa, info_matricula.IdAnio);
            aca_MatriculaGrado_Info edited_info = get_list(IdTransaccionSession).Where(m => m.IdMatricula == info_det.IdMatricula).FirstOrDefault();
            edited_info.CalificacionGrado = info_det.CalificacionGrado;
            edited_info.RegistroValido = info_det.RegistroValido;

            if (edited_info.RegistroValido==true)
            {
                bus_calificacion.modificarDB(edited_info);
            }
            
        }
    }

    public class UploadControlSettings_Grado
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".xlsx" },
            MaxFileSize = 40000000
        };

        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            #region Variables
            aca_MatriculaGrado_List Lista_Grado = new aca_MatriculaGrado_List();
            List<aca_MatriculaGrado_Info> Lista_Calificacion = new List<aca_MatriculaGrado_Info>();
            aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
            aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
            int cont = 0;
            decimal IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
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
                        var IdMatricula = (Convert.ToInt32(reader.GetValue(0)));
                        var pe_nombreCompleto = (Convert.ToString(reader.GetValue(1)).Trim());
                        var x = Convert.ToString(reader.GetValue(1));
                        decimal? CalificacionGrado = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(2))) ? (decimal?)null : Convert.ToDecimal(reader.GetValue(2));            
                        var info_matricula = bus_matricula.GetInfo(IdEmpresa, IdMatricula);
                        var info_anio = bus_anio.GetInfo(IdEmpresa, info_matricula.IdAnio);
                        aca_MatriculaGrado_Info info = new aca_MatriculaGrado_Info
                        {
                            IdEmpresa = IdEmpresa,
                            IdMatricula = IdMatricula,
                            pe_nombreCompleto = pe_nombreCompleto,
                            CalificacionGrado = CalificacionGrado,
                            RegistroValido = (info_anio==null ? false : (CalificacionGrado<= Convert.ToDecimal(info_anio.CalificacionMaxima) ? true :false)),
                        };

                        Lista_Calificacion.Add(info);
                    }
                    else
                        cont++;
                }

                Lista_Grado.set_list(Lista_Calificacion, IdTransaccionSession);
            }
        }
    }
}