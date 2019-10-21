﻿using Core.Data.Base;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Academico
{
    public class aca_Paralelo_Data
    {
        public List<aca_Paralelo_Info> getList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                List<aca_Paralelo_Info> Lista = new List<aca_Paralelo_Info>();

                using (EntitiesAcademico odata = new EntitiesAcademico())
                {
                    var lst = odata.aca_Paralelo.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == (MostrarAnulados ? q.Estado : true)).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new aca_Paralelo_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdParalelo = q.IdParalelo,
                            CodigoParalelo = q.CodigoParalelo,
                            NomParalelo = q.NomParalelo,
                            OrdenParalelo = q.OrdenParalelo,
                            Estado = q.Estado
                        });
                    });
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public aca_Paralelo_Info getInfo(int IdEmpresa, int IdParalelo)
        {
            try
            {
                aca_Paralelo_Info info;

                using (EntitiesAcademico db = new EntitiesAcademico())
                {
                    var Entity = db.aca_Paralelo.Where(q => q.IdEmpresa == IdEmpresa && q.IdParalelo == IdParalelo).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new aca_Paralelo_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdParalelo = Entity.IdParalelo,
                        CodigoParalelo = Entity.CodigoParalelo,
                        NomParalelo = Entity.NomParalelo,
                        OrdenParalelo = Entity.OrdenParalelo,
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

        public int getId(int IdEmpresa)
        {
            try
            {
                int ID = 1;

                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var lst = from q in Context.aca_Paralelo
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdParalelo) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int getOrden(int IdEmpresa)
        {
            try
            {
                int ID = 1;

                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var lst = from q in Context.aca_Paralelo
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.OrdenParalelo) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public aca_Paralelo_Info existeCodigo(int IdEmpresa, string CodigoParalelo)
        {
            try
            {
                aca_Paralelo_Info info;

                using (EntitiesAcademico db = new EntitiesAcademico())
                {
                    var Entity = db.aca_Paralelo.Where(q => q.IdEmpresa == IdEmpresa && q.CodigoParalelo.ToUpper() == CodigoParalelo.ToUpper()).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new aca_Paralelo_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdParalelo = Entity.IdParalelo,
                        CodigoParalelo = Entity.CodigoParalelo,
                        NomParalelo = Entity.NomParalelo,
                        OrdenParalelo = Entity.OrdenParalelo,
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

        public bool guardarDB(aca_Paralelo_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Paralelo Entity = new aca_Paralelo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdParalelo = info.IdParalelo = getId(info.IdEmpresa),
                        NomParalelo = info.NomParalelo,
                        CodigoParalelo = info.CodigoParalelo,
                        OrdenParalelo = info.OrdenParalelo,
                        Estado = true,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = info.FechaCreacion = DateTime.Now
                    };
                    Context.aca_Paralelo.Add(Entity);

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(aca_Paralelo_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Paralelo Entity = Context.aca_Paralelo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdParalelo == info.IdParalelo);
                    if (Entity == null)
                        return false;
                    Entity.IdEmpresa = info.IdEmpresa;
                    Entity.NomParalelo = info.NomParalelo;
                    Entity.OrdenParalelo = info.OrdenParalelo;
                    Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
                    Entity.FechaModificacion = info.FechaModificacion = DateTime.Now;

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(aca_Paralelo_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Paralelo Entity = Context.aca_Paralelo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdParalelo == info.IdParalelo);
                    if (Entity == null)
                        return false;
                    Entity.Estado = info.Estado = false;
                    Entity.MotivoAnulacion = info.MotivoAnulacion;
                    Entity.IdUsuarioAnulacion = info.IdUsuarioAnulacion;
                    Entity.FechaAnulacion = info.FechaAnulacion = DateTime.Now;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
