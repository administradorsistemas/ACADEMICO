﻿using Core.Data.Base;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Academico
{
    public class aca_Materia_Data
    {
        public List<aca_Materia_Info> getList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                List<aca_Materia_Info> Lista = new List<aca_Materia_Info>();

                using (EntitiesAcademico odata = new EntitiesAcademico())
                {
                    var lst = odata.aca_Materia.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == (MostrarAnulados ? q.Estado : true)).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new aca_Materia_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdMateria = q.IdMateria,
                            NomMateria = q.NomMateria,
                            OrdenMateria = q.OrdenMateria,
                            EsObligatorio = q.EsObligatorio,
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

        public aca_Materia_Info getInfo(int IdEmpresa, int IdMateria)
        {
            try
            {
                aca_Materia_Info info;

                using (EntitiesAcademico db = new EntitiesAcademico())
                {
                    var Entity = db.aca_Materia.Where(q => q.IdEmpresa == IdEmpresa && q.IdMateria == IdMateria).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new aca_Materia_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdMateria = Entity.IdMateria,
                        NomMateria = Entity.NomMateria,
                        OrdenMateria = Entity.OrdenMateria,
                        EsObligatorio = Entity.EsObligatorio,
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
                    var lst = from q in Context.aca_Materia
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdMateria) + 1;
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
                    var lst = from q in Context.aca_Materia
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.OrdenMateria) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(aca_Materia_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Materia Entity = new aca_Materia
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdMateria = info.IdMateria = getId(info.IdEmpresa),
                        NomMateria = info.NomMateria,
                        OrdenMateria = info.OrdenMateria,
                        EsObligatorio = info.EsObligatorio,
                        Estado = true,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = info.FechaCreacion = DateTime.Now
                    };
                    Context.aca_Materia.Add(Entity);

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(aca_Materia_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Materia Entity = Context.aca_Materia.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdMateria == info.IdMateria);
                    if (Entity == null)
                        return false;
                    Entity.IdEmpresa = info.IdEmpresa;
                    Entity.NomMateria = info.NomMateria;
                    Entity.OrdenMateria = info.OrdenMateria;
                    Entity.EsObligatorio = info.EsObligatorio;
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

        public bool anularDB(aca_Materia_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Materia Entity = Context.aca_Materia.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdMateria == info.IdMateria);
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
