﻿using Core.Data.Base;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.General
{
   public class tbl_TransaccionesAutorizadas_Data
    {

        public bool guardarDB(tbl_TransaccionesAutorizadas_info info)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tbl_TransaccionesAutorizadas Entity = new tbl_TransaccionesAutorizadas
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdTransaccion = info.IdTransaccion = get_id(info.IdEmpresa),
                        IdUsuarioAut = info.IdUsuarioAut,
                        IdUsuarioLog = info.IdUsuarioLog,
                        FechaTransaccion = DateTime.Now,
                        Observacion=info.Observacion

                    };
                    Context.tbl_TransaccionesAutorizadas.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public decimal get_id(int IdEmpresa)
        {
            try
            {
                decimal ID = 1;

                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    var lst = from q in Context.tbl_TransaccionesAutorizadas
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdTransaccion) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
