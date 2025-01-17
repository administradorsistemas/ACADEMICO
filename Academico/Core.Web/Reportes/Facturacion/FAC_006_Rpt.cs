﻿using Core.Bus.General;
using Core.Bus.Reportes.Facturacion;
using Core.Info.Reportes.Facturacion;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Core.Web.Reportes.Facturacion
{
    public partial class FAC_006_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        List<FAC_006_Info> Lista = new List<FAC_006_Info>();
        List<FAC_006_Info> Lista_detalle = new List<FAC_006_Info>();

        public string usuario { get; set; }
        public string empresa { get; set; }
        public FAC_006_Rpt()
        {
            InitializeComponent();
        }

        private void FAC_006_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lbl_empresa.Text = empresa;
            lbl_usuario.Text = usuario;

            int IdEmpresa = p_IdEmpresa.Value == null ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = p_IdSucursal.Value == null ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            int IdAlumno = (p_IdAlumno.Value == null ) ? 0 : Convert.ToInt32(p_IdAlumno.Value);
            DateTime fecha_ini = p_fecha_ini.Value == null ? DateTime.Now : Convert.ToDateTime(p_fecha_ini.Value);
            DateTime fech_fin = p_fecha_fin.Value == null ? DateTime.Now : Convert.ToDateTime(p_fecha_fin.Value);
            bool MostrarAnulados = string.IsNullOrEmpty(p_MostrarAnulados.Value.ToString()) ? false : Convert.ToBoolean(p_MostrarAnulados.Value);

            FAC_006_Bus bus_rpt = new FAC_006_Bus();
            List<FAC_006_Info> lst_rpt = bus_rpt.get_list(IdEmpresa, IdSucursal, IdAlumno, fecha_ini, fech_fin, MostrarAnulados);
            #region Grupo

            Lista = (from q in lst_rpt
                     group q by new
                          {
                              q.IdEmpresa,
                              q.IdSucursal,
                              q.IdCatalogo_FormaPago,
                              q.NombreFormaPago
                          } into Area
                          select new FAC_006_Info
                          {
                              Total = Area.Sum(q=>q.Total),
                              IdEmpresa = Area.Key.IdEmpresa,
                              IdSucursal = Area.Key.IdSucursal,
                              IdCatalogo_FormaPago = Area.Key.IdCatalogo_FormaPago,
                              NombreFormaPago = Area.Key.NombreFormaPago
                          }).ToList();

            Lista_detalle = (from q in lst_rpt
                     group q by new
                     {
                         q.IdEmpresa,
                         q.IdSucursal,
                         q.IdCatalogo_FormaPago,
                         q.NombreFormaPago,
                         q.vt_NumFactura,
                         q.vt_fecha,
                         q.Total
                     } into Factura
                     select new FAC_006_Info
                     {
                         IdEmpresa = Factura.Key.IdEmpresa,
                         IdSucursal = Factura.Key.IdSucursal,
                         IdCatalogo_FormaPago = Factura.Key.IdCatalogo_FormaPago,
                         NombreFormaPago = Factura.Key.NombreFormaPago,                         
                         vt_NumFactura = Factura.Key.vt_NumFactura,
                         vt_fecha = Factura.Key.vt_fecha,
                         Total = Factura.Key.Total
                     }).ToList();
            #endregion

            this.DataSource = lst_rpt;

            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var emp = bus_empresa.get_info(IdEmpresa);
            if (emp != null && emp.em_logo != null)
            {
                ImageConverter obj = new ImageConverter();
                lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
            }
        }

        private void GrupoEstado_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!Convert.ToBoolean(p_MostrarAnulados.Value))
            {
                e.Cancel = true;
            }
        }
    }
}
