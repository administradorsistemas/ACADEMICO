using Core.Data.Base;
using Core.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.CuentasPorCobrar
{
    public class CXC_008_Data
    {
        public List<CXC_008_Info> GetList(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, decimal IdAlumno, DateTime FechaCorte, int CantMin, int CantMax)
        {
            try
            {
                List<CXC_008_Info> Lista = new List<CXC_008_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = ""
                        + " IF OBJECT_ID('tempdb..#TempDatosAlumnosCXC_008') IS NOT NULL DROP TABLE #TempDatosAlumnosCXC_008"
                        + " create table #TempDatosAlumnosCXC_008"
                        + " ("
                        + " id int primary key identity(1,1),"
                        + " IdEmpresa int not null,"
                        + " IdMatricula numeric not null"
                        + " )"
                        + " DECLARE @IdEmpresa int = "+IdEmpresa.ToString()+", "
                        + " @IdAlumno numeric(18,0) = "+IdAlumno.ToString()+","
                        + " @IdAnio int = "+IdAnio.ToString()+","
                        + " @IdSede int = "+IdSede.ToString()+","
                        + " @IdNivel int = "+IdNivel.ToString()+","
                        + " @IdJornada int = "+IdJornada.ToString()+","
                        + " @IdCurso int = "+IdCurso.ToString()+","
                        + " @IdParalelo int = "+IdParalelo.ToString()+","

                        + " @FechaCorte date = datefromparts("+FechaCorte.Year.ToString()+","+FechaCorte.Month.ToString()+","+FechaCorte.Day.ToString()+"),"
                        + " @CantIni INT = "+CantMin.ToString()+","
                        + " @CantFin INT = "+CantMax.ToString()
                        + " if(ISNULL(@IdAlumno,0) <> 0)"
                        + " begin"
                            + " insert into #TempDatosAlumnosCXC_008 (IdEmpresa, IdMatricula)"
                            + " select a.IdEmpresa, max(a.IdMatricula) IdMatricula from aca_Matricula as a  with (nolock)"
                            + " where not exists("
                            + " select X.IdEmpresa from aca_AlumnoRetiro as x "
                            + " where x.Estado = 1 and a.IdEmpresa = x.IdEmpresa and a.IdMatricula = x.IdMatricula"
                            + " ) and a.IdEmpresa = @IdEmpresa and a.IdAlumno = @IdAlumno"
                            + " GROUP BY a.IdEmpresa"
                        + " end"
                        + " if(ISNULL(@IdAlumno,0) = 0)"
                        + " BEGIN"
                            + " insert into #TempDatosAlumnosCXC_008 (IdEmpresa, IdMatricula)"
                            + " select a.IdEmpresa, a.IdMatricula from aca_Matricula as a  with (nolock) JOIN"
                            + " ("
                            + " SELECT XX.IdEmpresa, XX.IdAlumno, COUNT(1) Cont"
                            + " FROM("
                                + " select c.IdEmpresa, c.IdAlumno, c.IdSucursal, c.IdBodega, c.IdCbteVta, c.vt_tipoDoc, a.Total"
                                + " from fa_factura_resumen as a with (nolock) INNER JOIN"
                                + " fa_factura AS c with (nolock) on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega and a.IdCbteVta = c.IdCbteVta left join"
                                + " cxc_cobro_det as b with (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega_Cbte and a.IdCbteVta = b.IdCbte_vta_nota and b.dc_TipoDocumento = 'FACT' AND B.estado = 'A' "
                                + " WHERE A.IdEmpresa = @IdEmpresa  AND C.Estado = 'A' and c.IdAlumno is not null and c.vt_fecha <= @FechaCorte"
                                + " group by c.IdEmpresa, c.IdAlumno, c.IdSucursal, c.IdBodega, c.IdCbteVta, c.vt_tipoDoc, a.Total"
                                + " having dbo.BankersRounding(a.Total - isnull(sum(b.dc_ValorPago),0),2) > 0"
                                + " union all"
                                + " select c.IdEmpresa, c.IdAlumno, c.IdSucursal, c.IdBodega, c.IdNota, c.CodDocumentoTipo, a.Total"
                                + " from fa_notaCreDeb_resumen as a with (nolock) INNER JOIN"
                                + " fa_notaCreDeb AS c with (nolock) on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega and a.IdNota = c.IdNota left join"
                                + " cxc_cobro_det as b with (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega_Cbte and a.IdNota = b.IdCbte_vta_nota and b.dc_TipoDocumento = 'NTDB' AND B.estado = 'A' "
                                + " WHERE A.IdEmpresa = @IdEmpresa AND C.Estado = 'A' and c.IdAlumno is not null and c.CreDeb= 'D' and c.no_fecha <= @FechaCorte"
                                + " group by c.IdEmpresa, c.IdAlumno, c.IdSucursal, c.IdBodega, c.IdNota, c.CodDocumentoTipo, a.Total"
                                + " having dbo.BankersRounding(a.Total - isnull(sum(b.dc_ValorPago),0),2) > 0"
                            + " ) XX GROUP BY XX.IdEmpresa, XX.IdAlumno "
                            + " HAVING COUNT(1) BETWEEN @CantIni AND @CantFin "
                            + " ) AS B On a.IdEmpresa = b.IdEmpresa and a.IdAlumno = b.IdAlumno"
                            + " where not exists("
                            + " select X.IdEmpresa from aca_AlumnoRetiro as x "
                            + " where x.Estado = 1 and a.IdEmpresa = x.IdEmpresa and a.IdMatricula = x.IdMatricula"
                            + " ) and a.IdEmpresa = @IdEmpresa "
                            + " and a.IdAnio = @IdAnio "
                            + " AND A.IdSede = CASE WHEN @IdSede = 0 THEN A.IdSede ELSE @IdSede END"
                            + " AND A.IdNivel = CASE WHEN @IdNivel = 0 THEN A.IdNivel ELSE @IdNivel END"
                            + " AND A.IdJornada = CASE WHEN @IdJornada = 0 THEN A.IdJornada ELSE @IdJornada END"
                            + " AND A.IdCurso = CASE WHEN @IdCurso = 0 THEN A.IdCurso ELSE @IdCurso END"
                            + " AND A.IdParalelo = CASE WHEN @IdParalelo = 0 THEN A.IdParalelo ELSE @IdParalelo END"
                        + " END"
                        + " select a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdCbteVta, a.vt_tipoDoc, a.vt_fecha, a.Referencia vt_NumFactura, a.vt_Observacion, a.IdAlumno, a.Saldo, a.Periodo, "
                        + " b.IdMatricula, b.Codigo, b.pe_nombreCompleto, b.IdAnio, b.IdSede, b.IdNivel, b.IdJornada, b.IdCurso, b.IdParalelo, b.Descripcion as Anio, NomSede, NomNivel, NomJornada, NomCurso, NomParalelo, OrdenNivel, OrdenJornada, OrdenCurso, OrdenParalelo"
                        + " from("
                        + " select a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdCbteVta, a.vt_tipoDoc, a.vt_fecha, a.vt_serie1+'-'+a.vt_serie2+'-'+a.vt_NumFactura as Referencia, a.vt_Observacion, a.IdAlumno,dbo.BankersRounding(b.Total - isnull(c.dc_valorPago,0),2) as Saldo,"
                        + " year(d.FechaDesde) as Periodo"
                        + " from fa_factura as a with (nolock) join"
                        + " fa_factura_resumen as b with (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdCbteVta = b.IdCbteVta left join"
                        + " ("
                            + " select a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota, a.dc_TipoDocumento, sum(a.dc_ValorPago) dc_ValorPago"
                            + " from cxc_cobro_det as a with (nolock) join"
                            + " cxc_cobro as b with (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro"
                            + " where a.estado = 'A' AND B.cr_estado = 'A' and b.cr_fecha <= @FechaCorte"
                            + " group by a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota, a.dc_TipoDocumento"
                        + " ) AS c on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega_Cbte and a.IdCbteVta = c.IdCbte_vta_nota and a.vt_tipoDoc = c.dc_TipoDocumento left join"
                        + " aca_AnioLectivo as d with (nolock) on b.IdEmpresa = d.IdEmpresa and b.IdAnio = d.IdAnio"
                        + " where a.Estado = 'A' and dbo.BankersRounding(b.Total - isnull(c.dc_valorPago,0),2) > 0 and a.IdAlumno is not null and a.vt_fecha <= @FechaCorte and a.IdEmpresa = @IdEmpresa"
                        + " UNION ALL"
                        + " select a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdNota, a.CodDocumentoTipo, a.no_fecha, isnull(a.Serie1+'-'+a.Serie2+'-'+a.NumNota_Impresa,a.CodNota) as Referencia, a.sc_observacion, a.IdAlumno,"
                        + " dbo.BankersRounding(b.Total - isnull(c.dc_valorPago,0),2) as Saldo,"
                        + " isnull(year(d.FechaDesde),year(a.no_fecha)) as Periodo"
                        + " from fa_notaCreDeb as a with (nolock) join"
                        + " fa_notaCreDeb_resumen as b with (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdNota = b.IdNota left join"
                        + " ("
                            + " select a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota, a.dc_TipoDocumento, sum(a.dc_ValorPago) dc_ValorPago"
                            + " from cxc_cobro_det as a with (nolock) join"
                            + " cxc_cobro as b with (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro"
                            + " where a.estado = 'A' AND B.cr_estado = 'A' and b.cr_fecha <= @FechaCorte"
                            + " group by a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota, a.dc_TipoDocumento"
                        + " ) AS c on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega_Cbte and a.IdNota = c.IdCbte_vta_nota and a.CodDocumentoTipo = c.dc_TipoDocumento left join"
                        + " aca_AnioLectivo as d with (nolock) on b.IdEmpresa = d.IdEmpresa and b.IdAnio = d.IdAnio"
                        + " where a.Estado = 'A' and dbo.BankersRounding(b.Total - isnull(c.dc_valorPago,0),2) > 0 and a.IdAlumno is not null and a.CreDeb = 'D' and a.no_fecha <= @FechaCorte and a.IdEmpresa = @IdEmpresa"
                        + " ) a join"
                        + " ("
                            + " select a.IdEmpresa, a.IdMatricula, a.IdAlumno, c.Codigo, d.pe_nombreCompleto,"
                            + " a.IdAnio, a.IdSede, a.IdNivel, a.IdJornada, a.IdCurso, a.IdParalelo,"
                            + " b.Descripcion, sn.NomSede, sn.NomNivel, nj.NomJornada, jc.NomCurso, cp.NomParalelo,"
                            + " sn.OrdenNivel, nj.OrdenJornada, jc.OrdenCurso, cp.OrdenParalelo"
                            + " from aca_Matricula as a with (nolock) left join"
                            + " aca_AnioLectivo as b with (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdAnio = b.IdAnio left join"
                            + " aca_Alumno as c with (nolock) on a.IdEmpresa = c.IdEmpresa and a.IdAlumno = c.IdAlumno left join"
                            + " tb_persona as d with (nolock) on c.IdPersona = d.IdPersona left join"
                            + " aca_AnioLectivo_Sede_NivelAcademico as sn with (nolock)on a.IdEmpresa = sn.IdEmpresa and a.IdAnio = sn.IdAnio and a.IdSede = sn.IdSede and a.IdNivel = sn.IdNivel left join"
                            + " aca_AnioLectivo_NivelAcademico_Jornada as nj with (nolock) on a.IdEmpresa = nj.IdEmpresa and a.IdAnio = nj.IdAnio and a.IdSede = nj.IdSede and a.IdJornada = nj.IdJornada and a.IdNivel = nj.IdNivel left join"
                            + " aca_AnioLectivo_Jornada_Curso as jc with (nolock) on a.IdEmpresa = jc.IdEmpresa and a.IdAnio = jc.IdAnio and a.IdSede = jc.IdSede and a.IdNivel = jc.IdNivel and a.IdJornada = jc.IdJornada and a.IdCurso = jc.IdCurso left join"
                            + " aca_AnioLectivo_Curso_Paralelo as cp with (nolock)on a.IdEmpresa = cp.IdEmpresa and a.IdAnio = cp.IdAnio and a.IdSede = cp.IdSede and a.IdNivel = cp.IdNivel and a.IdJornada = cp.IdJornada and a.IdCurso = cp.IdCurso and a.IdParalelo = cp.IdParalelo"
                            + " JOIN #TempDatosAlumnosCXC_008 AS x on a.IdEmpresa = x.IdEmpresa and a.IdMatricula = x.IdMatricula"
                        + " )as b on a.IdEmpresa = b.IdEmpresa and a.IdAlumno = b.IdAlumno";
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new CXC_008_Info
                        {
                            Num = 1,
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            IdBodega = Convert.ToInt32(reader["IdBodega"]),
                            IdCbteVta = Convert.ToInt32(reader["IdCbteVta"]),
                            vt_fecha = Convert.ToDateTime(reader["vt_fecha"]),
                            vt_Observacion = Convert.ToString(reader["vt_Observacion"]),
                            vt_NumFactura = Convert.ToString(reader["vt_NumFactura"]),
                            IdAlumno = Convert.ToInt32(reader["IdAlumno"]),
                            CodigoAlumno = Convert.ToString(reader["Codigo"]),
                            pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            Periodo = Convert.ToInt32(reader["Periodo"]),
                            Saldo = Convert.ToInt32(reader["Saldo"]),
                            IdMatricula = Convert.ToInt32(reader["IdMatricula"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            OrdenNivel = Convert.ToInt32(reader["OrdenNivel"]),
                            OrdenJornada = Convert.ToInt32(reader["OrdenJornada"]),
                            OrdenCurso = Convert.ToInt32(reader["OrdenCurso"]),
                            OrdenParalelo = Convert.ToInt32(reader["OrdenParalelo"]),
                            NomSede = Convert.ToString(reader["NomSede"]),
                            NomNivel = Convert.ToString(reader["NomNivel"]),
                            NomJornada = Convert.ToString(reader["NomJornada"]),
                            NomCurso = Convert.ToString(reader["NomCurso"]),
                            NomParalelo = Convert.ToString(reader["NomParalelo"]),
                            Anio = Convert.ToString(reader["Anio"])
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
