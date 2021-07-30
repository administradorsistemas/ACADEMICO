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
    public class aca_MatriculaGrado_Data
    {
        public List<aca_MatriculaGrado_Info> getList(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo)
        {
            try
            {
                List<aca_MatriculaGrado_Info> Lista = new List<aca_MatriculaGrado_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "select mc.IdEmpresa, mc.IdMatricula, mc.CalificacionGrado, "
                    + " a.Codigo, per.pe_nombreCompleto AS pe_nombreCompleto, "
                    + " m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno "
                    + " from aca_MatriculaGrado mc "
                    + " inner join aca_Matricula m on mc.IdEmpresa = m.IdEmpresa and mc.IdMatricula = m.IdMatricula "
                    + " inner join dbo.aca_Alumno AS a WITH (nolock)ON m.IdEmpresa = a.IdEmpresa AND m.IdAlumno = a.IdAlumno "
                    + " inner join dbo.tb_persona AS per WITH(nolock) ON a.IdPersona = per.IdPersona "
                    + " WHERE(NOT EXISTS "
                    + " (SELECT IdEmpresa "
                    + " FROM      dbo.aca_AlumnoRetiro AS f WITH (nolock) "
                    + " WHERE(IdEmpresa = mc.IdEmpresa) AND(IdMatricula = mc.IdMatricula) AND(Estado = 1))) "
                    + " AND mc.IdEmpresa = " + IdEmpresa.ToString() + " and m.IdAnio = " + IdAnio.ToString() + " and m.IdSede = " + IdSede.ToString()
                    + " AND m.IdNivel = " + IdNivel.ToString() + " and m.IdJornada = " + IdJornada.ToString() + " and m.IdCurso = " + IdCurso.ToString()
                    + " AND m.IdParalelo = " + IdParalelo.ToString();
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaGrado_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            CalificacionGrado = string.IsNullOrEmpty(reader["CalificacionGrado"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionGrado"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),                            
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            pe_nombreCompleto = string.IsNullOrEmpty(reader["pe_nombreCompleto"].ToString()) ? null : reader["pe_nombreCompleto"].ToString(),
                            RegistroValido = true
                        });
                    }
                    reader.Close();
                }
                Lista = Lista.OrderBy(q => q.pe_nombreCompleto).ToList();
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_MatriculaGrado_Info getInfo(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, decimal IdAlumno)
        {
            try
            {
                aca_MatriculaGrado_Info info = new aca_MatriculaGrado_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    string query = "select mc.IdEmpresa, mc.IdMatricula, mc.CalificacionGrado, "
                    + " a.Codigo, per.pe_nombreCompleto AS pe_nombreCompleto, "
                    + " m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo "
                    + " from aca_MatriculaGrado mc "
                    + " inner join aca_Matricula m on mc.IdEmpresa = m.IdEmpresa and mc.IdMatricula = m.IdMatricula "
                    + " inner join dbo.aca_Alumno AS a WITH (nolock)ON m.IdEmpresa = a.IdEmpresa AND m.IdAlumno = a.IdAlumno "
                    + " inner join dbo.tb_persona AS per WITH(nolock) ON a.IdPersona = per.IdPersona "
                    + " WHERE(NOT EXISTS "
                    + " (SELECT IdEmpresa "
                    + " FROM      dbo.aca_AlumnoRetiro AS f WITH (nolock) "
                    + " WHERE(IdEmpresa = mc.IdEmpresa) AND(IdMatricula = mc.IdMatricula) AND(Estado = 1))) "
                    + " AND mc.IdEmpresa = " + IdEmpresa.ToString() + " and m.IdAnio = " + IdAnio.ToString() + " and m.IdSede = " + IdSede.ToString()
                    + " AND m.IdNivel = " + IdNivel.ToString() + " and m.IdJornada = " + IdJornada.ToString() + " and m.IdCurso = " + IdCurso.ToString()
                    + " AND m.IdParalelo = " + IdParalelo.ToString() + " and m.IdAlumno = "+IdAlumno.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_MatriculaGrado_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            CalificacionGrado = string.IsNullOrEmpty(reader["CalificacionGrado"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionGrado"]),
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

        public aca_MatriculaGrado_Info getInfo_X_Matricula(int IdEmpresa, decimal IdMatricula)
        {
            try
            {
                aca_MatriculaGrado_Info info = new aca_MatriculaGrado_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_MatriculaGrado WITH (nolock) "
                    + " WHERE IdEmpresa = " + IdEmpresa.ToString() + " and IdMatricula = " + IdMatricula.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_MatriculaGrado_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            CalificacionGrado = string.IsNullOrEmpty(reader["CalificacionGrado"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionGrado"]),
                            IdUsuarioCreacion = string.IsNullOrEmpty(reader["IdUsuarioCreacion"].ToString()) ? null : reader["IdUsuarioCreacion"].ToString(),
                            FechaCreacion = string.IsNullOrEmpty(reader["FechaCreacion"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["FechaCreacion"]),
                            IdUsuarioModificacion = string.IsNullOrEmpty(reader["IdUsuarioCreacion"].ToString()) ? null : reader["IdUsuarioCreacion"].ToString(),
                            FechaModificacion = string.IsNullOrEmpty(reader["FechaModificacion"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["FechaModificacion"]),
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
        public bool modificarDB(aca_MatriculaGrado_Info info)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "UPDATE aca_MatriculaGrado "
                    + " SET [CalificacionGrado] = " + (string.IsNullOrEmpty(info.CalificacionGrado.ToString()) ? "NULL" : info.CalificacionGrado.ToString())
                    + " ,[IdUsuarioModificacion] = " + "'" + info.IdUsuarioModificacion + "'"
                    + " ,[FechaModificacion] =  GETDATE() "
                    + " WHERE[IdEmpresa] = " + info.IdEmpresa + " and IdMatricula = " + info.IdMatricula;
                    var ResultValue = command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool generarCalificacion(List<aca_MatriculaGrado_Info> lst_Grado)
        {
            try
            {
                List<aca_MatriculaGrado_Info> Lista = new List<aca_MatriculaGrado_Info>();

                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    foreach (var info in lst_Grado)
                    {
                        var lista_calificacion = Context.aca_MatriculaGrado.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdMatricula == info.IdMatricula).ToList();
                        Context.aca_MatriculaGrado.RemoveRange(lista_calificacion);

                        aca_MatriculaGrado Entity = new aca_MatriculaGrado
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdMatricula = info.IdMatricula,
                            CalificacionGrado = info.CalificacionGrado,
                            IdUsuarioCreacion = info.IdUsuarioCreacion,
                            FechaCreacion = info.FechaCreacion,
                            IdUsuarioModificacion = info.IdUsuarioModificacion,
                            FechaModificacion = info.FechaModificacion
                        };

                        Context.aca_MatriculaGrado.Add(Entity);
                        Context.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
