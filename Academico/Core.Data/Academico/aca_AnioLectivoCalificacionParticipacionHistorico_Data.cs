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
    public class aca_AnioLectivoCalificacionParticipacionHistorico_Data
    {
        public aca_AnioLectivoCalificacionParticipacionHistorico_Info getInfo(int IdEmpresa, int IdAnio, decimal IdAlumno)
        {
            try
            {
                aca_AnioLectivoCalificacionParticipacionHistorico_Info info = new aca_AnioLectivoCalificacionParticipacionHistorico_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_AnioLectivoCalificacionParticipacionHistorico c WITH (nolock) "
                    + " WHERE c.IdEmpresa = " + IdEmpresa.ToString() + " and c.IdAnio = " + IdAnio.ToString() + " and c.IdAlumno = " + IdAlumno.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_AnioLectivoCalificacionParticipacionHistorico_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdCampoAccion = Convert.ToInt32(reader["IdCampoAccion"]),
                            IdTematica = Convert.ToInt32(reader["IdTematica"]),
                            PromedioFinal = string.IsNullOrEmpty(reader["PromedioFinal"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinal"]),
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

        public bool guardarDB(aca_AnioLectivoCalificacionParticipacionHistorico_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_AnioLectivoCalificacionParticipacionHistorico Entity = new aca_AnioLectivoCalificacionParticipacionHistorico
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdAnio=info.IdAnio,
                        IdAlumno=info.IdAlumno,
                        IdSede = info.IdSede,
                        IdNivel = info.IdNivel,
                        IdJornada =info.IdJornada,
                        IdCurso=info.IdCurso,
                        IdCampoAccion=info.IdCampoAccion,
                        IdTematica = info.IdTematica,
                        PromedioFinal = info.PromedioFinal
                    };
                    Context.aca_AnioLectivoCalificacionParticipacionHistorico.Add(Entity);

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(aca_AnioLectivoCalificacionParticipacionHistorico_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_AnioLectivoCalificacionParticipacionHistorico Entity = Context.aca_AnioLectivoCalificacionParticipacionHistorico.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAnio == info.IdAnio && q.IdAlumno==info.IdAlumno);
                    if (Entity == null)
                        return false;

                    Entity.IdSede = info.IdSede;
                    Entity.IdNivel = info.IdNivel;
                    Entity.IdJornada = info.IdJornada;
                    Entity.IdCurso = info.IdCurso;
                    Entity.IdCampoAccion = info.IdCampoAccion;
                    Entity.IdTematica = info.IdTematica;
                    Entity.PromedioFinal = info.PromedioFinal;

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
