﻿using Core.Data.Base;
using Core.Info.SeguridadAcceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.SeguridadAcceso
{
    public class seg_usuario_Data
    {
        public List<seg_usuario_Info> get_list(bool mostrar_anulados)
        {
            try
            {
                List<seg_usuario_Info> Lista;

                using (EntitiesSeguridadAcceso Context = new EntitiesSeguridadAcceso())
                {
                    if (mostrar_anulados)
                        Lista = (from q in Context.seg_usuario
                                 select new seg_usuario_Info
                                 {
                                     IdUsuario = q.IdUsuario,
                                     estado = q.estado,
                                     Nombre = q.Nombre,

                                     EstadoBool = q.estado == "A" ? true : false
                                 }).ToList();
                    else
                        Lista = (from q in Context.seg_usuario
                                 where q.estado == "A"
                                 select new seg_usuario_Info
                                 {
                                     IdUsuario = q.IdUsuario,
                                     estado = q.estado,
                                     Nombre = q.Nombre,

                                     EstadoBool = q.estado == "A" ? true : false
                                 }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<seg_usuario_Info> GetListCobradores(int IdEmpresa)
        {
            try
            {
                List<seg_usuario_Info> Lista = new List<seg_usuario_Info>();

                using (EntitiesCuentasPorCobrar db = new Base.EntitiesCuentasPorCobrar())
                {
                    var lst = db.cxc_cobro.Where(q => q.IdEmpresa == IdEmpresa).GroupBy(q=> q.IdUsuario).ToList();
                    foreach (var item in lst)
                    {
                        Lista.Add(new seg_usuario_Info
                        {
                            IdUsuario =item.Key,
                            Nombre = item.Key
                        });
                    }
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public seg_usuario_Info validar_login(string IdUsuario, string contrasena)
        {
            try
            {
                seg_usuario_Info info = new seg_usuario_Info();
                using (EntitiesSeguridadAcceso Context = new EntitiesSeguridadAcceso())
                {
                    seg_usuario Entity = Context.seg_usuario.FirstOrDefault(q => q.IdUsuario == IdUsuario && q.Contrasena == contrasena && q.estado == "A");
                    if (Entity == null) return null;
                    info = new seg_usuario_Info
                    {
                        IdUsuario = Entity.IdUsuario,
                        Contrasena = Entity.Contrasena,
                        CambiarContraseniaSgtSesion = Entity.CambiarContraseniaSgtSesion == null ? false : Convert.ToBoolean(Entity.CambiarContraseniaSgtSesion)
                    };

                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool validar_existe_usuario(string IdUsuario)
        {
            try
            {
                using (EntitiesSeguridadAcceso Context = new EntitiesSeguridadAcceso())
                {
                    var lst = from q in Context.seg_usuario
                              where q.IdUsuario == IdUsuario
                              select q;

                    if (lst.Count() > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public seg_usuario_Info get_info(string IdUsuario)
        {
            try
            {
                seg_usuario_Info info = new seg_usuario_Info();

                using (EntitiesSeguridadAcceso Context = new EntitiesSeguridadAcceso())
                {
                    seg_usuario Entity = Context.seg_usuario.FirstOrDefault(q => q.IdUsuario.ToLower() == IdUsuario.ToLower());
                    if (Entity == null) return null;
                    info = new seg_usuario_Info
                    {
                        IdUsuario = Entity.IdUsuario,
                        Contrasena = Entity.Contrasena,
                        estado = Entity.estado,
                        Nombre = Entity.Nombre,
                        es_super_admin = Entity.es_super_admin,
                        contrasena_admin = Entity.contrasena_admin,
                        ExigirDirectivaContrasenia = Entity.ExigirDirectivaContrasenia == null ? false : Convert.ToBoolean(Entity.ExigirDirectivaContrasenia),
                        CambiarContraseniaSgtSesion = Entity.CambiarContraseniaSgtSesion == null ? false : Convert.ToBoolean(Entity.CambiarContraseniaSgtSesion),
                        IdMenu = Entity.IdMenu,
                        IPImpresora = Entity.IPImpresora,
                        IPMaquina = Entity.IPUsuario,
                        EsContador = Entity.EsContador,
                        ManejaAdmision = Entity.ManejaAdmision
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(seg_usuario_Info info)
        {
            try
            {
                using (EntitiesSeguridadAcceso Context = new EntitiesSeguridadAcceso())
                {
                    seg_usuario Entity = new seg_usuario
                    {
                        IdUsuario = info.IdUsuario,
                        Contrasena = info.Contrasena,
                        ExigirDirectivaContrasenia = info.ExigirDirectivaContrasenia,
                        CambiarContraseniaSgtSesion = info.CambiarContraseniaSgtSesion,
                        Nombre = info.Nombre,
                        es_super_admin = info.es_super_admin,
                        contrasena_admin = info.contrasena_admin,
                        estado = info.estado = "A",
                        IdMenu = info.IdMenu == 0 ? null : info.IdMenu,
                        IPImpresora = info.IPImpresora,
                        IPUsuario = info.IPMaquina,
                        Fecha_Transaccion = DateTime.Now,
                        EsContador = info.EsContador,
                        ManejaAdmision = info.ManejaAdmision==null ? false : info.ManejaAdmision
                    };
                    Context.seg_usuario.Add(Entity);
                    
                    if (info.lst_usuario_sede.Count() > 0)
                    {
                        foreach (var item in info.lst_usuario_sede)
                        {
                            Context.seg_usuario_x_aca_Sede.Add(new seg_usuario_x_aca_Sede
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdUsuario = info.IdUsuario,
                                IdSede = item.IdSede,
                                Observacion = item.Observacion                                
                            });
                        }
                        var Lista_empresa = (from q in info.lst_usuario_sede
                                             group q by new
                                         {
                                             q.IdEmpresa,
                                         } into emp
                                         select new 
                                         {
                                             IdEmpresa = emp.Key.IdEmpresa,
                                         }).ToList();

                        foreach (var item in Lista_empresa)
                        {
                            Context.seg_Usuario_x_Empresa.Add(new seg_Usuario_x_Empresa
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdUsuario = info.IdUsuario
                            });
                        }
                    }
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool modificarDB(seg_usuario_Info info)
        {
            try
            {
                using (EntitiesSeguridadAcceso Context = new EntitiesSeguridadAcceso())
                {
                    seg_usuario Entity = Context.seg_usuario.FirstOrDefault(q => q.IdUsuario == info.IdUsuario);
                    if (Entity == null) return false;
                    Entity.Nombre = info.Nombre;
                    Entity.CambiarContraseniaSgtSesion = info.CambiarContraseniaSgtSesion;
                    Entity.ExigirDirectivaContrasenia = info.ExigirDirectivaContrasenia;
                    Entity.contrasena_admin = info.contrasena_admin;
                    Entity.es_super_admin = info.es_super_admin;
                    Entity.IdUsuarioUltModi = info.IdUsuario;
                    Entity.Fecha_UltMod = DateTime.Now;
                    Entity.IdMenu = info.IdMenu == 0 ? null : info.IdMenu;
                    Entity.IPImpresora = info.IPImpresora;
                    Entity.IPUsuario = info.IPMaquina;
                    Entity.EsContador = info.EsContador;
                    Entity.ManejaAdmision = info.ManejaAdmision == null ? false : info.ManejaAdmision;
                    var lst = Context.seg_usuario_x_aca_Sede.Where(q => q.IdUsuario == info.IdUsuario).ToList();
                    Context.seg_usuario_x_aca_Sede.RemoveRange(lst);

                    var lista = Context.seg_Usuario_x_Empresa.Where(q => q.IdUsuario == info.IdUsuario).ToList();
                    Context.seg_Usuario_x_Empresa.RemoveRange(lista);
                    
                   if (info.lst_usuario_sede.Count() > 0)
                    {
                        foreach (var item in info.lst_usuario_sede)
                        {
                            Context.seg_usuario_x_aca_Sede.Add(new seg_usuario_x_aca_Sede
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdUsuario = info.IdUsuario,
                                IdSede = item.IdSede,
                                Observacion = item.Observacion                                
                            });
                        }
                        var Lista_empresa = (from q in info.lst_usuario_sede
                                             group q by new
                                         {
                                             q.IdEmpresa,
                                         } into emp
                                         select new 
                                         {
                                             IdEmpresa = emp.Key.IdEmpresa,
                                         }).ToList();

                        foreach (var item in Lista_empresa)
                        {
                            Context.seg_Usuario_x_Empresa.Add(new seg_Usuario_x_Empresa
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdUsuario = info.IdUsuario
                            });
                        }
                    }
                    
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(string IdUsuario, string old_Contrasena, string new_Contrasena)
        {
            try
            {
                using (EntitiesSeguridadAcceso Context = new EntitiesSeguridadAcceso())
                {
                    seg_usuario Entity = Context.seg_usuario.FirstOrDefault(q => q.IdUsuario == IdUsuario && q.Contrasena == old_Contrasena && q.estado == "A");
                    if (Entity == null)
                        return false;
                    Entity.Contrasena = new_Contrasena;
                    Entity.CambiarContraseniaSgtSesion = false;
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(seg_usuario_Info info)
        {
            try
            {
                using (EntitiesSeguridadAcceso Context = new EntitiesSeguridadAcceso())
                {
                    seg_usuario Entity = Context.seg_usuario.FirstOrDefault(q => q.IdUsuario == info.IdUsuario);
                    if (Entity == null) return false;
                    Entity.estado = info.estado = "I";

                    Entity.IdUsuarioUltAnu = info.IdUsuario;
                    Entity.Fecha_UltAnu = DateTime.Now;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ResetearContrasenia(string IdUsuario, string Contrasena)
        {
            try
            {
                using (EntitiesSeguridadAcceso Context = new EntitiesSeguridadAcceso())
                {
                    var usuario = Context.seg_usuario.Where(q => q.IdUsuario == IdUsuario).FirstOrDefault();
                    if (usuario == null) return false;
                    usuario.Contrasena = Contrasena;
                    usuario.CambiarContraseniaSgtSesion = true;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /*
        public List<seg_usuario_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            List<seg_usuario_Info> Lista = new List<seg_usuario_Info>();
            Lista = get_list(skip, take, args.Filter);
            return Lista;
        }

        public seg_usuario_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            //La variable args del devexpress ya trae el ID seleccionado en la propiedad Value, se pasa el IdEmpresa porque es un filtro que no tiene
            return get_info(args.Value == null ? "" : args.Value.ToString());
        }

        public List<seg_usuario_Info> get_list(int skip, int take, string filter)
        {
            try
            {
                List<seg_usuario_Info> Lista = new List<seg_usuario_Info>();

                Entities_seguridad_acceso Context = new Entities_seguridad_acceso();
                {
                    List<seg_usuario> ListaUsuarios;
                    ListaUsuarios = Context.seg_usuario.Where(q => q.estado == "A" && (q.IdUsuario + " " + q.Nombre).Contains(filter)).OrderBy(q => q.IdUsuario).Skip(skip).Take(take).ToList();
                    foreach (var q in ListaUsuarios)
                    {
                        Lista.Add(new seg_usuario_Info
                        {
                            IdUsuario = q.IdUsuario,
                            Nombre = q.Nombre
                        });
                    }
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }*/
    }
}
