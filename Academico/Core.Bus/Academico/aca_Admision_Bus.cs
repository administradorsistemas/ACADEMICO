using Core.Data.Academico;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Academico
{
    public class aca_Admision_Bus
    {
        aca_Admision_Data odata = new aca_Admision_Data();

        public List<aca_Admision_Info> GetList(int IdEmpresa, int IdSede, int IdAnio)
        {
            try
            {
                return odata.getList(IdEmpresa, IdSede, IdAnio);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_Admision_Info> GetList_Admisiones(int IdEmpresa, int IdSede)
        {
            try
            {
                return odata.getList_Admisiones(IdEmpresa, IdSede);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<aca_Admision_Info> GetList_Academico(int IdEmpresa, int IdSede, int IdAnio)
        {
            try
            {
                return odata.getList_Academico(IdEmpresa, IdSede, IdAnio);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_Admision_Info> GetList_PreMatriculaAcademico(int IdEmpresa, int IdSede, int IdAnio)
        {
            try
            {
                return odata.getList_PreMatriculaAcademico(IdEmpresa, IdSede, IdAnio);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(aca_Admision_Info info)
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

        public aca_Admision_Info GetInfo_CedulaAspirante(int IdEmpresa, string CedulaRuc_Aspirante)
        {
            try
            {
                return odata.getInfo_CedulaAspirante(IdEmpresa, CedulaRuc_Aspirante);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public aca_Admision_Info GetInfo(int IdEmpresa, decimal IdAdmision)
        {
            try
            {
                return odata.getInfo(IdEmpresa, IdAdmision);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public aca_Admision_Info ConsultaAdmision(int IdEmpresa, int IdAnio, string CedulaRuc_Aspirante)
        {
            try
            {
                return odata.consultaAdmision(IdEmpresa, IdAnio, CedulaRuc_Aspirante);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public decimal GetId(int IdEmpresa)
        {
            try
            {
                return odata.getId(IdEmpresa);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarEstadoEnProceso(aca_Admision_Info info)
        {
            try
            {
                return odata.modificarEstadoEnProceso(info);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool ModificarEstado(aca_Admision_Info info)
        {
            try
            {
                return odata.modificarEstado(info);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //DASHBOARD
        public List<aca_Admision_Info> Dashboard_Admisiones(int IdEmpresa)
        {
            try
            {
                return odata.Dashboard_Admisiones(IdEmpresa);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
