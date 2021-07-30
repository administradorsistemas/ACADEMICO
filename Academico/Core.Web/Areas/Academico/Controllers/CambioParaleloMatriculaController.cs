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
    public class CambioParaleloMatriculaController : Controller
    {
        #region Variables
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        aca_Matricula_List Lista_Matricula = new aca_Matricula_List();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        aca_Familia_Bus bus_familia = new aca_Familia_Bus();
        aca_Matricula_PorCurso_List Lista_Matricula_PorCurso = new aca_Matricula_PorCurso_List();
        aca_MecanismoDePago_Bus bus_mecanismo = new aca_MecanismoDePago_Bus();
        aca_Plantilla_Rubro_Bus bus_plantilla_rubro = new aca_Plantilla_Rubro_Bus();
        //aca_Plantilla_Rubro_List Lista_DetallePlantilla = new aca_Plantilla_Rubro_List();
        aca_PermisoMatricula_Bus bus_permiso = new aca_PermisoMatricula_Bus();
        aca_Matricula_Rubro_List ListaMatriculaRubro = new aca_Matricula_Rubro_List();
        aca_Matricula_Rubro_Bus bus_matricula_rubro = new aca_Matricula_Rubro_Bus();
        aca_AnioLectivo_Jornada_Curso_Bus bus_jornada_curso = new aca_AnioLectivo_Jornada_Curso_Bus();
        aca_Plantilla_Bus bus_plantilla = new aca_Plantilla_Bus();
        aca_AlumnoDocumento_Bus bus_alumno_documento = new aca_AlumnoDocumento_Bus();
        aca_AnioLectivo_Curso_Documento_List Lista_DocumentosMatricula = new aca_AnioLectivo_Curso_Documento_List();
        aca_AnioLectivo_Jornada_Curso_Bus bus_aniolectivo_jornada_curso = new aca_AnioLectivo_Jornada_Curso_Bus();
        aca_AnioLectivo_Curso_Documento_Bus bus_curso_documento = new aca_AnioLectivo_Curso_Documento_Bus();
        aca_SocioEconomico_Bus bus_socioeconomico = new aca_SocioEconomico_Bus();
        aca_AnioLectivo_Paralelo_Profesor_Bus bus_materias_x_paralelo = new aca_AnioLectivo_Paralelo_Profesor_Bus();
        aca_AnioLectivo_Periodo_Bus bus_anio_periodo = new aca_AnioLectivo_Periodo_Bus();
        aca_Paralelo_Bus bus_paralelo = new aca_Paralelo_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        string mensaje = string.Empty;
        aca_MatriculaCalificacionParcial_Bus bus_calificacion_parcial = new aca_MatriculaCalificacionParcial_Bus();
        aca_MatriculaCalificacion_Bus bus_calificacion = new aca_MatriculaCalificacion_Bus();
        aca_MatriculaCalificacionCualitativa_Bus bus_calificacion_cualitativa = new aca_MatriculaCalificacionCualitativa_Bus();
        aca_MatriculaCalificacionCualitativaPromedio_Bus bus_calificacion_cualitativa_promedio = new aca_MatriculaCalificacionCualitativaPromedio_Bus();
        aca_AnioLectivoParcial_Bus bus_parcial = new aca_AnioLectivoParcial_Bus();
        aca_MatriculaConducta_Bus bus_conducta = new aca_MatriculaConducta_Bus();
        aca_MatriculaAsistencia_Bus bus_asistencia = new aca_MatriculaAsistencia_Bus();
        aca_MatriculaGrado_Bus bus_grado = new aca_MatriculaGrado_Bus();
        #endregion

        #region Combos bajo demanada
        public ActionResult Cmb_MatriculaAlumno()
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
            int IdMatricula = !string.IsNullOrEmpty(Request.Params["IdMatricula"]) ? int.Parse(Request.Params["IdMatricula"]) : -1;
            return PartialView("_ComboBoxPartial_Curso", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada, IdMatricula= IdMatricula });
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

        #endregion

        #region Metodos
        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_mecanismo = bus_mecanismo.GetList(IdEmpresa, false);
            ViewBag.lst_mecanismo = lst_mecanismo;
        }

        private bool validar(aca_Matricula_Info info, ref string msg)
        {
            if (string.IsNullOrEmpty(info.ObservacionCambio))
            {
                msg = "Debe de ingresar observación";
                return false;
            }

            return true;
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

            aca_Matricula_Info model = new aca_Matricula_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdAnio = info_anio.IdAnio,
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<aca_Matricula_Info> lista = bus_matricula.GetList(model.IdEmpresa, model.IdAnio, model.IdSede, true);
            Lista_Matricula.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(aca_Matricula_Info model)
        {
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            List<aca_Matricula_Info> lista = bus_matricula.GetList(model.IdEmpresa, model.IdAnio, model.IdSede, true);
            Lista_Matricula.set_list(lista, Convert.ToDecimal(model.IdTransaccionSession));

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_CambioParaleloMatricula()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            List<aca_Matricula_Info> model = Lista_Matricula.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_CambioParaleloMatricula", model);
        }
        #endregion

        #region Json
        public JsonResult SetMatricula_PorCurso(int IdEmpresa = 0, int IdAnio = 0, int IdSede=0, int IdNivel=0, int IdJornada=0, int IdCurso=0, int IdParalelo = 0)
        {
            var lista_PorCurso = bus_matricula.GetList_PorCurso(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo);
            Lista_Matricula_PorCurso.set_list(lista_PorCurso, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return Json(lista_PorCurso, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GridDetalle alumnos paralelo
        [ValidateInput(false)]
        public ActionResult GridViewPartial_AlumnosPorParalelo()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            List<aca_Matricula_Info> model = Lista_Matricula_PorCurso.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_AlumnosPorParalelo", model);
        }
        #endregion

        #region Acciones
        public ActionResult Consultar(int IdEmpresa = 0, int IdMatricula = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_Matricula_Info model = bus_matricula.GetInfo(IdEmpresa, IdMatricula);
            model.Validar = "N";

            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "CambioParaleloMatricula", "Index");
            if (model.BloquearMatricula == true)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_matricula_curso = new List<aca_Matricula_Info>();
            model.lst_matricula_curso = bus_matricula.GetList_PorCurso(model.IdEmpresa, model.IdAnio, model.IdSede, model.IdNivel, model.IdJornada, model.IdCurso, model.IdParalelo);
            Lista_Matricula_PorCurso.set_list(model.lst_matricula_curso, model.IdTransaccionSession);

            model.lst_MatriculaRubro = new List<aca_Matricula_Rubro_Info>();
            model.lst_MatriculaRubro = bus_matricula_rubro.GetList(model.IdEmpresa, model.IdMatricula);
            ListaMatriculaRubro.set_list(model.lst_MatriculaRubro, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            cargar_combos();
            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0, int IdMatricula = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_Matricula_Info model = bus_matricula.GetInfo(IdEmpresa, IdMatricula);
            model.Validar = "N";

            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "CambioParaleloMatricula", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_matricula_curso = new List<aca_Matricula_Info>();
            model.lst_matricula_curso = bus_matricula.GetList_PorCurso(model.IdEmpresa, model.IdAnio, model.IdSede, model.IdNivel, model.IdJornada, model.IdCurso, model.IdParalelo);
            Lista_Matricula_PorCurso.set_list(model.lst_matricula_curso, model.IdTransaccionSession);

            model.lst_MatriculaRubro = new List<aca_Matricula_Rubro_Info>();
            model.lst_MatriculaRubro = bus_matricula_rubro.GetList(model.IdEmpresa, model.IdMatricula);
            ListaMatriculaRubro.set_list(model.lst_MatriculaRubro, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(aca_Matricula_Info model)
        {
            model.info_MatriculaCambios = new aca_MatriculaCambios_Info();
            aca_Matricula_Info info_matricula = bus_matricula.GetInfo(model.IdEmpresa, model.IdMatricula);

            model.info_MatriculaCambios = new aca_MatriculaCambios_Info
            {
                IdEmpresa = info_matricula.IdEmpresa,
                IdMatricula = info_matricula.IdMatricula,
                IdAnio = info_matricula.IdAnio,
                IdSede = info_matricula.IdSede,
                IdNivel = info_matricula.IdNivel,
                IdJornada = info_matricula.IdJornada,
                IdCurso = info_matricula.IdCurso,
                IdParalelo = info_matricula.IdParalelo,
                IdPlantilla = info_matricula.IdPlantilla,
                TipoCambio = "CURSOPARALELO",
                IdUsuarioCreacion = SessionFixed.IdUsuario
            };

            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            model.lst_MatriculaRubro = ListaMatriculaRubro.get_list(model.IdTransaccionSession);

            foreach (var item in model.lst_MatriculaRubro)
            {
                item.IdSede = model.IdSede;
                item.IdNivel = model.IdNivel;
                item.IdJornada = model.IdJornada;
                item.IdCurso = model.IdCurso;
                item.IdParalelo = model.IdParalelo;
            }

            List<aca_MatriculaCalificacionParcial_Info> lst_calificacion_parcial_existente = new List<aca_MatriculaCalificacionParcial_Info>();
            List<aca_MatriculaCalificacion_Info> lst_calificacion_existente = new List<aca_MatriculaCalificacion_Info>();

            List<aca_MatriculaCalificacionCualitativa_Info> lst_calificacion_cualitativa_parcial_existente = new List<aca_MatriculaCalificacionCualitativa_Info>();
            List<aca_MatriculaCalificacionCualitativaPromedio_Info> lst_calificacion_cualitativa_promedio_existente = new List<aca_MatriculaCalificacionCualitativaPromedio_Info>();

            List<aca_MatriculaCalificacionParticipacion_Info> lst_participacion_existente = new List<aca_MatriculaCalificacionParticipacion_Info>();

            List<aca_MatriculaCalificacionParcial_Info> lst_calificacion_parcial = new List<aca_MatriculaCalificacionParcial_Info>();
            List<aca_MatriculaCalificacion_Info> lst_calificacion = new List<aca_MatriculaCalificacion_Info>();

            List<aca_MatriculaCalificacionCualitativa_Info> lst_calificacion_cualitativa = new List<aca_MatriculaCalificacionCualitativa_Info>();
            List<aca_MatriculaCalificacionCualitativaPromedio_Info> lst_calificacion_cualitativa_promedio = new List<aca_MatriculaCalificacionCualitativaPromedio_Info>();

            aca_MatriculaConducta_Info info_conducta = new aca_MatriculaConducta_Info();
            aca_MatriculaAsistencia_Info info_asistencia = new aca_MatriculaAsistencia_Info();
            List<aca_MatriculaCalificacionParticipacion_Info> lst_participacion = new List<aca_MatriculaCalificacionParticipacion_Info>();
            aca_MatriculaGrado_Info info_CalificacionGrado = new aca_MatriculaGrado_Info();

            var lst_parcial = bus_parcial.GetList_x_Tipo(model.IdEmpresa, model.IdSede, model.IdAnio, Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1));
            lst_parcial.AddRange(bus_parcial.GetList_x_Tipo(model.IdEmpresa, model.IdSede, model.IdAnio, Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2)));
            var lst_materias_x_curso = bus_materias_x_paralelo.GetList(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdNivel, model.IdJornada, model.IdCurso, model.IdParalelo);

            var lst_materias_cualitativas = lst_materias_x_curso.Where(q => q.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUALI)).ToList();
            var lst_materias_cuantitativas = lst_materias_x_curso.Where(q => q.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUANTI)).ToList();

            lst_calificacion_parcial_existente = bus_calificacion_parcial.GetList(model.IdEmpresa, model.IdMatricula);
            lst_calificacion_existente = bus_calificacion.GetList(model.IdEmpresa, model.IdMatricula);

            lst_calificacion_cualitativa_parcial_existente = bus_calificacion_cualitativa.getList(model.IdEmpresa, model.IdMatricula);
            lst_calificacion_cualitativa_promedio_existente = bus_calificacion_cualitativa_promedio.GetList(model.IdEmpresa, model.IdMatricula);

            #region Cualitativas
            var info_Grado = bus_grado.getInfo_X_Matricula(model.IdEmpresa, model.IdMatricula);
            var info_anio = bus_anio.GetInfo(model.IdEmpresa, model.IdAnio);
            model.lst_MatriculaGrado = new List<aca_MatriculaGrado_Info>();
            if (info_anio.IdCursoBachiller == model.IdCurso)
            {
                if (info_Grado==null)
                {
                    info_CalificacionGrado = new aca_MatriculaGrado_Info{
                        IdEmpresa = model.IdEmpresa,
                        IdMatricula=model.IdMatricula,
                        CalificacionGrado = (decimal?)null,
                        IdUsuarioCreacion = SessionFixed.IdUsuario,
                        FechaCreacion = DateTime.Now,
                    };
                }
                else
                {
                    info_CalificacionGrado = new aca_MatriculaGrado_Info
                    {
                        IdEmpresa = model.IdEmpresa,
                        IdMatricula = model.IdMatricula,
                        CalificacionGrado = info_Grado.CalificacionGrado,
                        IdUsuarioCreacion = info_Grado.IdUsuarioCreacion,
                        FechaCreacion = info_Grado.FechaCreacion,
                        IdUsuarioModificacion = SessionFixed.IdUsuario,
                        FechaModificacion = DateTime.Now,
                    };
                }
                
                model.lst_MatriculaGrado.Add(info_CalificacionGrado);
            }

            if (lst_materias_cualitativas != null && lst_materias_cualitativas.Count > 0)
            {
                foreach (var item_materias_cualit in lst_materias_cualitativas)
                {
                    if (lst_parcial.Count() > 0)
                    {
                        foreach (var item_p in lst_parcial)
                        {
                            var calificacion_cualitativa_parcial = lst_calificacion_cualitativa_parcial_existente.Where(q => q.IdCatalogoParcial == item_p.IdCatalogoParcial && q.IdMateria == item_materias_cualit.IdMateria).FirstOrDefault();
                            var info_cualitativa = new aca_MatriculaCalificacionCualitativa_Info();
                            if (calificacion_cualitativa_parcial!=null)
                            {
                                info_cualitativa = new aca_MatriculaCalificacionCualitativa_Info()
                                {
                                    IdEmpresa = model.IdEmpresa,
                                    IdMatricula = model.IdMatricula,
                                    IdMateria = item_materias_cualit.IdMateria,
                                    IdCatalogoParcial = calificacion_cualitativa_parcial.IdCatalogoParcial,
                                    IdProfesor = item_materias_cualit.IdProfesor,
                                    IdCalificacionCualitativa = (calificacion_cualitativa_parcial.IdCalificacionCualitativa == null ? (int?)null : calificacion_cualitativa_parcial.IdCalificacionCualitativa),
                                    Conducta = (calificacion_cualitativa_parcial.Conducta == null ? null : calificacion_cualitativa_parcial.Conducta),
                                    MotivoConducta = (calificacion_cualitativa_parcial.MotivoConducta == null ? null : calificacion_cualitativa_parcial.MotivoConducta),
                                    IdUsuarioCreacion = (calificacion_cualitativa_parcial.IdUsuarioCreacion == null ? SessionFixed.IdUsuario : calificacion_cualitativa_parcial.IdUsuarioCreacion),
                                    FechaCreacion = (calificacion_cualitativa_parcial.FechaCreacion == null ? DateTime.Now : calificacion_cualitativa_parcial.FechaCreacion),
                                    IdUsuarioModificacion = (calificacion_cualitativa_parcial.IdUsuarioModificacion == null ? null : SessionFixed.IdUsuario),
                                    FechaModificacion = (calificacion_cualitativa_parcial.FechaModificacion == null ? (DateTime?)null : DateTime.Now)
                                };
                            }
                            else
                            {
                                info_cualitativa = new aca_MatriculaCalificacionCualitativa_Info()
                                {
                                    IdEmpresa = model.IdEmpresa,
                                    IdMatricula = model.IdMatricula,
                                    IdMateria = item_materias_cualit.IdMateria,
                                    IdCatalogoParcial = item_p.IdCatalogoParcial,
                                    IdProfesor = item_materias_cualit.IdProfesor,
                                    IdCalificacionCualitativa = (int?)null,
                                    Conducta = null,
                                    MotivoConducta = null ,
                                    IdUsuarioCreacion = SessionFixed.IdUsuario,
                                    FechaCreacion =  DateTime.Now,
                                    IdUsuarioModificacion = null,
                                    FechaModificacion = null
                                };
                            }
                            
                            lst_calificacion_cualitativa.Add(info_cualitativa);
                        }
                    }

                    var calificacion_cualitativa_promedio = lst_calificacion_cualitativa_promedio_existente.Where(q => q.IdMateria == item_materias_cualit.IdMateria).FirstOrDefault();
                    var info_cualitativa_promedio = new aca_MatriculaCalificacionCualitativaPromedio_Info();
                    if (calificacion_cualitativa_promedio!=null)
                    {
                        info_cualitativa_promedio = new aca_MatriculaCalificacionCualitativaPromedio_Info()
                        {
                            IdEmpresa = model.IdEmpresa,
                            IdMatricula = model.IdMatricula,
                            IdMateria = item_materias_cualit.IdMateria,
                            IdProfesor = item_materias_cualit.IdProfesor,
                            IdCalificacionCualitativaQ1 = calificacion_cualitativa_promedio.IdCalificacionCualitativaQ1 == null ? (int?)null : calificacion_cualitativa_promedio.IdCalificacionCualitativaQ1,
                            PromedioQ1 = calificacion_cualitativa_promedio.PromedioQ1 == null ? (decimal?)null : calificacion_cualitativa_promedio.PromedioQ1,
                            IdCalificacionCualitativaQ2 = calificacion_cualitativa_promedio.IdCalificacionCualitativaQ2 == null ? (int?)null : calificacion_cualitativa_promedio.IdCalificacionCualitativaQ2,
                            PromedioQ2 = calificacion_cualitativa_promedio.PromedioQ2 == null ? (decimal?)null : calificacion_cualitativa_promedio.PromedioQ2,
                            IdCalificacionCualitativaFinal = calificacion_cualitativa_promedio.IdCalificacionCualitativaFinal == null ? (int?)null : calificacion_cualitativa_promedio.IdCalificacionCualitativaFinal,
                            PromedioFinal = calificacion_cualitativa_promedio.PromedioFinal == null ? (decimal?)null : calificacion_cualitativa_promedio.PromedioFinal,
                            IdUsuarioCreacion = (calificacion_cualitativa_promedio.IdUsuarioCreacion == null ? SessionFixed.IdUsuario : calificacion_cualitativa_promedio.IdUsuarioCreacion),
                            FechaCreacion = (calificacion_cualitativa_promedio.FechaCreacion == null ? DateTime.Now : calificacion_cualitativa_promedio.FechaCreacion),
                            IdUsuarioModificacion = (calificacion_cualitativa_promedio.IdUsuarioModificacion == null ? null : SessionFixed.IdUsuario),
                            FechaModificacion = (calificacion_cualitativa_promedio.FechaModificacion == null ? (DateTime?)null : DateTime.Now)

                        };
                    }
                    else
                    {
                        info_cualitativa_promedio = new aca_MatriculaCalificacionCualitativaPromedio_Info()
                        {
                            IdEmpresa = model.IdEmpresa,
                            IdMatricula = model.IdMatricula,
                            IdMateria = item_materias_cualit.IdMateria,
                            IdProfesor = item_materias_cualit.IdProfesor,
                            IdCalificacionCualitativaQ1 =  (int?)null,
                            PromedioQ1 = (decimal?)null,
                            IdCalificacionCualitativaQ2 = (int?)null,
                            PromedioQ2 = (decimal?)null,
                            IdCalificacionCualitativaFinal = (int?)null,
                            PromedioFinal = (decimal?)null,
                            IdUsuarioCreacion = SessionFixed.IdUsuario,
                            FechaCreacion = DateTime.Now,
                            IdUsuarioModificacion = null,
                            FechaModificacion=null

                        };
                    }
                    lst_calificacion_cualitativa_promedio.Add(info_cualitativa_promedio);
                }
            }
            model.lst_MatriculaCalificacionCualitativa = new List<aca_MatriculaCalificacionCualitativa_Info>();
            model.lst_MatriculaCalificacionCualitativa = lst_calificacion_cualitativa;

            model.lst_MatriculaCalificacionCualitativaPromedio = new List<aca_MatriculaCalificacionCualitativaPromedio_Info>();
            model.lst_MatriculaCalificacionCualitativaPromedio = lst_calificacion_cualitativa_promedio;
            #endregion

            #region Cuantitativa
            if (lst_materias_cuantitativas != null && lst_materias_cuantitativas.Count > 0)
            {
                foreach (var item_materias in lst_materias_cuantitativas)
                {
                    if (lst_parcial.Count() > 0)
                    {
                        foreach (var item_p in lst_parcial)
                        {
                            var calificacion_parcial = lst_calificacion_parcial_existente.Where(q => q.IdCatalogoParcial == item_p.IdCatalogoParcial && q.IdMateria == item_materias.IdMateria).FirstOrDefault();

                            var info_calificacion_parcial = new aca_MatriculaCalificacionParcial_Info
                            {
                                IdEmpresa = model.IdEmpresa,
                                IdMatricula = model.IdMatricula,
                                IdMateria = item_materias.IdMateria,
                                IdCatalogoParcial = item_p.IdCatalogoParcial,
                                IdProfesor = item_materias.IdProfesor,
                                Calificacion1 = (calificacion_parcial == null ? null : calificacion_parcial.Calificacion1),
                                Calificacion2 = (calificacion_parcial == null ? null : calificacion_parcial.Calificacion2),
                                Calificacion3 = (calificacion_parcial == null ? null : calificacion_parcial.Calificacion3),
                                Calificacion4 = (calificacion_parcial == null ? null : calificacion_parcial.Calificacion4),
                                Evaluacion = (calificacion_parcial == null ? null : calificacion_parcial.Evaluacion),
                                Remedial1 = (calificacion_parcial == null ? null : calificacion_parcial.Remedial1),
                                Remedial2 = (calificacion_parcial == null ? null : calificacion_parcial.Remedial2),
                                Conducta = (calificacion_parcial == null ? null : calificacion_parcial.Conducta),
                                MotivoCalificacion = (calificacion_parcial == null ? null : calificacion_parcial.MotivoCalificacion),
                                MotivoConducta = (calificacion_parcial == null ? null : calificacion_parcial.MotivoConducta),
                                AccionRemedial = (calificacion_parcial == null ? null : calificacion_parcial.AccionRemedial),
                                IdUsuarioCreacion = (calificacion_parcial == null ? SessionFixed.IdUsuario : calificacion_parcial.IdUsuarioCreacion),
                                FechaCreacion = (calificacion_parcial == null ? DateTime.Now : calificacion_parcial.FechaCreacion),
                                IdUsuarioModificacion = (calificacion_parcial == null ? null : SessionFixed.IdUsuario),
                                FechaModificacion = (calificacion_parcial == null ? (DateTime?)null : calificacion_parcial.FechaModificacion),
                            };

                            lst_calificacion_parcial.Add(info_calificacion_parcial);
                        }
                    }

                    var calificacion = lst_calificacion_existente.Where(q => q.IdMateria == item_materias.IdMateria).FirstOrDefault();

                    var info_calificacion = new aca_MatriculaCalificacion_Info
                    {
                        IdEmpresa = model.IdEmpresa,
                        IdMatricula = model.IdMatricula,
                        IdMateria = item_materias.IdMateria,
                        IdProfesor = item_materias.IdProfesor,
                        CalificacionP1 = (calificacion == null ? null : calificacion.CalificacionP1),
                        CalificacionP2 = (calificacion == null ? null : calificacion.CalificacionP2),
                        CalificacionP3 = (calificacion == null ? null : calificacion.CalificacionP3),
                        CalificacionP4 = (calificacion == null ? null : calificacion.CalificacionP4),
                        CalificacionP5 = (calificacion == null ? null : calificacion.CalificacionP5),
                        CalificacionP6 = (calificacion == null ? null : calificacion.CalificacionP6),
                        PromedioQ1 = (calificacion == null ? null : calificacion.PromedioQ1),
                        PromedioQ2 = (calificacion == null ? null : calificacion.PromedioQ2),
                        ExamenQ1 = (calificacion == null ? null : calificacion.ExamenQ1),
                        ExamenQ2 = (calificacion == null ? null : calificacion.ExamenQ2),
                        PromedioFinalQ1 = (calificacion == null ? null : calificacion.PromedioFinalQ1),
                        PromedioFinalQ2 = (calificacion == null ? null : calificacion.PromedioFinalQ2),
                        ExamenMejoramiento = (calificacion == null ? null : calificacion.ExamenMejoramiento),
                        ExamenSupletorio = (calificacion == null ? null : calificacion.ExamenSupletorio),
                        ExamenRemedial = (calificacion == null ? null : calificacion.ExamenRemedial),
                        ExamenGracia = (calificacion == null ? null : calificacion.ExamenGracia),
                        PromedioFinal = (calificacion == null ? null : calificacion.PromedioFinal)
                    };

                    lst_calificacion.Add(info_calificacion);
                }
            }

            model.lst_MatriculaCalificacionParcial = new List<aca_MatriculaCalificacionParcial_Info>();
            model.lst_MatriculaCalificacionParcial = lst_calificacion_parcial;

            model.lst_MatriculaCalificacion = new List<aca_MatriculaCalificacion_Info>();
            model.lst_MatriculaCalificacion = lst_calificacion;
            #endregion

            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos();
                return View(model);
            }

            if (!bus_matricula.ModificarCursoParaleloDB(model))
            {
                ViewBag.mensaje = "No se ha podido modificar el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos();
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMatricula = model.IdMatricula, Exito = true });
        }
        #endregion
    }
}