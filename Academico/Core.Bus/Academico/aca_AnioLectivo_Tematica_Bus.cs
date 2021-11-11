using Core.Data.Academico;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Academico
{
    public class aca_AnioLectivo_Tematica_Bus
    {
        aca_AnioLectivo_Tematica_Data odata = new aca_AnioLectivo_Tematica_Data();
        public List<aca_AnioLectivo_Tematica_Info> get_list_asignacion(int IdEmpresa, int IdAnio)
        {
            try
            {
                return odata.get_list_asignacion(IdEmpresa, IdAnio);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public aca_AnioLectivo_Tematica_Info getInfo(int IdEmpresa, int IdAnio, int IdTematica)
        {
            try
            {
                return odata.getInfo(IdEmpresa, IdAnio, IdTematica);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(int IdEmpresa, int IdAnio, List<aca_AnioLectivo_Tematica_Info> lista)
        {
            try
            {
                return odata.guardarDB(IdEmpresa, IdAnio, lista);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_AnioLectivo_Tematica_Info> GetList(int IdEmpresa, int IdAnio)
        {
            try
            {
                return odata.getList(IdEmpresa, IdAnio);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<aca_AnioLectivo_Tematica_Info> GetListCampoAccion(int IdEmpresa, int IdAnio)
        {
            try
            {
                return odata.getList_CampoAccion(IdEmpresa, IdAnio);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_AnioLectivo_Tematica_Info> GetLisTematica(int IdEmpresa, int IdAnio, int IdTematica)
        {
            try
            {
                return odata.getListTematica(IdEmpresa, IdAnio, IdTematica);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool modificarDB(List<aca_AnioLectivo_Tematica_Info> lista)
        {
            try
            {
                return odata.modificarDB(lista);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
