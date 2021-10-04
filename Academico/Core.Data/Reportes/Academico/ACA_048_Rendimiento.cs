using Core.Data.Base;
using Core.Info.Helps;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.Academico
{
    public class ACA_048_Rendimiento_Data
    {
        public List<ACA_048_Rendimiento_Info> get_list(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria, int IdCatalogoParcialTipo)
        {
            try
            {
                List<ACA_048_Rendimiento_Info> Lista = new List<ACA_048_Rendimiento_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "select a.IdEmpresa, a.IdCalificacionCualitativa, a.IdAnio, a.Codigo, a.DescripcionCorta, a.DescripcionLarga, a.Calificacion, a.Estado, "
                    + " sum(b.ContadorTotal) ContadorTotal, sum(b.AlumnosConCalificacion) AlumnosConCalificacion, dbo.BankersRounding((sum(b.AlumnosConCalificacion) / sum(b.ContadorTotal)) * 100, 2)  Porcentaje "
                    + " from aca_AnioLectivoCalificacionCualitativa as a WITH (nolock) left join "
                    + " ( "
                        + " SELECT B.IdEmpresa, B.IdAnio, B.IdSede, B.IdNivel, B.IdJornada, B.IdCurso, B.IdParalelo,1 AS ContadorTotal, ";
                        if (IdCatalogoParcialTipo == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1))
                        {
                            query += " CASE WHEN A.PromedioQ1 IS NOT NULL THEN 1 ELSE 0 END AS AlumnosConCalificacion, a.IdCalificacionCualitativaQ1 as PromedioQuimestre";
                        }
                        else if (IdCatalogoParcialTipo == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2))
                        {
                            query += " CASE WHEN A.PromedioQ2 IS NOT NULL THEN 1 ELSE 0 END AS AlumnosConCalificacion, a.IdCalificacionCualitativaQ2 as PromedioQuimestre";
                        }
                        else
                        {
                            query += " 0 as PromedioQuimestre";
                        }
                    
                        query += " FROM aca_MatriculaCalificacionCualitativaPromedio AS A WITH (nolock) "
                        + " INNER JOIN aca_Matricula AS B WITH (nolock) ON A.IdEmpresa = B.IdEmpresa AND A.IdMatricula = B.IdMatricula "
                        + " where b.IdEmpresa = " + IdEmpresa + " and b.IdAnio = " + IdAnio + " and b.IdSede = " + IdSede + " and b.IdJornada = " + IdJornada
                        + " and b.IdNivel = " + IdNivel + " and b.IdCurso = " + IdCurso + " and b.IdParalelo = " + IdParalelo + " and a.IdMateria = " + IdMateria
                    + " ) as b on b.IdEmpresa = a.IdEmpresa and a.IdAnio = b.IdAnio and a.IdCalificacionCualitativa = b.PromedioQuimestre "
                    + "where b.IdEmpresa =  " + IdEmpresa + " and b.IdAnio = " + IdAnio
                    + " group by a.IdEmpresa, a.IdCalificacionCualitativa, a.IdAnio, a.Codigo, a.DescripcionCorta, a.DescripcionLarga, a.Calificacion, a.Estado";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new ACA_048_Rendimiento_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdCalificacionCualitativa = Convert.ToInt32(reader["IdCalificacionCualitativa"]),
                            Codigo = reader["Codigo"].ToString(),
                            DescripcionCorta = reader["DescripcionCorta"].ToString(),
                            DescripcionLarga = reader["DescripcionLarga"].ToString(),
                            Calificacion = Convert.ToDecimal(reader["Calificacion"]),
                            ContadorTotal = string.IsNullOrEmpty(reader["ContadorTotal"].ToString()) ? (int?)null : Convert.ToInt32(reader["ContadorTotal"]),
                            AlumnosConCalificacion = string.IsNullOrEmpty(reader["AlumnosConCalificacion"].ToString()) ? (int?)null : Convert.ToInt32(reader["AlumnosConCalificacion"]),
                            Porcentaje = string.IsNullOrEmpty(reader["Porcentaje"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Porcentaje"]),

                        });
                    }
                    reader.Close();
                }

                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
