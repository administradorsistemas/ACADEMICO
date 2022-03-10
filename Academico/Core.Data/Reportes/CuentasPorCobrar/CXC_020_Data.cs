using Core.Info.Helps;
using Core.Info.Reportes.Contabilidad;
using Core.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.Contabilidad
{
    public class CXC_020_Data
    {
        public List<CXC_020_Info> GetList(int IdEmpresa, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                List<CXC_020_Info> Lista = new List<CXC_020_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    #region Query
                    string query = "DECLARE @IdEmpresa int = " + IdEmpresa + ","
                    + " @FechaIni date = DATEFROMPARTS(" + fecha_ini.Year.ToString() + "," + fecha_ini.Month.ToString() + "," + fecha_ini.Day.ToString() + "), "
                    + " @FechaFin date = DATEFROMPARTS(" + fecha_fin.Year.ToString() + "," + fecha_fin.Month.ToString() + "," + fecha_fin.Day.ToString() + "), "
                    + " @oSaldoInicial float, "
                    + " @oFacturas float, "
                    + " @oNotasDebito float, "
                    + " @oNotasCredito float, "
                    + " @ONotasCreditoPagoAnticipado float, "
                    + " @oCobros float, "
                    + " @oSaldoAcreedorFinal float, "
                    + " @oSaldoFinal float, "

                    + " @oCXCMatutina float,"
                    + " @oCXCVespertina float,"
                    + " @oCXCDeudasAnteriores float,"
                    + " @oCXCAnticipados float,"
                    + " @oINGMatutina float,"
                    + " @oINGVespertina float"


                    + " BEGIN /*SALDO INICIAL*/ "
                    + " SELECT @oSaldoInicial = sum(a.Saldo) FROM( "
                    + " select a.IdEmpresa, DBO.BankersRounding(SUM(c.Total - isnull(b.dc_ValorPago, 0)), 2) as Saldo "
                    + " from fa_factura as a left join "
                    + " ( "
                        + " select a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota, sum(dc_ValorPago) dc_ValorPago "
                        + " from cxc_cobro_det as a inner join "
                        + " cxc_cobro as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro "
                        + " where a.dc_TipoDocumento = 'FACT' AND A.estado = 'A' and CAST(b.cr_fecha AS DATE) < @FechaIni "
                        + " GROUP BY a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota "
                    + " ) "
                    + " as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega_Cbte and a.IdCbteVta = b.IdCbte_vta_nota inner join "
                    + " fa_factura_resumen as c on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega and a.IdCbteVta = c.IdCbteVta "
                    + " where a.IdEmpresa = @IdEmpresa and a.Estado = 'A' and CAST(a.vt_fecha AS DATE) < @FechaIni and a.IdAlumno is not null "
                    + " GROUP BY a.IdEmpresa "
                    + " UNION ALL "
                    + " select a.IdEmpresa, DBO.BankersRounding(SUM(c.Total - isnull(b.dc_ValorPago, 0)), 2) as Saldo "
                    + " from fa_notaCreDeb as a left join "
                    + " ( "
                        + " select a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota, sum(dc_ValorPago) dc_ValorPago "
                        + " from cxc_cobro_det as a inner join "
                        + " cxc_cobro as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro "
                        + " where a.dc_TipoDocumento = 'NTDB' AND A.estado = 'A' and CAST(b.cr_fecha AS DATE) < @FechaIni "
                        + " GROUP BY a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota "
                    + " ) "
                    + " as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega_Cbte and a.IdNota = b.IdCbte_vta_nota inner join "
                    + " fa_notaCreDeb_resumen as c on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega and a.IdNota = c.IdNota "
                    + " where a.IdEmpresa = @IdEmpresa and a.Estado = 'A' and CAST(a.no_fecha AS DATE) < @FechaIni and a.CreDeb = 'D' "
                    + " GROUP BY a.IdEmpresa "
                    + " UNION ALL "
                    + " select a.IdEmpresa, dbo.BankersRounding(sum(b.Total - isnull(c.Valor_Aplicado, 0)), 2) * -1 Saldo "
                    + " from fa_notaCreDeb as a inner join "
                    + " fa_notaCreDeb_resumen as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdNota = b.IdNota left join "
                    + " ( "
                        + " select a.IdEmpresa_nt, a.IdSucursal_nt, a.IdBodega_nt, a.IdNota_nt, sum(a.Valor_Aplicado)Valor_Aplicado "
                        + " from fa_notaCreDeb_x_fa_factura_NotaDeb as a inner join "
                        + " fa_notaCreDeb as b on a.IdEmpresa_nt = b.IdEmpresa and a.IdSucursal_nt = b.IdSucursal and a.IdBodega_nt = b.IdBodega and a.IdNota_nt = b.IdNota "
                        + " where b.Estado = 'A' and b.IdEmpresa = @IdEmpresa and b.Estado = 'A' and b.CreDeb = 'C' and cast(a.fecha_cruce as date) < @FechaIni "
                        + " group by a.IdEmpresa_nt, a.IdSucursal_nt, a.IdBodega_nt, a.IdNota_nt "
                    + " ) as c on a.IdEmpresa = c.IdEmpresa_nt and a.IdSucursal = c.IdSucursal_nt and a.IdBodega = c.IdBodega_nt and a.IdNota = c.IdNota_nt "
                    + " where a.IdEmpresa = @IdEmpresa and a.Estado = 'A' and CAST(a.no_fecha AS DATE) < @FechaIni and a.CreDeb = 'C' "
                    + " group by a.IdEmpresa "
                    + " ) AS A "
                    + " GROUP BY IdEmpresa "
                + " END "
                + " BEGIN /*FACTURAS*/ "
                    + " select @oFacturas = dbo.BankersRounding(sum(b.Total), 2) "
                    + " from fa_factura as a inner join "
                    + " fa_factura_resumen as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdCbteVta = b.IdCbteVta "
                    + " where a.IdEmpresa = @IdEmpresa and a.Estado = 'A' and a.vt_fecha between @FechaIni and @FechaFin and a.IdAlumno is not null "
                + " END "
                + " BEGIN /*NOTAS DE DEBITO*/ "
                + " SELECT @oNotasDebito = dbo.BankersRounding(sum(b.Total), 2) "
                + " FROM fa_notaCreDeb AS A Inner join "
                + " fa_notaCreDeb_resumen as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdNota = b.IdNota "
                + " where a.IdEmpresa = @IdEmpresa and a.Estado = 'A' and a.no_fecha between @FechaIni and @FechaFin and a.IdAlumno is not null and a.CreDeb = 'D' "
            + " END "
            + " BEGIN /*NOTAS DE CREDITO*/ "
                + " SELECT @oNotasCredito = sum(b.Total) "
                + " FROM fa_notaCreDeb AS A INNER JOIN "
                + " fa_notaCreDeb_resumen AS B on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdNota = b.IdNota "
                + " where a.IdEmpresa = @IdEmpresa and a.no_fecha between @FechaIni and @FechaFin and a.CreDeb = 'C' and a.IdAlumno is not null and a.Estado = 'A' "
            + " END "
            + " BEGIN /*NOTAS DE CREDITO PAGO ANTICIPADO*/ "
                + " SELECT @ONotasCreditoPagoAnticipado = sum(b.Total) "
                + " FROM fa_notaCreDeb AS A INNER JOIN "
                + " fa_notaCreDeb_resumen AS B on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdNota = b.IdNota left join "
                + " ( "
                    + " select c.IdEmpresa_nt, c.IdSucursal_nt, c.IdBodega_nt, c.IdNota_nt "
                    + " from cxc_cobro as a left join "
                    + " cxc_cobro_det as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro inner join "
                    + " fa_notaCreDeb_x_cxc_cobro as c on a.IdEmpresa = c.IdEmpresa_cbr and a.IdSucursal = c.IdSucursal_cbr and a.IdCobro = c.IdCobro_cbr "
                    + " where a.IdEmpresa = @IdEmpresa and a.cr_estado = 'A' and a.cr_fecha between @FechaIni and @FechaFin and a.IdAlumno is not null and b.IdCobro is null "
                + " ) as c on a.IdEmpresa = c.IdEmpresa_nt and a.IdSucursal = c.IdSucursal_nt and a.IdBodega = c.IdBodega_nt and a.IdNota = c.IdNota_nt "
                + " where a.IdEmpresa = @IdEmpresa and a.no_fecha between @FechaIni and @FechaFin and a.CreDeb = 'C' and a.IdAlumno is not null and c.IdNota_nt is NOT null  and a.Estado = 'A' "
            + " END "
            + " BEGIN /*COBROS*/ "
                + " SELECT @oCobros = dbo.BankersRounding(SUM(A.cr_TotalCobro), 2) "
                + " FROM cxc_cobro AS A "
                + " WHERE A.IdEmpresa = @IdEmpresa AND A.cr_estado = 'A' AND A.cr_fecha BETWEEN @FechaIni AND @FechaFin AND A.IdAlumno IS NOT NULL "
                + " and a.IdCobro_tipo <> 'NTCR' "
            + " END "
            + " BEGIN /*SALDO ACREEDOR FINAL*/ "
            + " SELECT @oSaldoAcreedorFinal = dbo.BankersRounding(sum(b.Total - isnull(c.Valor_Aplicado, 0)), 2) "
            + " from fa_notaCreDeb as a inner join "
            + " fa_notaCreDeb_resumen as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdNota = b.IdNota left join "
            + " ( "
                + " select a.IdEmpresa_nt, a.IdSucursal_nt, a.IdBodega_nt, a.IdNota_nt, sum(a.Valor_Aplicado)Valor_Aplicado "
                + " from fa_notaCreDeb_x_fa_factura_NotaDeb as a inner join "
                + " fa_notaCreDeb as b on a.IdEmpresa_nt = b.IdEmpresa and a.IdSucursal_nt = b.IdSucursal and a.IdBodega_nt = b.IdBodega and a.IdNota_nt = b.IdNota "
                + " where b.Estado = 'A' and b.IdEmpresa = @IdEmpresa and b.Estado = 'A' and b.IdAlumno is not null and b.CreDeb = 'C' and cast(a.fecha_cruce as date) <= @FechaFin "
                + " group by a.IdEmpresa_nt, a.IdSucursal_nt, a.IdBodega_nt, a.IdNota_nt "
            + " ) as c on a.IdEmpresa = c.IdEmpresa_nt and a.IdSucursal = c.IdSucursal_nt and a.IdBodega = c.IdBodega_nt and a.IdNota = c.IdNota_nt "
            + " where a.IdEmpresa = @IdEmpresa and a.Estado = 'A' and a.no_fecha <= @FechaFin and a.CreDeb = 'C' "
            + " group by a.IdEmpresa "
        + " END "
        + " BEGIN /*SALDO FINAL*/ "
            + " SELECT @oSaldoFinal = sum(a.Saldo) FROM( "
             + " select a.IdEmpresa, DBO.BankersRounding(SUM(c.Total - isnull(b.dc_ValorPago, 0)), 2) as Saldo "
                    + " from fa_factura as a left join "
                    + " ( "
                        + " select a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota, sum(dc_ValorPago) dc_ValorPago "
                        + " from cxc_cobro_det as a inner join "
                        + " cxc_cobro as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro "
                        + " where a.dc_TipoDocumento = 'FACT' AND A.estado = 'A' and CAST(b.cr_fecha AS DATE) <= @FechaFin and B.IdAlumno is not null"
                        + " GROUP BY a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota "
                    + " ) "
                    + " as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega_Cbte and a.IdCbteVta = b.IdCbte_vta_nota inner join "
                    + " fa_factura_resumen as c on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega and a.IdCbteVta = c.IdCbteVta "
                    + " where a.IdEmpresa = @IdEmpresa and a.Estado = 'A' and CAST(a.vt_fecha AS DATE) <= @FechaFin and a.IdAlumno is not null "
                    + " GROUP BY a.IdEmpresa "
                    + " UNION ALL "
                    + " select a.IdEmpresa, DBO.BankersRounding(SUM(c.Total - isnull(b.dc_ValorPago, 0)), 2) as Saldo "
                    + " from fa_notaCreDeb as a left join "
                    + " ( "
                        + " select a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota, sum(dc_ValorPago) dc_ValorPago "
                        + " from cxc_cobro_det as a inner join "
                        + " cxc_cobro as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro "
                        + " where a.dc_TipoDocumento = 'NTDB' AND A.estado = 'A' and CAST(b.cr_fecha AS DATE) <= @FechaFin "
                        + " GROUP BY a.IdEmpresa, a.IdSucursal, a.IdBodega_Cbte, a.IdCbte_vta_nota "
                    + " ) "
                    + " as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega_Cbte and a.IdNota = b.IdCbte_vta_nota inner join "
                    + " fa_notaCreDeb_resumen as c on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega and a.IdNota = c.IdNota "
                    + " where a.IdEmpresa = @IdEmpresa and a.Estado = 'A' and CAST(a.no_fecha AS DATE) <= @FechaFin and a.CreDeb = 'D' "
                    + " GROUP BY a.IdEmpresa "
            + " ) AS A "
            + " GROUP BY IdEmpresa "
        + " END "

            + " select @oCXCMatutina = dbo.BankersRounding(sum(case when a.IdCtaCble like '0104%' then dc_Valor else 0 end), 2), "
            + " @oCXCVespertina = dbo.BankersRounding(sum(case when a.IdCtaCble like '0105%' then dc_Valor else 0 end), 2) , "
            + " @oCXCDeudasAnteriores = dbo.BankersRounding(sum(case when a.IdCtaCble like '0107%' then dc_Valor else 0 end), 2) , "
            + " @oCXCAnticipados = dbo.BankersRounding(sum(case when a.IdCtaCble = '0202002001' then dc_Valor else 0 end), 2) , "
            + " @oINGMatutina = dbo.BankersRounding(sum(case when b.cb_Fecha between @FechaIni and @FechaFin and(a.IdCtaCble like '0401%' or a.IdCtaCble like '0402%' or a.IdCtaCble like '0403%' or a.IdCtaCble like '0404%') then dc_Valor else 0 end), 2) , "
            + " @oINGVespertina = dbo.BankersRounding(sum(case when b.cb_Fecha between @FechaIni and @FechaFin and(a.IdCtaCble like '0405%' or a.IdCtaCble like '0406%' or a.IdCtaCble like '0407%') then dc_Valor else 0 end), 2) "
            + " from ct_cbtecble_det as a with(nolock) join "
            + " ct_cbtecble as b with(nolock) on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte = b.IdTipoCbte and a.IdCbteCble = b.IdCbteCble "
            + " where a.IdEmpresa = @IdEmpresa and b.cb_Fecha <= @FechaFin "

            + " SET @oINGMatutina = ABS(@oINGMatutina)"
            + " SET @oINGVespertina = ABS(@oINGVespertina)"
            + ""
            + ""
        //    + " select @oSaldoInicial AS SaldoInicial, @oFacturas as Facturas, @oNotasDebito as NotasDeDebito, @oSaldoInicial + @oFacturas + @oNotasDebito as SumanDebe, @oNotasCredito as NotasCredito, @ONotasCreditoPagoAnticipado as PagoAnticipado, @oNotasCredito - @ONotasCreditoPagoAnticipado as NetoNotaCredito, @oCobros as Cobros, "
        //+ " @oSaldoInicial + @oFacturas + @oNotasDebito - @oNotasCredito + @ONotasCreditoPagoAnticipado - @oCobros AS SaldoNeto, @oSaldoAcreedorFinal SaldoAcreedorFinal, "
        //+ " @oSaldoInicial + @oFacturas + @oNotasDebito - @oNotasCredito + @ONotasCreditoPagoAnticipado - @oCobros + @oSaldoAcreedorFinal as SaldoFinal, @oSaldoFinal SaldoCalculado, "
        //+ " dbo.BankersRounding(@oSaldoInicial + @oFacturas + @oNotasDebito - @oNotasCredito + @ONotasCreditoPagoAnticipado - @oCobros + @oSaldoAcreedorFinal - @oSaldoFinal, 2) Diferencia, "
        //+ " @oCXCMatutina CXCMatutina, @oCXCVespertina CXCVespertina, @oCXCDeudasAnteriores CXCDeudasAnteriores, @oCXCAnticipados CXCAnticipados, @oCXCMatutina + @oCXCVespertina + @oCXCDeudasAnteriores + @oCXCAnticipados as SaldoContableCXC,"
        //+ " (@oCXCMatutina + @oCXCVespertina + @oCXCDeudasAnteriores + @oCXCAnticipados) - (@oSaldoInicial + @oFacturas + @oNotasDebito - @oNotasCredito + @ONotasCreditoPagoAnticipado - @oCobros) as DiferenciaCXC,"
        //+ " @oINGMatutina oINGMatutina, @oINGVespertina oINGVespertina, @oINGMatutina + @oINGVespertina as TotalIngresos, (@oINGMatutina + @oINGVespertina) - @oFacturas as DiferenciaIngresos "
        // ******************  PARA QUE NO SALGA EN LAS SUMAS VALORES NULL EN LAS SUMATORIAS SE AÑADIO LAS 7 LINEAS DE ABAJO  **************************** BY ACUEVA 2022-03-10
       + " select ISNULL(@oSaldoInicial,0) AS SaldoInicial, ISNULL(@oFacturas,0) as Facturas, ISNULL(@oNotasDebito,0) as NotasDeDebito, ISNULL(@oSaldoInicial,0) + ISNULL(@oFacturas,0) + ISNULL(@oNotasDebito,0) as SumanDebe, ISNULL(@oNotasCredito,0) as NotasCredito, ISNULL(@ONotasCreditoPagoAnticipado,0) as PagoAnticipado, ISNULL(@oNotasCredito,0) - ISNULL(@ONotasCreditoPagoAnticipado,0) as NetoNotaCredito, ISNULL(@oCobros,0) as Cobros, "
        + " ISNULL(@oSaldoInicial,0) + ISNULL(@oFacturas,0) + ISNULL(@oNotasDebito,0) - ISNULL(@oNotasCredito,0) + ISNULL(@ONotasCreditoPagoAnticipado,0) - ISNULL(@oCobros,0) AS SaldoNeto, ISNULL(@oSaldoAcreedorFinal,0) SaldoAcreedorFinal, "
        + " ISNULL(@oSaldoInicial,0) + ISNULL(@oFacturas,0) + ISNULL(@oNotasDebito,0) - ISNULL(@oNotasCredito,0) + ISNULL(@ONotasCreditoPagoAnticipado,0) - ISNULL(@oCobros,0) + ISNULL(@oSaldoAcreedorFinal,0) as SaldoFinal, ISNULL(@oSaldoFinal,0) SaldoCalculado, "
        + " dbo.BankersRounding(ISNULL(@oSaldoInicial,0) + ISNULL(@oFacturas,0) + ISNULL(@oNotasDebito,0) - ISNULL(@oNotasCredito,0) + ISNULL(@ONotasCreditoPagoAnticipado,0) - ISNULL(@oCobros,0) + ISNULL(@oSaldoAcreedorFinal,0) - ISNULL(@oSaldoFinal,0), 2) Diferencia, "
        + " ISNULL(@oCXCMatutina,0) CXCMatutina, ISNULL(@oCXCVespertina,0) CXCVespertina, ISNULL(@oCXCDeudasAnteriores,0) CXCDeudasAnteriores, ISNULL(@oCXCAnticipados,0) CXCAnticipados, ISNULL(@oCXCMatutina,0) + ISNULL(@oCXCVespertina,0) + ISNULL(@oCXCDeudasAnteriores,0) + ISNULL(@oCXCAnticipados,0) as SaldoContableCXC,"
        + " (ISNULL(@oCXCMatutina,0) + ISNULL(@oCXCVespertina,0) + ISNULL(@oCXCDeudasAnteriores,0) + ISNULL(@oCXCAnticipados,0)) - (ISNULL(@oSaldoInicial,0) + ISNULL(@oFacturas,0) + ISNULL(@oNotasDebito,0) - ISNULL(@oNotasCredito,0) + ISNULL(@ONotasCreditoPagoAnticipado,0) - ISNULL(@oCobros,0)) as DiferenciaCXC,"
        + " ISNULL(@oINGMatutina,0) oINGMatutina, ISNULL(@oINGVespertina,0) oINGVespertina, ISNULL(@oINGMatutina,0) + ISNULL(@oINGVespertina,0) as TotalIngresos, (ISNULL(@oINGMatutina,0) + ISNULL(@oINGVespertina,0)) - ISNULL(@oFacturas,0) as DiferenciaIngresos "
        ;

                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new CXC_020_Info
                        {
                            SaldoInicial = reader["SaldoInicial"] == DBNull.Value ? 0 : Convert.ToDouble(reader["SaldoInicial"]),
                            Facturas = reader["Facturas"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Facturas"]),
                            NotasDeDebito = reader["NotasDeDebito"] == DBNull.Value ? 0 : Convert.ToDouble(reader["NotasDeDebito"]),
                            SumanDebe = reader["SumanDebe"] == DBNull.Value ? 0 : Convert.ToDouble(reader["SumanDebe"]),
                            NotasCredito = reader["NotasCredito"] == DBNull.Value ? 0 : Convert.ToDouble(reader["NotasCredito"]),
                            PagoAnticipado = reader["PagoAnticipado"] == DBNull.Value ? 0 : Convert.ToDouble(reader["PagoAnticipado"]),
                            NetoNotaCredito = reader["NetoNotaCredito"] == DBNull.Value ? 0 : Convert.ToDouble(reader["NetoNotaCredito"]),
                            Cobros = reader["Cobros"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Cobros"]),
                            SaldoNeto = reader["SaldoNeto"] == DBNull.Value ? 0 : Convert.ToDouble(reader["SaldoNeto"]),
                            SaldoAcreedorFinal = reader["SaldoAcreedorFinal"] == DBNull.Value ? 0 : Convert.ToDouble(reader["SaldoAcreedorFinal"]),
                            SaldoFinal = reader["SaldoFinal"] == DBNull.Value ? 0 : Convert.ToDouble(reader["SaldoFinal"]),
                            SaldoCalculado = reader["SaldoCalculado"] == DBNull.Value ? 0 : Convert.ToDouble(reader["SaldoCalculado"]),
                            Diferencia = reader["Diferencia"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Diferencia"]),

                            CXCMatutina = reader["CXCMatutina"] == DBNull.Value ? 0 : Convert.ToDouble(reader["CXCMatutina"]),
                            CXCVespertina = reader["CXCVespertina"] == DBNull.Value ? 0 : Convert.ToDouble(reader["CXCVespertina"]),
                            CXCDeudasAnteriores = reader["CXCDeudasAnteriores"] == DBNull.Value ? 0 : Convert.ToDouble(reader["CXCDeudasAnteriores"]),
                            CXCAnticipados = reader["CXCAnticipados"] == DBNull.Value ? 0 : Convert.ToDouble(reader["CXCAnticipados"]),
                            SaldoContableCXC = reader["SaldoContableCXC"] == DBNull.Value ? 0 : Convert.ToDouble(reader["SaldoContableCXC"]),
                            DiferenciaCXC = reader["DiferenciaCXC"] == DBNull.Value ? 0 : Convert.ToDouble(reader["DiferenciaCXC"]),
                            oINGMatutina = reader["oINGMatutina"] == DBNull.Value ? 0 : Convert.ToDouble(reader["oINGMatutina"]),
                            oINGVespertina = reader["oINGVespertina"] == DBNull.Value ? 0 : Convert.ToDouble(reader["oINGVespertina"]),
                            TotalIngresos = reader["TotalIngresos"] == DBNull.Value ? 0 : Convert.ToDouble(reader["TotalIngresos"]),
                            DiferenciaIngresos = reader["DiferenciaIngresos"] == DBNull.Value ? 0 : Convert.ToDouble(reader["DiferenciaIngresos"])
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
    }
}
