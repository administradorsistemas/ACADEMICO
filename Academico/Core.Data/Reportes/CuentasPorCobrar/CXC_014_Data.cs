using Core.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.CuentasPorCobrar
{
    public class CXC_014_Data
    {
        public List<CXC_014_Info> get_list(int IdEmpresa, int IdAnio, int IdSede, int IdJornada, int IdNivel, int IdCurso, int IdParalelo, decimal IdAlumno)
        {
            try
            {
                List<CXC_014_Info> Lista = new List<CXC_014_Info>();
                List<CXC_014_Info> ListaJoin = new List<CXC_014_Info>();
                List<CXC_014_Info> ListaFinal = new List<CXC_014_Info>();

                int IdNivelIni = IdNivel;
                int IdNivelFin = IdNivel == 0 ? 9999999 : IdNivel;

                int IdCursoIni = IdCurso;
                int IdCursoFin = IdCurso == 0 ? 9999999 : IdCurso;

                int IdParaleloIni = IdParalelo;
                int IdParaleloFin = IdParalelo == 0 ? 9999999 : IdParalelo;

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    #region Query
         /*
                    string query = "DECLARE @IdEmpresa int = " + IdEmpresa + ","
                                + " @IdAnio int = " + IdAnio + ","
                                + " @IdSede int = " + IdSede + ","
                                + " @IdJornada int = " + IdJornada + ","
                                + " @IdNivelIni int = " + IdNivelIni + ","
                                + " @IdNivelFin int = " + IdNivelFin + ","
                                + " @IdCursoIni int = " + IdCursoIni + ","
                                + " @IdCursoFin int = " + IdCursoFin + ","
                                + " @IdParaleloIni int = " + IdParaleloIni + ","
                                + " @IdParaleloFin int = " + IdParaleloFin + ","
                                + " @IdAlumno int = " + IdAlumno
                                + " select a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdCbteVta, a.vt_tipoDoc, b.Total, isnull(e.dc_ValorPago,0) as dc_ValorPago, dbo.BankersRounding(b.Total - isnull(e.dc_ValorPago,0),2) Saldo,"
                                + " a.vt_Observacion, a.vt_fecha, a.IdAlumno, f.IdAnio, f.IdSede, f.IdJornada, f.IdNivel, f.IdCurso, f.IdParalelo,f.OrdenJornada, f.OrdenNivel, f.OrdenCurso, f.OrdenParalelo, f.NomNivel, f.NomJornada, f.NomCurso, f.NomParalelo, D.pe_nombreCompleto AS Alumno, f.pe_nombreCompleto as Representante, c.Codigo,"
                                + " CASE WHEN ar.AplicaProntoPago = 1 AND ap.FechaProntoPago >= CAST(GETDATE() AS DATE)"
                                + " THEN b.ValorProntoPago ELSE b.Total END AS ValorProntoPago, ap.FechaProntoPago AS FechaProntoPago"
                                + " from fa_factura as a inner join"
                                + " fa_factura_resumen as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdCbteVta = b.IdCbteVta inner join"
                                + " aca_Alumno as c on a.IdEmpresa = c.IdEmpresa and a.IdAlumno = c.IdAlumno inner join"
                                + " tb_persona as d on c.IdPersona = d.IdPersona left join"
                                + " ("
                                + " select b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota, sum(b.dc_ValorPago) dc_ValorPago"
                                + " from cxc_cobro as a inner join"
                                + " cxc_cobro_det as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro"
                                + " where a.cr_estado = 'A' AND B.estado = 'A' AND B.dc_TipoDocumento = 'FACT' and a.IdEmpresa = " + IdEmpresa.ToString()
                                + " group by b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota"
                                + " ) e on a.IdEmpresa = e.IdEmpresa and a.IdSucursal = e.IdSucursal and a.IdBodega = e.IdBodega_Cbte and a.IdCbteVta = e.IdCbte_vta_nota left join"
                                + " ("
                                + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAlumno,  m.IdAnio, m.IdSede, m.IdJornada, m.IdNivel, m.IdCurso, m.IdParalelo,nj.OrdenJornada, sn.OrdenNivel, jc.OrdenCurso, cp.OrdenParalelo, sn.NomNivel, nj.NomJornada, jc.NomCurso, cp.NomParalelo, r.pe_nombreCompleto"
                                + " FROM     aca_AnioLectivo_Sede_NivelAcademico AS sn INNER JOIN"
                                + " aca_AnioLectivo AS a ON sn.IdEmpresa = a.IdEmpresa AND sn.IdAnio = a.IdAnio RIGHT OUTER JOIN"
                                + " aca_Matricula AS m LEFT OUTER JOIN"
                                + " aca_AnioLectivo_Curso_Paralelo AS cp ON m.IdEmpresa = cp.IdEmpresa AND m.IdAnio = cp.IdAnio AND m.IdSede = cp.IdSede AND m.IdNivel = cp.IdNivel AND m.IdJornada = cp.IdJornada AND m.IdCurso = cp.IdCurso AND"
                                + " m.IdParalelo = cp.IdParalelo LEFT OUTER JOIN"
                                + " aca_AnioLectivo_Jornada_Curso AS jc ON cp.IdEmpresa = jc.IdEmpresa AND cp.IdAnio = jc.IdAnio AND cp.IdSede = jc.IdSede AND cp.IdNivel = jc.IdNivel AND cp.IdJornada = jc.IdJornada AND cp.IdCurso = jc.IdCurso LEFT OUTER JOIN"
                                + " aca_AnioLectivo_NivelAcademico_Jornada AS nj ON jc.IdEmpresa = nj.IdEmpresa AND jc.IdAnio = nj.IdAnio AND jc.IdSede = nj.IdSede AND jc.IdNivel = nj.IdNivel AND jc.IdJornada = nj.IdJornada ON sn.IdEmpresa = nj.IdEmpresa AND"
                                + " sn.IdAnio = nj.IdAnio AND sn.IdSede = nj.IdSede AND sn.IdNivel = nj.IdNivel left join"
                                + " tb_persona as r on r.IdPersona = m.IdPersonaR"
                                + " WHERE M.IdEmpresa = @IdEmpresa ";
                                if (IdAlumno != 0)
                                {
                                    query += " and M.IdAlumno= @IdAlumno ";
                                }
                                else
                                {
                                    query += " and M.IdAnio = @IdAnio and M.IdSede = @IdSede and M.IdJornada = @IdJornada "
                                    + " and M.IdNivel between @IdNivelIni and @IdNivelFin "
                                    + " and M.IdCurso between @IdCursoIni and @IdCursoFin "
                                    + " and M.IdParalelo between @IdParaleloIni and @IdParaleloFin ";
                                }
                                query += " AND(a.EnCurso = 1) "
                                + " AND(NOT EXISTS "
                                + " (SELECT IdEmpresa "
                                + " FROM      aca_AlumnoRetiro AS f "
                                + " WHERE(m.IdEmpresa = IdEmpresa) AND(m.IdMatricula = IdMatricula) AND(Estado = 1))) "
                                + " ) as f on a.IdEmpresa = f.IdEmpresa and a.IdAlumno = f.IdAlumno left join"
                                + " aca_AnioLectivo_Rubro as ar on b.IdEmpresa = ar.IdEmpresa and b.IdAnio = ar.IdAnio and b.IdRubro = ar.IdRubro left join"
                                + " aca_AnioLectivo_Periodo as ap on b.IdEmpresa = ap.IdEmpresa and b.IdPeriodo = ap.IdPeriodo"
                                + " where a.IdEmpresa = @IdEmpresa ";
                                if (IdAlumno != 0)
                                {
                                    query += " and a.IdAlumno= @IdAlumno ";
                                }
                                else
                                {
                                    query += " and f.IdAnio = @IdAnio and f.IdSede = @IdSede and f.IdJornada = @IdJornada "
                                    + " and f.IdNivel between @IdNivelIni and @IdNivelFin "
                                    + " and f.IdCurso between @IdCursoIni and @IdCursoFin "
                                    + " and f.IdParalelo between @IdParaleloIni and @IdParaleloFin ";
                                }
                                query += " and dbo.BankersRounding(b.Total - isnull(e.dc_ValorPago, 0), 2) > 0 "
                                + " and a.Estado = 'A'"
                                + " UNION ALL"
                                + " select a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdNota, a.CodDocumentoTipo, b.Total, isnull(e.dc_ValorPago, 0) as dc_ValorPago, dbo.BankersRounding(b.Total - isnull(e.dc_ValorPago, 0), 2) Saldo,"
                                + " a.sc_observacion, a.no_fecha, a.IdAlumno, f.IdAnio, f.IdSede, f.IdJornada, f.IdNivel, f.IdCurso, f.IdParalelo,f.OrdenJornada, f.OrdenNivel, f.OrdenCurso, f.OrdenParalelo,f.NomNivel, f.NomJornada, f.NomCurso, f.NomParalelo, D.pe_nombreCompleto AS Alumno, f.pe_nombreCompleto as Representante, c.Codigo, b.Total AS ValorProntoPago, cast(GETDATE() as date) AS FechaProntoPago"
                                + " from fa_notaCreDeb as a inner join"
                                + " fa_notaCreDeb_resumen as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdNota = b.IdNota inner join"
                                + " aca_Alumno as c on a.IdEmpresa = c.IdEmpresa and a.IdAlumno = c.IdAlumno inner join"
                                + " tb_persona as d on c.IdPersona = d.IdPersona left join"
                                + " ("
                                + " select b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota, sum(b.dc_ValorPago) dc_ValorPago"
                                + " from cxc_cobro as a inner join"
                                + " cxc_cobro_det as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro"
                                + " where a.cr_estado = 'A' AND B.estado = 'A' AND B.dc_TipoDocumento = 'NTDB'"
                                + " group by b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota"
                                + " ) e on a.IdEmpresa = e.IdEmpresa and a.IdSucursal = e.IdSucursal and a.IdBodega = e.IdBodega_Cbte and a.IdNota = e.IdCbte_vta_nota left join"
                                + " ("
                                + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAlumno,  m.IdAnio, m.IdSede, m.IdJornada, m.IdNivel, m.IdCurso, m.IdParalelo,nj.OrdenJornada, sn.OrdenNivel, jc.OrdenCurso, cp.OrdenParalelo, sn.NomNivel, nj.NomJornada, jc.NomCurso, cp.NomParalelo, r.pe_nombreCompleto"
                                + " FROM     aca_AnioLectivo_Sede_NivelAcademico AS sn INNER JOIN"
                                + " aca_AnioLectivo AS a ON sn.IdEmpresa = a.IdEmpresa AND sn.IdAnio = a.IdAnio RIGHT OUTER JOIN"
                                + " aca_Matricula AS m LEFT OUTER JOIN"
                                + " aca_AnioLectivo_Curso_Paralelo AS cp ON m.IdEmpresa = cp.IdEmpresa AND m.IdAnio = cp.IdAnio AND m.IdSede = cp.IdSede AND m.IdNivel = cp.IdNivel AND m.IdJornada = cp.IdJornada AND m.IdCurso = cp.IdCurso AND"
                                + " m.IdParalelo = cp.IdParalelo LEFT OUTER JOIN"
                                + " aca_AnioLectivo_Jornada_Curso AS jc ON cp.IdEmpresa = jc.IdEmpresa AND cp.IdAnio = jc.IdAnio AND cp.IdSede = jc.IdSede AND cp.IdNivel = jc.IdNivel AND cp.IdJornada = jc.IdJornada AND cp.IdCurso = jc.IdCurso LEFT OUTER JOIN"
                                + " aca_AnioLectivo_NivelAcademico_Jornada AS nj ON jc.IdEmpresa = nj.IdEmpresa AND jc.IdAnio = nj.IdAnio AND jc.IdSede = nj.IdSede AND jc.IdNivel = nj.IdNivel AND jc.IdJornada = nj.IdJornada ON sn.IdEmpresa = nj.IdEmpresa AND"
                                + " sn.IdAnio = nj.IdAnio AND sn.IdSede = nj.IdSede AND sn.IdNivel = nj.IdNivel left join"
                                + " tb_persona as r on r.IdPersona = m.IdPersonaR"
                                + " WHERE M.IdEmpresa = @IdEmpresa ";
                                if (IdAlumno != 0)
                                {
                                    query += " and M.IdAlumno= @IdAlumno ";
                                }
                                else
                                {
                                    query += " and M.IdAnio = @IdAnio and M.IdSede = @IdSede and M.IdJornada = @IdJornada "
                                    + " and M.IdNivel between @IdNivelIni and @IdNivelFin "
                                    + " and M.IdCurso between @IdCursoIni and @IdCursoFin "
                                    + " and M.IdParalelo between @IdParaleloIni and @IdParaleloFin ";
                                }
                                query += " AND(a.EnCurso = 1) "
                                + " AND(NOT EXISTS "
                                + " (SELECT IdEmpresa "
                                + " FROM      aca_AlumnoRetiro AS f "
                                + " WHERE(m.IdEmpresa = IdEmpresa) AND(m.IdMatricula = IdMatricula) AND(Estado = 1))) "
                                + " ) as f on a.IdEmpresa = f.IdEmpresa and a.IdAlumno = f.IdAlumno"
                                + " where a.IdEmpresa = @IdEmpresa ";
                                if (IdAlumno != 0)
                                {
                                    query += " and a.IdAlumno= @IdAlumno ";
                                }
                                else
                                {
                                    query += " and f.IdAnio = @IdAnio and f.IdSede = @IdSede and f.IdJornada = @IdJornada "
                                    + " and f.IdNivel between @IdNivelIni and @IdNivelFin "
                                    + " and f.IdCurso between @IdCursoIni and @IdCursoFin "
                                    + " and f.IdParalelo between @IdParaleloIni and @IdParaleloFin ";
                                }
                                query += " and dbo.BankersRounding(b.Total - isnull(e.dc_ValorPago, 0), 2) > 0 "
                                + " and a.Estado = 'A' AND A.CreDeb = 'D'";*/
                    #endregion
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "IF OBJECT_ID('tempdb..#TempDatosAlumnosCXC_011') IS NOT NULL DROP TABLE #TempDatosAlumnosCXC_011 "
                                        + " DECLARE"
                                        + " @IdEmpresa int = " + IdEmpresa.ToString() + ","
                                        + " @IdAnio int = " + IdAnio.ToString() + ","
                                        + " @IdSede int = " + IdSede.ToString() + ","
                                        + " @IdJornada int = " + IdJornada.ToString() + ","
                                        + " @IdNivelIni int = " + IdNivelIni.ToString() + ","
                                        + " @IdNivelFin int = " + IdNivelFin.ToString() + ","
                                        + " @IdCursoIni int = " + IdCursoIni.ToString() + ","
                                        + " @IdCursoFin int = " + IdCursoFin.ToString() + ","
                                        + " @IdParaleloIni int = " + IdParaleloIni.ToString() + ","
                                        + " @IdParaleloFin int = " + IdParaleloFin.ToString() + ","
                                        + " @IdAlumno int = " + IdAlumno.ToString() + ""

                                        + " ; with UltMat as ("
                                        + " select a.IdEmpresa, a.IdAlumno, max(A.IdMatricula) IdMatricula"
                                        + " from aca_Matricula as a with(nolock) join"
                                        + " aca_AnioLectivo as b with(nolock) on a.IdEmpresa = b.IdEmpresa and a.IdAnio = b.IdAnio left join"
                                        + " aca_AlumnoRetiro as c with(nolock) on a.IdMatricula = c.IdMatricula and a.IdEmpresa = c.IdMatricula and c.Estado = 1"
                                        + " where c.IdMatricula is null"
                                        + " group by a.IdEmpresa, a.IdAlumno"

                                        + " ), MatPorAlumno as ("
                                        + " SELECT        m.IdEmpresa, m.IdMatricula, m.IdAlumno, m.IdAnio, m.IdSede, m.IdJornada, m.IdNivel, m.IdCurso, m.IdParalelo, nj.OrdenJornada, sn.OrdenNivel, jc.OrdenCurso, cp.OrdenParalelo, sn.NomNivel, nj.NomJornada, "
                                        + " jc.NomCurso, cp.NomParalelo, r.pe_nombreCompleto AS Representante"
                                        + " FROM            aca_AnioLectivo_Sede_NivelAcademico AS sn INNER JOIN"
                                        + " aca_AnioLectivo AS a with(nolock) ON sn.IdEmpresa = a.IdEmpresa AND sn.IdAnio = a.IdAnio RIGHT OUTER JOIN"
                                        + " aca_Matricula AS m with(nolock) LEFT OUTER JOIN"
                                        + " aca_AnioLectivo_Curso_Paralelo AS cp with(nolock) ON m.IdEmpresa = cp.IdEmpresa AND m.IdAnio = cp.IdAnio AND m.IdSede = cp.IdSede AND m.IdNivel = cp.IdNivel AND m.IdJornada = cp.IdJornada AND m.IdCurso = cp.IdCurso AND m.IdParalelo = cp.IdParalelo LEFT OUTER JOIN"
                                        + " aca_AnioLectivo_Jornada_Curso AS jc with(nolock) ON cp.IdEmpresa = jc.IdEmpresa AND cp.IdAnio = jc.IdAnio AND cp.IdSede = jc.IdSede AND cp.IdNivel = jc.IdNivel AND cp.IdJornada = jc.IdJornada AND cp.IdCurso = jc.IdCurso LEFT OUTER JOIN"
                                        + " aca_AnioLectivo_NivelAcademico_Jornada AS nj with(nolock) ON jc.IdEmpresa = nj.IdEmpresa AND jc.IdAnio = nj.IdAnio AND jc.IdSede = nj.IdSede AND jc.IdNivel = nj.IdNivel AND jc.IdJornada = nj.IdJornada ON sn.IdEmpresa = nj.IdEmpresa AND sn.IdAnio = nj.IdAnio AND sn.IdSede = nj.IdSede AND sn.IdNivel = nj.IdNivel LEFT OUTER JOIN"
                                        + " tb_persona AS r with(nolock) ON r.IdPersona = m.IdPersonaR"
                                        + " )"

                                        + " select b.*into #TempDatosAlumnosCXC_011"
                                        + " from UltMat as a"
                                        + " join"
                                        + " MatPorAlumno as b on a.IdEmpresa = b.IdEmpresa and a.IdAlumno = b.IdAlumno and a.IdMatricula = b.IdMatricula"
                                        + " WHERE A.IdEmpresa = @IdEmpresa AND";

                    if (IdAlumno > 0)
                    {
                        command.CommandText += " (@IdAlumno = A.IdAlumno)";
                    }
                    else
                    {
                        command.CommandText += " (b.IdAnio = @IdAnio"
                                        + " and b.IdSede = @IdSede"
                                        + " and b.IdJornada = @IdJornada"
                                        + " and b.IdNivel between @IdNivelIni and @IdNivelFin"
                                        + " and b.IdCurso between @IdCursoIni and @IdCursoFin"
                                        + " and b.IdParalelo between @IdParaleloIni and @IdParaleloFin)";
                    }

                    command.CommandText += " SELECT a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdCbteVta, a.vt_tipoDoc, b.Total, ISNULL(e.dc_ValorPago, 0) AS dc_ValorPago, dbo.BankersRounding(b.Total - ISNULL(e.dc_ValorPago, 0), 2) AS Saldo, a.vt_Observacion, a.vt_fecha, "
                    + " a.IdAlumno,"
                    + " d.pe_nombreCompleto AS Alumno,  c.Codigo, CASE WHEN ar.AplicaProntoPago = 1 AND ap.FechaProntoPago >= CAST(GETDATE() AS DATE)"
                    + " THEN b.ValorProntoPago ELSE b.Total END AS ValorProntoPago, ap.FechaProntoPago, "
                    + " t.IdAnio, t.IdSede, t.IdJornada, t.IdNivel, t.IdCurso, t.IdParalelo, t.OrdenJornada, t.OrdenNivel, t.OrdenCurso, t.OrdenParalelo, t.NomNivel, t.NomJornada, t.NomCurso, t.NomParalelo, t.Representante"
                    + " FROM            fa_factura AS a with (nolock)INNER JOIN"
                    + " fa_factura_resumen AS b with(nolock) ON a.IdEmpresa = b.IdEmpresa AND a.IdSucursal = b.IdSucursal AND a.IdBodega = b.IdBodega AND a.IdCbteVta = b.IdCbteVta INNER JOIN"
                    + " aca_Alumno AS c with(nolock) ON a.IdEmpresa = c.IdEmpresa AND a.IdAlumno = c.IdAlumno INNER JOIN"
                    + " tb_persona AS d with(nolock) ON c.IdPersona = d.IdPersona LEFT OUTER JOIN"
                    + " (SELECT        b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota, SUM(b.dc_ValorPago) AS dc_ValorPago"
                    + " FROM            cxc_cobro AS a with(nolock) INNER JOIN"
                    + " cxc_cobro_det AS b with(nolock) ON a.IdEmpresa = b.IdEmpresa AND a.IdSucursal = b.IdSucursal AND a.IdCobro = b.IdCobro"
                    + " WHERE(a.cr_estado = 'A') AND(b.estado = 'A') AND(b.dc_TipoDocumento = 'FACT')"
                    + " GROUP BY b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota) AS e ON a.IdEmpresa = e.IdEmpresa AND a.IdSucursal = e.IdSucursal AND a.IdBodega = e.IdBodega_Cbte AND"
                    + " a.IdCbteVta = e.IdCbte_vta_nota LEFT OUTER JOIN"
                    + " aca_AnioLectivo_Rubro AS ar with(nolock) ON b.IdEmpresa = ar.IdEmpresa AND b.IdAnio = ar.IdAnio AND b.IdRubro = ar.IdRubro LEFT OUTER JOIN"
                    + " aca_AnioLectivo_Periodo AS ap with(nolock) ON b.IdEmpresa = ap.IdEmpresa AND b.IdPeriodo = ap.IdPeriodo join"
                    + " #TempDatosAlumnosCXC_011 as t with (nolock) on a.IdEmpresa = t.IdEmpresa and a.IdAlumno = t.IdAlumno"
                    + " WHERE(dbo.BankersRounding(b.Total - ISNULL(e.dc_ValorPago, 0), 2) > 0) AND(a.Estado = 'A')"
                    + " UNION ALL"
                    + " SELECT        a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdNota, a.CodDocumentoTipo, b.Total, ISNULL(e_1.dc_ValorPago, 0) AS dc_ValorPago, dbo.BankersRounding(b.Total - ISNULL(e_1.dc_ValorPago, 0), 2) AS Saldo, a.sc_observacion,"
                    + " a.no_fecha, a.IdAlumno,"
                    + " d.pe_nombreCompleto AS Alumno, c.Codigo, b.Total AS ValorProntoPago, CAST(GETDATE() AS date) AS FechaProntoPago,"
                    + " t.IdAnio, t.IdSede, t.IdJornada, t.IdNivel, t.IdCurso, t.IdParalelo, t.OrdenJornada, t.OrdenNivel, t.OrdenCurso, t.OrdenParalelo, t.NomNivel, t.NomJornada, t.NomCurso, t.NomParalelo, t.Representante"
                    + " FROM            fa_notaCreDeb AS a with(nolock) INNER JOIN"
                    + " fa_notaCreDeb_resumen AS b with(nolock) ON a.IdEmpresa = b.IdEmpresa AND a.IdSucursal = b.IdSucursal AND a.IdBodega = b.IdBodega AND a.IdNota = b.IdNota INNER JOIN"
                    + " aca_Alumno AS c with(nolock) ON a.IdEmpresa = c.IdEmpresa AND a.IdAlumno = c.IdAlumno INNER JOIN"
                    + " tb_persona AS d with(nolock) ON c.IdPersona = d.IdPersona LEFT OUTER JOIN"
                    + " (SELECT        b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota, SUM(b.dc_ValorPago) AS dc_ValorPago"
                    + " FROM            cxc_cobro AS a with(nolock) INNER JOIN"
                    + " cxc_cobro_det AS b with(nolock) ON a.IdEmpresa = b.IdEmpresa AND a.IdSucursal = b.IdSucursal AND a.IdCobro = b.IdCobro"
                    + " WHERE(a.cr_estado = 'A') AND(b.estado = 'A') AND(b.dc_TipoDocumento = 'NTDB')"
                    + " GROUP BY b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota) AS e_1 ON a.IdEmpresa = e_1.IdEmpresa AND a.IdSucursal = e_1.IdSucursal AND a.IdBodega = e_1.IdBodega_Cbte AND"
                    + " a.IdNota = e_1.IdCbte_vta_nota join"
                    + " #TempDatosAlumnosCXC_011 as t with (nolock) on a.IdEmpresa = t.IdEmpresa and a.IdAlumno = t.IdAlumno"
                    + " WHERE(dbo.BankersRounding(b.Total - ISNULL(e_1.dc_ValorPago, 0), 2) > 0) AND(a.Estado = 'A') AND(a.CreDeb = 'D')";
                    command.CommandTimeout = 5000;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new CXC_014_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdCbteVta = Convert.ToDecimal(reader["IdCbteVta"]),
                            vt_fecha = Convert.ToDateTime(reader["vt_fecha"]),
                            vt_Observacion = Convert.ToString(reader["vt_Observacion"]),
                            NomCurso = Convert.ToString(reader["NomCurso"]),
                            NomJornada = Convert.ToString(reader["NomJornada"]),
                            Saldo = Convert.ToDecimal(reader["Saldo"]),
                            NomNivel = Convert.ToString(reader["NomNivel"]),
                            NomParalelo = Convert.ToString(reader["NomParalelo"]),
                            Alumno = Convert.ToString(reader["Alumno"]),
                            Codigo = Convert.ToString(reader["Codigo"]),
                            dc_ValorPago = Convert.ToDouble(reader["dc_ValorPago"]),
                            FechaProntoPago = Convert.ToDateTime(reader["FechaProntoPago"]),
                            Total = Convert.ToDecimal(reader["Total"]),
                            IdBodega = Convert.ToInt32(reader["IdBodega"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            Representante = Convert.ToString(reader["Representante"]),
                            ValorProntoPago = Convert.ToDecimal(reader["ValorProntoPago"]),
                            vt_tipoDoc = Convert.ToString(reader["vt_tipoDoc"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenNivel"]),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenJornada"]),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenCurso"]),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenParalelo"])
                        });
                    }

                    var ListaAgrupada = Lista.GroupBy(q => new
                    {
                        q.IdEmpresa,
                        q.IdAlumno
                    }).Select(q => new CXC_014_Info
                    {
                        IdEmpresa = q.Key.IdEmpresa,
                        IdAlumno = q.Key.IdAlumno,
                        CantFacturas = q.Count()
                    }).ToList();

                    ListaJoin = (from a in Lista
                                 join b in ListaAgrupada
                                 on new { a.IdEmpresa, a.IdAlumno } equals new { b.IdEmpresa, b.IdAlumno }
                                 select new CXC_014_Info
                                 {
                                     IdEmpresa = a.IdEmpresa,
                                     IdAlumno = a.IdAlumno,
                                     IdCbteVta = a.IdCbteVta,
                                     vt_fecha = a.vt_fecha,
                                     vt_Observacion = a.vt_Observacion,
                                     NomCurso = a.NomCurso,
                                     NomJornada = a.NomJornada,
                                     Saldo = a.Saldo,
                                     NomNivel = a.NomNivel,
                                     NomParalelo = a.NomParalelo,
                                     Alumno = a.Alumno,
                                     Codigo = a.Codigo,
                                     dc_ValorPago = a.dc_ValorPago,
                                     FechaProntoPago = a.FechaProntoPago,
                                     Total = a.Total,
                                     IdBodega = a.IdBodega,
                                     IdSucursal = a.IdSucursal,
                                     Representante = a.Representante,
                                     ValorProntoPago = a.ValorProntoPago,
                                     vt_tipoDoc = a.vt_tipoDoc,
                                     IdAnio = a.IdAnio,
                                     IdSede = a.IdSede,
                                     IdJornada = a.IdJornada,
                                     IdCurso = a.IdCurso,
                                     IdParalelo = a.IdParalelo,
                                     IdNivel = a.IdNivel,
                                     OrdenNivel = a.OrdenNivel,
                                     OrdenJornada = a.OrdenJornada,
                                     OrdenCurso = a.OrdenCurso,
                                     OrdenParalelo = a.OrdenParalelo,
                                     CantFacturas = b.CantFacturas,
                                 }).ToList();

                    ListaFinal = ListaJoin.Where(q => q.CantFacturas >= 4 && q.CantFacturas <= 6).ToList();

                    var FechaHasta = ListaFinal.Where(q => q.FechaProntoPago > DateTime.Now.Date).Min(q => q.FechaProntoPago);
                    var ValorHasta = "VALOR A PAGAR HASTA ";
                    FechaHasta = FechaHasta ?? DateTime.Now.Date;
                    ValorHasta += Convert.ToDateTime(FechaHasta).ToString("dd/MM/yyyy");

                    var FechaDesde = ListaFinal.Where(q => q.FechaProntoPago > DateTime.Now.Date).Max(q => q.FechaProntoPago);
                    var ValorDesde = "VALOR A PAGAR DESDE ";
                    FechaDesde = FechaDesde ?? DateTime.Now.Date;
                    FechaDesde = Convert.ToDateTime(FechaDesde).AddDays(1);
                    ValorDesde += Convert.ToDateTime(FechaDesde).ToString("dd/MM/yyyy");

                    var ValorProntoPagoHasta = "(-) PRONTO PAGO HASTA ";
                    ValorProntoPagoHasta += Convert.ToDateTime(FechaHasta).ToString("dd/MM/yyyy");


                    ListaFinal.ForEach(q => {
                        q.ValorDesde = ValorDesde;
                        q.ValorHasta = ValorHasta;
                        q.ValorProntoPagoHasta = ValorProntoPagoHasta;
                        q.MostrarValoresDesdeHasta = (DateTime.Now > FechaHasta ? false : true);
                    });
                    reader.Close();
                }
                return ListaFinal;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
