using Core.Data.Base;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.Academico
{
    public class ACA_005_Data
    {
        public ACA_005_Info get_info(int IdEmpresa, int IdAnio, decimal IdAlumno)
        {
            try
            {
                ACA_005_Info info = new ACA_005_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "DECLARE @IdEmpresa int = "+ IdEmpresa.ToString() + ", @IdAlumno numeric = " + IdAlumno.ToString() + ", @IdAnioMatricula numeric= " + IdAnio.ToString() +", @IdAnioAnterior numeric "
                    + " select @IdAnioAnterior = IdAnioLectivoAnterior from aca_AnioLectivo "
                    + " where IdEmpresa = @IdEmpresa and IdAnio = @IdAnioMatricula "
                    + " set @IdAnioAnterior = isnull(@IdAnioAnterior, 0) "
                    + " SELECT dbo.aca_SocioEconomico.IdEmpresa, dbo.aca_SocioEconomico.IdSocioEconomico, matricula.IdAnio, matricula.Descripcion, matricula.NomNivel,matricula.NomJornada, matricula.NomCurso, dbo.aca_SocioEconomico.IdAlumno, dbo.aca_Alumno.Codigo AS CodigoAlumno, PersonaAlumno.pe_nombreCompleto AS NombreAlumno, "
                    + " PersonaAlumno.pe_fechaNacimiento AS FechaNacAlumno, dbo.tb_provincia.Descripcion_Prov AS ProvinciaAlumno, dbo.tb_ciudad.Descripcion_Ciudad AS CiudadAlumno, CatalogoSexo.ca_descripcion AS SexoAlumno, "
                    + " PersonaAlumno.pe_cedulaRuc AS CedulaAlumno, dbo.aca_Alumno.Direccion AS DireccionAlumno, dbo.aca_Alumno.Celular as TelefonoAlumno, dbo.aca_Alumno.Sector AS SectorAlumno, "
                    + " iif((dbo.aca_Alumno.Dificultad_Escritura = 1), 'SI', 'NO') Dificultad_Escritura, iif((dbo.aca_Alumno.Dificultad_Lectura = 1), 'SI', 'NO') Dificultad_Lectura, iif((dbo.aca_Alumno.Dificultad_Matematicas = 1), 'SI', 'NO') Dificultad_Matematicas, PersonaAlumno.CodCatalogoCONADIS AS TieneDiscapacidadAlumno, "
                    + " CatalogoDiscapacidad.ca_descripcion AS DiscapacidadAlumno, dbo.aca_Alumno.LugarNacimiento, dbo.tb_parroquia.nom_parroquia AS ParroquiaAlumno, iif((dbo.aca_SocioEconomico.TieneElectricidad = 1), 'SI', 'NO') TieneElectricidad, "
                    + " iif((dbo.aca_SocioEconomico.TieneHermanos = 1), 'SI', 'NO') TieneHermanos, dbo.aca_SocioEconomico.CantidadHermanos, TipoVivienda.NomCatalogoFicha AS TipoVivienda, TenenciaVivienda.NomCatalogoFicha AS TenenciaVivienda, "
                    + " Agua.NomCatalogoFicha AS Agua, dbo.aca_SocioEconomico.SueldoPadre, dbo.aca_SocioEconomico.SueldoMadre, dbo.aca_SocioEconomico.OtroIngresoPadre, dbo.aca_SocioEconomico.OtroIngresoMadre, "
                    + " dbo.aca_SocioEconomico.GastoAlimentacion, dbo.aca_SocioEconomico.GastoEducacion, dbo.aca_SocioEconomico.GastoServicioBasico, dbo.aca_SocioEconomico.GastoSalud, dbo.aca_SocioEconomico.GastoArriendo, "
                    + " dbo.aca_SocioEconomico.GastoPrestamo, dbo.aca_SocioEconomico.OtroGasto, MotivoIngreso.NomCatalogoFicha AS MotivoIngreso, dbo.aca_SocioEconomico.OtroMotivoIngreso, "
                    + " InformacionInstitucion.NomCatalogoFicha AS InformacionInstitucion, dbo.aca_SocioEconomico.OtroInformacionInst, FinanciaEstudios.NomCatalogoFicha AS FinanciaEstudios, dbo.aca_SocioEconomico.OtroFinanciamiento, "
                    + " ViveCon.NomCatalogoFicha AS AlumnoViveCon,"
                    + " NomPadre,DireccionPadre,NomEstadoCivilPadre,CelularPadre,ProfesionPadre, NomInstruccionPadre,CorreoPadre,EmpresaTrabajoPadre,DireccionTrabajoPadre,TelefonoTrabajoPadre,CargoTrabajoPadre,AniosServicioPadre, "
                    + " IngresoMensualPadre,VehiculoPropioPadre,MarcaPadre,ModeloPadre, AnioVehiculoPadre, "
                    + " NomMadre,DireccionMadre,NomEstadoCivilMadre,CelularMadre,ProfesionMadre, NomInstruccionMadre,CorreoMadre,EmpresaTrabajoMadre,DireccionTrabajoMadre,TelefonoTrabajoMadre,CargoTrabajoMadre,AniosServicioMadre, "
                    + " IngresoMensualMadre,VehiculoPropioMadre,MarcaMadre,ModeloMadre, AnioVehiculoMadre, "
                    + " NomRepresentante,pe_cedulaRucRepresentante cedulaRepresentante, DireccionRepresentante,NomEstadoCivilRepresentante,CelularRepresentante,ProfesionRepresentante, NomInstruccionRepresentante,CorreoRepresentante,EmpresaTrabajoRepresentante,DireccionTrabajoRepresentante,TelefonoTrabajoRepresentante,CargoTrabajoRepresentante,AniosServicioRepresentante, "
                    + " IngresoMensualRepresentante,VehiculoPropioRepresentante,MarcaRepresentante,ModeloRepresentante, AnioVehiculoRepresentante, Calificacion.Conducta, Calificacion.Promedio "
                    + " FROM     dbo.aca_SocioEconomico WITH (nolock) INNER JOIN "
                    + " dbo.aca_Alumno WITH (nolock) ON dbo.aca_SocioEconomico.IdEmpresa = dbo.aca_Alumno.IdEmpresa AND dbo.aca_SocioEconomico.IdAlumno = dbo.aca_Alumno.IdAlumno INNER JOIN "
                    + " dbo.tb_persona AS PersonaAlumno WITH (nolock) ON dbo.aca_Alumno.IdPersona = PersonaAlumno.IdPersona LEFT OUTER JOIN "
                    + " dbo.tb_provincia WITH (nolock) ON dbo.aca_Alumno.IdProvincia = dbo.tb_provincia.IdProvincia LEFT OUTER JOIN "
                    + " dbo.tb_ciudad WITH (nolock) ON dbo.aca_Alumno.IdCiudad = dbo.tb_ciudad.IdCiudad LEFT OUTER JOIN "
                    + " dbo.tb_Catalogo AS CatalogoSexo WITH (nolock)ON PersonaAlumno.pe_sexo = CatalogoSexo.CodCatalogo LEFT OUTER JOIN "
                    + " dbo.tb_parroquia WITH (nolock) ON dbo.aca_Alumno.IdParroquia = dbo.tb_parroquia.IdParroquia LEFT OUTER JOIN "
                    + " dbo.aca_CatalogoFicha AS ViveCon WITH (nolock) ON dbo.aca_SocioEconomico.IdCatalogoFichaVive = ViveCon.IdCatalogoFicha LEFT OUTER JOIN "
                    + " dbo.aca_CatalogoFicha AS FinanciaEstudios WITH (nolock) ON dbo.aca_SocioEconomico.IdCatalogoFichaFin = FinanciaEstudios.IdCatalogoFicha LEFT OUTER JOIN "
                    + " dbo.aca_CatalogoFicha AS InformacionInstitucion WITH (nolock) ON dbo.aca_SocioEconomico.IdCatalogoFichaIns = InformacionInstitucion.IdCatalogoFicha LEFT OUTER JOIN "
                    + " dbo.aca_CatalogoFicha AS MotivoIngreso WITH (nolock) ON dbo.aca_SocioEconomico.IdCatalogoFichaMot = MotivoIngreso.IdCatalogoFicha LEFT OUTER JOIN "
                    + " dbo.aca_CatalogoFicha AS Agua WITH (nolock) ON dbo.aca_SocioEconomico.IdCatalogoFichaAg = Agua.IdCatalogoFicha LEFT OUTER JOIN "
                    + " dbo.aca_CatalogoFicha AS TenenciaVivienda WITH (nolock) ON dbo.aca_SocioEconomico.IdCatalogoFichaTVi = TenenciaVivienda.IdCatalogoFicha LEFT OUTER JOIN "
                    + " dbo.aca_CatalogoFicha AS TipoVivienda WITH (nolock) ON dbo.aca_SocioEconomico.IdCatalogoFichaVi = TipoVivienda.IdCatalogoFicha LEFT OUTER JOIN "
                    + " dbo.tb_Catalogo AS CatalogoDiscapacidad WITH (nolock) ON PersonaAlumno.CodCatalogoCONADIS = CatalogoSexo.CodCatalogo "
                    + " /*MATRICULA*/ "
                    + " inner join "
                    + " ( "
                    + " select m.IdEmpresa, m.IdAlumno, m.IdAnio, a.Descripcion, n.NomNivel, j.NomJornada, c.NomCurso from aca_Matricula m WITH (nolock) "
                    + " inner join aca_AnioLectivo a WITH (nolock) on m.IdEmpresa= a.IdEmpresa and m.IdAnio = a.IdAnio "
                    + " inner join aca_NivelAcademico n WITH (nolock) on  m.IdEmpresa= n.IdEmpresa and m.IdNivel = n.IdNivel "
                    + " inner join aca_Jornada j WITH (nolock) on  m.IdEmpresa= j.IdEmpresa and m.IdJornada = j.IdJornada "
                    + " inner join aca_Curso c WITH (nolock) on  m.IdEmpresa= c.IdEmpresa and m.IdCurso = c.IdCurso "
                    + " where m.IdEmpresa = @IdEmpresa and m.IdAlumno = @IdAlumno and m.IdAnio = @IdAnioMatricula "
                    + " )matricula on matricula.IdEmpresa = @IdEmpresa and matricula.IdAlumno = @IdAlumno and matricula.IdAnio = @IdAnioMatricula "
                    + " /*PADRE*/ "
                    + " left join "
                    + " ( "
                    + " SELECT f.IdEmpresa, f.IdAlumno, f.Secuencia, "
                    + " 'DATOS DE LA PADRE' AS TituloPadre, p.pe_nombreCompleto AS NomPadre, dbo.tb_Catalogo.ca_descripcion AS NomEstadoCivilPadre, f.Direccion as DireccionPadre, dbo.aca_CatalogoFicha.NomCatalogoFicha AS NomInstruccionPadre, "
                    + " f.EmpresaTrabajo as EmpresaTrabajoPadre, f.DireccionTrabajo DireccionTrabajoPadre, f.TelefonoTrabajo TelefonoTrabajoPadre, f.CargoTrabajo CargoTrabajoPadre, f.AniosServicio AniosServicioPadre, "
                    + " f.Correo as CorreoPadre, CASE WHEN f.VehiculoPropio = 1 THEN 'SI' ELSE 'NO' END AS VehiculoPropioPadre, f.Marca as MarcaPadre, f.Modelo as ModeloPadre, p.pe_cedulaRuc as pe_cedulaRucPadre, "
                    + " dbo.tb_profesion.Descripcion as ProfesionPadre, f.Celular as CelularPadre, f.IngresoMensual IngresoMensualPadre, f.AnioVehiculo as AnioVehiculoPadre, "
                    + " ISNULL(dbo.aca_SocioEconomico.OtroIngresoPadre, 0) AS OtrosIngresosPadre, tb_profesion.Descripcion as NomProfesionPadre "
                    + " FROM     dbo.aca_Familia AS f WITH (nolock) INNER JOIN "
                    + " dbo.tb_persona AS p WITH (nolock) ON f.IdPersona = p.IdPersona LEFT JOIN "
                    + " dbo.aca_SocioEconomico WITH (nolock) ON f.IdEmpresa = dbo.aca_SocioEconomico.IdEmpresa AND f.IdAlumno = dbo.aca_SocioEconomico.IdAlumno LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS c WITH (nolock) ON f.IdCatalogoPAREN = c.IdCatalogo LEFT OUTER JOIN "
                    + " dbo.tb_Catalogo WITH (nolock) ON p.IdEstadoCivil = dbo.tb_Catalogo.CodCatalogo LEFT OUTER JOIN "
                    + " dbo.tb_profesion WITH (nolock) ON p.IdProfesion = dbo.tb_profesion.IdProfesion LEFT OUTER JOIN "
                    + " dbo.aca_CatalogoFicha WITH (nolock) ON f.IdCatalogoFichaInst = dbo.aca_CatalogoFicha.IdCatalogoFicha "
                    + " WHERE f.IdEmpresa = @IdEmpresa and f.IdAlumno = @IdAlumno and f.IdCatalogoPAREN = 10 "
                    + " )  "
                    + " Padre on aca_Alumno.IdEmpresa = Padre.IdEmpresa and aca_Alumno.IdAlumno = Padre.IdAlumno "
                    + " /*MADRE*/ "
                    + " left join "
                    + " ( "
                    + " SELECT f.IdEmpresa, f.IdAlumno, f.Secuencia, "
                    + " 'DATOS DE LA MADRE' AS TituloMadre, p.pe_nombreCompleto AS NomMadre, dbo.tb_Catalogo.ca_descripcion AS NomEstadoCivilMadre, f.Direccion as DireccionMadre, dbo.aca_CatalogoFicha.NomCatalogoFicha AS NomInstruccionMadre, "
                    + " f.EmpresaTrabajo as EmpresaTrabajoMadre, f.DireccionTrabajo DireccionTrabajoMadre, f.TelefonoTrabajo TelefonoTrabajoMadre, f.CargoTrabajo CargoTrabajoMadre, f.AniosServicio AniosServicioMadre, "
                    + " f.Correo as CorreoMadre, CASE WHEN f.VehiculoPropio = 1 THEN 'SI' ELSE 'NO' END AS VehiculoPropioMadre, f.Marca as MarcaMadre, f.Modelo as ModeloMadre, p.pe_cedulaRuc as pe_cedulaRucMadre, "
                    + " dbo.tb_profesion.Descripcion as ProfesionMadre, f.Celular as CelularMadre, f.IngresoMensual IngresoMensualMadre, f.AnioVehiculo as AnioVehiculoMadre, "
                    + " ISNULL(dbo.aca_SocioEconomico.OtroIngresoMadre, 0) AS OtrosIngresosMadre, tb_profesion.Descripcion as NomProfesionMadre "
                    + " FROM     dbo.aca_Familia AS f WITH (nolock) INNER JOIN "
                    + " dbo.tb_persona AS p WITH (nolock) ON f.IdPersona = p.IdPersona LEFT JOIN "
                    + " dbo.aca_SocioEconomico WITH (nolock) ON f.IdEmpresa = dbo.aca_SocioEconomico.IdEmpresa AND f.IdAlumno = dbo.aca_SocioEconomico.IdAlumno LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS c WITH (nolock) ON f.IdCatalogoPAREN = c.IdCatalogo LEFT OUTER JOIN "
                    + " dbo.tb_Catalogo WITH (nolock) ON p.IdEstadoCivil = dbo.tb_Catalogo.CodCatalogo LEFT OUTER JOIN "
                    + " dbo.tb_profesion WITH (nolock) ON p.IdProfesion = dbo.tb_profesion.IdProfesion LEFT OUTER JOIN "
                    + " dbo.aca_CatalogoFicha WITH (nolock) ON f.IdCatalogoFichaInst = dbo.aca_CatalogoFicha.IdCatalogoFicha "
                    + " WHERE f.IdEmpresa = @IdEmpresa and f.IdAlumno = @IdAlumno and f.IdCatalogoPAREN = 11 "
                    + " )  "
                    + " Madre on aca_Alumno.IdEmpresa = Madre.IdEmpresa and aca_Alumno.IdAlumno = Madre.IdAlumno "
                    + " /*REPRESENTANTE*/ "
                    + " left join "
                    + " ( "
                    + " SELECT f.IdEmpresa, f.IdAlumno, f.Secuencia, "
                    + " 'DATOS DEL REPRESENTANTE' AS TituloRepresentante, p.pe_nombreCompleto AS NomRepresentante, dbo.tb_Catalogo.ca_descripcion AS NomEstadoCivilRepresentante, f.Direccion as DireccionRepresentante, dbo.aca_CatalogoFicha.NomCatalogoFicha AS NomInstruccionRepresentante, "
                    + " f.EmpresaTrabajo as EmpresaTrabajoRepresentante, f.DireccionTrabajo DireccionTrabajoRepresentante, f.TelefonoTrabajo TelefonoTrabajoRepresentante, f.CargoTrabajo CargoTrabajoRepresentante, f.AniosServicio AniosServicioRepresentante, "
                    + " f.Correo as CorreoRepresentante, CASE WHEN f.VehiculoPropio = 1 THEN 'SI' ELSE 'NO' END AS VehiculoPropioRepresentante, f.Marca as MarcaRepresentante, f.Modelo as ModeloRepresentante, p.pe_cedulaRuc as pe_cedulaRucRepresentante, "
                    + " dbo.tb_profesion.Descripcion as ProfesionRepresentante, f.Celular as CelularRepresentante, f.IngresoMensual IngresoMensualRepresentante, f.AnioVehiculo as AnioVehiculoRepresentante, "
                    + " CASE WHEN F.IdCatalogoPAREN = 11 THEN  ISNULL(dbo.aca_SocioEconomico.OtroIngresoPadre, 0) WHEN F.IdCatalogoPAREN = 10 THEN ISNULL(dbo.aca_SocioEconomico.OtroIngresoMadre, 0) ELSE 0 END AS OtrosIngresosRepresentante, tb_profesion.Descripcion as NomProfesionRepresentante "
                    + " FROM     dbo.aca_Familia AS f WITH (nolock) INNER JOIN "
                    + " dbo.tb_persona AS p WITH (nolock) ON f.IdPersona = p.IdPersona LEFT JOIN "
                    + " dbo.aca_SocioEconomico WITH (nolock) ON f.IdEmpresa = dbo.aca_SocioEconomico.IdEmpresa AND f.IdAlumno = dbo.aca_SocioEconomico.IdAlumno LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS c WITH (nolock) ON f.IdCatalogoPAREN = c.IdCatalogo LEFT OUTER JOIN "
                    + " dbo.tb_Catalogo WITH (nolock) ON p.IdEstadoCivil = dbo.tb_Catalogo.CodCatalogo LEFT OUTER JOIN "
                    + " dbo.tb_profesion WITH (nolock) ON p.IdProfesion = dbo.tb_profesion.IdProfesion LEFT OUTER JOIN "
                    + " dbo.aca_CatalogoFicha WITH (nolock) ON f.IdCatalogoFichaInst = dbo.aca_CatalogoFicha.IdCatalogoFicha "
                    + " WHERE f.IdEmpresa = @IdEmpresa and f.IdAlumno = @IdAlumno AND F.EsRepresentante = 1 "
                    + " ) "
                    + " Representante on aca_Alumno.IdEmpresa = Representante.IdEmpresa and aca_Alumno.IdAlumno = Representante.IdAlumno left join "
                    + " ( "
                    + " select a.IdEmpresa, a.IdAlumno, a.IdAnio, a.Conducta, a.Promedio "
                    + " from aca_AnioLectivoCalificacionHistorico as a WITH (nolock) inner join aca_AnioLectivo as b WITH (nolock) "
                    + " on a.IdEmpresa = b.IdEmpresa and a.IdAnio = b.IdAnio "
                    + " where a.IdEmpresa = @IdEmpresa and a.IdAlumno = @IdAlumno and a.IdAnio = @IdAnioAnterior "
                    + " ) Calificacion on aca_Alumno.IdEmpresa = Calificacion.IdEmpresa and aca_Alumno.IdAlumno = Calificacion.IdAlumno "
                    + " where "
                    + " aca_SocioEconomico.IdEmpresa = @IdEmpresa AND aca_SocioEconomico.IdAlumno = @IdAlumno"; 
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new ACA_005_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            AlumnoViveCon = string.IsNullOrEmpty(reader["AlumnoViveCon"].ToString()) ? null : reader["AlumnoViveCon"].ToString(),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            CantidadHermanos = string.IsNullOrEmpty(reader["CantidadHermanos"].ToString()) ? (int?)null : Convert.ToInt32(reader["CantidadHermanos"]),
                            CedulaAlumno = string.IsNullOrEmpty(reader["CedulaAlumno"].ToString()) ? null : reader["CedulaAlumno"].ToString(),
                            CodigoAlumno = string.IsNullOrEmpty(reader["CodigoAlumno"].ToString()) ? null : reader["CodigoAlumno"].ToString(),
                            IdSocioEconomico = string.IsNullOrEmpty(reader["IdSocioEconomico"].ToString()) ? 0 : Convert.ToInt32(reader["IdSocioEconomico"]),
                            DireccionAlumno = string.IsNullOrEmpty(reader["DireccionAlumno"].ToString()) ? null : reader["DireccionAlumno"].ToString(),
                            DiscapacidadAlumno = string.IsNullOrEmpty(reader["DiscapacidadAlumno"].ToString()) ? null : reader["DiscapacidadAlumno"].ToString(),
                            FinanciaEstudios = string.IsNullOrEmpty(reader["FinanciaEstudios"].ToString()) ? null : reader["FinanciaEstudios"].ToString(),
                            FechaNacAlumno = string.IsNullOrEmpty(reader["FechaNacAlumno"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["FechaNacAlumno"]),
                            GastoAlimentacion = Convert.ToDouble(reader["GastoAlimentacion"]),
                            GastoArriendo = Convert.ToDouble(reader["GastoArriendo"]),
                            GastoEducacion = Convert.ToDouble(reader["GastoEducacion"]),
                            GastoPrestamo = Convert.ToDouble(reader["GastoPrestamo"]),
                            GastoSalud = Convert.ToDouble(reader["GastoSalud"]),
                            GastoServicioBasico = Convert.ToDouble(reader["GastoServicioBasico"]),
                            OtroGasto = Convert.ToDouble(reader["OtroGasto"]),
                            InformacionInstitucion = string.IsNullOrEmpty(reader["InformacionInstitucion"].ToString()) ? null : reader["InformacionInstitucion"].ToString(),
                            LugarNacimiento = string.IsNullOrEmpty(reader["LugarNacimiento"].ToString()) ? null : reader["LugarNacimiento"].ToString(),
                            MotivoIngreso = string.IsNullOrEmpty(reader["MotivoIngreso"].ToString()) ? null : reader["MotivoIngreso"].ToString(),
                            OtroFinanciamiento = string.IsNullOrEmpty(reader["OtroFinanciamiento"].ToString()) ? null : reader["OtroFinanciamiento"].ToString(),
                            OtroInformacionInst = string.IsNullOrEmpty(reader["OtroInformacionInst"].ToString()) ? null : reader["OtroInformacionInst"].ToString(),
                            OtroIngresoMadre = Convert.ToDouble(reader["OtroIngresoMadre"]),
                            OtroIngresoPadre = Convert.ToDouble(reader["OtroIngresoPadre"]),
                            OtroMotivoIngreso = string.IsNullOrEmpty(reader["OtroMotivoIngreso"].ToString()) ? null : reader["OtroMotivoIngreso"].ToString(),
                            NombreAlumno = string.IsNullOrEmpty(reader["NombreAlumno"].ToString()) ? null : reader["NombreAlumno"].ToString(),
                            ParroquiaAlumno = string.IsNullOrEmpty(reader["ParroquiaAlumno"].ToString()) ? null : reader["ParroquiaAlumno"].ToString(),
                            ProvinciaAlumno = string.IsNullOrEmpty(reader["ProvinciaAlumno"].ToString()) ? null : reader["ProvinciaAlumno"].ToString(),
                            SectorAlumno = string.IsNullOrEmpty(reader["SectorAlumno"].ToString()) ? null : reader["SectorAlumno"].ToString(),
                            SexoAlumno = string.IsNullOrEmpty(reader["SexoAlumno"].ToString()) ? null : reader["SexoAlumno"].ToString(),
                            SueldoMadre = Convert.ToDouble(reader["SueldoMadre"]),
                            SueldoPadre = Convert.ToDouble(reader["SueldoPadre"]),
                            TenenciaVivienda = string.IsNullOrEmpty(reader["TenenciaVivienda"].ToString()) ? null : reader["TenenciaVivienda"].ToString(),
                            TieneDiscapacidadAlumno = string.IsNullOrEmpty(reader["TieneDiscapacidadAlumno"].ToString()) ? null : reader["TieneDiscapacidadAlumno"].ToString(),
                            TieneElectricidad = string.IsNullOrEmpty(reader["TieneElectricidad"].ToString()) ? null : reader["TieneElectricidad"].ToString(),
                            TieneHermanos = string.IsNullOrEmpty(reader["TieneHermanos"].ToString()) ? null : reader["TieneHermanos"].ToString(),
                            TipoVivienda = string.IsNullOrEmpty(reader["TipoVivienda"].ToString()) ? null : reader["TipoVivienda"].ToString(),
                            TelefonoAlumno = string.IsNullOrEmpty(reader["TelefonoAlumno"].ToString()) ? null : reader["TelefonoAlumno"].ToString(),
                            NomPadre = string.IsNullOrEmpty(reader["NomPadre"].ToString()) ? null : reader["NomPadre"].ToString(),
                            DireccionPadre = string.IsNullOrEmpty(reader["DireccionPadre"].ToString()) ? null : reader["DireccionPadre"].ToString(),
                            NomEstadoCivilPadre = string.IsNullOrEmpty(reader["NomEstadoCivilPadre"].ToString()) ? null : reader["NomEstadoCivilPadre"].ToString(),
                            CelularPadre = string.IsNullOrEmpty(reader["CelularPadre"].ToString()) ? null : reader["CelularPadre"].ToString(),
                            ProfesionPadre = string.IsNullOrEmpty(reader["ProfesionPadre"].ToString()) ? null : reader["ProfesionPadre"].ToString(),
                            NomInstruccionPadre = string.IsNullOrEmpty(reader["NomInstruccionPadre"].ToString()) ? null : reader["NomInstruccionPadre"].ToString(),
                            CorreoPadre = string.IsNullOrEmpty(reader["CorreoPadre"].ToString()) ? null : reader["CorreoPadre"].ToString(),
                            EmpresaTrabajoPadre = string.IsNullOrEmpty(reader["EmpresaTrabajoPadre"].ToString()) ? null : reader["EmpresaTrabajoPadre"].ToString(),
                            DireccionTrabajoPadre = string.IsNullOrEmpty(reader["DireccionTrabajoPadre"].ToString()) ? null : reader["DireccionTrabajoPadre"].ToString(),
                            TelefonoTrabajoPadre = string.IsNullOrEmpty(reader["TelefonoTrabajoPadre"].ToString()) ? null : reader["TelefonoTrabajoPadre"].ToString(),
                            CargoTrabajoPadre = string.IsNullOrEmpty(reader["CargoTrabajoPadre"].ToString()) ? null : reader["CargoTrabajoPadre"].ToString(),
                            AniosServicioPadre = string.IsNullOrEmpty(reader["AniosServicioPadre"].ToString()) ? (int?)null : Convert.ToInt32(reader["AniosServicioPadre"]),
                            IngresoMensualPadre = string.IsNullOrEmpty(reader["IngresoMensualPadre"].ToString()) ? (double?)null : Convert.ToDouble(reader["IngresoMensualPadre"]),
                            VehiculoPropioPadre = string.IsNullOrEmpty(reader["VehiculoPropioPadre"].ToString()) ? null : reader["VehiculoPropioPadre"].ToString(),
                            MarcaPadre = string.IsNullOrEmpty(reader["MarcaPadre"].ToString()) ? null : reader["MarcaPadre"].ToString(),
                            ModeloPadre = string.IsNullOrEmpty(reader["ModeloPadre"].ToString()) ? null : reader["ModeloPadre"].ToString(),
                            AnioVehiculoPadre = string.IsNullOrEmpty(reader["AnioVehiculoPadre"].ToString()) ? (int?)null : Convert.ToInt32(reader["AnioVehiculoPadre"]),
                            NomMadre = string.IsNullOrEmpty(reader["NomMadre"].ToString()) ? null : reader["NomMadre"].ToString(),
                            DireccionMadre = string.IsNullOrEmpty(reader["DireccionMadre"].ToString()) ? null : reader["DireccionMadre"].ToString(),
                            NomEstadoCivilMadre = string.IsNullOrEmpty(reader["NomEstadoCivilMadre"].ToString()) ? null : reader["NomEstadoCivilMadre"].ToString(),
                            CelularMadre = string.IsNullOrEmpty(reader["CelularMadre"].ToString()) ? null : reader["CelularMadre"].ToString(),
                            ProfesionMadre = string.IsNullOrEmpty(reader["ProfesionMadre"].ToString()) ? null : reader["ProfesionMadre"].ToString(),
                            NomInstruccionMadre = string.IsNullOrEmpty(reader["NomInstruccionMadre"].ToString()) ? null : reader["NomInstruccionMadre"].ToString(),
                            CorreoMadre = string.IsNullOrEmpty(reader["CorreoMadre"].ToString()) ? null : reader["CorreoMadre"].ToString(),
                            EmpresaTrabajoMadre = string.IsNullOrEmpty(reader["EmpresaTrabajoMadre"].ToString()) ? null : reader["EmpresaTrabajoMadre"].ToString(),
                            DireccionTrabajoMadre = string.IsNullOrEmpty(reader["DireccionTrabajoMadre"].ToString()) ? null : reader["DireccionTrabajoMadre"].ToString(),
                            TelefonoTrabajoMadre = string.IsNullOrEmpty(reader["TelefonoTrabajoMadre"].ToString()) ? null : reader["TelefonoTrabajoMadre"].ToString(),
                            CargoTrabajoMadre = string.IsNullOrEmpty(reader["CargoTrabajoMadre"].ToString()) ? null : reader["CargoTrabajoMadre"].ToString(),
                            AniosServicioMadre = string.IsNullOrEmpty(reader["AniosServicioMadre"].ToString()) ? (int?)null : Convert.ToInt32(reader["AniosServicioMadre"]),
                            IngresoMensualMadre = string.IsNullOrEmpty(reader["IngresoMensualMadre"].ToString()) ? (double?)null : Convert.ToDouble(reader["IngresoMensualMadre"]),
                            VehiculoPropioMadre = string.IsNullOrEmpty(reader["VehiculoPropioMadre"].ToString()) ? null : reader["VehiculoPropioMadre"].ToString(),
                            MarcaMadre = string.IsNullOrEmpty(reader["MarcaMadre"].ToString()) ? null : reader["MarcaMadre"].ToString(),
                            ModeloMadre = string.IsNullOrEmpty(reader["ModeloMadre"].ToString()) ? null : reader["ModeloMadre"].ToString(),
                            AnioVehiculoMadre = string.IsNullOrEmpty(reader["AnioVehiculoMadre"].ToString()) ? (int?)null : Convert.ToInt32(reader["AnioVehiculoMadre"]),
                            NomRepresentante = string.IsNullOrEmpty(reader["NomRepresentante"].ToString()) ? null : reader["NomRepresentante"].ToString(),
                            DireccionRepresentante = string.IsNullOrEmpty(reader["DireccionRepresentante"].ToString()) ? null : reader["DireccionRepresentante"].ToString(),
                            NomEstadoCivilRepresentante = string.IsNullOrEmpty(reader["NomEstadoCivilRepresentante"].ToString()) ? null : reader["NomEstadoCivilRepresentante"].ToString(),
                            CelularRepresentante = string.IsNullOrEmpty(reader["CelularRepresentante"].ToString()) ? null : reader["CelularRepresentante"].ToString(),
                            ProfesionRepresentante = string.IsNullOrEmpty(reader["ProfesionRepresentante"].ToString()) ? null : reader["ProfesionRepresentante"].ToString(),
                            NomInstruccionRepresentante = string.IsNullOrEmpty(reader["NomInstruccionRepresentante"].ToString()) ? null : reader["NomInstruccionRepresentante"].ToString(),
                            CorreoRepresentante = string.IsNullOrEmpty(reader["CorreoRepresentante"].ToString()) ? null : reader["CorreoRepresentante"].ToString(),
                            EmpresaTrabajoRepresentante = string.IsNullOrEmpty(reader["EmpresaTrabajoRepresentante"].ToString()) ? null : reader["EmpresaTrabajoRepresentante"].ToString(),
                            DireccionTrabajoRepresentante = string.IsNullOrEmpty(reader["DireccionTrabajoRepresentante"].ToString()) ? null : reader["DireccionTrabajoRepresentante"].ToString(),
                            TelefonoTrabajoRepresentante = string.IsNullOrEmpty(reader["TelefonoTrabajoRepresentante"].ToString()) ? null : reader["TelefonoTrabajoRepresentante"].ToString(),
                            CargoTrabajoRepresentante = string.IsNullOrEmpty(reader["CargoTrabajoRepresentante"].ToString()) ? null : reader["CargoTrabajoRepresentante"].ToString(),
                            AniosServicioRepresentante = string.IsNullOrEmpty(reader["AniosServicioRepresentante"].ToString()) ? (int?)null : Convert.ToInt32(reader["AniosServicioRepresentante"]),
                            IngresoMensualRepresentante = string.IsNullOrEmpty(reader["IngresoMensualRepresentante"].ToString()) ? (double?)null : Convert.ToDouble(reader["IngresoMensualRepresentante"]),
                            VehiculoPropioRepresentante = string.IsNullOrEmpty(reader["VehiculoPropioRepresentante"].ToString()) ? null : reader["VehiculoPropioRepresentante"].ToString(),
                            MarcaRepresentante = string.IsNullOrEmpty(reader["MarcaRepresentante"].ToString()) ? null : reader["MarcaRepresentante"].ToString(),
                            ModeloRepresentante = string.IsNullOrEmpty(reader["ModeloRepresentante"].ToString()) ? null : reader["ModeloRepresentante"].ToString(),
                            AnioVehiculoRepresentante = string.IsNullOrEmpty(reader["AnioVehiculoRepresentante"].ToString()) ? (int?)null : Convert.ToInt32(reader["AnioVehiculoRepresentante"]),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            Conducta = string.IsNullOrEmpty(reader["Conducta"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Conducta"]),
                            Promedio = string.IsNullOrEmpty(reader["Promedio"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Promedio"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Dificultad_Escritura = string.IsNullOrEmpty(reader["Dificultad_Escritura"].ToString()) ? null : reader["Dificultad_Escritura"].ToString(),
                            Dificultad_Lectura = string.IsNullOrEmpty(reader["Dificultad_Lectura"].ToString()) ? null : reader["Dificultad_Lectura"].ToString(),
                            Dificultad_Matematicas = string.IsNullOrEmpty(reader["Dificultad_Matematicas"].ToString()) ? null : reader["Dificultad_Matematicas"].ToString(),
                            Agua= string.IsNullOrEmpty(reader["Agua"].ToString()) ? null : reader["Agua"].ToString(),
                            CiudadAlumno = string.IsNullOrEmpty(reader["CiudadAlumno"].ToString()) ? null : reader["CiudadAlumno"].ToString(),
                            cedulaRepresentante = string.IsNullOrEmpty(reader["cedulaRepresentante"].ToString()) ? null : reader["cedulaRepresentante"].ToString(),
                        };
                    }
                }
                /*
                using (EntitiesReportes Context = new EntitiesReportes())
                {
                    info = (from q in Context.SPACA_005(IdEmpresa, IdAlumno)
                             select new ACA_005_Info
                             {
                                Agua=q.Agua,
                                AlumnoViveCon=q.AlumnoViveCon,
                                CantidadHermanos =q.CantidadHermanos,
                                CedulaAlumno=q.CedulaAlumno,
                                CiudadAlumno=q.CiudadAlumno,
                                IdSocioEconomico =q.IdSocioEconomico,
                                CodigoAlumno=q.CodigoAlumno,
                                DireccionAlumno=q.DireccionAlumno,
                                DiscapacidadAlumno=q.DiscapacidadAlumno,
                                FechaNacAlumno=q.FechaNacAlumno,
                                FinanciaEstudios=q.FinanciaEstudios,
                                GastoAlimentacion=q.GastoAlimentacion,
                                GastoArriendo=q.GastoArriendo,
                                GastoEducacion=q.GastoEducacion,
                                GastoPrestamo=q.GastoPrestamo,
                                GastoSalud=q.GastoSalud,
                                GastoServicioBasico=q.GastoServicioBasico,
                                OtroGasto=q.OtroGasto,
                                IdAlumno=q.IdAlumno,
                                IdEmpresa=q.IdEmpresa,
                                InformacionInstitucion=q.InformacionInstitucion,
                                LugarNacimiento=q.LugarNacimiento,
                                MotivoIngreso=q.MotivoIngreso,
                                OtroFinanciamiento=q.OtroFinanciamiento,
                                OtroInformacionInst=q.OtroInformacionInst,
                                OtroIngresoMadre=q.OtroIngresoMadre,
                                OtroIngresoPadre=q.OtroIngresoPadre,
                                OtroMotivoIngreso=q.OtroMotivoIngreso,
                                NombreAlumno=q.NombreAlumno,
                                ParroquiaAlumno=q.ParroquiaAlumno,
                                ProvinciaAlumno=q.ProvinciaAlumno,
                                SectorAlumno=q.SectorAlumno,
                                SexoAlumno=q.SexoAlumno,
                                SueldoMadre=q.SueldoMadre,
                                SueldoPadre=q.SueldoPadre,
                                TenenciaVivienda=q.TenenciaVivienda,
                                TieneDiscapacidadAlumno=q.TieneDiscapacidadAlumno,
                                TieneElectricidad=q.TieneElectricidad,
                                TieneHermanos=q.TieneHermanos,
                                TipoVivienda=q.TipoVivienda,
                                TelefonoAlumno=q.TelefonoAlumno,
                                NomPadre =q.NomPadre,
                                DireccionPadre=q.DireccionPadre,
                                NomEstadoCivilPadre=q.NomEstadoCivilPadre,
                                CelularPadre=q.CelularPadre,
                                ProfesionPadre=q.ProfesionPadre,
                                NomInstruccionPadre=q.NomInstruccionPadre,
                                CorreoPadre=q.CorreoPadre,
                                EmpresaTrabajoPadre=q.EmpresaTrabajoPadre,
                                DireccionTrabajoPadre=q.DireccionTrabajoPadre,
                                TelefonoTrabajoPadre=q.TelefonoTrabajoPadre,
                                CargoTrabajoPadre=q.CargoTrabajoPadre,
                                AniosServicioPadre=q.AniosServicioPadre,
                                IngresoMensualPadre=q.IngresoMensualPadre,
                                VehiculoPropioPadre=q.VehiculoPropioPadre,
                                MarcaPadre=q.MarcaPadre,
                                ModeloPadre=q.ModeloPadre,
                                AnioVehiculoPadre=q.AnioVehiculoPadre,
                                NomMadre=q.NomMadre,
                                DireccionMadre=q.DireccionMadre,
                                NomEstadoCivilMadre=q.NomEstadoCivilMadre,
                                CelularMadre=q.CelularMadre,
                                ProfesionMadre=q.ProfesionMadre,
                                NomInstruccionMadre=q.NomInstruccionMadre,
                                CorreoMadre=q.CorreoMadre,
                                EmpresaTrabajoMadre=q.EmpresaTrabajoMadre,
                                DireccionTrabajoMadre=q.DireccionTrabajoMadre,
                                TelefonoTrabajoMadre=q.TelefonoTrabajoMadre,
                                CargoTrabajoMadre=q.CargoTrabajoMadre,
                                AniosServicioMadre=q.AniosServicioMadre,
                                IngresoMensualMadre=q.IngresoMensualMadre,
                                VehiculoPropioMadre=q.VehiculoPropioMadre,
                                MarcaMadre=q.MarcaMadre,
                                ModeloMadre=q.ModeloMadre,
                                AnioVehiculoMadre=q.AnioVehiculoMadre,
                                NomRepresentante=q.NomRepresentante,
                                DireccionRepresentante=q.DireccionRepresentante,
                                NomEstadoCivilRepresentante=q.NomEstadoCivilRepresentante,
                                CelularRepresentante=q.CelularRepresentante,
                                ProfesionRepresentante=q.ProfesionRepresentante,
                                NomInstruccionRepresentante=q.NomInstruccionRepresentante,
                                CorreoRepresentante=q.CorreoRepresentante,
                                EmpresaTrabajoRepresentante=q.EmpresaTrabajoRepresentante,
                                DireccionTrabajoRepresentante=q.DireccionTrabajoRepresentante,
                                TelefonoTrabajoRepresentante=q.TelefonoTrabajoRepresentante,
                                CargoTrabajoRepresentante=q.CargoTrabajoRepresentante,
                                AniosServicioRepresentante=q.AniosServicioRepresentante,
                                IngresoMensualRepresentante=q.IngresoMensualRepresentante,
                                VehiculoPropioRepresentante=q.VehiculoPropioRepresentante,
                                MarcaRepresentante=q.MarcaRepresentante,
                                ModeloRepresentante=q.ModeloRepresentante,
                                AnioVehiculoRepresentante=q.AnioVehiculoRepresentante,
                                Descripcion = q.Descripcion,
                                NomJornada=q.NomJornada,
                                NomCurso=q.NomCurso,
                                NomNivel=q.NomNivel,
                                Conducta=q.Conducta,
                                Promedio=q.Promedio,
                                IdAnio = q.IdAnio
                             }).FirstOrDefault();
                }
                */
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
