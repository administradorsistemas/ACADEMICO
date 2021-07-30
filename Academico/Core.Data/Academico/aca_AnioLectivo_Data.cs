using Core.Data.Base;
using Core.Data.SeguridadAcceso;
using Core.Info.Academico;
using Core.Info.Helps;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Academico
{
    public class aca_AnioLectivo_Data
    {
        aca_AnioLectivo_Periodo_Data odata_periodo = new aca_AnioLectivo_Periodo_Data();
        aca_AnioLectivo_Sede_NivelAcademico_Data odata_sede_nivel = new aca_AnioLectivo_Sede_NivelAcademico_Data();
        aca_AnioLectivo_NivelAcademico_Jornada_Data odata_nivel_jornada = new aca_AnioLectivo_NivelAcademico_Jornada_Data();
        aca_AnioLectivo_Jornada_Curso_Data odata_jornada_curso = new aca_AnioLectivo_Jornada_Curso_Data();
        aca_AnioLectivo_Curso_Paralelo_Data odata_curso_paralelo = new aca_AnioLectivo_Curso_Paralelo_Data();
        aca_AnioLectivo_Curso_Documento_Data odata_curso_documento = new aca_AnioLectivo_Curso_Documento_Data();
        aca_AnioLectivo_Curso_Materia_Data odata_curso_materia = new aca_AnioLectivo_Curso_Materia_Data();
        aca_AnioLectivo_Paralelo_Profesor_Data odata_paralelo_profesor = new aca_AnioLectivo_Paralelo_Profesor_Data();
        aca_Sede_Data odata_sede = new aca_Sede_Data();
        aca_NivelAcademico_Data odata_nivel = new aca_NivelAcademico_Data();
        aca_Jornada_Data odata_jornada = new aca_Jornada_Data();
        aca_Curso_Data odata_curso = new aca_Curso_Data();
        aca_Paralelo_Data odata_paralelo = new aca_Paralelo_Data();
        aca_Materia_Data odata_materia = new aca_Materia_Data();
        aca_Profesor_Data odata_profesor = new aca_Profesor_Data();
        aca_AnioLectivoCalificacionCualitativa_Data odata_calificacion_cualitativa = new aca_AnioLectivoCalificacionCualitativa_Data();
        aca_AnioLectivoConductaEquivalencia_Data odata_conducta_equivalencia = new aca_AnioLectivoConductaEquivalencia_Data();
        aca_AnioLectivoEquivalenciaPromedio_Data odata_equivalencia_promedio = new aca_AnioLectivoEquivalenciaPromedio_Data();
        aca_Plantilla_Data odata_plantilla = new aca_Plantilla_Data();
        aca_Plantilla_Rubro_Data odata_plantilla_rubro = new aca_Plantilla_Rubro_Data();
        aca_AnioLectivo_Curso_Plantilla_Data odata_curso_plantilla = new aca_AnioLectivo_Curso_Plantilla_Data();
        aca_AnioLectivo_Curso_Plantilla_Parametrizacion_Data odata_curso_plantilla_parametrizacion = new aca_AnioLectivo_Curso_Plantilla_Parametrizacion_Data();
        aca_AnioLectivo_Rubro_Data odata_anio_rubro = new aca_AnioLectivo_Rubro_Data();
        aca_AnioLectivo_Rubro_Periodo_Data odata_anio_rubro_periodo = new aca_AnioLectivo_Rubro_Periodo_Data();
        aca_Documento_Data odata_documento = new aca_Documento_Data();
        seg_usuario_Data odata_usuario = new seg_usuario_Data();
        aca_AnioLectivoParcial_Data odata_parcial = new aca_AnioLectivoParcial_Data();
        aca_Catalogo_Data odata_catalogo = new aca_Catalogo_Data();
        public List<aca_AnioLectivo_Info> getList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                List<aca_AnioLectivo_Info> Lista = new List<aca_AnioLectivo_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    #region Query
                    string query = "SELECT * FROM aca_AnioLectivo a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString();
                    if (MostrarAnulados == false)
                    {
                        query += " and a.Estado = 1";
                    }
                    query += " order by a.Descripcion";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_AnioLectivo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            EnCurso = Convert.ToBoolean(reader["EnCurso"]),
                            BloquearMatricula = Convert.ToBoolean(reader["BloquearMatricula"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdAnioLectivoAnterior = string.IsNullOrEmpty(reader["IdAnioLectivoAnterior"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdAnioLectivoAnterior"]),
                            PromedioMinimoParcial = string.IsNullOrEmpty(reader["PromedioMinimoParcial"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoParcial"]),
                            PromedioMinimoPromocion = string.IsNullOrEmpty(reader["PromedioMinimoPromocion"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoPromocion"]),
                            IdCursoBachiller = string.IsNullOrEmpty(reader["IdCursoBachiller"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCursoBachiller"])
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

        public List<aca_AnioLectivo_Info> getList_Matricula(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                List<aca_AnioLectivo_Info> Lista = new List<aca_AnioLectivo_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    #region Query
                    string query = "SELECT * FROM aca_AnioLectivo a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and a.BloquearMatricula=0 ";
                    if (MostrarAnulados == false)
                    {
                        query += " and a.Estado = 1";
                    }
                    query += " order by a.Descripcion";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_AnioLectivo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            EnCurso = Convert.ToBoolean(reader["EnCurso"]),
                            BloquearMatricula = Convert.ToBoolean(reader["BloquearMatricula"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdAnioLectivoAnterior = string.IsNullOrEmpty(reader["IdAnioLectivoAnterior"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdAnioLectivoAnterior"]),
                            PromedioMinimoParcial = string.IsNullOrEmpty(reader["PromedioMinimoParcial"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoParcial"]),
                            PromedioMinimoPromocion = string.IsNullOrEmpty(reader["PromedioMinimoPromocion"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoPromocion"]),
                            IdCursoBachiller = string.IsNullOrEmpty(reader["IdCursoBachiller"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCursoBachiller"]),
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
        public List<aca_AnioLectivo_Info> getList_Admision(int IdEmpresa)
        {
            try
            {
                List<aca_AnioLectivo_Info> Lista = new List<aca_AnioLectivo_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    #region Query
                    string query = "SELECT * FROM aca_AnioLectivo a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and a.BloquearMatricula=0 "
                    + " and FechaHasta=(select MAX(FechaHasta) FechaMaxima from aca_AnioLectivo)";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_AnioLectivo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            EnCurso = Convert.ToBoolean(reader["EnCurso"]),
                            BloquearMatricula = Convert.ToBoolean(reader["BloquearMatricula"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdAnioLectivoAnterior = string.IsNullOrEmpty(reader["IdAnioLectivoAnterior"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdAnioLectivoAnterior"]),
                            PromedioMinimoParcial = string.IsNullOrEmpty(reader["PromedioMinimoParcial"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoParcial"]),
                            PromedioMinimoPromocion = string.IsNullOrEmpty(reader["PromedioMinimoPromocion"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoPromocion"]),
                            IdCursoBachiller = string.IsNullOrEmpty(reader["IdCursoBachiller"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCursoBachiller"]),
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
        public aca_AnioLectivo_Info getInfo(int IdEmpresa, int IdAnio)
        {
            try
            {
                aca_AnioLectivo_Info info = new aca_AnioLectivo_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_AnioLectivo a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and a.IdAnio = " + IdAnio.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_AnioLectivo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            EnCurso = Convert.ToBoolean(reader["EnCurso"]),
                            BloquearMatricula = Convert.ToBoolean(reader["BloquearMatricula"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdAnioLectivoAnterior = string.IsNullOrEmpty(reader["IdAnioLectivoAnterior"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdAnioLectivoAnterior"]),
                            PromedioMinimoParcial = string.IsNullOrEmpty(reader["PromedioMinimoParcial"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoParcial"]),
                            PromedioMinimoPromocion = string.IsNullOrEmpty(reader["PromedioMinimoPromocion"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoPromocion"]),
                            CalificacionMaxima = string.IsNullOrEmpty(reader["CalificacionMaxima"].ToString()) ? (double?)null : Convert.ToInt32(reader["CalificacionMaxima"]),
                            IdCursoBachiller = string.IsNullOrEmpty(reader["IdCursoBachiller"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCursoBachiller"]),
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
        public List<aca_AnioLectivo_Info> getList_update(int IdEmpresa)
        {
            try
            {
                List<aca_AnioLectivo_Info> Lista = new List<aca_AnioLectivo_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    #region Query
                    string query = "SELECT * FROM aca_AnioLectivo a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and a.Estado = 1 and a.BloquearMatricula = 0";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_AnioLectivo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            EnCurso = Convert.ToBoolean(reader["EnCurso"]),
                            BloquearMatricula = Convert.ToBoolean(reader["BloquearMatricula"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdAnioLectivoAnterior = string.IsNullOrEmpty(reader["IdAnioLectivoAnterior"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdAnioLectivoAnterior"]),
                            PromedioMinimoParcial = string.IsNullOrEmpty(reader["PromedioMinimoParcial"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoParcial"]),
                            PromedioMinimoPromocion = string.IsNullOrEmpty(reader["PromedioMinimoPromocion"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoPromocion"]),
                            IdCursoBachiller = string.IsNullOrEmpty(reader["IdCursoBachiller"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCursoBachiller"]),
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

        public aca_AnioLectivo_Info getInfo_AnioAdmision(int IdEmpresa)
        {
            try
            {
                aca_AnioLectivo_Info info = new aca_AnioLectivo_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_AnioLectivo a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and a.BloquearMatricula=0 "
                    + " and FechaHasta=(select MAX(FechaHasta) FechaMaxima from aca_AnioLectivo)";
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        info = new aca_AnioLectivo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            EnCurso = Convert.ToBoolean(reader["EnCurso"]),
                            BloquearMatricula = Convert.ToBoolean(reader["BloquearMatricula"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdAnioLectivoAnterior = string.IsNullOrEmpty(reader["IdAnioLectivoAnterior"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdAnioLectivoAnterior"]),
                            PromedioMinimoParcial = string.IsNullOrEmpty(reader["PromedioMinimoParcial"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoParcial"]),
                            PromedioMinimoPromocion = string.IsNullOrEmpty(reader["PromedioMinimoPromocion"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoPromocion"]),
                            CalificacionMaxima = string.IsNullOrEmpty(reader["CalificacionMaxima"].ToString()) ? (double?)null : Convert.ToInt32(reader["CalificacionMaxima"]),
                            IdCursoBachiller = string.IsNullOrEmpty(reader["IdCursoBachiller"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCursoBachiller"])
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

        public aca_AnioLectivo_Info getInfo_x_Anio(int IdEmpresa, int AnioIni, int AnioFin)
        {
            try
            {
                aca_AnioLectivo_Info info = new aca_AnioLectivo_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_AnioLectivo a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and YEAR(a.FechaDesde) = " + AnioIni.ToString() + " and YEAR(a.FechaHasta)= "+ AnioFin.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_AnioLectivo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            EnCurso = Convert.ToBoolean(reader["EnCurso"]),
                            BloquearMatricula = Convert.ToBoolean(reader["BloquearMatricula"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdAnioLectivoAnterior = string.IsNullOrEmpty(reader["IdAnioLectivoAnterior"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdAnioLectivoAnterior"]),
                            PromedioMinimoParcial = string.IsNullOrEmpty(reader["PromedioMinimoParcial"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoParcial"]),
                            PromedioMinimoPromocion = string.IsNullOrEmpty(reader["PromedioMinimoPromocion"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoPromocion"]),
                            CalificacionMaxima = string.IsNullOrEmpty(reader["CalificacionMaxima"].ToString()) ? (double?)null : Convert.ToInt32(reader["CalificacionMaxima"]),
                            IdCursoBachiller = string.IsNullOrEmpty(reader["IdCursoBachiller"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCursoBachiller"])
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

        public aca_AnioLectivo_Info getInfo_AnioAnterior(int IdEmpresa, int Anio)
        {
            try
            {
                aca_AnioLectivo_Info info = new aca_AnioLectivo_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_AnioLectivo a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and YEAR(a.FechaDesde) = " + Anio.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_AnioLectivo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            EnCurso = Convert.ToBoolean(reader["EnCurso"]),
                            BloquearMatricula = Convert.ToBoolean(reader["BloquearMatricula"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdAnioLectivoAnterior = string.IsNullOrEmpty(reader["IdAnioLectivoAnterior"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdAnioLectivoAnterior"]),
                            PromedioMinimoParcial = string.IsNullOrEmpty(reader["PromedioMinimoParcial"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoParcial"]),
                            PromedioMinimoPromocion = string.IsNullOrEmpty(reader["PromedioMinimoPromocion"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoPromocion"]),
                            CalificacionMaxima = string.IsNullOrEmpty(reader["CalificacionMaxima"].ToString()) ? (double?)null : Convert.ToInt32(reader["CalificacionMaxima"]),
                            IdCursoBachiller = string.IsNullOrEmpty(reader["IdCursoBachiller"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCursoBachiller"])
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

        public aca_AnioLectivo_Info getInfo_AnioEnCurso(int IdEmpresa, int IdAnio)
        {
            try
            {
                aca_AnioLectivo_Info info = new aca_AnioLectivo_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_AnioLectivo a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and a.Estado = 1 and a.EnCurso=1";
                    if (IdAnio==0)
                    {
                        //command.CommandText += " and a.IdAnio = " + IdAnio.ToString();
                    }
                    else
                    {
                        command.CommandText += " and a.IdAnio != " + IdAnio.ToString();
                    }
                    
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_AnioLectivo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaDesde = Convert.ToDateTime(reader["FechaDesde"]),
                            FechaHasta = Convert.ToDateTime(reader["FechaHasta"]),
                            EnCurso = Convert.ToBoolean(reader["EnCurso"]),
                            BloquearMatricula = Convert.ToBoolean(reader["BloquearMatricula"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdAnioLectivoAnterior = string.IsNullOrEmpty(reader["IdAnioLectivoAnterior"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdAnioLectivoAnterior"]),
                            PromedioMinimoParcial = string.IsNullOrEmpty(reader["PromedioMinimoParcial"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoParcial"]),
                            PromedioMinimoPromocion = string.IsNullOrEmpty(reader["PromedioMinimoPromocion"].ToString()) ? (double?)null : Convert.ToInt32(reader["PromedioMinimoPromocion"]),
                            CalificacionMaxima = string.IsNullOrEmpty(reader["CalificacionMaxima"].ToString()) ? (double?)null : Convert.ToInt32(reader["CalificacionMaxima"]),
                            IdCursoBachiller = string.IsNullOrEmpty(reader["IdCursoBachiller"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCursoBachiller"])
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
                    var cont = Context.aca_AnioLectivo.Where(q => q.IdEmpresa == IdEmpresa).Count();
                    if (cont > 0)
                        ID = Context.aca_AnioLectivo.Where(q => q.IdEmpresa == IdEmpresa).Max(q => q.IdAnio) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(aca_AnioLectivo_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var IdPeriodo = 0;
                    aca_AnioLectivo Entity = new aca_AnioLectivo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdAnio = info.IdAnio = getId(info.IdEmpresa),
                        Descripcion = info.Descripcion,
                        FechaDesde = info.FechaDesde,
                        FechaHasta = info.FechaHasta,
                        EnCurso = info.EnCurso,
                        BloquearMatricula = info.BloquearMatricula,
                        IdAnioLectivoAnterior = info.IdAnioLectivoAnterior,
                        PromedioMinimoParcial = info.PromedioMinimoParcial,
                        PromedioMinimoPromocion = info.PromedioMinimoPromocion,
                        IdCursoBachiller = info.IdCursoBachiller,
                        CalificacionMaxima = info.CalificacionMaxima,
                        Estado = true,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = info.FechaCreacion = DateTime.Now
                    };
                    Context.aca_AnioLectivo.Add(Entity);

                    if (info.lst_periodos.Count >0)
                    {
                        IdPeriodo = odata_periodo.getId(info.IdEmpresa);
                        foreach (var item in info.lst_periodos)
                        {
                            aca_AnioLectivo_Periodo Entity_Periodo = new aca_AnioLectivo_Periodo
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdPeriodo = IdPeriodo++,
                                IdAnio = info.IdAnio,
                                IdMes = item.IdMes,
                                FechaDesde = item.FechaDesde,
                                FechaHasta = item.FechaHasta,
                                FechaProntoPago = null,
                                Estado = true,
                                IdUsuarioCreacion = info.IdUsuarioCreacion,
                                FechaCreacion = info.FechaCreacion = DateTime.Now

                            };
                                Context.aca_AnioLectivo_Periodo.Add(Entity_Periodo);
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

        public bool modificarDB(aca_AnioLectivo_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_AnioLectivo Entity = Context.aca_AnioLectivo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAnio == info.IdAnio);
                    if (Entity == null)
                        return false;

                    Entity.Descripcion = info.Descripcion;
                    Entity.FechaDesde = info.FechaDesde;
                    Entity.FechaHasta = info.FechaHasta;
                    Entity.EnCurso = info.EnCurso;
                    Entity.BloquearMatricula = info.BloquearMatricula;
                    Entity.IdAnioLectivoAnterior = info.IdAnioLectivoAnterior;
                    Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
                    Entity.PromedioMinimoParcial = info.PromedioMinimoParcial;
                    Entity.PromedioMinimoPromocion = info.PromedioMinimoPromocion;
                    Entity.IdCursoBachiller = info.IdCursoBachiller;
                    Entity.CalificacionMaxima = info.CalificacionMaxima;
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

        public bool anularDB(aca_AnioLectivo_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_AnioLectivo Entity = Context.aca_AnioLectivo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAnio == info.IdAnio);
                    if (Entity == null)
                        return false;
                    Entity.Estado = info.Estado = false;
                    Entity.MotivoAnulacion = info.MotivoAnulacion;
                    Entity.IdUsuarioAnulacion = info.IdUsuarioAnulacion;
                    Entity.FechaAnulacion = info.FechaAnulacion = DateTime.Now;
                    Entity.EnCurso = false;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarAperturaDB(aca_AnioLectivo_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var lst_calificaciones = new List<aca_AnioLectivoParcial_Info>();
                    var lst_quim1 = odata_catalogo.getList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1), false);
                    var lst_quim2 = odata_catalogo.getList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2), false);
                    var lst_examenes = odata_catalogo.getList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.EXSUP), false);

                    foreach (var item in lst_quim1)
                    {
                        lst_calificaciones.Add(new aca_AnioLectivoParcial_Info
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdSede = info.IdSede,
                            IdAnio = info.IdAnioApertura,
                            IdCatalogoParcial = item.IdCatalogo,
                            EsExamen = false,
                            ValidaEstadoAlumno = false
                        });
                    }

                    foreach (var item in lst_quim2)
                    {
                        lst_calificaciones.Add(new aca_AnioLectivoParcial_Info
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdSede = info.IdSede,
                            IdAnio = info.IdAnioApertura,
                            IdCatalogoParcial = item.IdCatalogo,
                            EsExamen = false,
                            ValidaEstadoAlumno = false
                        });
                    }
                    foreach (var item in lst_examenes)
                    {
                        var ValidaEstadoAlumno = (item.IdCatalogo == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXQUI2) || item.IdCatalogo == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXMEJ) || item.IdCatalogo == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXSUP) || item.IdCatalogo == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXREM) || item.IdCatalogo == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXGRA) ? true : false);
                        lst_calificaciones.Add(new aca_AnioLectivoParcial_Info
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdSede = info.IdSede,
                            IdAnio = info.IdAnioApertura,
                            IdCatalogoParcial = item.IdCatalogo,
                            EsExamen = true,
                            ValidaEstadoAlumno = ValidaEstadoAlumno
                        });
                    }

                    if (lst_calificaciones.Count>0)
                    {
                        foreach (var item in lst_calificaciones)
                        {
                            aca_AnioLectivoParcial Entity_AnioLectivoParcial = new aca_AnioLectivoParcial
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdSede = item.IdSede,
                                IdAnio = item.IdAnio,
                                IdCatalogoParcial = item.IdCatalogoParcial,
                                EsExamen = item.EsExamen,
                                ValidaEstadoAlumno = item.ValidaEstadoAlumno,
                                IdUsuarioCreacion = info.IdUsuarioCreacion,
                                FechaCreacion = DateTime.Now
                            };
                            Context.aca_AnioLectivoParcial.Add(Entity_AnioLectivoParcial);
                        }
                    }

                    var lst_equivalencia_promedio_x_anio = odata_equivalencia_promedio.getList(info.IdEmpresa,info.IdAnio,false);
                    if (lst_equivalencia_promedio_x_anio.Count > 0)
                    {
                        var IdEquivalenciaPromedio = odata_equivalencia_promedio.getId(info.IdEmpresa);
                        foreach (var item in lst_equivalencia_promedio_x_anio)
                        {
                            aca_AnioLectivoEquivalenciaPromedio Entity_EquivalenciaPromedio = new aca_AnioLectivoEquivalenciaPromedio
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdEquivalenciaPromedio = IdEquivalenciaPromedio++,
                                IdAnio = info.IdAnioApertura,
                                Descripcion = item.Descripcion,
                                Codigo = item.Codigo,
                                ValorMinimo = item.ValorMinimo,
                                ValorMaximo = item.ValorMaximo,
                                Estado = item.Estado,
                                IdUsuarioCreacion = info.IdUsuarioCreacion,
                                FechaCreacion = DateTime.Now
                            };
                            Context.aca_AnioLectivoEquivalenciaPromedio.Add(Entity_EquivalenciaPromedio);
                        }
                    }

                    var lst_equivalencia_cualitativa_x_anio = odata_calificacion_cualitativa.getList(info.IdEmpresa, info.IdAnio, false);
                    if (lst_equivalencia_cualitativa_x_anio.Count > 0)
                    {
                        var IdEquivalenciaPromedio = odata_calificacion_cualitativa.getId(info.IdEmpresa);
                        foreach (var item in lst_equivalencia_cualitativa_x_anio)
                        {
                            aca_AnioLectivoCalificacionCualitativa Entity_EquivalenciaCualitativa = new aca_AnioLectivoCalificacionCualitativa
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdCalificacionCualitativa = IdEquivalenciaPromedio++,
                                IdAnio = info.IdAnioApertura,
                                DescripcionCorta = item.DescripcionCorta,
                                DescripcionLarga = item.DescripcionLarga,
                                Codigo = item.Codigo,
                                Calificacion = (item.Calificacion ==null) ? (decimal?)null : item.Calificacion,
                                Estado = item.Estado,
                                IdUsuarioCreacion = info.IdUsuarioCreacion,
                                FechaCreacion = DateTime.Now
                            };
                            Context.aca_AnioLectivoCalificacionCualitativa.Add(Entity_EquivalenciaCualitativa);
                        }
                    }

                    var lst_equivalencia_conducta_x_anio = odata_conducta_equivalencia.getList(info.IdEmpresa, info.IdAnio, false);
                    if (lst_equivalencia_conducta_x_anio.Count > 0)
                    {
                        var Secuencia = 1;
                        foreach (var item in lst_equivalencia_conducta_x_anio)
                        {
                            aca_AnioLectivoConductaEquivalencia Entity_EquivalenciaConducta = new aca_AnioLectivoConductaEquivalencia
                            {
                                IdEmpresa = item.IdEmpresa,
                                Secuencia = Secuencia++,
                                IdAnio = info.IdAnioApertura,
                                Equivalencia = item.Equivalencia,
                                DescripcionEquivalencia = item.DescripcionEquivalencia,
                                Letra = item.Letra,
                                Calificacion = item.Calificacion,
                                IngresaInspector = (item.IngresaInspector==null) ? false : item.IngresaInspector,
                                IngresaMotivo = (item.IngresaMotivo == null) ? false : item.IngresaMotivo,
                                IngresaProfesor = (item.IngresaProfesor == null) ? false : item.IngresaProfesor
                            };
                            Context.aca_AnioLectivoConductaEquivalencia.Add(Entity_EquivalenciaConducta);
                        }
                    }

                    //RUBROS//
                    var lst_rubros_x_anio = odata_anio_rubro.getList(info.IdEmpresa, info.IdAnio, false);
                    if (lst_rubros_x_anio.Count > 0)
                    {
                        foreach (var item in lst_rubros_x_anio)
                        {
                            aca_AnioLectivo_Rubro Entity_AnioRubro = new aca_AnioLectivo_Rubro
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdAnio = info.IdAnioApertura,
                                IdRubro = item.IdRubro,
                                AplicaProntoPago = item.AplicaProntoPago,
                                NomRubro = item.NomRubro,
                                IdProducto = item.IdProducto,
                                Subtotal = item.Subtotal,
                                IdCod_Impuesto_Iva = item.IdCod_Impuesto_Iva,
                                Porcentaje = item.Porcentaje,
                                ValorIVA = item.ValorIVA,
                                Total = item.Total,
                                NumeroCuotas = item.NumeroCuotas
                            };
                            Context.aca_AnioLectivo_Rubro.Add(Entity_AnioRubro);

                            var lst_periodo_anio_anterior = odata_periodo.getList(info.IdEmpresa, info.IdAnio, false).OrderBy(q => q.FechaDesde).ToList();
                            var OrdenAnterior = 1;
                            lst_periodo_anio_anterior.ForEach(q=>q.Orden = OrdenAnterior++);

                            var lst_periodo_anio_apertura = odata_periodo.getList(info.IdEmpresa, info.IdAnioApertura, false).OrderBy(q => q.FechaDesde).ToList();
                            var OrdenApertura = 1;
                            lst_periodo_anio_apertura.ForEach(q => q.Orden = OrdenApertura++);

                            #region DATATABLE PERIODO ANTERIOR Y APERTURA
                            DataTable tabla = new DataTable();

                            // Variables para las columnas y las filas
                            DataColumn column_periodo;
                            DataRow row_periodo;

                            // Se tiene que crear primero la columna asignandole Nombre y Tipo de datos 
                            column_periodo = new DataColumn();
                            column_periodo.DataType = System.Type.GetType("System.Int32");
                            column_periodo.ColumnName = "IdAnioAnterior";
                            tabla.Columns.Add(column_periodo);

                            column_periodo = new DataColumn();
                            column_periodo.DataType = System.Type.GetType("System.Int32");
                            column_periodo.ColumnName = "IdPeriodoAnterior";
                            tabla.Columns.Add(column_periodo);

                            column_periodo = new DataColumn();
                            column_periodo.DataType = System.Type.GetType("System.Int32");
                            column_periodo.ColumnName = "OrdenAnterior";
                            tabla.Columns.Add(column_periodo);

                            column_periodo = new DataColumn();
                            column_periodo.DataType = System.Type.GetType("System.Int32");
                            column_periodo.ColumnName = "IdAnioApertura";
                            tabla.Columns.Add(column_periodo);

                            column_periodo = new DataColumn();
                            column_periodo.DataType = System.Type.GetType("System.Int32");
                            column_periodo.ColumnName = "IdPeriodoApertura";
                            tabla.Columns.Add(column_periodo);

                            column_periodo = new DataColumn();
                            column_periodo.DataType = System.Type.GetType("System.Int32");
                            column_periodo.ColumnName = "OrdenApertura";
                            tabla.Columns.Add(column_periodo);

                            foreach (var item_periodo in lst_periodo_anio_apertura)
                            {
                                var periodo_anterior = lst_periodo_anio_anterior.Where(q => q.Orden == item_periodo.Orden).FirstOrDefault();

                                if (periodo_anterior!=null)
                                {
                                    row_periodo = tabla.NewRow();
                                    row_periodo["IdAnioAnterior"] = periodo_anterior.IdAnio;
                                    row_periodo["IdPeriodoAnterior"] = periodo_anterior.IdPeriodo;
                                    row_periodo["OrdenAnterior"] = periodo_anterior.Orden;
                                    row_periodo["IdAnioApertura"] = item_periodo.IdAnio;
                                    row_periodo["IdPeriodoApertura"] = item_periodo.IdPeriodo;
                                    row_periodo["OrdenApertura"] = item_periodo.Orden;

                                    tabla.Rows.Add(row_periodo);
                                }
                            }
                            #endregion
   
                            var periodo_actual = tabla;
                            var lst_rubros_anioanterior_periodos = odata_anio_rubro_periodo.get_list_asignacion(info.IdEmpresa, info.IdAnio, item.IdRubro).Where(q=> q.seleccionado==true).ToList();

                            if (lst_rubros_anioanterior_periodos.Count > 0)
                            {
                                var SecuenciaRP = 1;
                                var IdPeriodoApertura = 0;
                                foreach (var item_p in lst_rubros_anioanterior_periodos)
                                {
                                    foreach (DataRow fila in tabla.Rows)
                                    {
                                        if (Convert.ToInt32(fila["IdAnioAnterior"]) == item_p.IdAnio && Convert.ToInt32(fila["IdPeriodoAnterior"]) == item_p.IdPeriodo)
                                        {
                                            IdPeriodoApertura = Convert.ToInt32(fila["IdPeriodoApertura"]);
                                            break;
                                        }
                                    }

                                    aca_AnioLectivo_Rubro_Periodo Entity_AnioRubro_Periodo = new aca_AnioLectivo_Rubro_Periodo
                                    {
                                        IdEmpresa = item.IdEmpresa,
                                        IdAnio = info.IdAnioApertura,
                                        IdRubro = item.IdRubro,
                                        IdPeriodo = IdPeriodoApertura,
                                        Secuencia = SecuenciaRP++,
                                        Observacion = item_p.Observacion
                                    };
                                    Context.aca_AnioLectivo_Rubro_Periodo.Add(Entity_AnioRubro_Periodo);
                        
                                }
                            }
                        }
                    }


                    #region COLUMNAS DATATABLE PLANTILLA ANIO ANTERIOR Y APERTURA
                    DataTable tabla_plantilla = new DataTable();

                    // Variables para las columnas y las filas
                    DataColumn column;
                    DataRow row;

                    // Se tiene que crear primero la columna asignandole Nombre y Tipo de datos 
                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int32");
                    column.ColumnName = "IdAnioAnterior";
                    tabla_plantilla.Columns.Add(column);

                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int32");
                    column.ColumnName = "IdPlantillaAnterior";
                    tabla_plantilla.Columns.Add(column);

                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int32");
                    column.ColumnName = "IdAnioApertura";
                    tabla_plantilla.Columns.Add(column);

                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int32");
                    column.ColumnName = "IdPlantillaApertura";
                    tabla_plantilla.Columns.Add(column);
                    #endregion

                    // PLANTILLAS POR AÑO
                    var lst_plantillas_x_año = odata_plantilla.getList(info.IdEmpresa, info.IdAnio, false);
                    if (lst_plantillas_x_año.Count > 0)
                    {
                        var IdPlantilla = odata_plantilla.getId(info.IdEmpresa);
                        foreach (var item in lst_plantillas_x_año)
                        {
                            #region COLUMNAS DATATABLE PLANTILLA ANIO ANTERIOR Y APERTURA
                            row = tabla_plantilla.NewRow();
                            row["IdAnioAnterior"] = item.IdAnio;
                            row["IdPlantillaAnterior"] = item.IdPlantilla;
                            row["IdAnioApertura"] = info.IdAnioApertura;
                            row["IdPlantillaApertura"] = IdPlantilla;

                            tabla_plantilla.Rows.Add(row);
                            #endregion

                            aca_Plantilla Entity_Plantilla = new aca_Plantilla
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdAnio = info.IdAnioApertura,
                                IdPlantilla = IdPlantilla,
                                NomPlantilla = item.NomPlantilla,
                                Valor = item.Valor,
                                TipoDescuento = item.TipoDescuento,
                                Estado = item.Estado,
                                IdUsuarioCreacion = info.IdUsuarioCreacion,
                                FechaCreacion = DateTime.Now,
                                IdTipoPlantilla = (item.IdTipoPlantilla==null) ? (int?)null : item.IdTipoPlantilla,
                                IdTipoNota = item.IdTipoNota,
                                AplicaParaTodo = (item.AplicaParaTodo == null) ? false : item.AplicaParaTodo
                            };
                            Context.aca_Plantilla.Add(Entity_Plantilla);

                            var lst_plantilla_x_anio_rubro = odata_plantilla_rubro.getList(info.IdEmpresa, info.IdAnio, item.IdPlantilla);
                            if (lst_plantilla_x_anio_rubro.Count>0)
                            {
                                foreach (var item_r in lst_plantilla_x_anio_rubro)
                                {
                                    aca_Plantilla_Rubro Entity_PlantillaRubro = new aca_Plantilla_Rubro
                                    {
                                        IdEmpresa = item.IdEmpresa,
                                        IdAnio = info.IdAnioApertura,
                                        IdPlantilla = IdPlantilla,
                                        IdRubro = item_r.IdRubro,
                                        IdProducto = item_r.IdProducto,
                                        Subtotal = item_r.Subtotal,
                                        IdCod_Impuesto_Iva = item_r.IdCod_Impuesto_Iva,
                                        Porcentaje = item_r.Porcentaje,
                                        ValorIVA = item_r.ValorIVA,
                                        Total = item_r.Total,
                                        TipoDescuento_descuentoDet = item_r.TipoDescuento_descuentoDet,
                                        Valor_descuentoDet = item_r.Valor_descuentoDet,
                                        IdTipoNota_descuentoDet = (item_r.IdTipoNota_descuentoDet == null) ? (int?)null : item_r.IdTipoNota_descuentoDet
                                    };
                                    Context.aca_Plantilla_Rubro.Add(Entity_PlantillaRubro);
                                }
                            }
                            IdPlantilla++;
                        }
                    }

                    var lst_asignacion_sede_nivel = odata_sede_nivel.get_list_asignacion(info.IdEmpresa, info.IdSede, info.IdAnio).Where(q=> q.seleccionado==true).ToList();
                    if (lst_asignacion_sede_nivel.Count > 0)
                    {
                        #region SedeNivel
                        foreach (var item_sn in lst_asignacion_sede_nivel)
                        {
                            var info_sede = odata_sede.GetInfo(info.IdEmpresa, item_sn.IdSede);
                            var info_nivel = odata_nivel.getInfo(info.IdEmpresa, item_sn.IdNivel);
                            aca_AnioLectivo_Sede_NivelAcademico Entity_SedeNivel = new aca_AnioLectivo_Sede_NivelAcademico
                            {
                                IdEmpresa = item_sn.IdEmpresa,
                                IdAnio = info.IdAnioApertura,
                                IdSede = item_sn.IdSede,
                                IdNivel = item_sn.IdNivel,
                                NomNivel = info_nivel.NomNivel,
                                NomSede = info_sede.NomSede
                            };
                            Context.aca_AnioLectivo_Sede_NivelAcademico.Add(Entity_SedeNivel);
                        }
                        #endregion

                        var lst_nivel = lst_asignacion_sede_nivel.GroupBy(q => q.IdNivel).ToList();
                        if (lst_nivel.Count > 0)
                        {
                            foreach (var item_niv in lst_nivel)
                            {
                                var lst_asignacion_nivel_jornada = odata_nivel_jornada.get_list_asignacion(info.IdEmpresa, info.IdSede, info.IdAnio, Convert.ToInt32(item_niv.Key)).Where(q => q.seleccionado == true).ToList();

                                foreach (var item_nj in lst_asignacion_nivel_jornada)
                                {
                                    var info_jornada = odata_jornada.getInfo(info.IdEmpresa, item_nj.IdJornada);
                                    aca_AnioLectivo_NivelAcademico_Jornada Entity_NivelJornada = new aca_AnioLectivo_NivelAcademico_Jornada
                                    {
                                        IdEmpresa = item_nj.IdEmpresa,
                                        IdAnio = info.IdAnioApertura,
                                        IdSede = item_nj.IdSede,
                                        IdNivel = item_nj.IdNivel,
                                        IdJornada = item_nj.IdJornada,
                                        NomJornada = info_jornada.NomJornada,
                                        OrdenJornada = info_jornada.OrdenJornada
                                    };
                                    Context.aca_AnioLectivo_NivelAcademico_Jornada.Add(Entity_NivelJornada);
                                }

                                var lst_jornada = lst_asignacion_nivel_jornada.GroupBy(q => q.IdJornada).ToList();
                                if (lst_jornada.Count > 0)
                                {
                                    foreach (var item_jor in lst_jornada)
                                    {
                                        var lst_asignacion_jornada_curso = odata_jornada_curso.get_list_asignacion(info.IdEmpresa, info.IdSede, info.IdAnio, Convert.ToInt32(item_niv.Key), Convert.ToInt32(item_jor.Key)).Where(q => q.seleccionado == true).ToList();

                                        foreach (var item_jc in lst_asignacion_jornada_curso)
                                        {
                                            var info_curso = odata_curso.getInfo(info.IdEmpresa, item_jc.IdCurso);
                                            aca_AnioLectivo_Jornada_Curso Entity_JornadaCurso = new aca_AnioLectivo_Jornada_Curso
                                            {
                                                IdEmpresa = item_jc.IdEmpresa,
                                                IdAnio = info.IdAnioApertura,
                                                IdSede = item_jc.IdSede,
                                                IdNivel = item_jc.IdNivel,
                                                IdJornada = item_jc.IdJornada,
                                                IdCurso = item_jc.IdCurso,
                                                NomCurso = info_curso.NomCurso,
                                                OrdenCurso = info_curso.OrdenCurso
                                            };
                                            Context.aca_AnioLectivo_Jornada_Curso.Add(Entity_JornadaCurso);
                                        }

                                        var lst_curso = lst_asignacion_jornada_curso.GroupBy(q => q.IdCurso).ToList();
                                        if (lst_curso.Count > 0)
                                        {
                                            foreach (var item_cur in lst_curso)
                                            {
                                                var lst_asignacion_curso_paralelo = odata_curso_paralelo.get_list_asignacion(info.IdEmpresa, info.IdSede, info.IdAnio, Convert.ToInt32(item_niv.Key), Convert.ToInt32(item_jor.Key), Convert.ToInt32(item_cur.Key)).Where(q => q.seleccionado == true).ToList();

                                                foreach (var item_cp in lst_asignacion_curso_paralelo)
                                                {
                                                    var inf_paralelo = odata_paralelo.getInfo(info.IdEmpresa, item_cp.IdParalelo);
                                                    var info_profesor_insp = odata_profesor.getInfo(info.IdEmpresa, Convert.ToDecimal(item_cp.IdProfesorInspector));
                                                    var info_profesor_tutor = odata_profesor.getInfo(info.IdEmpresa, Convert.ToDecimal(item_cp.IdProfesorTutor));

                                                    aca_AnioLectivo_Curso_Paralelo Entity_CursoParalelo = new aca_AnioLectivo_Curso_Paralelo
                                                    {
                                                        IdEmpresa = item_cp.IdEmpresa,
                                                        IdAnio = info.IdAnioApertura,
                                                        IdSede = item_cp.IdSede,
                                                        IdNivel = item_cp.IdNivel,
                                                        IdJornada = item_cp.IdJornada,
                                                        IdCurso = item_cp.IdCurso,
                                                        IdParalelo = item_cp.IdParalelo,
                                                        CodigoParalelo = inf_paralelo.CodigoParalelo,
                                                        NomParalelo = inf_paralelo.NomParalelo,
                                                        OrdenParalelo = inf_paralelo.OrdenParalelo,
                                                        IdProfesorInspector = (info_profesor_insp==null ? (decimal?)null : (info_profesor_insp.Estado==false ? (decimal?)null : item_cp.IdProfesorInspector)),
                                                        IdProfesorTutor = (info_profesor_tutor == null ? (decimal?)null : (info_profesor_tutor.Estado == false ? (decimal?)null : item_cp.IdProfesorInspector)),
                                                    };
                                                    Context.aca_AnioLectivo_Curso_Paralelo.Add(Entity_CursoParalelo);
                                                }

                                                var lst_asignacion_curso_materia = odata_curso_materia.get_list_asignacion(info.IdEmpresa, info.IdSede, info.IdAnio, Convert.ToInt32(item_niv.Key), Convert.ToInt32(item_jor.Key), Convert.ToInt32(item_cur.Key)).Where(q => q.seleccionado == true).ToList();
                                                if (lst_asignacion_curso_materia.Count > 0)
                                                {
                                                    foreach (var item_cm in lst_asignacion_curso_materia)
                                                    {
                                                        var info_materia = odata_materia.getInfo(info.IdEmpresa, item_cm.IdMateria);

                                                        aca_AnioLectivo_Curso_Materia Entity_CursoMateria = new aca_AnioLectivo_Curso_Materia
                                                        {
                                                            IdEmpresa = item_cm.IdEmpresa,
                                                            IdAnio = info.IdAnioApertura,
                                                            IdSede = item_cm.IdSede,
                                                            IdNivel = item_cm.IdNivel,
                                                            IdJornada = item_cm.IdJornada,
                                                            IdCurso = item_cm.IdCurso,
                                                            IdMateria = item_cm.IdMateria,
                                                            NomMateria = info_materia.NomMateria,
                                                            NomMateriaArea = info_materia.NomMateriaArea,
                                                            NomMateriaGrupo = info_materia.NomMateriaGrupo,
                                                            EsObligatorio = info_materia.EsObligatorio,
                                                            OrdenMateria = info_materia.OrdenMateria,
                                                            OrdenMateriaArea = (info_materia.OrdenMateriaArea==null) ? (int?)null : info_materia.OrdenMateriaArea,
                                                            OrdenMateriaGrupo = (info_materia.OrdenMateriaGrupo == null) ? (int?)null : info_materia.OrdenMateriaGrupo,
                                                            IdCatalogoTipoCalificacion = (info_materia.IdCatalogoTipoCalificacion == null) ? (int?)null : info_materia.IdCatalogoTipoCalificacion
                                                        };
                                                        Context.aca_AnioLectivo_Curso_Materia.Add(Entity_CursoMateria);
                                                    }
                                                }

                                                var lst_asignacion_curso_documento = odata_curso_documento.get_list_asignacion(info.IdEmpresa, info.IdSede, info.IdAnio, Convert.ToInt32(item_niv.Key), Convert.ToInt32(item_jor.Key), Convert.ToInt32(item_cur.Key)).Where(q => q.seleccionado == true).ToList();
                                                if (lst_asignacion_curso_documento.Count > 0)
                                                {
                                                    foreach (var item_cd in lst_asignacion_curso_documento)
                                                    {
                                                        var inf_documento = odata_documento.getInfo(info.IdEmpresa, item_cd.IdDocumento);
                                                        aca_AnioLectivo_Curso_Documento Entity_CursoDocumento = new aca_AnioLectivo_Curso_Documento
                                                        {
                                                            IdEmpresa = item_cd.IdEmpresa,
                                                            IdAnio = info.IdAnioApertura,
                                                            IdSede = item_cd.IdSede,
                                                            IdNivel = item_cd.IdNivel,
                                                            IdJornada = item_cd.IdJornada,
                                                            IdCurso = item_cd.IdCurso,
                                                            IdDocumento = item_cd.IdDocumento,
                                                            NomDocumento = inf_documento.NomDocumento,
                                                            OrdenDocumento = (inf_documento.OrdenDocumento==null) ? (int?)null : inf_documento.OrdenDocumento
                                                        };
                                                        Context.aca_AnioLectivo_Curso_Documento.Add(Entity_CursoDocumento);
                                                    }
                                                }
                                            }
                                            
                                            //Context.SaveChanges();

                                            //PARALELO POR PROFESOR
                                            foreach (var item_cur in lst_curso)
                                            {
                                                var lst_asignacion_curso_paralelo = odata_curso_paralelo.get_list_asignacion(info.IdEmpresa, info.IdSede, info.IdAnio, Convert.ToInt32(item_niv.Key), Convert.ToInt32(item_jor.Key), Convert.ToInt32(item_cur.Key)).Where(q => q.seleccionado == true).ToList();
                                                if (lst_asignacion_curso_paralelo.Count > 0)
                                                {
                                                    foreach (var item_cp in lst_asignacion_curso_paralelo)
                                                    {
                                                        var lst_asignacion_paralelo_profesor = odata_paralelo_profesor.get_list_asignacion(info.IdEmpresa, info.IdSede, info.IdAnio, Convert.ToInt32(item_niv.Key), Convert.ToInt32(item_jor.Key), Convert.ToInt32(item_cur.Key), Convert.ToInt32(item_cp.IdParalelo)).ToList();
                                                        if (lst_asignacion_paralelo_profesor.Count>0)
                                                        { 
                                                            foreach (var item_pp in lst_asignacion_paralelo_profesor)
                                                            {
                                                                var info_profesor = odata_profesor.getInfo(info.IdEmpresa, Convert.ToDecimal(item_pp.IdProfesor));
                                                                aca_AnioLectivo_Paralelo_Profesor Entity_ParaleloProfesor = new aca_AnioLectivo_Paralelo_Profesor
                                                                {
                                                                    IdEmpresa = item_pp.IdEmpresa,
                                                                    IdAnio = info.IdAnioApertura,
                                                                    IdSede = item_pp.IdSede,
                                                                    IdNivel = item_pp.IdNivel,
                                                                    IdJornada = item_pp.IdJornada,
                                                                    IdCurso = item_pp.IdCurso,
                                                                    IdMateria = item_pp.IdMateria,
                                                                    IdParalelo = item_pp.IdParalelo,
                                                                    IdProfesor = (info_profesor == null ? (decimal?)null : (info_profesor.Estado == false ? (decimal?)null : item_pp.IdProfesor))
                                                                };
                                                                Context.aca_AnioLectivo_Paralelo_Profesor.Add(Entity_ParaleloProfesor);
                                                            }
                                                        }
                                                    }
                                                }

                                                var lst_plantillas_x_curso = odata_curso_plantilla.getList(info.IdEmpresa, info.IdSede, info.IdAnio, Convert.ToInt32(item_niv.Key), Convert.ToInt32(item_jor.Key), Convert.ToInt32(item_cur.Key));
                                                if (lst_plantillas_x_curso.Count > 0)
                                                {
                                                    var IdPlantillaApertura = 0;
                                                    foreach (var item_p in lst_plantillas_x_curso)
                                                    {
                                                        foreach (DataRow fila in tabla_plantilla.Rows)
                                                        {
                                                            if (Convert.ToInt32(fila["IdAnioAnterior"]) == item_p.IdAnio && Convert.ToInt32(fila["IdPlantillaAnterior"]) == item_p.IdPlantilla)
                                                            {
                                                                IdPlantillaApertura = Convert.ToInt32(fila["IdPlantillaApertura"]);
                                                                break;
                                                            }
                                                        }

                                                        aca_AnioLectivo_Curso_Plantilla Entity_CursoPlantilla = new aca_AnioLectivo_Curso_Plantilla
                                                        {
                                                            IdEmpresa = item_p.IdEmpresa,
                                                            IdAnio = info.IdAnioApertura,
                                                            IdSede = item_p.IdSede,
                                                            IdNivel = item_p.IdNivel,
                                                            IdJornada = item_p.IdJornada,
                                                            IdCurso = item_p.IdCurso,
                                                            IdPlantilla = IdPlantillaApertura,
                                                            Observacion = item_p.Observacion
                                                        };
                                                        Context.aca_AnioLectivo_Curso_Plantilla.Add(Entity_CursoPlantilla);


                                                        // PARAMETRIZACION //
                                                        var lst_plantillas_x_anio_curso = odata_curso_plantilla_parametrizacion.GetList_x_Curso_x_Plantilla(info.IdEmpresa, info.IdSede, info.IdAnio, Convert.ToInt32(item_niv.Key), Convert.ToInt32(item_jor.Key), Convert.ToInt32(item_cur.Key), item_p.IdPlantilla);
                                                        if (lst_plantillas_x_anio_curso.Count > 0)
                                                        {
                                                            foreach (var item_param in lst_plantillas_x_anio_curso)
                                                            {
                                                                aca_AnioLectivo_Curso_Plantilla_Parametrizacion Entity_CursoPlantilla_Parametrizacion = new aca_AnioLectivo_Curso_Plantilla_Parametrizacion
                                                                {
                                                                    IdEmpresa = item_param.IdEmpresa,
                                                                    IdAnio = info.IdAnioApertura,
                                                                    IdSede = item_param.IdSede,
                                                                    IdNivel = item_param.IdNivel,
                                                                    IdJornada = item_param.IdJornada,
                                                                    IdCurso = item_param.IdCurso,
                                                                    IdPlantilla = IdPlantillaApertura,
                                                                    IdRubro = item_param.IdRubro,
                                                                    IdCtaCbleDebe = string.IsNullOrEmpty(item_param.IdCtaCbleDebe) ? null : item_param.IdCtaCbleDebe,
                                                                    IdCtaCbleHaber = string.IsNullOrEmpty(item_param.IdCtaCbleDebe) ? null : item_param.IdCtaCbleHaber
                                                                };
                                                                Context.aca_AnioLectivo_Curso_Plantilla_Parametrizacion.Add(Entity_CursoPlantilla_Parametrizacion);
                                                            }
                                                        }

                                                    }
                                                }
                                                
                                                //Context.SaveChanges();
                                            } //lista de curso
                                        }
                                    }
                                }
                            }
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
    }
}
