using Core.Data.Academico;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Academico
{
    public class aca_Materia_Bus
    {
        aca_Materia_Data odata = new aca_Materia_Data();
        aca_AnioLectivo_Data odata_anio = new aca_AnioLectivo_Data();
        aca_AnioLectivo_Curso_Materia_Data odata_curso_materia = new aca_AnioLectivo_Curso_Materia_Data();
        public List<aca_Materia_Info> GetList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                return odata.getList(IdEmpresa, MostrarAnulados);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<aca_Materia_Info> GetList(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso)
        {
            try
            {
                return odata.getList(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_Materia_Info GetInfo(int IdEmpresa, int IdMateria)
        {
            try
            {
                return odata.getInfo(IdEmpresa, IdMateria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetOrden(int IdEmpresa)
        {
            try
            {
                return odata.getOrden(IdEmpresa);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarDB(aca_Materia_Info info)
        {
            try
            {
                return odata.guardarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarDB(aca_Materia_Info info)
        {
            try
            {
                var lst_anios = odata_anio.getList_update(info.IdEmpresa);
                if (odata.modificarDB(info))
                {
                    if (lst_anios.Count > 0)
                    {
                        foreach (var item in lst_anios)
                        {
                            //var info_anio_curso = odata_anio.getInfo(info.IdEmpresa, item.IdAnio);
                            if (item.EnCurso == true)
                            {
                                var lst_curso_materia = odata_curso_materia.getList_Update(info.IdEmpresa, item.IdAnio, info.IdMateria);
                                if (lst_curso_materia.Count > 0)
                                {
                                    foreach (var info_curso_materia in lst_curso_materia)
                                    {
                                        info_curso_materia.NomMateria = info.NomMateria;
                                        info_curso_materia.EsObligatorio = info.EsObligatorio;
                                        info_curso_materia.OrdenMateria = info.OrdenMateria;
                                        info_curso_materia.OrdenMateriaArea = info.OrdenMateriaArea;
                                        info_curso_materia.OrdenMateriaGrupo = info.OrdenMateriaGrupo;
                                        info_curso_materia.NomMateriaArea = info.NomMateriaArea;
                                        info_curso_materia.NomMateriaGrupo = info.NomMateriaGrupo;
                                        info_curso_materia.IdCatalogoTipoCalificacion = info.IdCatalogoTipoCalificacion;
                                        info_curso_materia.PromediarGrupo = info.PromediarGrupo;
                                    }

                                    odata_curso_materia.modificarDB(lst_curso_materia);
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

        public bool AnularDB(aca_Materia_Info info)
        {
            try
            {
                return odata.anularDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
