﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web;
using Core.Info.General;
using Core.Data.Base;

namespace Core.Data.General
{
   public class tb_ciudad_Data
    {
        public List< tb_ciudad_Info> get_list(string IdProvincia, bool mostrar_anulados)
        {
            try
            {
                List< tb_ciudad_Info> Lista;
               
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    if (mostrar_anulados)
                        Lista = (from q in Context.tb_ciudad
                                 join p in Context.tb_provincia
                                 on q.IdProvincia equals p.IdProvincia
                                 where q.IdProvincia.Contains(IdProvincia)
                                 select new tb_ciudad_Info
                                 {
                                     IdProvincia = q.IdProvincia,
                                     IdCiudad = q.IdCiudad,
                                     Cod_Ciudad = q.Cod_Ciudad,
                                     Descripcion_Ciudad = q.Descripcion_Ciudad,
                                     Estado = q.Estado,

                                     EstadoBool = q.Estado == "A" ? true : false

                                 }).ToList();
                    else
                        Lista = (from q in Context.tb_ciudad
                                 join p in Context.tb_provincia
                                 on q.IdProvincia equals p.IdProvincia
                                 where q.IdProvincia.Contains(IdProvincia)
                                 && q.Estado == "A"
                                 select new tb_ciudad_Info
                                 {
                                     IdProvincia = q.IdProvincia,
                                     IdCiudad = q.IdCiudad,
                                     Cod_Ciudad = q.Cod_Ciudad,
                                     Descripcion_Ciudad = q.Descripcion_Ciudad,
                                     Estado = q.Estado,

                                     EstadoBool = q.Estado == "A" ? true : false
                                 }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<tb_ciudad_Info> get_listCombos(string IdProvincia, bool mostrar_anulados)
        {
            try
            {
                List<tb_ciudad_Info> Lista;

                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    if (mostrar_anulados)
                        Lista = (from q in Context.tb_ciudad
                                 join p in Context.tb_provincia
                                 on q.IdProvincia equals p.IdProvincia
                                 where q.IdProvincia == IdProvincia
                                 select new tb_ciudad_Info
                                 {
                                     IdProvincia = q.IdProvincia,
                                     IdCiudad = q.IdCiudad,
                                     Cod_Ciudad = q.Cod_Ciudad,
                                     Descripcion_Ciudad = q.Descripcion_Ciudad,
                                     Estado = q.Estado,

                                     EstadoBool = q.Estado == "A" ? true : false

                                 }).ToList();
                    else
                        Lista = (from q in Context.tb_ciudad
                                 join p in Context.tb_provincia
                                 on q.IdProvincia equals p.IdProvincia
                                 where q.IdProvincia == IdProvincia
                                 && q.Estado == "A"
                                 select new tb_ciudad_Info
                                 {
                                     IdProvincia = q.IdProvincia,
                                     IdCiudad = q.IdCiudad,
                                     Cod_Ciudad = q.Cod_Ciudad,
                                     Descripcion_Ciudad = q.Descripcion_Ciudad,
                                     Estado = q.Estado,

                                     EstadoBool = q.Estado == "A" ? true : false
                                 }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string get_id()
        {
            try
            {
                int ID = 1;

                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    var lst = from q in Context.vwtb_ciudad_id
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdCiudad) + 1;
                }

                return ID.ToString("0000");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public  tb_ciudad_Info get_info(string IdCiudad)
        {
            try
            {
                 tb_ciudad_Info info = new  tb_ciudad_Info();

                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                     tb_ciudad Entity = Context. tb_ciudad.FirstOrDefault(q => q.IdCiudad==IdCiudad);
                    if (Entity == null) return null;
                    info = new  tb_ciudad_Info
                    {
                        IdProvincia = Entity.IdProvincia,
                        IdCiudad = Entity.IdCiudad,
                        Cod_Ciudad = Entity.Cod_Ciudad,
                        Descripcion_Ciudad=Entity.Descripcion_Ciudad,
                        Estado = Entity.Estado 
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB( tb_ciudad_Info info)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                     tb_ciudad Entity = new  tb_ciudad
                    {
                        IdCiudad = info.IdCiudad=get_id(),
                        IdProvincia=info.IdProvincia,
                        Cod_Ciudad = info.Cod_Ciudad,
                        Descripcion_Ciudad = info.Descripcion_Ciudad,
                        Estado = info.Estado = "A",
                        IdUsuario = info.IdUsuario,
                        Fecha_Transac = info.Fecha_Transac,
                    };
                    Context. tb_ciudad.Add(Entity);
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB( tb_ciudad_Info info)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                     tb_ciudad Entity = Context.tb_ciudad.FirstOrDefault(q => q.IdCiudad == info.IdCiudad);
                    if (Entity == null) return false;

                    Entity.Cod_Ciudad = info.Cod_Ciudad;
                    Entity.Descripcion_Ciudad = info.Descripcion_Ciudad;
                    Entity.IdProvincia = info.IdProvincia;
                    Entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                    Entity.Fecha_UltMod = info.Fecha_UltMod;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB( tb_ciudad_Info info)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_ciudad Entity = Context.tb_ciudad.FirstOrDefault(q => q.IdCiudad == info.IdCiudad);
                    if (Entity == null) return false;
                    Entity.Estado = info.Estado = "I";

                    Entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                    Entity.Fecha_UltAnu = info.Fecha_UltAnu;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<tb_ciudad_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            List<tb_ciudad_Info> Lista = new List<tb_ciudad_Info>();
            Lista = get_list(skip, take, args.Filter);
            return Lista;
        }

        public List<tb_ciudad_Info> get_list(int skip, int take, string filter)
        {
            try
            {
                List<tb_ciudad_Info> Lista;

                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    Lista = (from q in Context.tb_ciudad
                             where (q.IdCiudad.ToString() + " " + q.Descripcion_Ciudad).Contains(filter)
                                select new tb_ciudad_Info
                                {
                                    IdCiudad = q.IdCiudad,
                                    Descripcion_Ciudad = q.Descripcion_Ciudad
                                })
                                .OrderBy(p => p.IdCiudad)
                                .Skip(skip)
                                .Take(take)
                                .ToList();
                }
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tb_ciudad_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return get_info(args.Value == null ? "" : (string)args.Value);
        }
    }
}
