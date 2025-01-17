﻿using Core.Data.Base;
using Core.Data.General;
using Core.Info.Caja;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Caja
{
    public class caj_Caja_Data
    {
        public List<caj_Caja_Info> GetList(int IdEmpresa, int IdSucursal, bool mostrar_anulados, string IdUsuario, bool EsContador)
        {
            try
            {
                List<caj_Caja_Info> Lista;
                using (EntitiesCaja Context = new EntitiesCaja())
                {
                    if (EsContador)
                    {
                        var lst = Context.caj_Caja.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == (mostrar_anulados ? q.Estado : "A") 
                        && q.IdSucursal == (IdSucursal == 0 ? q.IdSucursal : IdSucursal)
                        ).ToList();
                        Lista = lst.Select(q => new caj_Caja_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            ca_Codigo = q.ca_Codigo,
                            Estado = q.Estado,
                            IdCaja = q.IdCaja,
                            IdCtaCble = q.IdCtaCble,
                            IdSucursal = q.IdSucursal,
                            ca_Descripcion = q.ca_Descripcion,
                            IdUsuario_Responsable = q.IdUsuario_Responsable,

                            EstadoBool = q.Estado == "A" ? true : false
                        }).ToList();
                    }
                    else
                    {
                        var lst = Context.caj_Caja_x_seg_usuario.Include("caj_Caja").Where(q => q.IdEmpresa == IdEmpresa && q.IdUsuario.ToLower() == IdUsuario.ToLower() && q.caj_Caja.Estado == (mostrar_anulados ? q.caj_Caja.Estado : "A") && q.caj_Caja.IdSucursal == IdSucursal).ToList();
                        Lista = lst.Select(q => new caj_Caja_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            ca_Codigo = q.caj_Caja.ca_Codigo,
                            Estado = q.caj_Caja.Estado,
                            IdCaja = q.IdCaja,
                            IdCtaCble = q.caj_Caja.IdCtaCble,
                            IdSucursal = q.caj_Caja.IdSucursal,
                            ca_Descripcion = q.caj_Caja.ca_Descripcion,
                            IdUsuario_Responsable = q.caj_Caja.IdUsuario_Responsable,
                            EstadoBool = q.caj_Caja.Estado == "A" ? true : false
                        }).ToList();
                    }
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetIdCajaPorUsuario(int IdEmpresa, string IdUsuario)
        {
            try
            {
                int ID = 0;

                using (EntitiesCaja db = new EntitiesCaja())
                {
                    var PrimerCaja = db.caj_Caja_x_seg_usuario.Where(q => q.IdEmpresa == IdEmpresa && q.IdUsuario.ToLower() == IdUsuario.ToLower()).FirstOrDefault();
                    if(PrimerCaja != null)
                    {
                        ID = PrimerCaja.IdCaja;
                        return ID;
                    }
                    var caja = db.caj_Caja.Where(q => q.IdEmpresa == IdEmpresa && q.IdUsuario_Responsable.ToLower() == IdUsuario.ToLower() && q.Estado == "A").FirstOrDefault();
                    if (caja != null)
                        ID = caja.IdCaja;
                }

                return ID;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public caj_Caja_Info get_info(int IdEmpresa, int IdCaja)
        {
            try
            {
                caj_Caja_Info info = new caj_Caja_Info();
                using (EntitiesCaja Context = new EntitiesCaja())
                {
                    caj_Caja Entity = Context.caj_Caja.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdCaja == IdCaja);
                    if (Entity == null) return null;
                    info = new caj_Caja_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        ca_Codigo = Entity.ca_Codigo,
                        Estado = Entity.Estado,
                        IdCaja = Entity.IdCaja,
                        IdCtaCble = Entity.IdCtaCble,
                        IdSucursal = Entity.IdSucursal,
                        ca_Descripcion = Entity.ca_Descripcion,
                        IdUsuario_Responsable = Entity.IdUsuario_Responsable,

                    };
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int get_id(int IdEmpresa)
        {
            try
            {
                int ID = 1;

                using (EntitiesCaja Context = new EntitiesCaja())
                {
                    var lst = from q in Context.caj_Caja
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdCaja) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(caj_Caja_Info info)
        {
            try
            {
                using (EntitiesCaja Context = new EntitiesCaja())
                {
                    caj_Caja Entity = new caj_Caja
                    {

                        IdEmpresa = info.IdEmpresa,
                        ca_Codigo = info.ca_Codigo,
                        Estado = info.Estado="A",
                        IdCaja = info.IdCaja=get_id(info.IdEmpresa),
                        IdCtaCble = info.IdCtaCble,
                        IdSucursal = info.IdSucursal,
                        ca_Descripcion = info.ca_Descripcion,
                        IdUsuario_Responsable = info.IdUsuario_Responsable,


                         IdUsuario = info.IdUsuario,
                         Fecha_Transac = DateTime.Now
                    };

                    if (info.ListaResponsables != null)
                    {
                        int Secuencia = 1;

                        foreach (var item in info.ListaResponsables)
                        {
                            Context.caj_Caja_x_seg_usuario.Add(new caj_Caja_x_seg_usuario
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdCaja = info.IdCaja,
                                Secuencia = Secuencia++,
                                IdUsuario = item.IdUsuario
                            });

                        }
                    }

                    Context.caj_Caja.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "caj_Caja_Data", Metodo = "guardarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }

        public bool modificarDB(caj_Caja_Info info)
        {
            try
            {
                using (EntitiesCaja Context = new EntitiesCaja())
                {
                    caj_Caja Entity = Context.caj_Caja.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCaja == info.IdCaja);
                    if (Entity == null) return false;

                    Entity.ca_Codigo = info.ca_Codigo;
                    Entity.IdCtaCble = info.IdCtaCble;
                    Entity.IdSucursal = info.IdSucursal;
                    Entity.ca_Descripcion = info.ca_Descripcion;
                    Entity.IdUsuario_Responsable = info.IdUsuario_Responsable;
                    Entity.IdSucursal = info.IdSucursal;


                    Entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                    Entity.Fecha_UltMod = info.Fecha_UltMod;
                   
                    var lst_Responsables = Context.caj_Caja_x_seg_usuario.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCaja == info.IdCaja).ToList();
                    Context.caj_Caja_x_seg_usuario.RemoveRange(lst_Responsables);

                    if (info.ListaResponsables != null)
                    {
                        int Secuencia = 1;

                        foreach (var item in info.ListaResponsables)
                        {
                            Context.caj_Caja_x_seg_usuario.Add(new caj_Caja_x_seg_usuario
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdCaja = info.IdCaja,
                                Secuencia = Secuencia++,
                                IdUsuario= item.IdUsuario
                            });

                        }
                    }
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "caj_Caja_Data", Metodo = "modificarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }

        public bool anularDB(caj_Caja_Info info)
        {
            try
            {
                using (EntitiesCaja Context = new EntitiesCaja())
                {
                    caj_Caja Entity = Context.caj_Caja.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCaja == info.IdCaja);
                    if (Entity == null) return false;

                    Entity.Estado = Entity.Estado="I";

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

        public string get_IdCtaCble(int IdEmpresa, int IdCaja)
        {
            try
            {
                string IdCtaCble = string.Empty;

                using (EntitiesCaja Context = new EntitiesCaja())
                {
                    var Entity = Context.caj_Caja.Where(q => q.IdEmpresa == IdEmpresa && q.IdCaja == IdCaja).FirstOrDefault();
                    if (Entity != null)
                        IdCtaCble = Entity.IdCtaCble;
                }

                return IdCtaCble;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetIdCajaPorSucursal(int IdEmpresa, int IdSucursal)
        {
            try
            {

                int ID = 1;
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    var sucursal = Context.fa_PuntoVta.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal).FirstOrDefault();
                    if (sucursal != null)
                        ID = sucursal.IdCaja;
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
