using Core.Data.Academico;
using Core.Data.Base;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Academico
{
    public class aca_AnioLectivoCalificacionHistorico_Bus
    {
        aca_AnioLectivoCalificacionHistorico_Data odata = new aca_AnioLectivoCalificacionHistorico_Data();
        public List<aca_AnioLectivoCalificacionHistorico_Info> GetList(int IdEmpresa, decimal IdAlumno, bool MostrarAnulados)
        {
            try
            {
                return odata.getList(IdEmpresa, IdAlumno, MostrarAnulados);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_AnioLectivoCalificacionHistorico_Info GetInfo(int IdEmpresa, int IdAnio, decimal IdAlumno)
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

        public bool GuardarDB(aca_AnioLectivoCalificacionHistorico_Info info)
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

        public bool ModificarDB(aca_AnioLectivoCalificacionHistorico_Info info)
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
        public bool EliminarDB(aca_AnioLectivoCalificacionHistorico_Info info)
        {
            try
            {
                return odata.EliminarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
