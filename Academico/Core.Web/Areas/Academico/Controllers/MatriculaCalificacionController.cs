using Core.Bus.Academico;
using Core.Bus.General;
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
    public class MatriculaCalificacionController : Controller
    {
        #region Variables
        aca_Sede_Bus bus_sede = new aca_Sede_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_NivelAcademico_Bus bus_nivel = new aca_NivelAcademico_Bus();
        aca_Jornada_Bus bus_jornada = new aca_Jornada_Bus();
        aca_Curso_Bus bus_curso = new aca_Curso_Bus();
        aca_Paralelo_Bus bus_paralelo = new aca_Paralelo_Bus();
        aca_Materia_Bus bus_materia = new aca_Materia_Bus();
        aca_AnioLectivo_Paralelo_Profesor_Bus bus_MateriaPorProfesor = new aca_AnioLectivo_Paralelo_Profesor_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        aca_MatriculaCalificacionParcial_Bus bus_calificacion_parcial = new aca_MatriculaCalificacionParcial_Bus();
        aca_MatriculaCalificacion_Bus bus_calificacion = new aca_MatriculaCalificacion_Bus();
        aca_AnioLectivo_Paralelo_Profesor_Bus bus_materias_x_paralelo = new aca_AnioLectivo_Paralelo_Profesor_Bus();
        aca_AnioLectivoParcial_Bus bus_parcial = new aca_AnioLectivoParcial_Bus();
        aca_MatriculaConducta_Bus bus_conducta = new aca_MatriculaConducta_Bus();
        aca_MatriculaGeneracionCalificaciones_List Lista_MatriculaCalificaciones = new aca_MatriculaGeneracionCalificaciones_List();
        aca_MatriculaCalificacionCualitativa_Bus bus_calificacion_cualitativa = new aca_MatriculaCalificacionCualitativa_Bus();
        aca_MatriculaCalificacionCualitativaPromedio_Bus bus_calificacion_cualitativa_promedio = new aca_MatriculaCalificacionCualitativaPromedio_Bus();
        aca_MatriculaAsistencia_Bus bus_asistencia = new aca_MatriculaAsistencia_Bus();
        aca_MatriculaGrado_Bus bus_grado = new aca_MatriculaGrado_Bus();
        string mensaje = string.Empty;
        string MensajeSuccess = string.Empty;
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
            var info = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);
            aca_MatriculaCalificacion_Info model = new aca_MatriculaCalificacion_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdAnio = (info == null ? 0 : info.IdAnio),
                IdNivel = 0,
                IdJornada = 0,
                IdCurso = 0,
                IdParalelo = 0,
                IdAlumno = 0,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            List<aca_Matricula_Info> lista = new List<aca_Matricula_Info>();
            Lista_MatriculaCalificaciones.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(aca_MatriculaCalificacion_Info model)
        {
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();

            List<aca_Matricula_Info> lista = bus_matricula.GetList_Calificaciones(model.IdEmpresa, model.IdAnio, model.IdSede, model.IdNivel, model.IdJornada, model.IdCurso, model.IdParalelo, model.IdAlumno);
            List<aca_MatriculaCalificacionParcial_Info> lst_calificacion_parcial_existente = new List<aca_MatriculaCalificacionParcial_Info>();
            List<aca_MatriculaCalificacionCualitativa_Info> lst_calificacion_cualitativa_parcial_existente = new List<aca_MatriculaCalificacionCualitativa_Info>();
            List<aca_MatriculaCalificacionCualitativaPromedio_Info> lst_calificacion_cualitativa_existente = new List<aca_MatriculaCalificacionCualitativaPromedio_Info>();
            List<aca_MatriculaCalificacion_Info> lst_calificacion_existente = new List<aca_MatriculaCalificacion_Info>();
            List<aca_MatriculaConducta_Info> lst_conducta_existente = new List<aca_MatriculaConducta_Info>();
            List<aca_MatriculaAsistencia_Info> lst_asistencia_existente = new List<aca_MatriculaAsistencia_Info>();

            List<aca_MatriculaCalificacionParcial_Info> lst_calificacion_parcial = new List<aca_MatriculaCalificacionParcial_Info>();
            List<aca_MatriculaCalificacionCualitativa_Info> lst_calificacion_cualitativa = new List<aca_MatriculaCalificacionCualitativa_Info>();
            List<aca_MatriculaCalificacionCualitativaPromedio_Info> lst_calificacion_cualitativa_promedio = new List<aca_MatriculaCalificacionCualitativaPromedio_Info>();
            List<aca_MatriculaCalificacion_Info> lst_calificacion = new List<aca_MatriculaCalificacion_Info>();
            List<aca_MatriculaConducta_Info> lst_conducta = new List<aca_MatriculaConducta_Info>();
            List<aca_MatriculaAsistencia_Info> lst_asistencia = new List<aca_MatriculaAsistencia_Info>();
            List<aca_MatriculaGrado_Info> lst_MatriculaGrado = new List<aca_MatriculaGrado_Info>();

            if (lista.Count()>0)
            {
                foreach (var item in lista)
                {
                    lst_calificacion_parcial_existente = bus_calificacion_parcial.GetList(model.IdEmpresa, item.IdMatricula);
                    lst_calificacion_cualitativa_parcial_existente = bus_calificacion_cualitativa.getList(model.IdEmpresa, item.IdMatricula);
                    lst_calificacion_cualitativa_existente = bus_calificacion_cualitativa_promedio.GetList(model.IdEmpresa, item.IdMatricula);
                    lst_calificacion_existente = bus_calificacion.GetList(model.IdEmpresa, item.IdMatricula);
                    lst_conducta_existente = bus_conducta.GetList(model.IdEmpresa, item.IdMatricula);
                    lst_asistencia_existente = bus_asistencia.getList(model.IdEmpresa, item.IdMatricula);

                    var lst_materias_x_curso = bus_materias_x_paralelo.GetList(item.IdEmpresa, item.IdSede, item.IdAnio, item.IdNivel, item.IdJornada, item.IdCurso, item.IdParalelo);
                    var lst_materias_cualitativas = lst_materias_x_curso.Where(q => q.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUALI)).ToList();
                    var lst_materias_cuantitativas = lst_materias_x_curso.Where(q => q.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUANTI)).ToList();

                    var lst_parcial = bus_parcial.GetList_x_Tipo(model.IdEmpresa, model.IdSede, model.IdAnio, Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1));
                    lst_parcial.AddRange(bus_parcial.GetList_x_Tipo(model.IdEmpresa, model.IdSede, model.IdAnio, Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2)));

                    var conducta = lst_conducta_existente.Where(q => q.IdMatricula == item.IdMatricula).FirstOrDefault();

                    #region Grado
                    var info_Grado = bus_grado.getInfo_X_Matricula(model.IdEmpresa, item.IdMatricula);
                    var info_anio = bus_anio.GetInfo(model.IdEmpresa, model.IdAnio);                    
                    var info_CalificacionGrado = new aca_MatriculaGrado_Info();

                    if (info_anio.IdCursoBachiller == model.IdCurso)
                    {
                        if (info_Grado == null)
                        {
                            info_CalificacionGrado = new aca_MatriculaGrado_Info
                            {
                                IdEmpresa = model.IdEmpresa,
                                IdMatricula = item.IdMatricula,
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
                                IdMatricula = item.IdMatricula,
                                CalificacionGrado = info_Grado.CalificacionGrado,
                                IdUsuarioCreacion = info_Grado.IdUsuarioCreacion,
                                FechaCreacion = info_Grado.FechaCreacion,
                                IdUsuarioModificacion = SessionFixed.IdUsuario,
                                FechaModificacion = DateTime.Now,
                            };
                        }

                        lst_MatriculaGrado.Add(info_CalificacionGrado);
                    }
                    #endregion

                    #region Cualitativas
                    if (lst_materias_cualitativas != null && lst_materias_cualitativas.Count > 0)
                    {
                        foreach (var item_materias_cualit in lst_materias_cualitativas)
                        {
                            var calificacion_cualitativa = lst_calificacion_cualitativa_existente.Where(q => q.IdMateria == item_materias_cualit.IdMateria).FirstOrDefault();
                            if (lst_parcial.Count() > 0)
                            {
                                foreach (var item_p in lst_parcial)
                                {
                                    var calificacion_cualitativa_parcial = lst_calificacion_cualitativa_parcial_existente.Where(q => q.IdCatalogoParcial == item_p.IdCatalogoParcial && q.IdMateria == item_materias_cualit.IdMateria).FirstOrDefault();

                                    var info_cualitativa = new aca_MatriculaCalificacionCualitativa_Info()
                                    {
                                        IdEmpresa = item.IdEmpresa,
                                        IdMatricula = item.IdMatricula,
                                        IdMateria = item_materias_cualit.IdMateria,
                                        IdCatalogoParcial = item_p.IdCatalogoParcial,
                                        IdProfesor = item_materias_cualit.IdProfesor,
                                        IdCalificacionCualitativa = (calificacion_cualitativa_parcial == null ? null : calificacion_cualitativa_parcial.IdCalificacionCualitativa),
                                        Conducta = (calificacion_cualitativa_parcial == null ? null : calificacion_cualitativa_parcial.Conducta),
                                        MotivoConducta = (calificacion_cualitativa_parcial == null ? null : calificacion_cualitativa_parcial.MotivoConducta),
                                        IdUsuarioCreacion = (calificacion_cualitativa_parcial == null ? SessionFixed.IdUsuario :calificacion_cualitativa_parcial.IdUsuarioCreacion),
                                        FechaCreacion = (calificacion_cualitativa_parcial == null ? DateTime.Now : calificacion_cualitativa_parcial.FechaCreacion),
                                        IdUsuarioModificacion = (calificacion_cualitativa_parcial == null ? null : SessionFixed.IdUsuario),
                                        FechaModificacion = (calificacion_cualitativa_parcial == null ? (DateTime?)null : calificacion_cualitativa_parcial.FechaModificacion)
                                    };
                                    lst_calificacion_cualitativa.Add(info_cualitativa);
                                }
                            }

                            var info_calificacion_cualitativa = new aca_MatriculaCalificacionCualitativaPromedio_Info
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdMatricula = item.IdMatricula,
                                IdMateria = item_materias_cualit.IdMateria,
                                IdProfesor = item_materias_cualit.IdProfesor,
                                IdCalificacionCualitativaQ1 = (calificacion_cualitativa == null ? null : calificacion_cualitativa.IdCalificacionCualitativaQ1),
                                PromedioQ1 = (calificacion_cualitativa == null ? null : calificacion_cualitativa.PromedioQ1),
                                IdCalificacionCualitativaQ2 = (calificacion_cualitativa == null ? null : calificacion_cualitativa.IdCalificacionCualitativaQ2),
                                PromedioQ2 = (calificacion_cualitativa == null ? null : calificacion_cualitativa.PromedioQ2),
                                IdCalificacionCualitativaFinal = (calificacion_cualitativa == null ? null : calificacion_cualitativa.IdCalificacionCualitativaFinal),
                                PromedioFinal = (calificacion_cualitativa == null ? null : calificacion_cualitativa.PromedioFinal),
                                IdUsuarioCreacion = (calificacion_cualitativa == null ? SessionFixed.IdUsuario : calificacion_cualitativa.IdUsuarioCreacion),
                                FechaCreacion = (calificacion_cualitativa == null ? DateTime.Now : calificacion_cualitativa.FechaCreacion),
                                IdUsuarioModificacion = (calificacion_cualitativa == null ? null : SessionFixed.IdUsuario),
                                FechaModificacion = (calificacion_cualitativa == null ? (DateTime?)null : calificacion_cualitativa.FechaModificacion)

                            };

                            lst_calificacion_cualitativa_promedio.Add(info_calificacion_cualitativa);
                        }
                        var x = lst_calificacion_cualitativa_promedio;
                    }
                    #endregion

                    #region Calificacion y conducta
                    if (lst_materias_cuantitativas != null && lst_materias_cuantitativas.Count > 0)
                    {
                        foreach (var item_materias in lst_materias_cuantitativas)
                        {
                            var calificacion = lst_calificacion_existente.Where(q => q.IdMateria == item_materias.IdMateria).FirstOrDefault();

                            if (lst_parcial.Count() > 0)
                            {
                                foreach (var item_p in lst_parcial)
                                {
                                    var calificacion_parcial = lst_calificacion_parcial_existente.Where(q => q.IdCatalogoParcial == item_p.IdCatalogoParcial && q.IdMateria == item_materias.IdMateria).FirstOrDefault();

                                    var info_calificacion_parcial = new aca_MatriculaCalificacionParcial_Info
                                    {
                                        IdEmpresa = item.IdEmpresa,
                                        IdMatricula = item.IdMatricula,
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

                                var info_calificacion = new aca_MatriculaCalificacion_Info
                                {
                                    IdEmpresa = item.IdEmpresa,
                                    IdMatricula = item.IdMatricula,
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
                                    PromedioQuimestres = (calificacion == null ? null : calificacion.PromedioQuimestres),
                                    ExamenQ2 = (calificacion == null ? null : calificacion.ExamenQ2),
                                    PromedioFinalQ1 = (calificacion == null ? null : calificacion.PromedioFinalQ1),
                                    PromedioFinalQ2 = (calificacion == null ? null : calificacion.PromedioFinalQ2),
                                    ExamenMejoramiento = (calificacion == null ? null : calificacion.ExamenMejoramiento),
                                    ExamenSupletorio = (calificacion == null ? null : calificacion.ExamenSupletorio),
                                    ExamenRemedial = (calificacion == null ? null : calificacion.ExamenRemedial),
                                    ExamenGracia = (calificacion == null ? null : calificacion.ExamenGracia),
                                    PromedioFinal = (calificacion == null ? null : calificacion.PromedioFinal),
                                    IdEquivalenciaPromedioP1 = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioP1),
                                    IdEquivalenciaPromedioP2 = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioP2),
                                    IdEquivalenciaPromedioP3 = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioP2),
                                    IdEquivalenciaPromedioEQ1 = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioEQ1),
                                    IdEquivalenciaPromedioQ1 = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioQ1),
                                    IdEquivalenciaPromedioP4 = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioP4),
                                    IdEquivalenciaPromedioP5 = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioP5),
                                    IdEquivalenciaPromedioP6 = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioP6),
                                    IdEquivalenciaPromedioEQ2 = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioEQ2),
                                    IdEquivalenciaPromedioQ2 = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioQ2),
                                    IdEquivalenciaPromedioPF = (calificacion == null ? null : calificacion.IdEquivalenciaPromedioPF),
                                    CampoMejoramiento = (calificacion == null ? null : calificacion.CampoMejoramiento),
                                };

                                lst_calificacion.Add(info_calificacion);
                            }

                            var info_conducta = new aca_MatriculaConducta_Info
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdMatricula = item.IdMatricula,
                                SecuenciaPromedioP1 = (conducta == null ? null : conducta.SecuenciaPromedioP1),
                                PromedioP1 = (conducta == null ? null : conducta.PromedioP1),
                                SecuenciaPromedioFinalP1 = (conducta == null ? null : conducta.SecuenciaPromedioFinalP1),
                                PromedioFinalP1 = (conducta == null ? null : conducta.PromedioFinalP1),
                                SecuenciaPromedioP2 = (conducta == null ? null : conducta.SecuenciaPromedioP2),
                                PromedioP2 = (conducta == null ? null : conducta.PromedioP2),
                                SecuenciaPromedioFinalP2 = (conducta == null ? null : conducta.SecuenciaPromedioFinalP2),
                                PromedioFinalP2 = (conducta == null ? null : conducta.PromedioFinalP2),
                                SecuenciaPromedioP3 = (conducta == null ? null : conducta.SecuenciaPromedioP3),
                                PromedioP3 = (conducta == null ? null : conducta.PromedioP3),
                                SecuenciaPromedioFinalP3 = (conducta == null ? null : conducta.SecuenciaPromedioFinalP3),
                                PromedioFinalP3 = (conducta == null ? null : conducta.PromedioFinalP3),
                                SecuenciaPromedioP4 = (conducta == null ? null : conducta.SecuenciaPromedioP4),
                                PromedioP4 = (conducta == null ? null : conducta.PromedioP4),
                                SecuenciaPromedioFinalP4 = (conducta == null ? null : conducta.SecuenciaPromedioFinalP4),
                                PromedioFinalP4 = (conducta == null ? null : conducta.PromedioFinalP4),
                                SecuenciaPromedioP5 = (conducta == null ? null : conducta.SecuenciaPromedioP5),
                                PromedioP5 = (conducta == null ? null : conducta.PromedioP5),
                                SecuenciaPromedioFinalP5 = (conducta == null ? null : conducta.SecuenciaPromedioFinalP5),
                                PromedioFinalP5 = (conducta == null ? null : conducta.PromedioFinalP5),
                                SecuenciaPromedioP6 = (conducta == null ? null : conducta.SecuenciaPromedioP6),
                                PromedioP6 = (conducta == null ? null : conducta.PromedioP6),
                                SecuenciaPromedioFinalP6 = (conducta == null ? null : conducta.SecuenciaPromedioFinalP6),
                                PromedioFinalP6 = (conducta == null ? null : conducta.PromedioFinalP6),
                                SecuenciaPromedioQ1 = (conducta == null ? null : conducta.SecuenciaPromedioQ1),
                                PromedioQ1 = (conducta == null ? null : conducta.PromedioQ1),
                                SecuenciaPromedioFinalQ1 = (conducta == null ? null : conducta.SecuenciaPromedioFinalQ1),
                                PromedioFinalQ1 = (conducta == null ? null : conducta.PromedioFinalQ1),
                                SecuenciaPromedioQ2 = (conducta == null ? null : conducta.SecuenciaPromedioQ2),
                                PromedioQ2 = (conducta == null ? null : conducta.PromedioQ2),
                                SecuenciaPromedioFinalQ2 = (conducta == null ? null : conducta.SecuenciaPromedioFinalQ2),
                                PromedioFinalQ2 = (conducta == null ? null : conducta.PromedioFinalQ2),
                                SecuenciaPromedioGeneral = (conducta == null ? null : conducta.SecuenciaPromedioGeneral),
                                PromedioGeneral = (conducta == null ? null : conducta.PromedioGeneral),
                                SecuenciaPromedioFinal = (conducta == null ? null : conducta.SecuenciaPromedioFinal),
                                PromedioFinal = (conducta == null ? null : conducta.PromedioFinal)
                            };

                            lst_conducta.Add(info_conducta);
                        }
                    }
                    #endregion

                    #region Asistencia
                    var asistencia = lst_asistencia_existente.Where(q => q.IdMatricula == item.IdMatricula).FirstOrDefault();
                    var info_asistencia = new aca_MatriculaAsistencia_Info
                    {
                        IdEmpresa = item.IdEmpresa,
                        IdMatricula = item.IdMatricula,
                        FInjustificadaP1 = (asistencia == null ? null : asistencia.FInjustificadaP1),
                        FJustificadaP1 = (asistencia == null ? null : asistencia.FJustificadaP1),
                        AtrasosP1 = (asistencia == null ? null : asistencia.AtrasosP1),
                        FInjustificadaP2 = (asistencia == null ? null : asistencia.FInjustificadaP2),
                        FJustificadaP2 = (asistencia == null ? null : asistencia.FJustificadaP2),
                        AtrasosP2 = (asistencia == null ? null : asistencia.AtrasosP2),
                        FInjustificadaP3 = (asistencia == null ? null : asistencia.FInjustificadaP3),
                        FJustificadaP3 = (asistencia == null ? null : asistencia.FJustificadaP3),
                        AtrasosP3 = (asistencia == null ? null : asistencia.AtrasosP3),
                        FInjustificadaP4 = (asistencia == null ? null : asistencia.FInjustificadaP4),
                        FJustificadaP4 = (asistencia == null ? null : asistencia.FJustificadaP4),
                        AtrasosP4 = (asistencia == null ? null : asistencia.AtrasosP4),
                        FInjustificadaP5 = (asistencia == null ? null : asistencia.FInjustificadaP5),
                        FJustificadaP5 = (asistencia == null ? null : asistencia.FJustificadaP5),
                        AtrasosP5 = (asistencia == null ? null : asistencia.AtrasosP5),
                        FInjustificadaP6 = (asistencia == null ? null : asistencia.FInjustificadaP6),
                        FJustificadaP6 = (asistencia == null ? null : asistencia.FJustificadaP6),
                        AtrasosP6 = (asistencia == null ? null : asistencia.AtrasosP6),
                        IdUsuarioCreacion = (asistencia == null ? SessionFixed.IdUsuario : asistencia.IdUsuarioCreacion),
                        FechaCreacion = (asistencia == null ? DateTime.Now : asistencia.FechaCreacion),
                        IdUsuarioModificacion = (asistencia == null ? SessionFixed.IdUsuario : asistencia.IdUsuarioModificacion),
                        FechaModificacion = (asistencia == null ? DateTime.Now : asistencia.FechaModificacion),
                    };
                    lst_asistencia.Add(info_asistencia);
                    #endregion
                }

                if (bus_calificacion_parcial.GenerarCalificacion(lst_calificacion_parcial))
                {
                    if (bus_calificacion_cualitativa.generarCalificacion(lst_calificacion_cualitativa))
                    {
                        if (bus_calificacion.GenerarCalificacion(lst_calificacion))
                        {
                            if (bus_calificacion_cualitativa_promedio.GenerarCalificacion(lst_calificacion_cualitativa_promedio))
                            {
                                if (bus_conducta.GenerarCalificacion(lst_conducta))
                                {
                                    if (bus_asistencia.GenerarCalificacion(lst_asistencia))
                                    {
                                        if (bus_grado.GenerarCalificacion(lst_MatriculaGrado))
                                        {
                                            MensajeSuccess = "Registros generados exitosamente";
                                            ViewBag.MensajeSuccess = MensajeSuccess;
                                            var ListaGenerada = new List<aca_Matricula_Info>();
                                            //var ListaGenerada = bus_calificacion.GetList(model.IdEmpresa, model.IdAnio, model.IdSede, model.IdNivel, model.IdJornada, model.IdCurso, model.IdParalelo, model.IdAlumno);

                                            var lst_matricula = (from q in lst_asistencia
                                                                 group q by new
                                                                 {
                                                                     q.IdEmpresa,
                                                                     q.IdMatricula
                                                                 } into mat
                                                                 select new aca_Matricula_Info
                                                                 {
                                                                     IdEmpresa = mat.Key.IdEmpresa,
                                                                     IdMatricula = mat.Key.IdMatricula
                                                                 }).ToList();

                                            foreach (var item in lst_matricula)
                                            {
                                                var matricula = lista.Where(q => q.IdEmpresa == item.IdEmpresa && q.IdMatricula == item.IdMatricula).FirstOrDefault();
                                                if (matricula != null)
                                                {
                                                    ListaGenerada.Add(matricula);
                                                }
                                            }
                                            Lista_MatriculaCalificaciones.set_list(ListaGenerada, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                                        }                                        
                                    }
                                }
                                else
                                {
                                    mensaje = "Ha ocurrido un error al generar las calificaciones de conducta";
                                    ViewBag.mensaje = mensaje;
                                }
                            }
                        }
                        else
                        {
                            mensaje = "Ha ocurrido un error al generar las calificaciones";
                            ViewBag.mensaje = mensaje;
                        }
                    }
                    else
                    {
                        mensaje = "Ha ocurrido un error al generar las calificaciones cualitativas";
                        ViewBag.mensaje = mensaje;
                    }
                }
                else
                {
                    mensaje = "Ha ocurrido un error al generar las calificaciones parciales";
                    ViewBag.mensaje = mensaje;
                }
            }

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_MatriculaCalificacion()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            List<aca_Matricula_Info> model = Lista_MatriculaCalificaciones.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_MatriculaCalificacion", model);
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

        public ActionResult ComboBoxPartial_Alumno()
        {
            int IdAnio = !string.IsNullOrEmpty(Request.Params["IdAnio"]) ? int.Parse(Request.Params["IdAnio"]) : -1;
            int IdSede = !string.IsNullOrEmpty(Request.Params["IdSede"]) ? int.Parse(Request.Params["IdSede"]) : -1;
            int IdNivel = !string.IsNullOrEmpty(Request.Params["IdNivel"]) ? int.Parse(Request.Params["IdNivel"]) : -1;
            int IdJornada = !string.IsNullOrEmpty(Request.Params["IdJornada"]) ? int.Parse(Request.Params["IdJornada"]) : -1;
            int IdCurso = !string.IsNullOrEmpty(Request.Params["IdCurso"]) ? int.Parse(Request.Params["IdCurso"]) : -1;
            var IdParalelo = !string.IsNullOrEmpty(Request.Params["IdParalelo"]) ? int.Parse(Request.Params["IdParalelo"]) : -1;
            return PartialView("_ComboBoxPartial_Alumno", new aca_Matricula_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada, IdCurso = IdCurso, IdParalelo = IdParalelo });
        }
        #endregion

        #region Json
        public JsonResult RevisarConfiguracion(int IdEmpresa = 0, int IdSede = 0, int IdAnio = 0, int IdNivel = 0, int IdJornada = 0, int IdCurso = 0, int IdParalelo=0, decimal IdAlumno=0, decimal IdTransaccionSession = 0)
        {
            var texto = "";
            List<aca_Matricula_Info> lista = bus_matricula.GetList_Calificaciones(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo, IdAlumno);
            List<aca_Matricula_Info> ListaGeneracion = new List<aca_Matricula_Info>();
            ListaGeneracion = (from q in lista
                               group q by new
                               {
                                   q.IdEmpresa,
                                   q.IdAnio,
                                   q.IdSede,
                                   q.IdJornada,
                                   q.IdNivel,
                                   q.IdCurso,
                                   q.IdParalelo,
                                   q.Descripcion,
                                   q.NomSede,
                                   q.NomJornada,
                                   q.NomNivel,
                                   q.NomCurso,
                                   q.NomParalelo,
                                   q.OrdenJornada,
                                   q.OrdenNivel,
                                   q.OrdenCurso,
                                   q.OrdenParalelo
                               } into a
                               select new aca_Matricula_Info
                               {
                                   IdEmpresa = a.Key.IdEmpresa,
                                   IdAnio = a.Key.IdAnio,
                                   IdSede = a.Key.IdSede,
                                   IdJornada = a.Key.IdJornada,
                                   IdNivel = a.Key.IdNivel,
                                   IdCurso = a.Key.IdCurso,
                                   IdParalelo = a.Key.IdParalelo,
                                   Descripcion = a.Key.Descripcion,
                                   NomSede = a.Key.NomSede,
                                   NomJornada = a.Key.NomJornada,
                                   NomNivel = a.Key.NomNivel,
                                   NomCurso = a.Key.NomCurso,
                                   NomParalelo = a.Key.NomParalelo,
                                   OrdenJornada = a.Key.OrdenJornada,
                                   OrdenNivel = a.Key.OrdenNivel,
                                   OrdenCurso = a.Key.OrdenCurso,
                                   OrdenParalelo = a.Key.OrdenParalelo
                               }).ToList();


            if (ListaGeneracion.Count() > 0)
            {
                var cant = 0;
                foreach (var item in ListaGeneracion)
                {
                    cant = 1;
                    texto += "<div class='row'>";
                    texto += "<div class='col-md-12'>";
                    texto += "<label style='font-size:small'>" + "<h4><b><u>" + item.NomJornada + " - " + item.NomCurso + " - " + item.NomParalelo + "</u></b></h34>" +"</label>";
                    texto += "<br>";

                    var lst_materias_x_curso = bus_materias_x_paralelo.GetList(item.IdEmpresa, item.IdSede, item.IdAnio, item.IdNivel, item.IdJornada, item.IdCurso, item.IdParalelo).OrderBy(q=>q.NomMateria);
                    texto += "<table width=100% style='font-size:12px; font-weight:400;'>";
                    foreach (var mat in lst_materias_x_curso)
                    {
                        var Tipo = (mat.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUALI) ? "<b>*</b>" : "");
                        texto += "<tr>";
                        texto += "<td width=5%>" + Tipo + "</td>";
                        texto += "<td width=45%>" + mat.NomMateria +"</td>";
                        texto += "<td width=5%>"+"</td>";
                        texto += "<td width=45%>" + mat.pe_nombreCompleto + "</td>";
                        texto += "</tr>";
                    }
                    texto += "</table>";
                    texto += "</div>";
                    texto += "</div>";
                }
            }

            return Json(texto, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class aca_MatriculaCalificacion_List
    {
        aca_Profesor_Bus bus_profesor = new aca_Profesor_Bus();
        string Variable = "aca_MatriculaCalificacion_Info";
        public List<aca_MatriculaCalificacion_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_MatriculaCalificacion_Info> list = new List<aca_MatriculaCalificacion_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_MatriculaCalificacion_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_MatriculaCalificacion_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void UpdateRow(aca_MatriculaCalificacion_Info info_det, decimal IdTransaccionSession)
        {
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);

            aca_MatriculaCalificacion_Info edited_info = get_list(IdTransaccionSession).Where(m => m.IdAlumno == info_det.IdAlumno).FirstOrDefault();
            edited_info.IdProfesor = info_det.IdProfesor;

            var profesor = bus_profesor.GetInfo(IdEmpresa, Convert.ToDecimal(info_det.IdProfesor));
            if (profesor != null)
                info_det.pe_nombreCompleto = profesor.pe_nombreCompleto;
            edited_info.pe_nombreCompleto = info_det.pe_nombreCompleto;
        }
    }

    public class aca_MatriculaGeneracionCalificaciones_List
    {
        string Variable = "aca_MatriculaGeneracionCalificaciones";
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