﻿using Core.Data.Academico;
using Core.Info.Academico;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Academico
{
    public class aca_Sede_Bus
    {
        aca_Sede_Data odata = new aca_Sede_Data();

        public List<aca_Sede_Info> GetList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                return odata.GetList(IdEmpresa, MostrarAnulados);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public aca_Sede_Info GetInfo(int IdEmpresa, int IdSede)
        {
            try
            {
                return odata.GetInfo(IdEmpresa, IdSede);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_Sede_Info> GetListSinEmpresa(bool MostrarAnulados)
        {
            try
            {
                return odata.GetListSinEmpresa(MostrarAnulados);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_Sede_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args, int IdEmpresa)
        {
            return odata.get_list_bajo_demanda(args, IdEmpresa);
        }

        public aca_Sede_Info get_info_bajo_demanda(int IdEmpresa, ListEditItemRequestedByValueEventArgs args)
        {
            return odata.get_info_bajo_demanda(IdEmpresa, args);
        }

        public bool guardarDB(aca_Sede_Info info)
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

        public bool modificarDB(aca_Sede_Info info)
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

        public bool anularDB(aca_Sede_Info info)
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
