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
                        + " DECLARE @IdEmpresa int = " + IdEmpresa.ToString() + ", "
                        + " @IdAlumno numeric(18,0) = " + IdAlumno.ToString() + ","
                        + " @IdAnio int = " + IdAnio.ToString() + ","
                        + " @IdSede int = " + IdSede.ToString() + ","
                        + " @IdNivel int = " + IdNivel.ToString() + ","
                        + " @IdJornada int = " + IdJornada.ToString() + ","
                        + " @IdCurso int = " + IdCurso.ToString() + ","
                        + " @IdParalelo int = " + IdParalelo.ToString() + ","

                        + " @FechaCorte date = datefromparts(" + FechaCorte.Year.ToString() + "," + FechaCorte.Month.ToString() + "," + FechaCorte.Day.ToString() + "),"
                        + " @CantIni INT = " + CantMin.ToString() + ","
                        + " @CantFin INT = " + CantMax.ToString()
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
                        + " select a.IdEmpresa,a.IdSucursal,a.IdBodega,a.IdCbteVta, a.vt_tipoDoc, a.vt_fecha, a.vt_serie1+'-'+a.vt_serie2+'-'+ a.vt_NumFactura vt_NumFactura, a.vt_Observacion, a.IdAlumno, dbo.BankersRounding(b.Total - isnull(e.dc_valorPago,0),2) Saldo, year(f.FechaDesde) as Periodo, c.IdMatricula,"
                        + " g.Codigo, h.pe_nombreCompleto, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, f.Descripcion Anio,"
                        + " sn.NomSede, sn.NomNivel, nj.NomJornada, jc.NomCurso, cp.NomParalelo,"
                        + " sn.OrdenNivel, nj.OrdenJornada, jc.OrdenCurso, cp.OrdenParalelo"
                        + " from fa_factura as a with(nolock) join"
                        + " fa_factura_resumen as b with(nolock) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdCbteVta = b.IdCbteVta join"
                        + " aca_Matricula_Rubro as c with(nolock) on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega and a.IdCbteVta = c.IdCbteVta join"
                        + " #TempDatosAlumnosCXC_008 as d with (nolock) on c.IdEmpresa = d.IdEmpresa and c.IdMatricula = d.IdMatricula left join"
                        + " ("
                            + " select b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota, sum(b.dc_ValorPago) dc_ValorPago"
                            + " from cxc_cobro as a with(nolock) join"
                            + " cxc_cobro_det as b with(nolock) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro"
                            + " where a.cr_estado = 'A' and b.dc_TipoDocumento = 'FACT' and a.cr_fecha <= @FechaCorte"
                            + " group by b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota"
                        + " ) as e on a.IdEmpresa = e.IdEmpresa and a.IdSucursal = e.IdSucursal and a.IdBodega = e.IdBodega_Cbte and a.IdCbteVta = e.IdCbte_vta_nota left join"
                        + " aca_AnioLectivo as f with(nolock) on f.IdEmpresa = c.IdEmpresa and f.IdAnio = c.IdAnio left join"
                        + " aca_Alumno as g with(nolock) on a.IdEmpresa = g.IdEmpresa and a.IdAlumno = g.IdAlumno left join"
                        + " tb_persona as h with(nolock) on h.IdPersona = g.IdPersona left join"
                        + " aca_matricula as i with(nolock) on i.IdEmpresa = c.IdEmpresa and i.IdMatricula = c.IdMatricula left join"
                        + " aca_AnioLectivo_Sede_NivelAcademico as sn with(nolock)on i.IdEmpresa = sn.IdEmpresa and i.IdAnio = sn.IdAnio and i.IdSede = sn.IdSede and i.IdNivel = sn.IdNivel left join"
                        + " aca_AnioLectivo_NivelAcademico_Jornada as nj with(nolock) on i.IdEmpresa = nj.IdEmpresa and i.IdAnio = nj.IdAnio and i.IdSede = nj.IdSede and i.IdJornada = nj.IdJornada and i.IdNivel = nj.IdNivel left join"
                        + " aca_AnioLectivo_Jornada_Curso as jc with(nolock) on i.IdEmpresa = jc.IdEmpresa and i.IdAnio = jc.IdAnio and i.IdSede = jc.IdSede and i.IdNivel = jc.IdNivel and i.IdJornada = jc.IdJornada and i.IdCurso = jc.IdCurso left join"
                        + " aca_AnioLectivo_Curso_Paralelo as cp with(nolock)on i.IdEmpresa = cp.IdEmpresa and i.IdAnio = cp.IdAnio and i.IdSede = cp.IdSede and i.IdNivel = cp.IdNivel and i.IdJornada = cp.IdJornada and i.IdCurso = cp.IdCurso and i.IdParalelo = cp.IdParalelo"
                        + " where dbo.BankersRounding(b.Total - isnull(e.dc_valorPago, 0), 2) > 0";
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
                            Saldo = Convert.ToDouble(reader["Saldo"]),
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
