using Core.Data.Academico;
using Core.Info.Academico;
using Core.Info.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Academico
{
    public class aca_MatriculaCalificacion_Bus
    {
        aca_MatriculaCalificacion_Data odata = new aca_MatriculaCalificacion_Data();
        aca_AnioLectivoParcial_Data odata_parcial = new aca_AnioLectivoParcial_Data();
        aca_Alumno_Bus odata_alumno = new aca_Alumno_Bus();
        aca_MatriculaCalificacion_Data odata_calificacion = new aca_MatriculaCalificacion_Data();
        aca_AnioLectivo_Data odata_anio = new aca_AnioLectivo_Data();
        aca_Matricula_Data odata_matricula = new aca_Matricula_Data();
        aca_AnioLectivoEquivalenciaPromedio_Data odata_promedio_equivalencia = new aca_AnioLectivoEquivalenciaPromedio_Data();

        public List<aca_MatriculaCalificacion_Info> GetList(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria)
        {
            try
            {
                return odata.getList(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo, IdMateria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> GetList_PaseAnio(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, decimal IdAlumno)
        {
            try
            {
                return odata.getList_PaseAnio(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo, IdAlumno);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public aca_MatriculaCalificacion_Info GetInfo_Modificar(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria, decimal IdAlumno)
        {
            try
            {
                return odata.getInfo_modificar(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo, IdMateria, IdAlumno);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<aca_MatriculaCalificacion_Info> GetList_x_Profesor(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria, decimal IdProfesor)
        {
            try
            {
                return odata.getList_x_Profesor(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo, IdMateria, IdProfesor);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<aca_MatriculaCalificacion_Info> GetList_SuperAdmin(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria)
        {
            try
            {
                return odata.getList_SuperAdmin(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo, IdMateria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<aca_MatriculaCalificacion_Info> GetList(int IdEmpresa, decimal IdMatricula)
        {
            try
            {
                return odata.getList(IdEmpresa, IdMatricula);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarDB(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria,  List<aca_MatriculaCalificacion_Info> lista)
        {
            try
            {
                return odata.guardarDB(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo, IdMateria, lista);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GenerarCalificacion(List<aca_MatriculaCalificacion_Info> lst_calificacion)
        {
            try
            {
                return odata.generarCalificacion(lst_calificacion);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> GetList_Combos(int IdEmpresa, int IdAnio, int IdSede)
        {
            try
            {
                return odata.getList_Combos(IdEmpresa, IdAnio, IdSede);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> GetList_CombosCalificaciones(int IdEmpresa, int IdSede)
        {
            try
            {
                return odata.getList_CombosCalificaciones(IdEmpresa, IdSede);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<aca_MatriculaCalificacion_Info> GetList_Combos(int IdEmpresa, int IdAnio, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                return odata.getList_Combos(IdEmpresa, IdAnio, IdSede, IdProfesor, EsSuperAdmin);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> GetList_CombosCalificaciones(int IdEmpresa, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                return odata.getList_CombosCalificaciones(IdEmpresa, IdSede, IdProfesor, EsSuperAdmin);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> GetList_CombosCalificacionesGrado(int IdEmpresa, int IdSede, int IdCurso)
        {
            try
            {
                return odata.getList_CombosCalificacionesGrado(IdEmpresa, IdSede, IdCurso);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> GetList_Combos_Tutor(int IdEmpresa, int IdAnio, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                return odata.getList_Combos_Tutor(IdEmpresa, IdAnio, IdSede, IdProfesor, EsSuperAdmin);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> GetList_Combos_TutorCalificacion(int IdEmpresa, int IdAnio, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                return odata.getList_Combos_TutorCalificacion(IdEmpresa, IdAnio, IdSede, IdProfesor, EsSuperAdmin);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<aca_MatriculaCalificacion_Info> GetList_Combos_Inspector(int IdEmpresa, int IdAnio, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                return odata.getList_Combos_Inspector(IdEmpresa, IdAnio, IdSede, IdProfesor, EsSuperAdmin);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public List<aca_MatriculaCalificacion_Info> getList_Combos_InspectorCalificacion(int IdEmpresa, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                return odata.getList_Combos_InspectorCalificacion(IdEmpresa, IdSede, IdProfesor, EsSuperAdmin);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public aca_MatriculaCalificacion_Info GetInfo(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria, decimal IdAlumno)
        {
            try
            {
                return odata.getInfo(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo, IdMateria, IdAlumno);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_MatriculaCalificacion_Info GetInfo_X_Matricula(int IdEmpresa, decimal IdMatricula, int IdMateria)
        {
            try
            {
                return odata.getInfo_X_Matricula(IdEmpresa, IdMatricula, IdMateria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ModicarDB(aca_MatriculaCalificacion_Info info)
        {
            try
            {
                if (odata.modificarDB(info))
                {
                    var info_matricula = odata_matricula.getInfo(info.IdEmpresa, info.IdMatricula);
                    var info_parcial = odata_parcial.getInfo(info.IdEmpresa, info_matricula.IdSede, info_matricula.IdAnio, info.IdCatalogoParcial);

                    if (info_parcial!=null)
                    {
                        if (info_parcial.ValidaEstadoAlumno == true)
                        {
                            var info_alumno = odata_alumno.GetInfo(info_matricula.IdEmpresa, info_matricula.IdAlumno);
                            var info_calificacion_modificar = GetInfo_Modificar(info_matricula.IdEmpresa, info_matricula.IdAnio, info_matricula.IdSede, info_matricula.IdNivel, info_matricula.IdJornada, info_matricula.IdCurso, info_matricula.IdParalelo, info.IdMateria, info_matricula.IdAlumno);
                            var info_anio = odata_anio.getInfo(info.IdEmpresa, info_matricula.IdAnio);

                            decimal? PromedioFinal = (decimal?)null;
                            decimal? PromedioFinalTemp = (decimal?)null;
                            decimal PromedioMinimoPromocion = Math.Round(Convert.ToDecimal(info_anio.PromedioMinimoPromocion), 2, MidpointRounding.AwayFromZero);

                            if(info_calificacion_modificar!=null)
                            {
                                string CampoMejoramiento = null;
                                var PromedioFinalQ1 = info_calificacion_modificar.PromedioFinalQ1==null ? (decimal?)null : Convert.ToDecimal(info_calificacion_modificar.PromedioFinalQ1);
                                var PromedioFinalQ2 = info_calificacion_modificar.PromedioFinalQ2 == null ? (decimal?)null : Convert.ToDecimal(info_calificacion_modificar.PromedioFinalQ2);
                                var ExamenMejoramiento = info_calificacion_modificar.ExamenMejoramiento == null ? (decimal?)null : Convert.ToDecimal(info_calificacion_modificar.ExamenMejoramiento);
                                var ExamenSupletorio = info_calificacion_modificar.ExamenSupletorio == null ? (decimal?)null : Convert.ToDecimal(info_calificacion_modificar.ExamenSupletorio);
                                var ExamenRemedial = info_calificacion_modificar.ExamenRemedial == null ? (decimal?)null : Convert.ToDecimal(info_calificacion_modificar.ExamenRemedial);
                                var ExamenGracia = info_calificacion_modificar.ExamenGracia == null ? (decimal?)null : Convert.ToDecimal(info_calificacion_modificar.ExamenGracia);

                                if (PromedioFinalQ1!=null && PromedioFinalQ2!=null)
                                {
                                    PromedioFinalTemp = Math.Round(Convert.ToDecimal((PromedioFinalQ1 + PromedioFinalQ2) / 2), 2, MidpointRounding.AwayFromZero);
                                }
                                
                                //decimal PromedioFinalQ1 = Convert.ToDecimal(info_calificacion_modificar.PromedioFinalQ1);
                                //decimal PromedioFinalQ2 = Convert.ToDecimal(info_calificacion_modificar.PromedioFinalQ2);
                                //decimal ExamenMejoramiento = Convert.ToDecimal(info_calificacion_modificar.ExamenMejoramiento);
                                //decimal ExamenSupletorio = Convert.ToDecimal(info_calificacion_modificar.ExamenSupletorio);
                                //decimal ExamenRemedial = Convert.ToDecimal(info_calificacion_modificar.ExamenRemedial);
                                //decimal ExamenGracia = Convert.ToDecimal(info_calificacion_modificar.ExamenGracia);
                                //PromedioFinalTemp = Math.Round(Convert.ToDecimal((PromedioFinalQ1 + PromedioFinalQ2) / 2), 2, MidpointRounding.AwayFromZero);

                                if (PromedioFinalTemp < PromedioMinimoPromocion)
                                {
                                    if (ExamenSupletorio >= PromedioMinimoPromocion)
                                    {
                                        PromedioFinal = PromedioMinimoPromocion;
                                    }
                                    else if (ExamenRemedial >= PromedioMinimoPromocion)
                                    {
                                        PromedioFinal = PromedioMinimoPromocion;
                                    }
                                    else if (ExamenGracia >= PromedioMinimoPromocion)
                                    {
                                        PromedioFinal = PromedioMinimoPromocion;
                                    }
                                    else
                                    {
                                        PromedioFinal = PromedioFinalTemp;
                                    }
                                }
                                else
                                {
                                    if (ExamenMejoramiento > 0)
                                    {
                                        if (PromedioFinalQ1 < PromedioFinalQ2)
                                        {
                                            CampoMejoramiento = "Q1";
                                            PromedioFinal = Math.Round(Convert.ToDecimal((ExamenMejoramiento + PromedioFinalQ2) / 2), 2, MidpointRounding.AwayFromZero);
                                        }
                                        else if (PromedioFinalQ2 < PromedioFinalQ1)
                                        {
                                            CampoMejoramiento = "Q2";
                                            PromedioFinal = Math.Round(Convert.ToDecimal((PromedioFinalQ1 + ExamenMejoramiento) / 2), 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            PromedioFinal = PromedioFinalTemp;
                                        }
                                    }
                                    else
                                    {
                                        PromedioFinal = PromedioFinalTemp;
                                    }
                                    
                                }
                                info.CampoMejoramiento = CampoMejoramiento;
                                info.PromedioQuimestres = PromedioFinalTemp;
                                info.PromedioFinal = PromedioFinal;
                                var info_equivalencia = odata_promedio_equivalencia.getInfo_x_Promedio(info_matricula.IdEmpresa, info_matricula.IdAnio, PromedioFinal);
                                info.IdEquivalenciaPromedioPF = (info_equivalencia==null ? (int?)null : info_equivalencia.IdEquivalenciaPromedio);

                                odata_calificacion.modicarPaseAnioDB(info);

                                if (PromedioFinal!=null)
                                {
                                    if (PromedioFinal < PromedioMinimoPromocion)
                                    {
                                        info_alumno.IdCatalogoESTALU = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoAlumno.SUPLENCIA);
                                        info_alumno.IdCatalogoESTMAT = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoMatricula.REPROBADO);
                                        odata_alumno.PaseAnioDB(info_alumno);
                                    }
                                    else
                                    {
                                        info_alumno.IdCatalogoESTALU = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoAlumno.PROMOVIDO);
                                        info_alumno.IdCatalogoESTMAT = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoMatricula.APROBADO);
                                        odata_alumno.PaseAnioDB(info_alumno);
                                    }
                                }
                                
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ModicarPaseAnioDB(aca_MatriculaCalificacion_Info info)
        {
            try
            {
                return odata.modicarPaseAnioDB(info);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<aca_MatriculaCalificacion_Info> GetList_CombosParticipacion(int IdEmpresa, int IdAnio, int IdSede)
        {
            try
            {
                return odata.getList_CombosParticipacion(IdEmpresa, IdAnio, IdSede);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
