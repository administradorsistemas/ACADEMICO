using Core.Data.Base;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Academico
{
    public class aca_AnioLectivo_Tematica_Data
    {
        public List<aca_AnioLectivo_Tematica_Info> get_list_asignacion(int IdEmpresa, int IdAnio)
        {
            try
            {
                List<aca_AnioLectivo_Tematica_Info> Lista;

                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    Lista = (from q in Context.aca_AnioLectivo_Tematica
                             join n in Context.aca_Tematica
                             on new { q.IdEmpresa, q.IdTematica } equals new { n.IdEmpresa, n.IdTematica }
                             where q.IdEmpresa == IdEmpresa
                             && q.IdAnio == IdAnio
                             && n.Estado == true
                             select new aca_AnioLectivo_Tematica_Info
                             {
                                 seleccionado = true,
                                 IdEmpresa = q.IdEmpresa,
                                 IdAnio = q.IdAnio,
                                 IdTematica = q.IdTematica,
                                 NombreTematica = q.NombreTematica,
                                 NombreCampoAccion = q.NombreCampoAccion,
                                 OrdenTematica = q.OrdenTematica,
                                 OrdenCampoAccion = q.OrdenCampoAccion
                             }).ToList();

                    Lista.AddRange((from q in Context.aca_Tematica
                                    join r in Context.aca_CampoAccion
                                    on new { q.IdEmpresa, q.IdCampoAccion } equals new { r.IdEmpresa, r.IdCampoAccion }
                                    where !Context.aca_AnioLectivo_Tematica.Any(n => n.IdTematica == q.IdTematica && n.IdEmpresa == IdEmpresa && n.IdAnio == IdAnio)
                                    && q.Estado == true && q.IdEmpresa== IdEmpresa
                                    select new aca_AnioLectivo_Tematica_Info
                                    {
                                        seleccionado = false,
                                        IdEmpresa = IdEmpresa,
                                        IdAnio = IdAnio,
                                        IdTematica = q.IdTematica,
                                        IdCampoAccion = q.IdCampoAccion,
                                        NombreCampoAccion = r.NombreCampoAccion,
                                        NombreTematica = q.NombreTematica,
                                        OrdenTematica = q.OrdenTematica
                                    }).ToList());
                }

                return Lista;
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
                aca_AnioLectivo_Tematica_Info info;

                using (EntitiesAcademico db = new EntitiesAcademico())
                {
                    var Entity = db.aca_AnioLectivo_Tematica.Where(q => q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdTematica == IdTematica).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new aca_AnioLectivo_Tematica_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdAnio = Entity.IdAnio,
                        IdTematica = Entity.IdTematica,
                        IdCampoAccion = Entity.IdCampoAccion,
                        NombreTematica = Entity.NombreTematica,
                        NombreCampoAccion = Entity.NombreCampoAccion,
                        OrdenTematica = Entity.OrdenTematica,
                        OrdenCampoAccion = Entity.OrdenCampoAccion
                    };
                }

                return info;
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
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var info_anio_curso = Context.aca_AnioLectivo.Where(q => q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.EnCurso == true).FirstOrDefault();
                    if (info_anio_curso != null)
                    {
                        var lst_Tematica_x_Anio = Context.aca_AnioLectivo_Tematica.Where(q => q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio).ToList();
                        Context.aca_AnioLectivo_Tematica.RemoveRange(lst_Tematica_x_Anio);

                        if (lista.Count > 0)
                        {
                            foreach (var info in lista)
                            {
                                aca_AnioLectivo_Tematica Entity = new aca_AnioLectivo_Tematica
                                {
                                    IdEmpresa = info.IdEmpresa,
                                    IdAnio = info.IdAnio,
                                    IdTematica = info.IdTematica,
                                    IdCampoAccion = info.IdCampoAccion,
                                    NombreCampoAccion = info.NombreCampoAccion,
                                    NombreTematica = info.NombreTematica,
                                    OrdenCampoAccion = info.OrdenCampoAccion,
                                    OrdenTematica = info.OrdenTematica,
                                    FechaModificacion = DateTime.Now
                                };
                                Context.aca_AnioLectivo_Tematica.Add(Entity);
                            }
                        }
                        Context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_AnioLectivo_Tematica_Info> getList(int IdEmpresa, int IdAnio)
        {
            try
            {
                List<aca_AnioLectivo_Tematica_Info> Lista = new List<aca_AnioLectivo_Tematica_Info>();

                using (EntitiesAcademico odata = new EntitiesAcademico())
                {
                    var lst = odata.aca_AnioLectivo_Tematica.Where(q => q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new aca_AnioLectivo_Tematica_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdAnio = q.IdAnio,
                            IdTematica = q.IdTematica,
                            IdCampoAccion = q.IdCampoAccion,
                            NombreCampoAccion = q.NombreCampoAccion,
                            NombreTematica = q.NombreTematica,
                            OrdenTematica = q.OrdenTematica,
                            OrdenCampoAccion = q.OrdenCampoAccion
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

        public List<aca_AnioLectivo_Tematica_Info> getList_CampoAccion(int IdEmpresa, int IdAnio)
        {
            try
            {
                List<aca_AnioLectivo_Tematica_Info> Lista = new List<aca_AnioLectivo_Tematica_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "select IdEmpresa, IdCampoAccion, NombreCampoAccion "
                    + " from aca_AnioLectivo_Tematica"
                    + " where IdEmpresa = " + IdEmpresa.ToString() + " and IdAnio = " + IdAnio.ToString()
                    + " group by IdEmpresa, IdCampoAccion,NombreCampoAccion";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_AnioLectivo_Tematica_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdCampoAccion = Convert.ToInt32(reader["IdCampoAccion"]),
                            NombreCampoAccion = reader["NombreCampoAccion"].ToString(),
                        });
                    }
                    reader.Close();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_AnioLectivo_Tematica_Info> getListTematica(int IdEmpresa, int IdAnio, int IdCampoAccion)
        {
            try
            {
                List<aca_AnioLectivo_Tematica_Info> Lista = new List<aca_AnioLectivo_Tematica_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "select IdEmpresa, IdTematica, NombreTematica "
                    + " from aca_AnioLectivo_Tematica "
                    + " where IdEmpresa = " + IdEmpresa.ToString() + " and IdAnio = " + IdAnio.ToString()
                    + " group by IdEmpresa, IdTematica,NombreTematica ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_AnioLectivo_Tematica_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdTematica = Convert.ToInt32(reader["IdTematica"]),
                            NombreTematica = reader["NombreTematica"].ToString(),
                        });
                    }
                    reader.Close();
                }

                return Lista;
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
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    if (lista.Count > 0)
                    {
                        foreach (var item in lista)
                        {
                            aca_AnioLectivo_Tematica Entity = Context.aca_AnioLectivo_Tematica.FirstOrDefault(q => q.IdEmpresa == item.IdEmpresa &&
                            q.IdAnio == item.IdAnio && q.IdTematica == item.IdTematica);
                            if (Entity == null)
                                return false;

                            Entity.NombreTematica = item.NombreTematica;
                            Entity.NombreCampoAccion = item.NombreCampoAccion;
                            Entity.OrdenCampoAccion = item.OrdenCampoAccion;
                            Entity.OrdenTematica = item.OrdenTematica;
                            Entity.IdUsuarioModificacion = item.IdUsuarioModificacion;
                            Entity.FechaModificacion = DateTime.Now;
                        }
                        Context.SaveChanges();
                    }
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
