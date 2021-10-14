using Core.Data.Base;
using Core.Info.Academico;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Academico
{
    public class aca_Alumno_Data
    {
        aca_AnioLectivo_Data data_Anio = new aca_AnioLectivo_Data();
        public List<aca_Alumno_Info> getList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                List<aca_Alumno_Info> Lista = new List<aca_Alumno_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT a.IdEmpresa, a.IdAlumno, a.Codigo, a.IdPersona, p.pe_Naturaleza, p.pe_nombreCompleto, p.pe_apellido, p.pe_nombre, p.IdTipoDocumento, p.pe_cedulaRuc, a.Direccion, a.Celular, a.Correo, p.pe_sexo, p.pe_fechaNacimiento, "
                    + " p.CodCatalogoSangre, p.CodCatalogoCONADIS, p.PorcentajeDiscapacidad, p.NumeroCarnetConadis, a.Estado, a.IdCatalogoESTMAT, a.IdCurso, a.IdCatalogoESTALU, p.pe_telfono_Contacto, cm.NomCatalogo AS NomCatalogoESTMAT, "
                    + " c.NomCatalogo AS NomCatalogoESTALU, a.FechaIngreso, a.LugarNacimiento, a.IdPais, a.Cod_Region, a.IdProvincia, a.IdCiudad, a.IdParroquia, a.Sector, p.IdReligion, p.AsisteCentroCristiano, p.IdGrupoEtnico, SUBSTRING(a.Codigo,0,5) AnioCodigo"
                    + " FROM dbo.aca_Alumno AS a WITH (nolock) INNER JOIN "
                    + " dbo.tb_persona AS p WITH (nolock) ON a.IdPersona = p.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS c WITH (nolock) ON a.IdCatalogoESTALU = c.IdCatalogo LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS cm WITH (nolock) ON a.IdCatalogoESTMAT = cm.IdCatalogo "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString();
                    if (MostrarAnulados == false)
                    {
                        query += " and a.Estado = 1";
                    }
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_Alumno_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdPersona = Convert.ToInt32(reader["IdPersona"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            Codigo = reader["Codigo"].ToString(),
                            IdTipoDocumento = reader["IdTipoDocumento"].ToString(),
                            pe_Naturaleza = reader["pe_Naturaleza"].ToString(),
                            pe_cedulaRuc = reader["pe_cedulaRuc"].ToString(),
                            pe_nombre = reader["pe_nombre"].ToString(),
                            pe_apellido = reader["pe_apellido"].ToString(),
                            pe_nombreCompleto = reader["pe_nombreCompleto"].ToString(),
                            pe_sexo = reader["pe_sexo"].ToString(),
                            FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                            pe_fechaNacimiento = string.IsNullOrEmpty(reader["pe_fechaNacimiento"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["pe_fechaNacimiento"]),
                            CodCatalogoSangre = string.IsNullOrEmpty(reader["CodCatalogoSangre"].ToString()) ? null : reader["CodCatalogoSangre"].ToString(),
                            CodCatalogoCONADIS = string.IsNullOrEmpty(reader["CodCatalogoCONADIS"].ToString()) ? null : string.IsNullOrEmpty(reader["Correo"].ToString()) ? null : reader["CodCatalogoCONADIS"].ToString(),
                            NumeroCarnetConadis = string.IsNullOrEmpty(reader["NumeroCarnetConadis"].ToString()) ? null : reader["NumeroCarnetConadis"].ToString(),
                            PorcentajeDiscapacidad = string.IsNullOrEmpty(reader["PorcentajeDiscapacidad"].ToString()) ? (double?)null : Convert.ToDouble(reader["PorcentajeDiscapacidad"]),
                            Correo = string.IsNullOrEmpty(reader["Correo"].ToString()) ? null : reader["Correo"].ToString(),
                            NomCatalogoESTALU = reader["NomCatalogoESTALU"].ToString(),
                            NomCatalogoESTMAT = reader["NomCatalogoESTMAT"].ToString(),
                            AnioCodigo = reader["AnioCodigo"].ToString(),
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
        
        public List<aca_Alumno_Info> getList_PeriodoActual(int IdEmpresa)
        {
            try
            {
                List<aca_Alumno_Info> Lista = new List<aca_Alumno_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "select a.IdEmpresa, a.Codigo, a.IdAlumno, b.IdMatricula, c.pe_nombreCompleto, c.pe_cedulaRuc, pfr.pe_nombreCompleto as NombreRepresentante, fr.Correo as CorreoRepresentante,"
                    + " pfc.pe_nombreCompleto NombreFactura, fc.Correo as CorreoEmiteFactura, fr.Celular as CelularRepresentante, fc.Celular as CelularEmiteFactura, fr.Telefono as TelefonoRepresentante, fc.Telefono as TelefonoEmiteFactura,"
                    + " b.IdAnio, b.IdSede, b.IdNivel, b.IdJornada, b.IdCurso, b.IdParalelo,  sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, jc.OrdenCurso, cp.NomParalelo, cp.OrdenParalelo,"
                    + " isnull(e.Saldo, 0) Saldo, isnull(e.SaldoProntoPago, 0)SaldoProntoPago, isnull(e.CantidadDeudas, 0) CantidadDeudas, pt.NomPlantillaTipo"
                    + " from aca_Alumno as a WITH (nolock) inner join"
                    + " aca_Matricula as b WITH (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdAlumno = b.IdAlumno left join"
                    + " tb_persona as c WITH (nolock) on a.IdPersona = c.IdPersona inner join"
                    + " aca_AnioLectivo as d WITH (nolock) on b.IdEmpresa = d.IdEmpresa and b.IdAnio = d.IdAnio left join"
                    + " ("
                    + " select az.IdEmpresa, az.IdAlumno, dbo.bankersrounding(sum(az.Saldo), 2) Saldo, sum(az.SaldoProntoPago) SaldoProntoPago, sum(CantidadFacturas) as CantidadDeudas from("
                    + " SELECT ay.IdEmpresa, ay.IdAlumno, sum(ay.Saldo) Saldo, sum(ay.SaldoProntoPago) SaldoProntoPago, count(*) as CantidadFacturas"
                    + " FROM("
                    + " select a1.IdEmpresa, a2.IdAlumno, dbo.bankersrounding(a1.Total - ISNULL(A3.dc_ValorPago, 0), 2) as Saldo,"
                    + " case when a5.FechaProntoPago >= CAST(GETDATE() AS DATE) THEN dbo.bankersrounding(a1.ValorProntoPago - ISNULL(A3.dc_ValorPago, 0), 2) ELSE dbo.bankersrounding(a1.Total - ISNULL(A3.dc_ValorPago, 0), 2) end as SaldoProntoPago"
                    + " from fa_factura_resumen as a1 WITH (nolock) inner join"
                    + " fa_factura as a2 WITH (nolock) on a1.IdEmpresa = a2.IdEmpresa and a1.IdSucursal = a2.IdSucursal and a1.IdBodega = a2.IdBodega and a1.IdCbteVta = a2.IdCbteVta left join"
                    + " ("
                    + " select x1.IdEmpresa, x1.IdSucursal, x1.IdBodega_Cbte, x1.IdCbte_vta_nota, x1.dc_TipoDocumento, sum(x1.dc_ValorPago) dc_ValorPago"
                    + " from cxc_cobro_det as x1 WITH (nolock) "
                    + " where x1.IdEmpresa = " + IdEmpresa.ToString() + " and x1.estado = 'A'"
                    + " group by x1.IdEmpresa, x1.IdSucursal, x1.IdBodega_Cbte, x1.IdCbte_vta_nota, x1.dc_TipoDocumento"
                    + " ) as a3 on a2.IdEmpresa = a3.IdEmpresa and a2.IdSucursal = a3.IdSucursal and a2.IdBodega = a3.IdBodega_Cbte and a2.IdCbteVta = a3.IdCbte_vta_nota and a2.vt_tipoDoc = a3.dc_TipoDocumento left join"
                    + " aca_Matricula_Rubro as a4 WITH (nolock) on a2.IdEmpresa = a4.IdEmpresa and a2.IdSucursal = a4.IdSucursal and a2.IdBodega = a4.IdBodega and a2.IdCbteVta = a4.IdCbteVta left join"
                    + " aca_AnioLectivo_Periodo as a5 WITH (nolock) on a5.IdEmpresa = a4.IdEmpresa and a5.IdAnio = a4.IdAnio and a5.IdPeriodo = a4.IdPeriodo"
                    + " where a2.IdEmpresa = " + IdEmpresa.ToString() + " and a2.Estado = 'A' and dbo.BankersRounding(a1.Total - isnull(a3.dc_ValorPago, 0), 2) > 0"
                    + " ) ay group by ay.IdEmpresa, ay.IdAlumno"
                    + " UNION ALL"
                    + " select a1.IdEmpresa, a2.IdAlumno, dbo.bankersrounding(sum(a1.Total) - ISNULL(SUM(A3.dc_ValorPago), 0), 2) as Saldo, dbo.bankersrounding(sum(a1.Total) - ISNULL(SUM(A3.dc_ValorPago), 0), 2), count(*) CantidadFacturas"
                    + " from fa_notaCreDeb_resumen as a1 WITH (nolock) inner join"
                    + " fa_notaCreDeb as a2 WITH (nolock) on a1.IdEmpresa = a2.IdEmpresa and a1.IdSucursal = a2.IdSucursal and a1.IdBodega = a2.IdBodega and a1.IdNota = a2.IdNota left join"
                    + " ("
                    + " select x1.IdEmpresa, x1.IdSucursal, x1.IdBodega_Cbte, x1.IdCbte_vta_nota, x1.dc_TipoDocumento, sum(x1.dc_ValorPago) dc_ValorPago"
                    + " from cxc_cobro_det as x1 WITH (nolock) "
                    + " where x1.IdEmpresa = " + IdEmpresa.ToString() + " and x1.estado = 'A'"
                    + " group by x1.IdEmpresa, x1.IdSucursal, x1.IdBodega_Cbte, x1.IdCbte_vta_nota, x1.dc_TipoDocumento"
                    + " ) as a3 on a2.IdEmpresa = a3.IdEmpresa and a2.IdSucursal = a3.IdSucursal and a2.IdBodega = a3.IdBodega_Cbte and a2.IdNota = a3.IdCbte_vta_nota and a2.CodDocumentoTipo = a3.dc_TipoDocumento"
                    + " where a2.IdEmpresa = " + IdEmpresa.ToString() + " and a2.Estado = 'A' and a2.CreDeb = 'D' and dbo.BankersRounding(a1.Total - isnull(a3.dc_ValorPago, 0), 2) > 0"
                    + " group by a1.IdEmpresa, a2.IdAlumno"
                    + " ) az group by az.IdEmpresa, az.IdAlumno"
                    + " ) as e on a.IdEmpresa = e.IdEmpresa and a.IdAlumno = e.IdAlumno left join"
                    + " aca_AnioLectivo_Curso_Paralelo as cp WITH (nolock) on b.IdEmpresa = cp.IdEmpresa and b.IdAnio = cp.IdAnio and b.IdSede = cp.IdSede and b.IdNivel = cp.IdNivel and b.IdJornada = cp.IdJornada and b.IdCurso = cp.IdCurso and b.IdParalelo = cp.IdParalelo left join"
                    + " aca_AnioLectivo_Jornada_Curso as jc on b.IdEmpresa = jc.IdEmpresa and b.IdAnio = jc.IdAnio and b.IdSede = jc.IdSede and b.IdNivel = jc.IdNivel and b.IdJornada = jc.IdJornada and b.IdCurso = jc.IdCurso left join"
                    + " aca_AnioLectivo_NivelAcademico_Jornada as nj WITH (nolock) on nj.IdEmpresa = b.IdEmpresa and nj.IdAnio = b.IdAnio and nj.IdSede = b.IdSede and nj.IdNivel = b.IdNivel and nj.IdJornada = b.IdJornada left join"
                    + " aca_AnioLectivo_Sede_NivelAcademico as sn WITH (nolock) on sn.IdEmpresa = b.IdEmpresa and sn.IdAnio = b.IdAnio and sn.IdSede = b.IdSede and sn.IdNivel = b.IdNivel left join"
                    + " aca_Familia as fr WITH (nolock) on fr.IdEmpresa = a.IdEmpresa and fr.IdAlumno = a.IdAlumno and fr.EsRepresentante = 1 left join"
                    + " tb_persona as pfr WITH (nolock) on pfr.IdPersona = fr.IdPersona left join"
                    + " aca_Familia as fc WITH (nolock) on fc.IdEmpresa = a.IdEmpresa and fc.IdAlumno = a.IdAlumno and fc.SeFactura = 1 left join"
                    + " tb_persona as pfc WITH (nolock) on pfc.IdPersona = fc.IdPersona LEFT JOIN"
                    + " aca_Plantilla as p WITH (nolock) on b.IdEmpresa = p.IdEmpresa and b.IdPlantilla = p.IdPlantilla left join"
                    + " aca_PlantillaTipo as pt WITH (nolock) on pt.IdEmpresa = p.IdEmpresa and pt.IdTipoPlantilla = p.IdTipoPlantilla"
                    + " where a.IdEmpresa = " + IdEmpresa.ToString() + " and d.EnCurso = 1 and not exists("
                    + " select x1.IdEmpresa from aca_AlumnoRetiro as x1 WITH (nolock) "
                    + " where x1.IdEmpresa = b.IdEmpresa and x1.IdMatricula = b.IdMatricula and x1.Estado = 1)";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_Alumno_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            Codigo = reader["Codigo"].ToString(),
                            pe_nombreCompleto = reader["pe_nombreCompleto"].ToString(),
                            pe_cedulaRuc = reader["pe_cedulaRuc"].ToString(),
                            NomRepEconomico = reader["NombreFactura"].ToString(),
                            correoRepEconomico = reader["CorreoEmiteFactura"].ToString(),
                            NomRepLegal = reader["NombreRepresentante"].ToString(),
                            CorreoRepLegal = reader["CorreoRepresentante"].ToString(),
                            CelularRepresentante = reader["CelularRepresentante"].ToString(),
                            CelularEmiteFactura = reader["CelularEmiteFactura"].ToString(),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            NomSede = reader["NomSede"].ToString(),
                            NomNivel = reader["NomNivel"].ToString(),
                            NomJornada = reader["NomJornada"].ToString(),
                            NomCurso = reader["NomCurso"].ToString(),
                            NomParalelo = reader["NomParalelo"].ToString(),
                            Saldo = Convert.ToDouble(reader["Saldo"]),
                            SaldoProntoPago = Convert.ToDouble(reader["SaldoProntoPago"]),
                            TelefonoRepresentante = reader["TelefonoRepresentante"].ToString(),
                            TelefonoEmiteFactura = reader["TelefonoEmiteFactura"].ToString(),
                            NomPlantillaTipo = reader["NomPlantillaTipo"].ToString()
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

        public aca_Alumno_Info getInfo_PeriodoActual(int IdEmpresa, decimal IdAlumno)
        {
            try
            {
                aca_Alumno_Info info = new aca_Alumno_Info();
                using (EntitiesAcademico odata = new EntitiesAcademico())
                {
                    var Entity = odata.vwaca_Alumno_PeriodoActual.Where(q => q.IdEmpresa == IdEmpresa && q.IdAlumno == IdAlumno).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new aca_Alumno_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdAlumno = Entity.IdAlumno,
                        IdMatricula = Entity.IdMatricula,
                        Codigo = Entity.Codigo,
                        pe_nombreCompleto = Entity.NombreAlumno,
                        pe_cedulaRuc = Entity.pe_cedulaRuc,
                        NomRepEconomico = Entity.NombreEmiteFactura,
                        correoRepEconomico = Entity.CorreoEmiteFactura,
                        CelularEmiteFactura = Entity.CelularEmiteFactura,
                        NomRepLegal = Entity.NombreRepresentante,
                        CorreoRepLegal = Entity.CorreoRepresentante,
                        CelularRepresentante = Entity.CelularRepresentante,
                        Saldo = Entity.Saldo,
                        SaldoProntoPago = Entity.SaldoProntoPago,
                        IdSede = Entity.IdSede,
                        IdJornada = Entity.IdJornada,
                        IdNivel = Entity.IdNivel,
                        IdCurso = Entity.IdCurso,
                        IdParalelo = Entity.IdParalelo,
                        NomSede = Entity.NomSede,
                        NomJornada = Entity.NomJornada,
                        NomNivel = Entity.NomNivel,
                        NomCurso = Entity.NomCurso,
                        NomParalelo = Entity.NomParalelo,
                        NomPlantillaTipo = Entity.NomPlantillaTipo,
                        TelefonoRepresentante = Entity.TelefonoRepresentante,
                        TelefonoEmiteFactura = Entity.TelefonoEmiteFactura
                    };
                }
                
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<aca_Alumno_Info> getList_PeriodoActual(int IdEmpresa, int IdAnio, int IdSede, int IdJornada, int IdNivel, int IdCurso, int IdParalelo)
        {
            try
            {
                int IdSedeIni = IdSede;
                int IdSedeFin = IdSede == 0 ? 9999999 : IdSede;

                int IdJornadaIni = IdJornada;
                int IdJornadaFin = IdJornada == 0 ? 9999999 : IdJornada;

                int IdNivelIni = IdNivel;
                int IdNivelFin = IdNivel == 0 ? 9999999 : IdNivel;

                int IdCursoIni = IdCurso;
                int IdCursoFin = IdCurso == 0 ? 9999999 : IdCurso;

                int IdParaleloIni = IdParalelo;
                int IdParaleloFin = IdParalelo == 0 ? 9999999 : IdParalelo;

                List<aca_Alumno_Info> Lista = new List<aca_Alumno_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "select a.IdEmpresa, a.Codigo, a.IdAlumno, b.IdMatricula, c.pe_nombreCompleto, c.pe_cedulaRuc, pfr.pe_nombreCompleto as NombreRepresentante, fr.Correo as CorreoRepresentante,"
                    + " pfc.pe_nombreCompleto NombreFactura, fc.Correo as CorreoEmiteFactura, fr.Celular as CelularRepresentante, fc.Celular as CelularEmiteFactura, fr.Telefono as TelefonoRepresentante, fc.Telefono as TelefonoEmiteFactura,"
                    + " b.IdAnio, b.IdSede, b.IdNivel, b.IdJornada, b.IdCurso, b.IdParalelo,  sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, jc.OrdenCurso, cp.NomParalelo, cp.OrdenParalelo,"
                    + " isnull(e.Saldo, 0) Saldo, isnull(e.SaldoProntoPago, 0)SaldoProntoPago, isnull(e.CantidadDeudas, 0) CantidadDeudas, pt.NomPlantillaTipo"
                    + " from aca_Alumno as a WITH (nolock) inner join"
                    + " aca_Matricula as b WITH (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdAlumno = b.IdAlumno left join"
                    + " tb_persona as c WITH (nolock) on a.IdPersona = c.IdPersona inner join"
                    + " aca_AnioLectivo as d WITH (nolock) on b.IdEmpresa = d.IdEmpresa and b.IdAnio = d.IdAnio left join"
                    + " ("
                    + " select az.IdEmpresa, az.IdAlumno, dbo.bankersrounding(sum(az.Saldo), 2) Saldo, sum(az.SaldoProntoPago) SaldoProntoPago, sum(CantidadFacturas) as CantidadDeudas from("
                    + " SELECT ay.IdEmpresa, ay.IdAlumno, sum(ay.Saldo) Saldo, sum(ay.SaldoProntoPago) SaldoProntoPago, count(*) as CantidadFacturas"
                    + " FROM("
                    + " select a1.IdEmpresa, a2.IdAlumno, dbo.bankersrounding(a1.Total - ISNULL(A3.dc_ValorPago, 0), 2) as Saldo,"
                    + " case when a5.FechaProntoPago >= CAST(GETDATE() AS DATE) THEN dbo.bankersrounding(a1.ValorProntoPago - ISNULL(A3.dc_ValorPago, 0), 2) ELSE dbo.bankersrounding(a1.Total - ISNULL(A3.dc_ValorPago, 0), 2) end as SaldoProntoPago"
                    + " from fa_factura_resumen as a1 WITH (nolock) inner join"
                    + " fa_factura as a2 WITH (nolock) on a1.IdEmpresa = a2.IdEmpresa and a1.IdSucursal = a2.IdSucursal and a1.IdBodega = a2.IdBodega and a1.IdCbteVta = a2.IdCbteVta left join"
                    + " ("
                    + " select x1.IdEmpresa, x1.IdSucursal, x1.IdBodega_Cbte, x1.IdCbte_vta_nota, x1.dc_TipoDocumento, sum(x1.dc_ValorPago) dc_ValorPago"
                    + " from cxc_cobro_det as x1 WITH (nolock) "
                    + " where x1.IdEmpresa = "+IdEmpresa.ToString()+" and x1.estado = 'A'"
                    + " group by x1.IdEmpresa, x1.IdSucursal, x1.IdBodega_Cbte, x1.IdCbte_vta_nota, x1.dc_TipoDocumento"
                    + " ) as a3 on a2.IdEmpresa = a3.IdEmpresa and a2.IdSucursal = a3.IdSucursal and a2.IdBodega = a3.IdBodega_Cbte and a2.IdCbteVta = a3.IdCbte_vta_nota and a2.vt_tipoDoc = a3.dc_TipoDocumento left join"
                    + " aca_Matricula_Rubro as a4 WITH (nolock) on a2.IdEmpresa = a4.IdEmpresa and a2.IdSucursal = a4.IdSucursal and a2.IdBodega = a4.IdBodega and a2.IdCbteVta = a4.IdCbteVta left join"
                    + " aca_AnioLectivo_Periodo as a5 WITH (nolock) on a5.IdEmpresa = a4.IdEmpresa and a5.IdAnio = a4.IdAnio and a5.IdPeriodo = a4.IdPeriodo"
                    + " where a2.IdEmpresa = " + IdEmpresa.ToString() + " and a2.Estado = 'A' and dbo.BankersRounding(a1.Total - isnull(a3.dc_ValorPago, 0), 2) > 0"
                    + " ) ay group by ay.IdEmpresa, ay.IdAlumno"
                    + " UNION ALL"
                    + " select a1.IdEmpresa, a2.IdAlumno, dbo.bankersrounding(sum(a1.Total) - ISNULL(SUM(A3.dc_ValorPago), 0), 2) as Saldo, dbo.bankersrounding(sum(a1.Total) - ISNULL(SUM(A3.dc_ValorPago), 0), 2), count(*) CantidadFacturas"
                    + " from fa_notaCreDeb_resumen as a1 inner join"
                    + " fa_notaCreDeb as a2 WITH (nolock) on a1.IdEmpresa = a2.IdEmpresa and a1.IdSucursal = a2.IdSucursal and a1.IdBodega = a2.IdBodega and a1.IdNota = a2.IdNota left join"
                    + " ("
                    + " select x1.IdEmpresa, x1.IdSucursal, x1.IdBodega_Cbte, x1.IdCbte_vta_nota, x1.dc_TipoDocumento, sum(x1.dc_ValorPago) dc_ValorPago"
                    + " from cxc_cobro_det as x1 WITH (nolock) "
                    + " where x1.IdEmpresa = " + IdEmpresa.ToString() + " and x1.estado = 'A'"
                    + " group by x1.IdEmpresa, x1.IdSucursal, x1.IdBodega_Cbte, x1.IdCbte_vta_nota, x1.dc_TipoDocumento"
                    + " ) as a3 on a2.IdEmpresa = a3.IdEmpresa and a2.IdSucursal = a3.IdSucursal and a2.IdBodega = a3.IdBodega_Cbte and a2.IdNota = a3.IdCbte_vta_nota and a2.CodDocumentoTipo = a3.dc_TipoDocumento"
                    + " where a2.IdEmpresa = " + IdEmpresa.ToString() + " and a2.Estado = 'A' and a2.CreDeb = 'D' and dbo.BankersRounding(a1.Total - isnull(a3.dc_ValorPago, 0), 2) > 0"
                    + " group by a1.IdEmpresa, a2.IdAlumno"
                    + " ) az group by az.IdEmpresa, az.IdAlumno"
                    + " ) as e on a.IdEmpresa = e.IdEmpresa and a.IdAlumno = e.IdAlumno left join"
                    + " aca_AnioLectivo_Curso_Paralelo as cp WITH (nolock) on b.IdEmpresa = cp.IdEmpresa and b.IdAnio = cp.IdAnio and b.IdSede = cp.IdSede and b.IdNivel = cp.IdNivel and b.IdJornada = cp.IdJornada and b.IdCurso = cp.IdCurso and b.IdParalelo = cp.IdParalelo left join"
                    + " aca_AnioLectivo_Jornada_Curso as jc WITH (nolock) on b.IdEmpresa = jc.IdEmpresa and b.IdAnio = jc.IdAnio and b.IdSede = jc.IdSede and b.IdNivel = jc.IdNivel and b.IdJornada = jc.IdJornada and b.IdCurso = jc.IdCurso left join"
                    + " aca_AnioLectivo_NivelAcademico_Jornada as nj WITH (nolock) on nj.IdEmpresa = b.IdEmpresa and nj.IdAnio = b.IdAnio and nj.IdSede = b.IdSede and nj.IdNivel = b.IdNivel and nj.IdJornada = b.IdJornada left join"
                    + " aca_AnioLectivo_Sede_NivelAcademico as sn WITH (nolock) on sn.IdEmpresa = b.IdEmpresa and sn.IdAnio = b.IdAnio and sn.IdSede = b.IdSede and sn.IdNivel = b.IdNivel left join"
                    + " aca_Familia as fr WITH (nolock) on fr.IdEmpresa = a.IdEmpresa and fr.IdAlumno = a.IdAlumno and fr.EsRepresentante = 1 left join"
                    + " tb_persona as pfr WITH (nolock) on pfr.IdPersona = fr.IdPersona left join"
                    + " aca_Familia as fc WITH (nolock) on fc.IdEmpresa = a.IdEmpresa and fc.IdAlumno = a.IdAlumno and fc.SeFactura = 1 left join"
                    + " tb_persona as pfc WITH (nolock) on pfc.IdPersona = fc.IdPersona LEFT JOIN"
                    + " aca_Plantilla as p WITH (nolock) on b.IdEmpresa = p.IdEmpresa and b.IdPlantilla = p.IdPlantilla left join"
                    + " aca_PlantillaTipo as pt WITH (nolock) on pt.IdEmpresa = p.IdEmpresa and pt.IdTipoPlantilla = p.IdTipoPlantilla"
                    + " where a.IdEmpresa = "+IdEmpresa.ToString()+" and b.IdAnio = "+ IdAnio.ToString() + " and not exists("
                    + " select x1.IdEmpresa from aca_AlumnoRetiro as x1 WITH (nolock) "
                    + " where x1.IdEmpresa = b.IdEmpresa and x1.IdMatricula = b.IdMatricula and x1.Estado = 1)"
                    + " and b.IdSede between " + IdSedeIni.ToString() + " and " + IdSedeFin.ToString() + " and b.IdNivel between " + IdNivelIni.ToString() + " and " + IdNivelFin.ToString() + " and b.IdJornada between " + IdJornadaIni.ToString() + " and " + IdJornadaFin.ToString() + " and b.IdCurso between " + IdCursoIni.ToString() + " and " + IdCursoFin.ToString() + " and b.IdParalelo between " + IdParaleloIni.ToString() + " and " + IdParaleloFin.ToString();
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_Alumno_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            Codigo = reader["Codigo"].ToString(),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            pe_nombreCompleto = reader["pe_nombreCompleto"].ToString(),
                            pe_cedulaRuc = reader["pe_cedulaRuc"].ToString(),
                            NomRepEconomico = reader["NombreFactura"].ToString(),
                            correoRepEconomico = reader["CorreoEmiteFactura"].ToString(),
                            NomRepLegal = reader["NombreRepresentante"].ToString(),
                            CorreoRepLegal = reader["CorreoRepresentante"].ToString(),

                            CelularRepresentante = reader["CelularRepresentante"].ToString(),
                            CelularEmiteFactura = reader["CelularEmiteFactura"].ToString(),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            NomSede = reader["NomSede"].ToString(),
                            NomNivel = reader["NomNivel"].ToString(),
                            NomJornada = reader["NomJornada"].ToString(),
                            NomCurso = reader["NomCurso"].ToString(),
                            NomParalelo = reader["NomParalelo"].ToString(),
                            Saldo = Convert.ToDouble(reader["Saldo"]),
                            SaldoProntoPago = Convert.ToDouble(reader["SaldoProntoPago"]),
                            TelefonoRepresentante = reader["TelefonoRepresentante"].ToString(),
                            TelefonoEmiteFactura = reader["TelefonoEmiteFactura"].ToString(),
                            NomPlantillaTipo = reader["NomPlantillaTipo"].ToString()
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

        public List<aca_Alumno_Info> getList_AlumnoRepresentante(int IdEmpresa, decimal IdAlumno)
        {
            try
            {
                List<aca_Alumno_Info> Lista = new List<aca_Alumno_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "select a.IdEmpresa, a.Codigo, a.IdAlumno, b.IdMatricula, c.pe_nombreCompleto, c.pe_cedulaRuc, pfr.pe_nombreCompleto as NombreRepresentante, "
                    + " fr.Correo as CorreoRepresentante, pfc.pe_nombreCompleto NombreFactura, fc.Correo as CorreoEmiteFactura, fr.Celular as CelularRepresentante, "
                    + " fc.Celular as CelularEmiteFactura, fr.Telefono as TelefonoRepresentante, fc.Telefono as TelefonoEmiteFactura, b.IdAnio, b.IdSede, b.IdNivel, "
                    + " b.IdJornada, b.IdCurso, b.IdParalelo,  sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pt.NomPlantillaTipo, d.Descripcion "
                    + " from aca_Alumno as a WITH (nolock) inner join aca_Matricula as b WITH (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdAlumno = b.IdAlumno "
                    + " left join tb_persona as c WITH (nolock) on a.IdPersona = c.IdPersona inner join aca_AnioLectivo as d on b.IdEmpresa = d.IdEmpresa and b.IdAnio = d.IdAnio "
                    + " left join aca_AnioLectivo_Curso_Paralelo as cp WITH (nolock) on b.IdEmpresa = cp.IdEmpresa and b.IdAnio = cp.IdAnio and b.IdSede = cp.IdSede and b.IdNivel = cp.IdNivel "
                    + " and b.IdJornada = cp.IdJornada and b.IdCurso = cp.IdCurso and b.IdParalelo = cp.IdParalelo "
                    + " left join aca_AnioLectivo_Jornada_Curso as jc WITH (nolock) on b.IdEmpresa = jc.IdEmpresa and b.IdAnio = jc.IdAnio and b.IdSede = jc.IdSede and b.IdNivel = jc.IdNivel "
                    + " and b.IdJornada = jc.IdJornada and b.IdCurso = jc.IdCurso "
                    + " left join aca_AnioLectivo_NivelAcademico_Jornada as nj WITH (nolock) on nj.IdEmpresa = b.IdEmpresa and nj.IdAnio = b.IdAnio and nj.IdSede = b.IdSede "
                    + " and nj.IdNivel = b.IdNivel and nj.IdJornada = b.IdJornada "
                    + " left join aca_AnioLectivo_Sede_NivelAcademico as sn WITH (nolock) on sn.IdEmpresa = b.IdEmpresa and sn.IdAnio = b.IdAnio and sn.IdSede = b.IdSede "
                    + " and sn.IdNivel = b.IdNivel left join aca_Familia as fr WITH (nolock) on fr.IdEmpresa = a.IdEmpresa and fr.IdAlumno = a.IdAlumno and fr.EsRepresentante = 1 "
                    + " left join tb_persona as pfr WITH (nolock) on pfr.IdPersona = fr.IdPersona left join aca_Familia as fc on fc.IdEmpresa = a.IdEmpresa and fc.IdAlumno = a.IdAlumno "
                    + " and fc.SeFactura = 1 left join tb_persona as pfc WITH (nolock) on pfc.IdPersona = fc.IdPersona "
                    + " LEFT JOIN aca_Plantilla as p WITH (nolock) on b.IdEmpresa = p.IdEmpresa and b.IdPlantilla = p.IdPlantilla "
                    + " left join aca_PlantillaTipo as pt WITH (nolock) on pt.IdEmpresa = p.IdEmpresa and pt.IdTipoPlantilla = p.IdTipoPlantilla "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString()
                    + " and a.IdAlumno = " + IdAlumno.ToString();
                    //+ " and not exists(select x1.IdEmpresa from aca_AlumnoRetiro as x1 where x1.IdEmpresa = b.IdEmpresa and x1.IdMatricula = b.IdMatricula and x1.Estado = 1) ";
                
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_Alumno_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            Codigo = reader["Codigo"].ToString(),
                            pe_nombreCompleto = reader["pe_nombreCompleto"].ToString(),
                            pe_cedulaRuc = reader["pe_cedulaRuc"].ToString(),
                            NomRepEconomico = reader["NombreFactura"].ToString(),
                            correoRepEconomico = reader["CorreoEmiteFactura"].ToString(),
                            NomRepLegal = reader["NombreRepresentante"].ToString(),
                            CorreoRepLegal = reader["CorreoRepresentante"].ToString(),
                            CelularRepresentante = reader["CelularRepresentante"].ToString(),
                            CelularEmiteFactura = reader["CelularEmiteFactura"].ToString(),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            NomSede = reader["NomSede"].ToString(),
                            NomNivel = reader["NomNivel"].ToString(),
                            NomJornada = reader["NomJornada"].ToString(),
                            NomCurso = reader["NomCurso"].ToString(),
                            NomParalelo = reader["NomParalelo"].ToString(),
                            TelefonoRepresentante = reader["TelefonoRepresentante"].ToString(),
                            TelefonoEmiteFactura = reader["TelefonoEmiteFactura"].ToString(),
                            NomPlantillaTipo = reader["NomPlantillaTipo"].ToString(),
                            anio = reader["Descripcion"].ToString()
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
        public aca_Alumno_Info getInfo(int IdEmpresa, decimal IdAlumno)
        {
            try
            {
                aca_Alumno_Info info=new aca_Alumno_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT a.IdEmpresa, a.IdAlumno, a.Codigo, a.IdPersona, p.pe_Naturaleza, p.pe_nombreCompleto, p.pe_apellido, p.pe_nombre, p.IdTipoDocumento, p.pe_cedulaRuc, a.Direccion, a.Celular, a.Correo, p.pe_sexo, p.pe_fechaNacimiento, "
                    + " p.CodCatalogoSangre, p.CodCatalogoCONADIS, p.PorcentajeDiscapacidad, p.NumeroCarnetConadis, a.Estado, a.IdCatalogoESTMAT, a.IdCurso, a.IdCatalogoESTALU, p.pe_telfono_Contacto, cm.NomCatalogo AS NomCatalogoESTMAT, "
                    + " c.NomCatalogo AS NomCatalogoESTALU, a.FechaIngreso, a.LugarNacimiento, a.IdPais, a.Cod_Region, a.IdProvincia, a.IdCiudad, a.IdParroquia, a.Sector, p.IdReligion, p.AsisteCentroCristiano, p.IdGrupoEtnico, a.Dificultad_Escritura, a.Dificultad_Lectura, a.Dificultad_Matematicas, a.Celular "
                    + " FROM dbo.aca_Alumno AS a WITH (nolock) INNER JOIN "
                    + " dbo.tb_persona AS p WITH (nolock) ON a.IdPersona = p.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS c WITH (nolock) ON a.IdCatalogoESTALU = c.IdCatalogo LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS cm WITH (nolock) ON a.IdCatalogoESTMAT = cm.IdCatalogo "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + "and a.IdAlumno = " + IdAlumno.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_Alumno_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdPersona = Convert.ToInt32(reader["IdPersona"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            Codigo = reader["Codigo"].ToString(),
                            IdTipoDocumento = reader["IdTipoDocumento"].ToString(),
                            pe_Naturaleza = reader["pe_Naturaleza"].ToString(),
                            pe_cedulaRuc = reader["pe_cedulaRuc"].ToString(),
                            pe_nombre = reader["pe_nombre"].ToString(),
                            pe_apellido = reader["pe_apellido"].ToString(),
                            pe_nombreCompleto = reader["pe_nombreCompleto"].ToString(),
                            pe_telfono_Contacto = reader["pe_telfono_Contacto"].ToString(),
                            Correo = string.IsNullOrEmpty(reader["Correo"].ToString()) ? null : reader["Correo"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            pe_sexo = reader["pe_sexo"].ToString(),
                            FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                            pe_fechaNacimiento = string.IsNullOrEmpty(reader["pe_fechaNacimiento"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["pe_fechaNacimiento"]),
                            CodCatalogoSangre = string.IsNullOrEmpty(reader["CodCatalogoSangre"].ToString()) ? null : reader["CodCatalogoSangre"].ToString(),
                            CodCatalogoCONADIS = string.IsNullOrEmpty(reader["CodCatalogoCONADIS"].ToString()) ? null : string.IsNullOrEmpty(reader["Correo"].ToString()) ? null : reader["CodCatalogoCONADIS"].ToString(),
                            NumeroCarnetConadis = string.IsNullOrEmpty(reader["NumeroCarnetConadis"].ToString()) ? null : reader["NumeroCarnetConadis"].ToString(),
                            PorcentajeDiscapacidad = string.IsNullOrEmpty(reader["PorcentajeDiscapacidad"].ToString()) ? (double?)null : Convert.ToDouble(reader["PorcentajeDiscapacidad"]),
                            NomCatalogoESTALU = reader["NomCatalogoESTALU"].ToString(),
                            NomCatalogoESTMAT = reader["NomCatalogoESTMAT"].ToString(),
                            IdCatalogoESTALU = Convert.ToInt32(reader["IdCatalogoESTALU"]),
                            IdCatalogoESTMAT = Convert.ToInt32(reader["IdCatalogoESTMAT"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdPais = reader["IdPais"].ToString(),
                            Cod_Region = reader["Cod_Region"].ToString(),
                            IdProvincia = reader["IdProvincia"].ToString(),
                            IdCiudad = reader["IdCiudad"].ToString(),
                            IdParroquia = reader["IdParroquia"].ToString(),
                            Sector = string.IsNullOrEmpty(reader["Sector"].ToString()) ? null : reader["Sector"].ToString(),
                            LugarNacimiento = string.IsNullOrEmpty(reader["LugarNacimiento"].ToString()) ? null : reader["LugarNacimiento"].ToString(),
                            IdReligion = string.IsNullOrEmpty(reader["IdReligion"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdReligion"]),
                            AsisteCentroCristiano = string.IsNullOrEmpty(reader["AsisteCentroCristiano"].ToString()) ? false : Convert.ToBoolean(reader["AsisteCentroCristiano"]),
                            IdGrupoEtnico = string.IsNullOrEmpty(reader["IdGrupoEtnico"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdGrupoEtnico"]),
                            Dificultad_Escritura = string.IsNullOrEmpty(reader["Dificultad_Escritura"].ToString()) ? false : Convert.ToBoolean(reader["Dificultad_Escritura"]),
                            Dificultad_Lectura = string.IsNullOrEmpty(reader["Dificultad_Lectura"].ToString()) ? false : Convert.ToBoolean(reader["Dificultad_Lectura"]),
                            Dificultad_Matematicas = string.IsNullOrEmpty(reader["Dificultad_Matematicas"].ToString()) ? false : Convert.ToBoolean(reader["Dificultad_Matematicas"]),
                            Celular = string.IsNullOrEmpty(reader["Celular"].ToString()) ? null : reader["Celular"].ToString(),
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
        public aca_Alumno_Info getInfo_IdPersona(int IdEmpresa, decimal IdPersona)
        {
            try
            {
                aca_Alumno_Info info = new aca_Alumno_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT a.IdEmpresa, a.IdAlumno, a.Codigo, a.IdPersona, p.pe_Naturaleza, p.pe_nombreCompleto, p.pe_apellido, p.pe_nombre, p.IdTipoDocumento, p.pe_cedulaRuc, a.Direccion, a.Celular, a.Correo, p.pe_sexo, p.pe_fechaNacimiento, "
                    + " p.CodCatalogoSangre, p.CodCatalogoCONADIS, p.PorcentajeDiscapacidad, p.NumeroCarnetConadis, a.Estado, a.IdCatalogoESTMAT, a.IdCurso, a.IdCatalogoESTALU, p.pe_telfono_Contacto, cm.NomCatalogo AS NomCatalogoESTMAT, "
                    + " c.NomCatalogo AS NomCatalogoESTALU, a.FechaIngreso, a.LugarNacimiento, a.IdPais, a.Cod_Region, a.IdProvincia, a.IdCiudad, a.IdParroquia, a.Sector, p.IdReligion, p.AsisteCentroCristiano, p.IdGrupoEtnico, a.Dificultad_Escritura, a.Dificultad_Lectura, a.Dificultad_Matematicas, a.Celular "
                    + " FROM dbo.aca_Alumno AS a WITH (nolock) INNER JOIN "
                    + " dbo.tb_persona AS p WITH (nolock) ON a.IdPersona = p.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS c WITH (nolock) ON a.IdCatalogoESTALU = c.IdCatalogo LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS cm WITH (nolock) ON a.IdCatalogoESTMAT = cm.IdCatalogo "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + "and a.IdPersona = " + IdPersona.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_Alumno_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdPersona = Convert.ToInt32(reader["IdPersona"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            Codigo = reader["Codigo"].ToString(),
                            IdTipoDocumento = reader["IdTipoDocumento"].ToString(),
                            pe_Naturaleza = reader["pe_Naturaleza"].ToString(),
                            pe_cedulaRuc = reader["pe_cedulaRuc"].ToString(),
                            pe_nombre = reader["pe_nombre"].ToString(),
                            pe_apellido = reader["pe_apellido"].ToString(),
                            pe_nombreCompleto = reader["pe_nombreCompleto"].ToString(),
                            pe_telfono_Contacto = reader["pe_telfono_Contacto"].ToString(),
                            Correo = string.IsNullOrEmpty(reader["Correo"].ToString()) ? null : reader["Correo"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            pe_sexo = reader["pe_sexo"].ToString(),
                            FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                            pe_fechaNacimiento = string.IsNullOrEmpty(reader["pe_fechaNacimiento"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["pe_fechaNacimiento"]),
                            CodCatalogoSangre = string.IsNullOrEmpty(reader["CodCatalogoSangre"].ToString()) ? null : reader["CodCatalogoSangre"].ToString(),
                            CodCatalogoCONADIS = string.IsNullOrEmpty(reader["CodCatalogoCONADIS"].ToString()) ? null : string.IsNullOrEmpty(reader["Correo"].ToString()) ? null : reader["CodCatalogoCONADIS"].ToString(),
                            NumeroCarnetConadis = string.IsNullOrEmpty(reader["NumeroCarnetConadis"].ToString()) ? null : reader["NumeroCarnetConadis"].ToString(),
                            PorcentajeDiscapacidad = string.IsNullOrEmpty(reader["PorcentajeDiscapacidad"].ToString()) ? (double?)null : Convert.ToDouble(reader["PorcentajeDiscapacidad"]),
                            NomCatalogoESTALU = reader["NomCatalogoESTALU"].ToString(),
                            NomCatalogoESTMAT = reader["NomCatalogoESTMAT"].ToString(),
                            IdCatalogoESTALU = Convert.ToInt32(reader["IdCatalogoESTALU"]),
                            IdCatalogoESTMAT = Convert.ToInt32(reader["IdCatalogoESTMAT"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            IdPais = reader["IdPais"].ToString(),
                            Cod_Region = reader["Cod_Region"].ToString(),
                            IdProvincia = reader["IdProvincia"].ToString(),
                            IdCiudad = reader["IdCiudad"].ToString(),
                            IdParroquia = reader["IdParroquia"].ToString(),
                            Sector = string.IsNullOrEmpty(reader["Sector"].ToString()) ? null : reader["Sector"].ToString(),
                            LugarNacimiento = string.IsNullOrEmpty(reader["LugarNacimiento"].ToString()) ? null : reader["LugarNacimiento"].ToString(),
                            IdReligion = string.IsNullOrEmpty(reader["IdReligion"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdReligion"]),
                            AsisteCentroCristiano = string.IsNullOrEmpty(reader["AsisteCentroCristiano"].ToString()) ? false : Convert.ToBoolean(reader["AsisteCentroCristiano"]),
                            IdGrupoEtnico = string.IsNullOrEmpty(reader["IdGrupoEtnico"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdGrupoEtnico"]),
                            Dificultad_Escritura = string.IsNullOrEmpty(reader["Dificultad_Escritura"].ToString()) ? false : Convert.ToBoolean(reader["Dificultad_Escritura"]),
                            Dificultad_Lectura = string.IsNullOrEmpty(reader["Dificultad_Lectura"].ToString()) ? false : Convert.ToBoolean(reader["Dificultad_Lectura"]),
                            Dificultad_Matematicas = string.IsNullOrEmpty(reader["Dificultad_Matematicas"].ToString()) ? false : Convert.ToBoolean(reader["Dificultad_Matematicas"]),
                            Celular = string.IsNullOrEmpty(reader["Celular"].ToString()) ? null : reader["Celular"].ToString(),
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


        public aca_Alumno_Info getInfo_Codigo(int IdEmpresa, string Codigo)
        {
            try
            {
                aca_Alumno_Info info= new aca_Alumno_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT a.IdEmpresa, a.IdAlumno, a.Codigo, a.IdPersona, p.pe_Naturaleza, p.pe_nombreCompleto, p.pe_apellido, p.pe_nombre, p.IdTipoDocumento, p.pe_cedulaRuc, a.Direccion, a.Celular, a.Correo, p.pe_sexo, p.pe_fechaNacimiento, "
                        + " p.CodCatalogoSangre, p.CodCatalogoCONADIS, p.PorcentajeDiscapacidad, p.NumeroCarnetConadis, a.Estado, a.IdCatalogoESTMAT, a.IdCurso, a.IdCatalogoESTALU, p.pe_telfono_Contacto, "
                        + " aca_Catalogo_1.NomCatalogo AS NomCatalogoESTMAT, c.NomCatalogo AS NomCatalogoESTALU, a.FechaIngreso, a.LugarNacimiento, a.IdPais, a.Cod_Region, a.IdProvincia, a.IdCiudad, a.IdParroquia, a.Sector, p.IdReligion, "
                        + " p.AsisteCentroCristiano, p.IdGrupoEtnico, a.Dificultad_Lectura, a.Dificultad_Escritura, a.Dificultad_Matematicas "
                        + " FROM     dbo.aca_Alumno AS a WITH (nolock)INNER JOIN "
                        + " dbo.tb_persona AS p WITH(nolock) ON a.IdPersona = p.IdPersona LEFT OUTER JOIN "
                        + " dbo.aca_Catalogo AS c WITH(nolock) ON a.IdCatalogoESTALU = c.IdCatalogo LEFT OUTER JOIN "
                        + " dbo.aca_Catalogo AS aca_Catalogo_1 WITH(nolock) ON a.IdCatalogoESTMAT = aca_Catalogo_1.IdCatalogo "
                        + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + "and a.Codigo = " + Codigo.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_Alumno_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdPersona = Convert.ToDecimal(reader["IdPersona"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                            //pe_cedulaRuc = Convert.ToString(reader["pe_cedulaRuc"])
                        };
                    }
                }
                /*
                using (EntitiesAcademico db = new EntitiesAcademico())
                {
                    var Entity = db.vwaca_Alumno.Where(q => q.IdEmpresa == IdEmpresa && q.Codigo == Codigo).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new aca_Alumno_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        pe_nombreCompleto = Entity.pe_nombreCompleto,
                        IdAlumno = Entity.IdAlumno,
                        IdPersona = Entity.IdPersona,
                        Codigo = Entity.Codigo
                    };
                }
                */
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public aca_Alumno_Info getInfo(int IdEmpresa, decimal IdAlumno, string pe_cedulaRucFamiliar)
        {
            try
            {
                aca_Alumno_Info info = new aca_Alumno_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT a.IdEmpresa, a.IdAlumno, a.Codigo, a.IdPersona, p.pe_Naturaleza, p.pe_nombreCompleto, p.pe_apellido, p.pe_nombre, p.IdTipoDocumento, p.pe_cedulaRuc, a.Direccion, a.Celular, a.Correo, p.pe_sexo, p.pe_fechaNacimiento, "
                        + " p.CodCatalogoSangre, p.CodCatalogoCONADIS, p.PorcentajeDiscapacidad, p.NumeroCarnetConadis, a.Estado, a.IdCatalogoESTMAT, a.IdCurso, a.IdCatalogoESTALU, p.pe_telfono_Contacto, "
                        + " aca_Catalogo_1.NomCatalogo AS NomCatalogoESTMAT, c.NomCatalogo AS NomCatalogoESTALU, a.FechaIngreso, a.LugarNacimiento, a.IdPais, a.Cod_Region, a.IdProvincia, a.IdCiudad, a.IdParroquia, a.Sector, p.IdReligion, "
                        + " p.AsisteCentroCristiano, p.IdGrupoEtnico, a.Dificultad_Lectura, a.Dificultad_Escritura, a.Dificultad_Matematicas "
                        + " FROM     dbo.aca_Alumno AS a WITH (nolock)INNER JOIN "
                        + " dbo.tb_persona AS p WITH(nolock) ON a.IdPersona = p.IdPersona LEFT OUTER JOIN "
                        + " dbo.aca_Catalogo AS c WITH(nolock) ON a.IdCatalogoESTALU = c.IdCatalogo LEFT OUTER JOIN "
                        + " dbo.aca_Catalogo AS aca_Catalogo_1 WITH(nolock) ON a.IdCatalogoESTMAT = aca_Catalogo_1.IdCatalogo "
                        + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + "and a.IdAlumno = " + IdAlumno.ToString() + "and p.pe_cedulaRuc = " + pe_cedulaRucFamiliar.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_Alumno_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            pe_nombre = string.IsNullOrEmpty(reader["pe_nombre"].ToString()) ? null : reader["pe_nombre"].ToString(),
                            pe_apellido = string.IsNullOrEmpty(reader["pe_apellido"].ToString()) ? null : reader["pe_apellido"].ToString(),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdPersona = Convert.ToDecimal(reader["IdPersona"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                            pe_cedulaRuc = Convert.ToString(reader["pe_cedulaRuc"])
                        };
                    }
                }
                /*
                using (EntitiesAcademico db = new EntitiesAcademico())
                {
                    var Entity = db.vwaca_Alumno.Where(q => q.IdEmpresa == IdEmpresa && q.IdAlumno == IdAlumno && q.pe_cedulaRuc== pe_cedulaRucFamiliar).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new aca_Alumno_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        pe_nombre = Entity.pe_nombre,
                        pe_apellido = Entity.pe_apellido,
                        pe_nombreCompleto = Entity.pe_nombreCompleto,
                        IdAlumno = Entity.IdAlumno,
                        IdPersona = Entity.IdPersona,
                        pe_cedulaRuc = Entity.pe_cedulaRuc,
                        Codigo = Entity.Codigo
                    };
                }
                */
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public aca_Alumno_Info get_info_x_num_cedula(int IdEmpresa, string pe_cedulaRuc)
        {
            try
            {
                aca_Alumno_Info info = new aca_Alumno_Info();

                EntitiesGeneral Context_general = new EntitiesGeneral();
                tb_persona Entity_per = Context_general.tb_persona.Where(q => q.pe_cedulaRuc == pe_cedulaRuc).FirstOrDefault();
                if (Entity_per == null)
                {
                    Context_general.Dispose();
                    return info;
                }

                var Entity_aca = getInfo_IdPersona(IdEmpresa, Convert.ToDecimal(Entity_per.IdPersona));
                /*
                EntitiesAcademico Context_academico = new EntitiesAcademico();
                var Entity_aca = Context_academico.vwaca_Alumno.Where(q => q.IdEmpresa == IdEmpresa && q.IdPersona == Entity_per.IdPersona).FirstOrDefault();
                */
                if (Entity_aca == null)
                {
                    info.IdPersona = Entity_per.IdPersona;
                    info.Direccion = Entity_per.pe_direccion;
                    info.Correo = Entity_per.pe_correo;
                    info.Celular = Entity_per.pe_celular;
                    info.pe_sexo = Entity_per.pe_sexo;
                    info.pe_Naturaleza = Entity_per.pe_Naturaleza;
                    info.IdTipoDocumento = Entity_per.IdTipoDocumento;
                    info.pe_apellido = Entity_per.pe_apellido;
                    info.pe_nombre = Entity_per.pe_nombre;
                    info.pe_nombreCompleto = Entity_per.pe_nombreCompleto;
                    info.pe_telfono_Contacto = Entity_per.pe_telfono_Contacto;
                    info.CodCatalogoSangre = Entity_per.CodCatalogoSangre;
                    info.CodCatalogoCONADIS = Entity_per.CodCatalogoCONADIS;
                    info.NumeroCarnetConadis = Entity_per.NumeroCarnetConadis;
                    info.PorcentajeDiscapacidad = Entity_per.PorcentajeDiscapacidad;
                    info.pe_fechaNacimiento = Entity_per.pe_fechaNacimiento;
                    info.IdReligion = Entity_per.IdReligion;
                    info.AsisteCentroCristiano = Entity_per.AsisteCentroCristiano;
                    info.IdGrupoEtnico = Entity_per.IdGrupoEtnico;

                    Context_general.Dispose();
                    //Context_academico.Dispose();
                    return info;
                }

                info = new aca_Alumno_Info
                {
                    IdEmpresa = Entity_aca.IdEmpresa,
                    Codigo = Entity_aca.Codigo,
                    IdAlumno = Entity_aca.IdAlumno,
                    Direccion = Entity_aca.Direccion,
                    Correo = Entity_aca.Correo,
                    Celular = Entity_aca.Celular,
                    IdPersona = Entity_aca.IdPersona,
                    pe_apellido = Entity_aca.pe_apellido,
                    pe_nombre = Entity_aca.pe_nombre,
                    pe_Naturaleza = Entity_aca.pe_Naturaleza,
                    IdTipoDocumento = Entity_aca.IdTipoDocumento,
                    pe_cedulaRuc = Entity_aca.pe_cedulaRuc,
                    pe_nombreCompleto = Entity_aca.pe_nombreCompleto,
                    pe_telfono_Contacto = Entity_aca.pe_telfono_Contacto,
                    CodCatalogoSangre = Entity_aca.CodCatalogoSangre,
                    CodCatalogoCONADIS = Entity_aca.CodCatalogoCONADIS,
                    NumeroCarnetConadis = Entity_aca.NumeroCarnetConadis,
                    PorcentajeDiscapacidad = Entity_aca.PorcentajeDiscapacidad,
                    LugarNacimiento = Entity_aca.LugarNacimiento,
                    IdPais = Entity_aca.IdPais,
                    Cod_Region = Entity_aca.Cod_Region,
                    IdProvincia = Entity_aca.IdProvincia,
                    IdCiudad = Entity_aca.IdCiudad,
                    IdParroquia = Entity_aca.IdParroquia,
                    Sector = Entity_aca.Sector,
                    pe_fechaNacimiento = Entity_aca.pe_fechaNacimiento,
                    pe_sexo = Entity_aca.pe_sexo,
                    IdReligion = Entity_aca.IdReligion,
                    IdGrupoEtnico = Entity_aca.IdGrupoEtnico,
                    AsisteCentroCristiano = Entity_aca.AsisteCentroCristiano
                };

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public decimal getId(int IdEmpresa)
        {
            try
            {
                decimal ID = 1;

                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var cont = Context.aca_Alumno.Where(q => q.IdEmpresa == IdEmpresa).Count();
                    if (cont > 0)
                        ID = Context.aca_Alumno.Where(q => q.IdEmpresa == IdEmpresa).Max(q => q.IdAlumno) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(aca_Alumno_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    //var anio = info.FechaIngreso.Year;
                    var anio = data_Anio.getInfo(info.IdEmpresa, info.IdAnioRegistro);
                    var FechaIngreso = anio.FechaDesde;
                    var lista = getList(info.IdEmpresa, true);
                    var ListaAnio = lista.Where(q => (q.AnioCodigo == anio.FechaDesde.Year.ToString())).ToList();
                    var NumEstudiante = 1;
                    var Codigo = "";

                    if (ListaAnio.Count == 0)
                    {
                        Codigo = FechaIngreso.Year + NumEstudiante.ToString("0000");
                    }
                    else
                    {
                        NumEstudiante = Convert.ToInt32(ListaAnio.Max(q => q.Codigo)) + 1;
                        Codigo = NumEstudiante.ToString("0000");
                    }

                    if (info.Codigo=="" || info.Codigo==null)
                    {
                        info.Codigo = Codigo;
                    }

                    aca_Alumno Entity = new aca_Alumno
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdAlumno = info.IdAlumno = getId(info.IdEmpresa),
                        IdPersona = info.IdPersona,
                        Codigo = info.Codigo,
                        Estado = true,
                        Correo = info.Correo,
                        Direccion = info.Direccion,
                        Celular = info.Celular,
                        FechaIngreso = info.FechaIngreso,
                        IdCatalogoESTALU = info.IdCatalogoESTALU,
                        IdCatalogoESTMAT = info.IdCatalogoESTMAT,
                        IdPais = info.IdPais,
                        Cod_Region = info.Cod_Region,
                        IdProvincia = info.IdProvincia,
                        IdCiudad = info.IdCiudad,
                        IdParroquia = info.IdParroquia,
                        Sector = info.Sector,
                        LugarNacimiento = info.LugarNacimiento,
                        Dificultad_Lectura=info.Dificultad_Lectura,
                        Dificultad_Escritura=info.Dificultad_Escritura,
                        Dificultad_Matematicas = info.Dificultad_Matematicas,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = info.FechaCreacion = DateTime.Now
                    };
                    Context.aca_Alumno.Add(Entity);

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(aca_Alumno_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Alumno Entity = Context.aca_Alumno.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAlumno == info.IdAlumno);
                    if (Entity == null)
                        return false;
                    Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
                    Entity.FechaModificacion = info.FechaModificacion = DateTime.Now;
                    Entity.Correo = info.Correo;
                    Entity.Direccion = info.Direccion;
                    Entity.Celular = info.Celular;
                    Entity.IdPais = info.IdPais;
                    Entity.Cod_Region = info.Cod_Region;
                    Entity.IdProvincia = info.IdProvincia;
                    Entity.IdCiudad = info.IdCiudad;
                    Entity.IdParroquia = info.IdParroquia;
                    Entity.Sector = info.Sector;
                    Entity.LugarNacimiento = info.LugarNacimiento;
                    Entity.Dificultad_Lectura = info.Dificultad_Lectura;
                    Entity.Dificultad_Escritura = info.Dificultad_Escritura;
                    Entity.Dificultad_Matematicas = info.Dificultad_Matematicas;

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(aca_Alumno_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Alumno Entity = Context.aca_Alumno.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAlumno == info.IdAlumno);
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

        public bool paseAnioDB(aca_Alumno_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Alumno Entity = Context.aca_Alumno.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAlumno == info.IdAlumno);
                    if (Entity == null)
                        return false;

                    Entity.IdCatalogoESTALU = info.IdCatalogoESTALU;
                    Entity.IdCatalogoESTMAT = info.IdCatalogoESTMAT;
                    Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
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
