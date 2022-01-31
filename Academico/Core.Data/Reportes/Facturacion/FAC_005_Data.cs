using Core.Data.Base;
using Core.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.Facturacion
{
   public class FAC_005_Data
    {
      public List< FAC_005_Info> GetList(int IdEmpresa, int IdTipoNota, DateTime FechaDesde, DateTime FechaHasta, string CreDeb, string NaturalezaNota)
        {
            try
            {
                int IdTipoNotaIni = IdTipoNota;
                int IdTipoNotaFin = IdTipoNota == 0 ? 999999999 : IdTipoNota;
                FechaDesde = FechaDesde.Date;
                FechaHasta = FechaHasta.Date;

                List<FAC_005_Info> Lista = new List<FAC_005_Info>();
                using (EntitiesReportes db = new EntitiesReportes())
                {
                    db.SetCommandTimeOut(6000);// By Acueva 2022-01-31
                    var lst = db.SPFAC_005(IdEmpresa, FechaDesde, FechaHasta, CreDeb, NaturalezaNota).ToList();
                    lst = lst.Where(q => IdTipoNotaIni <= q.IdTipoNota && q.IdTipoNota <= IdTipoNotaFin).ToList();
                    foreach (var q in lst)
                    {
                        Lista.Add(new FAC_005_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdSucursal = q.IdSucursal,
                            IdBodega = q.IdBodega,
                            IdNota = q.IdNota,
                            no_fecha = q.no_fecha,
                            CreDeb = q.CreDeb,
                            NaturalezaNota = q.NaturalezaNota,
                            Estado = q.Estado,
                            NumeroNota = q.NumeroNota,
                            SubtotalConDscto = q.SubtotalConDscto,
                            ValorIVA = q.ValorIVA,
                            Total = q.Total,
                            Valor_Aplicado = q.Valor_Aplicado + q.Cruce,
                            Saldo = q.Saldo,
                            NombreCliente = q.NombreCliente,
                            NombreAlumno = q.NombreAlumno,
                            IdTipoNota = q.IdTipoNota,
                            No_Descripcion = q.No_Descripcion,
                            Cruce = q.Cruce,
                            Aplicacion = q.Valor_Aplicado,
                            Tipo = q.Valor_Aplicado > 0 ? "Aplicado" : "Conciliado"
                        });
                    }
                             
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
