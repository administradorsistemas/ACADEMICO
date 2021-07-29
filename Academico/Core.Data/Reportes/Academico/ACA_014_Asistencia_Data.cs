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
    public class ACA_014_Asistencia_Data
    {
        public List<ACA_014_Asistencia_Info> get_list(int IdEmpresa, decimal IdMatricula, int IdCatalogoParcial)
        {
            try
            {

                List<ACA_014_Asistencia_Info> Lista = new List<ACA_014_Asistencia_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "select IdEmpresa, IdMatricula, "
                        + " (ISNULL(FJustificadaP1, 0) + ISNULL(FJustificadaP2, 0) + ISNULL(FJustificadaP3, 0)) FJustificadaQ1, "
                        + " (ISNULL(FInjustificadaP1, 0) + ISNULL(FInjustificadaP2, 0) + ISNULL(FInjustificadaP3, 0)) FInJustificadaQ1, "
                        + " (ISNULL(AtrasosP1, 0) + ISNULL(AtrasosP2, 0) + ISNULL(AtrasosP3, 0)) AtrasoQ1, "
                        + " (ISNULL(FJustificadaP4, 0) + ISNULL(FJustificadaP5, 0) + ISNULL(FJustificadaP6, 0)) FJustificadaQ2, "
                        + " (ISNULL(FInjustificadaP4, 0) + ISNULL(FInjustificadaP5, 0) + ISNULL(FInjustificadaP6, 0)) FInJustificadaQ2, "
                        + " (ISNULL(AtrasosP4, 0) + ISNULL(AtrasosP5, 0) + ISNULL(AtrasosP6, 0)) AtrasoQ2, "
                        + " (ISNULL(FJustificadaP1,0)+ISNULL(FJustificadaP2,0)+ISNULL(FJustificadaP3,0)+ISNULL(FJustificadaP4,0)+ISNULL(FJustificadaP5,0)+ISNULL(FJustificadaP6,0)) FJustificada, "
                        + " (ISNULL(FInjustificadaP1, 0) + ISNULL(FInjustificadaP2, 0) + ISNULL(FInjustificadaP3, 0) + ISNULL(FInjustificadaP4, 0) + ISNULL(FInjustificadaP5, 0) + ISNULL(FInjustificadaP6, 0)) FInJustificada, "
                        + " (ISNULL(AtrasosP1, 0) + ISNULL(AtrasosP2, 0) + ISNULL(AtrasosP3, 0) + ISNULL(AtrasosP4, 0) + ISNULL(AtrasosP5, 0) + ISNULL(AtrasosP6, 0)) Atrasos, "
                        + " FJustificadaP1, FJustificadaP2,FJustificadaP3,FJustificadaP4,FJustificadaP5,FJustificadaP6, "
                        + " FInjustificadaP1, FInjustificadaP2, FInjustificadaP3, FInjustificadaP4, FInjustificadaP5, FInjustificadaP6, "
                        + " AtrasosP1, AtrasosP2, AtrasosP3, AtrasosP4, AtrasosP5, AtrasosP6 "
                        + " from aca_MatriculaAsistencia "
                        + " where IdEmpresa = "+IdEmpresa+" and IdMatricula = "+IdMatricula;
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 5000;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new ACA_014_Asistencia_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            FJustificadaP1 = string.IsNullOrEmpty(reader["FJustificadaP1"].ToString()) ? (int?)null : Convert.ToInt32(reader["FJustificadaP1"]),
                            FInjustificadaP1 = string.IsNullOrEmpty(reader["FInjustificadaP1"].ToString()) ? (int?)null : Convert.ToInt32(reader["FInjustificadaP1"]),
                            AtrasosP1 = string.IsNullOrEmpty(reader["AtrasosP1"].ToString()) ? (int?)null : Convert.ToInt32(reader["AtrasosP1"]),
                            FJustificadaP2 = string.IsNullOrEmpty(reader["FJustificadaP2"].ToString()) ? (int?)null : Convert.ToInt32(reader["FJustificadaP2"]),
                            FInjustificadaP2 = string.IsNullOrEmpty(reader["FInjustificadaP2"].ToString()) ? (int?)null : Convert.ToInt32(reader["FInjustificadaP2"]),
                            AtrasosP2 = string.IsNullOrEmpty(reader["AtrasosP2"].ToString()) ? (int?)null : Convert.ToInt32(reader["AtrasosP2"]),
                            FInjustificadaP3 = string.IsNullOrEmpty(reader["FInjustificadaP3"].ToString()) ? (int?)null : Convert.ToInt32(reader["FInjustificadaP3"]),
                            FJustificadaP3 = string.IsNullOrEmpty(reader["FJustificadaP3"].ToString()) ? (int?)null : Convert.ToInt32(reader["FJustificadaP3"]),
                            AtrasosP3 = string.IsNullOrEmpty(reader["AtrasosP3"].ToString()) ? (int?)null : Convert.ToInt32(reader["AtrasosP3"]),
                            FJustificadaP4 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? string.IsNullOrEmpty(reader["FJustificadaP4"].ToString()) ? (int?)null : Convert.ToInt32(reader["FJustificadaP4"]) : (int?)null ),
                            FInjustificadaP4 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? string.IsNullOrEmpty(reader["FInjustificadaP4"].ToString()) ? (int?)null : Convert.ToInt32(reader["FInjustificadaP4"]) : (int?)null),
                            AtrasosP4 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? string.IsNullOrEmpty(reader["AtrasosP4"].ToString()) ? (int?)null : Convert.ToInt32(reader["AtrasosP4"]) : (int?)null),
                            FJustificadaP5 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? string.IsNullOrEmpty(reader["FJustificadaP5"].ToString()) ? (int?)null : Convert.ToInt32(reader["FJustificadaP5"]) : (int?)null),
                            FInjustificadaP5 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? string.IsNullOrEmpty(reader["FInjustificadaP5"].ToString()) ? (int?)null : Convert.ToInt32(reader["FInjustificadaP5"]) : (int?)null),
                            AtrasosP5 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? string.IsNullOrEmpty(reader["AtrasosP5"].ToString()) ? (int?)null : Convert.ToInt32(reader["AtrasosP5"]) : (int?)null),
                            FJustificadaP6 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? string.IsNullOrEmpty(reader["FJustificadaP6"].ToString()) ? (int?)null : Convert.ToInt32(reader["FJustificadaP6"]) : (int?)null),
                            FInjustificadaP6 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? string.IsNullOrEmpty(reader["FInjustificadaP6"].ToString()) ? (int?)null : Convert.ToInt32(reader["FInjustificadaP6"]) : (int?)null),
                            AtrasosP6 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? string.IsNullOrEmpty(reader["AtrasosP6"].ToString()) ? (int?)null : Convert.ToInt32(reader["AtrasosP6"]) : (int?)null),
                            FJustificadaQ1 = reader["FJustificadaQ1"].ToString(),
                            FInJustificadaQ1 = reader["FInJustificadaQ1"].ToString(),
                            AtrasoQ1 = reader["AtrasoQ1"].ToString(),
                            FJustificadaQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? reader["FJustificadaQ2"].ToString() : ""),
                            FInJustificadaQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? reader["FInJustificadaQ2"].ToString() : ""),
                            AtrasoQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? reader["AtrasoQ2"].ToString() : ""),
                            FJustificada = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? reader["FJustificada"].ToString() : ""),
                            FInJustificada = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? reader["FInJustificada"].ToString() : ""),
                            Atrasos = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? reader["Atrasos"].ToString() : ""),
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
