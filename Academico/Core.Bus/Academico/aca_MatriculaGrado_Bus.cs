using Core.Data.Academico;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Academico
{    
    public class aca_MatriculaGrado_Bus
    {
        aca_MatriculaGrado_Data odata = new aca_MatriculaGrado_Data();
        public List<aca_MatriculaGrado_Info> getList(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo)
        {
            try
            {
                return odata.getList(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_MatriculaGrado_Info getInfo(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, decimal IdAlumno)
        {
            try
            {
                return odata.getInfo(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo, IdAlumno);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_MatriculaGrado_Info getInfo_X_Matricula(int IdEmpresa, decimal IdMatricula)
        {
            try
            {
                return odata.getInfo_X_Matricula(IdEmpresa, IdMatricula);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool modificarDB(aca_MatriculaGrado_Info info)
        {
            try
            {
                return odata.modificarDB(info);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool GenerarCalificacion(List<aca_MatriculaGrado_Info> lst_grado)
        {
            try
            {
                return odata.generarCalificacion(lst_grado);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
