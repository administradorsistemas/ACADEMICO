using Core.Data.Base;
using Core.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.Facturacion
{
    public class FAC_009_Data
    {
        public List<FAC_009_Info> GetList(int IdEmpresa, int IdTipoNota, DateTime FechaIni, DateTime FechaFin)
        {
            try
            {
                List<FAC_009_Info> Lista = new List<FAC_009_Info>();
                FechaIni = FechaIni.Date;
                FechaFin = FechaFin.Date;
                int IdTipoNotaFin = IdTipoNota == 0 ? 99999 : IdTipoNota;

                using (EntitiesReportes db = new EntitiesReportes())
                {
                    db.SetCommandTimeOut(6000); // by Acueva 2022-01-31
                    var lst = db.VWFAC_009.Where(q => q.IdEmpresa == IdEmpresa && IdTipoNota <= q.IdTipoNota && q.IdTipoNota <= IdTipoNotaFin && FechaIni <= q.fecha_cruce && q.fecha_cruce <= FechaFin).ToList();
                    foreach (var item in lst)
                    {
                        Lista.Add(new FAC_009_Info
                        {
                            IdEmpresa = item.IdEmpresa,
                            IdSucursal = item.IdSucursal,
                            IdBodega = item.IdBodega,
                            IdNota = item.IdNota,
                            IdSucursal_fac_nd_doc_mod = item.IdSucursal_fac_nd_doc_mod,
                            IdBodega_fac_nd_doc_mod = item.IdBodega_fac_nd_doc_mod,
                            IdCbteVta_fac_nd_doc_mod = item.IdCbteVta_fac_nd_doc_mod,
                            vt_tipoDoc = item.vt_tipoDoc,
                            no_fecha = item.no_fecha,
                            sc_observacion = item.sc_observacion,
                            vt_Observacion = item.vt_Observacion,
                            IdTipoNota = item.IdTipoNota,
                            No_Descripcion = item.No_Descripcion,
                            fecha_cruce = item.fecha_cruce,
                            Total = item.Total,
                            Valor_Aplicado = item.Valor_Aplicado,
                            Tipo = item.Tipo,
                            vt_NumFactura = item.vt_NumFactura,
                            IdAlumno = item.IdAlumno,
                            pe_nombreCompleto = item.pe_nombreCompleto,
                            CodigoAlumno = item.CodigoAlumno,
                            vt_fecha = item.vt_fecha,
                            IdConciliacion = item.IdConciliacion
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
