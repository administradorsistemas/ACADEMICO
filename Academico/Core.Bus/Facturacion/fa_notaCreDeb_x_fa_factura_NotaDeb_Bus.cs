﻿using Core.Data.Facturacion;
using Core.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Facturacion
{
    public class fa_notaCreDeb_x_fa_factura_NotaDeb_Bus
    {
        fa_notaCreDeb_x_fa_factura_NotaDeb_Data odata = new fa_notaCreDeb_x_fa_factura_NotaDeb_Data();
        public List<fa_notaCreDeb_x_fa_factura_NotaDeb_Info> get_list_cartera(int IdEmpresa, int IdSucursal, decimal IdCliente, bool mostrar_saldo0)
        {
            try
            {
                return odata.get_list_cartera(IdEmpresa, IdSucursal, IdCliente, mostrar_saldo0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<fa_notaCreDeb_x_fa_factura_NotaDeb_Info> get_list_cartera_academico(int IdEmpresa, int IdSucursal, decimal IdCliente, decimal IdAlumno, bool mostrar_saldo0)
        {
            try
            {
                return odata.get_list_cartera_academico(IdEmpresa, IdSucursal, IdCliente, IdAlumno, mostrar_saldo0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<fa_notaCreDeb_x_fa_factura_NotaDeb_Info> get_list_cartera_saldo_cero(int IdEmpresa, int IdSucursal, decimal IdCliente, decimal IdAlumno)
        {
            try
            {
                return odata.get_list_cartera_saldo_cero(IdEmpresa, IdSucursal, IdCliente, IdAlumno);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<fa_notaCreDeb_x_fa_factura_NotaDeb_Info> get_list(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdNota)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, IdBodega, IdNota);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public fa_notaCreDeb_x_fa_factura_NotaDeb_Info Get_info_SaldoDocumento(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCliente, decimal IdAlumno, decimal IdCbteVta, string vt_tipoDoc)
        {
            try
            {
                return odata.get_info_SaldoDocumento(IdEmpresa, IdSucursal, IdBodega, IdCliente, IdAlumno, IdCbteVta, vt_tipoDoc);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
