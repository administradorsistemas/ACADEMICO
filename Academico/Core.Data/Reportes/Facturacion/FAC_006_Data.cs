using Core.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data.Base;

namespace Core.Data.Reportes.Facturacion
{
    public class FAC_006_Data
    {
        public List<FAC_006_Info> get_list(int IdEmpresa, int IdSucursal, decimal IdAlumno, DateTime fecha_ini, DateTime fecha_fin, bool MostrarAnulados)
        {
            try
            {
                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 999999 : IdSucursal;
                decimal IdAlumnoIni = IdAlumno;
                decimal IdAlumnoFin = IdAlumno == 0 ? 999999999 : IdAlumno;
                fecha_ini = fecha_ini.Date;
                fecha_fin = fecha_fin.Date;
                List<FAC_006_Info> Lista;
                using (EntitiesReportes Context = new EntitiesReportes())
                {
                    Context.SetCommandTimeOut(6000); // ByAcueva 2022-01-31
                    Lista = (from q in Context.SPFAC_006(IdEmpresa, IdSucursalIni, IdSucursalFin, IdAlumnoIni, IdAlumnoFin, fecha_ini, fecha_fin, MostrarAnulados)
                             select new FAC_006_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega = q.IdBodega,
                                 IdCbteVta = q.IdCbteVta,
                                 Estado = q.Estado,
                                 vt_NumFactura = q.vt_NumFactura,
                                 IdAlumno = q.IdAlumno,
                                 pe_nombreCompleto = q.pe_nombreCompleto,
                                 NombreFormaPago = q.NombreFormaPago,
                                 IdCatalogo_FormaPago = q.IdCatalogo_FormaPago,
                                 vt_fecha = q.vt_fecha,
                                 Ve_Vendedor = q.Ve_Vendedor,
                                 IdVendedor = q.IdVendedor,
                                 Su_Descripcion = q.Su_Descripcion,
                                 Su_Telefonos = q.Su_Telefonos,
                                 Su_Direccion = q.Su_Direccion,
                                 Su_Ruc = q.Su_Ruc,
                                 SubtotalIVAConDscto = q.SubtotalIVAConDscto,
                                 SubtotalSinIVAConDscto = q.SubtotalSinIVAConDscto,
                                 ValorIVA = q.ValorIVA,
                                 Total = q.Total,
                                 FacturasAnuladas = q.FacturasAnuladas,
                                 nom_FormaPago = q.nom_FormaPago,
                                 pe_cedulaRuc = q.pe_cedulaRuc,
                                 Tarifa = q.Tarifa,
                                 vt_Observacion = q.vt_Observacion,
                                 IdJornada = q.IdJornada,
                                 NomJornada = q.NomJornada,
                                 OrdenJornada = q.OrdenJornada,
                                 IdNivel = q.IdNivel,
                                 NomNivel = q.NomNivel,
                                 OrdenNivel = q.OrdenNivel,
                                 Cantidad=1,
                                 idMes = q.idMes,
                                 smes = q.smes,
                                 NomRubro = q.NomRubro,
                                 IdPeriodo = q.IdPeriodo,
                                 NomRuboDetalle = q.NomRuboDetalle
                                 
                             }).ToList();
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
