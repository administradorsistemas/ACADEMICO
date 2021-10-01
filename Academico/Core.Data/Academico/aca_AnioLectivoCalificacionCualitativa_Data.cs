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
    public class aca_AnioLectivoCalificacionCualitativa_Data
    {
        public List<aca_AnioLectivoCalificacionCualitativa_Info> getList(int IdEmpresa, int IdAnio, bool MostrarAnulados)
        {
            try
            {
                List<aca_AnioLectivoCalificacionCualitativa_Info> Lista = new List<aca_AnioLectivoCalificacionCualitativa_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT * FROM aca_AnioLectivoCalificacionCualitativa WITH (nolock) "
                    + " WHERE IdEmpresa = " + IdEmpresa.ToString() + " and IdAnio = " + IdAnio.ToString();
                    if (MostrarAnulados==false)
                    {
                        query += " and Estado = 1";
                    }
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_AnioLectivoCalificacionCualitativa_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdCalificacionCualitativa = Convert.ToInt32(reader["IdCalificacionCualitativa"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            DescripcionCorta = string.IsNullOrEmpty(reader["DescripcionCorta"].ToString()) ? null : reader["DescripcionCorta"].ToString(),
                            DescripcionLarga = string.IsNullOrEmpty(reader["DescripcionLarga"].ToString()) ? null : reader["DescripcionLarga"].ToString(),
                            Calificacion = string.IsNullOrEmpty(reader["Calificacion"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Calificacion"]),
                            Estado = Convert.ToBoolean(reader["Estado"])
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

        public aca_AnioLectivoCalificacionCualitativa_Info getInfo(int IdEmpresa, int IdAnio, int IdCalificacionCualitativa)
        {
            try
            {
                aca_AnioLectivoCalificacionCualitativa_Info info = new aca_AnioLectivoCalificacionCualitativa_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_AnioLectivoCalificacionCualitativa WITH (nolock) "
                    + " WHERE IdEmpresa = " + IdEmpresa.ToString() + " and IdAnio = " + IdAnio.ToString() + " and IdCalificacionCualitativa = " + IdCalificacionCualitativa.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_AnioLectivoCalificacionCualitativa_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdCalificacionCualitativa = Convert.ToInt32(reader["IdCalificacionCualitativa"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            DescripcionCorta = string.IsNullOrEmpty(reader["DescripcionCorta"].ToString()) ? null : reader["DescripcionCorta"].ToString(),
                            DescripcionLarga = string.IsNullOrEmpty(reader["DescripcionLarga"].ToString()) ? null : reader["DescripcionLarga"].ToString(),
                            Calificacion = string.IsNullOrEmpty(reader["Calificacion"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Calificacion"]),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                    }
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public aca_AnioLectivoCalificacionCualitativa_Info getInfo_Codigo(int IdEmpresa, int IdAnio, string Codigo)
        {
            try
            {
                aca_AnioLectivoCalificacionCualitativa_Info info = new aca_AnioLectivoCalificacionCualitativa_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_AnioLectivoCalificacionCualitativa WITH (nolock) "
                    + " WHERE IdEmpresa = " + IdEmpresa.ToString() + " and IdAnio = " + IdAnio.ToString() + " and Codigo = " + "'" + Codigo.ToString() + "'";
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_AnioLectivoCalificacionCualitativa_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdCalificacionCualitativa = Convert.ToInt32(reader["IdCalificacionCualitativa"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            DescripcionCorta = string.IsNullOrEmpty(reader["DescripcionCorta"].ToString()) ? null : reader["DescripcionCorta"].ToString(),
                            DescripcionLarga = string.IsNullOrEmpty(reader["DescripcionLarga"].ToString()) ? null : reader["DescripcionLarga"].ToString(),
                            Calificacion = string.IsNullOrEmpty(reader["Calificacion"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Calificacion"]),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                    }
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
                    var cont = Context.aca_AnioLectivoCalificacionCualitativa.Where(q => q.IdEmpresa == IdEmpresa).Count();
                    if (cont > 0)
                        ID = Context.aca_AnioLectivoCalificacionCualitativa.Where(q => q.IdEmpresa == IdEmpresa).Max(q => q.IdCalificacionCualitativa) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(aca_AnioLectivoCalificacionCualitativa_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_AnioLectivoCalificacionCualitativa Entity = new aca_AnioLectivoCalificacionCualitativa
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdAnio = info.IdAnio,
                        IdCalificacionCualitativa = info.IdCalificacionCualitativa=getId(info.IdEmpresa),
                        Codigo = info.Codigo,
                        DescripcionCorta = info.DescripcionCorta,
                        DescripcionLarga = info.DescripcionLarga,
                        Calificacion = info.Calificacion,
                        Estado = true,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = DateTime.Now
                    };
                    Context.aca_AnioLectivoCalificacionCualitativa.Add(Entity);

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(aca_AnioLectivoCalificacionCualitativa_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_AnioLectivoCalificacionCualitativa Entity = Context.aca_AnioLectivoCalificacionCualitativa.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCalificacionCualitativa == info.IdCalificacionCualitativa);
                    if (Entity == null)
                        return false;

                    Entity.Codigo = info.Codigo;
                    Entity.DescripcionCorta = info.DescripcionCorta;
                    Entity.DescripcionLarga = info.DescripcionLarga;
                    Entity.Calificacion = info.Calificacion;
                    Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
                    Entity.FechaModificacion = DateTime.Now;

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(aca_AnioLectivoCalificacionCualitativa_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_AnioLectivoCalificacionCualitativa Entity = Context.aca_AnioLectivoCalificacionCualitativa.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCalificacionCualitativa == info.IdCalificacionCualitativa);
                    if (Entity == null)
                        return false;

                    Entity.Estado = false;
                    Entity.IdUsuarioAnulacion = info.IdUsuarioAnulacion;
                    Entity.FechaAnulacion = DateTime.Now;

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public aca_AnioLectivoCalificacionCualitativa_Info getInfo_x_Promedio(int IdEmpresa, int IdAnio, decimal? PromedioFinal)
        {
            try
            {
                aca_AnioLectivoCalificacionCualitativa_Info info;

                using (EntitiesAcademico db = new EntitiesAcademico())
                {
                    var PromedioRedondeado = Math.Round(Convert.ToDecimal(PromedioFinal), 2, MidpointRounding.AwayFromZero);
                    var Entity = db.aca_AnioLectivoCalificacionCualitativa.Where(q => q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.Calificacion <= PromedioRedondeado).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    if (PromedioFinal == null)
                        return null;

                    info = new aca_AnioLectivoCalificacionCualitativa_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdAnio = Entity.IdAnio,
                        IdCalificacionCualitativa = Entity.IdCalificacionCualitativa,
                        Codigo = Entity.Codigo,
                        Calificacion = Entity.Calificacion
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
