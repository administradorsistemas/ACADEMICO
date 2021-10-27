using Core.Data.Reportes.Academico;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Reportes.Academico
{
    public class ACA_005_Bus
    {
        ACA_005_Data odata = new ACA_005_Data();
        public ACA_005_Info GetInfo(int IdEmpresa, int IdAnio, decimal IdAlumno)
        { 
            try
            {
                return odata.get_info(IdEmpresa, IdAnio, IdAlumno);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
