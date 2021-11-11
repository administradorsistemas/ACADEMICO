using Core.Data.Academico;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus
{
    public class aca_AnioLectivoCalificacionParticipacionHistorico_Bus
    {
        aca_AnioLectivoCalificacionParticipacionHistorico_Data odata = new aca_AnioLectivoCalificacionParticipacionHistorico_Data();
        public aca_AnioLectivoCalificacionParticipacionHistorico_Info GetInfo(int IdEmpresa, int IdAnio, decimal IdAlumno)
        {
            try
            {
                return odata.getInfo(IdEmpresa, IdAnio, IdAlumno);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarDB(aca_AnioLectivoCalificacionParticipacionHistorico_Info info)
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

        public bool ModificarDB(aca_AnioLectivoCalificacionParticipacionHistorico_Info info)
        {
            try
            {
                return odata.modificarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
