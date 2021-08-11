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
    public class aca_AnioLectivo_Periodo_Data
    {
        public List<aca_AnioLectivo_Periodo_Info> getList(int IdEmpresa)
        {
            try
            {
                List<aca_AnioLectivo_Periodo_Info> Lista = new List<aca_AnioLectivo_Periodo_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT ap.IdEmpresa, ap.IdAnio, a.Descripcion, COUNT(ap.IdPeriodo) AS NumPeriodos "
                    + " FROM dbo.aca_AnioLectivo_Periodo AS ap WITH (nolock) INNER JOIN "
                    + " dbo.aca_AnioLectivo AS a WITH (nolock) ON ap.IdEmpresa = a.IdEmpresa AND ap.IdAnio = a.IdAnio "
                    + " WHERE ap.IdEmpresa = " + IdEmpresa.ToString()
                    + " GROUP BY ap.IdEmpresa, ap.IdAnio, a.Descripcion ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_AnioLectivo_Periodo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            NumPeriodos = string.IsNullOrEmpty(reader["NumPeriodos"].ToString()) ? 0 : Convert.ToInt32(reader["NumPeriodos"]),
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
        public List<aca_AnioLectivo_Periodo_Info> getList_AnioCurso(int IdEmpresa)
        {
            try
            {
                List<aca_AnioLectivo_Periodo_Info> Lista = new List<aca_AnioLectivo_Periodo_Info>();

                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    Lista = (from ap in Context.aca_AnioLectivo_Periodo
                             join a in Context.aca_AnioLectivo on new { ap.IdEmpresa, ap.IdAnio } equals new { a.IdEmpresa, a.IdAnio }
                             where ap.IdEmpresa == IdEmpresa
                             && a.EnCurso==true
                             select new aca_AnioLectivo_Periodo_Info
                             {
                                IdEmpresa = ap.IdEmpresa,
                                IdAnio = ap.IdAnio,
                                FechaDesde = ap.FechaDesde,
                                Descripcion = a.Descripcion,
                                IdPeriodo = ap.IdPeriodo,
                                IdMes = ap.IdMes,
                                FechaHasta = ap.FechaHasta,
                                Estado = ap.Estado,
                                Procesado=ap.Procesado,
                                TotalValorFacturado = ap.TotalValorFacturado
                             }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_AnioLectivo_Periodo_Info> getList(int IdEmpresa, int IdAnio, bool MostrarAnulados)
        {
            try
            {
                List<aca_AnioLectivo_Periodo_Info> Lista = new List<aca_AnioLectivo_Periodo_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT * FROM aca_AnioLectivo_Periodo WITH (nolock) "
                    + " WHERE IdEmpresa = " + IdEmpresa.ToString() + " and IdAnio = "+IdAnio.ToString();
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
                        Lista.Add(new aca_AnioLectivo_Periodo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdPeriodo = Convert.ToInt32(reader["IdPeriodo"]),
                            IdMes = Convert.ToInt32(reader["IdMes"]),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            FechaProntoPago = string.IsNullOrEmpty(reader["FechaProntoPago"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["FechaProntoPago"]),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        });
                    }
                    reader.Close();
                }

                Lista.ForEach(v => { v.NomPeriodo = v.FechaDesde.Year.ToString("0000") + v.FechaDesde.Month.ToString("00"); });
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public aca_AnioLectivo_Periodo_Info getInfo(int IdEmpresa, int IdAnio, int IdPeriodo)
        {
            try
            {
                aca_AnioLectivo_Periodo_Info info = new aca_AnioLectivo_Periodo_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_AnioLectivo_Periodo WITH (nolock) "
                    + " WHERE IdEmpresa = " + IdEmpresa.ToString() + " and IdAnio = " + IdAnio.ToString() + " and IdPeriodo = " + IdPeriodo.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_AnioLectivo_Periodo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdPeriodo = Convert.ToInt32(reader["IdPeriodo"]),
                            IdMes = Convert.ToInt32(reader["IdMes"]),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            FechaProntoPago = string.IsNullOrEmpty(reader["FechaProntoPago"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["FechaProntoPago"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            Procesado = string.IsNullOrEmpty(reader["Procesado"].ToString()) ? false : Convert.ToBoolean(reader["Procesado"])
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
        public bool modificarDB(List<aca_AnioLectivo_Periodo_Info> info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    foreach (var item in info)
                    {
                        aca_AnioLectivo_Periodo Entity = Context.aca_AnioLectivo_Periodo.FirstOrDefault(q => q.IdEmpresa == item.IdEmpresa && q.IdAnio == item.IdAnio && q.IdPeriodo == item.IdPeriodo);
                        if (Entity == null)
                            return false;

                        Entity.IdMes = item.IdMes;
                        Entity.FechaDesde = item.FechaDesde;
                        Entity.FechaHasta = item.FechaHasta;
                        Entity.FechaProntoPago = item.FechaProntoPago;
                        Entity.IdUsuarioModificacion = item.IdUsuarioModificacion;
                        Entity.FechaModificacion = item.FechaModificacion = DateTime.Now;

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

        public bool anularDB(aca_AnioLectivo_Periodo_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_AnioLectivo_Periodo Entity = Context.aca_AnioLectivo_Periodo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAnio == info.IdAnio);
                    if (Entity == null)
                        return false;

                    Entity.Estado = false;
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

        public int getId(int IdEmpresa)
        {
            try
            {
                int ID = 1;

                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var cont = Context.aca_AnioLectivo_Periodo.Where(q => q.IdEmpresa == IdEmpresa).Count();
                    if (cont > 0)
                        ID = Context.aca_AnioLectivo_Periodo.Where(q => q.IdEmpresa == IdEmpresa).Max(q => q.IdPeriodo) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public bool modificar_FacturacionMasiva(aca_AnioLectivo_Periodo_Info info)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "  SELECT IdEmpresa  "
                        + " FROM aca_Matricula_Rubro WITH (nolock)"
                        + " WHERE IdEmpresa = " + info.IdEmpresa.ToString() + " AND IdPeriodo = " + info.IdPeriodo.ToString() + " AND IdAnio = " + info.IdAnio.ToString()
                        + " AND FechaFacturacion IS NULL and not exists("
                        + " select x.IdEmpresa from aca_AlumnoRetiro as x with (nolock)"
                        + " where aca_Matricula_Rubro.IdEmpresa = x.IdEmpresa"
                        + " and aca_Matricula_Rubro.IdMatricula = x.IdMatricula"
                        + " and x.Estado = 1)";
                    
                    var ResultValue = command.ExecuteScalar();
                    if( ResultValue == null)
                    {
                        command.CommandText = "update aca_AnioLectivo_Periodo set "
                         + " IdUsuarioModificacion = " + info.IdUsuarioModificacion
                         + " FechaModificacion = BETWEEN DATEFROMPARTS(" + DateTime.Now.Year.ToString() + "," + DateTime.Now.Month.ToString() + "," + DateTime.Now.Day.ToString() + ")"
                         + " IdSucursal = " + info.IdSucursal.ToString()
                         + " IdPuntoVta = " + info.IdPuntoVta.ToString()
                         + " Procesado = " + true
                         + " FechaProceso = BETWEEN DATEFROMPARTS(" + DateTime.Now.Year.ToString() + "," + DateTime.Now.Month.ToString() + "," + DateTime.Now.Day.ToString() + ")"
                         + " TotalAlumnos = " + info.lst_det_fact_masiva.Count()
                         + " TotalValorFacturado = " + info.lst_det_fact_masiva.Sum(q => q.Total)
                         + " WHERE IdEmpresa = " + info.IdEmpresa.ToString() + " and IdAnio = " + info.IdAnio.ToString() + " and IdPeriodo = " + info.IdPeriodo.ToString();
                        command.ExecuteNonQuery();

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
