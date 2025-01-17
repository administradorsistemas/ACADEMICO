﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Bus.Reportes.Facturacion;
using System.Collections.Generic;
using Core.Info.Reportes.Facturacion;
using Core.Bus.General;

namespace Core.Web.Reportes.Facturacion
{
    public partial class FAC_008_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }
        public FAC_008_Rpt()
        {
            InitializeComponent();
        }

        private void FAC_008_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
            int IdSucursal = string.IsNullOrEmpty(p_IdSucursal.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSucursal.Value);
            int IdBodega = string.IsNullOrEmpty(p_IdBodega.Value.ToString()) ? 0 : Convert.ToInt32(p_IdBodega.Value);
            decimal IdNota = string.IsNullOrEmpty(p_IdNota.Value.ToString()) ? 0 : Convert.ToInt32(p_IdNota.Value);

            FAC_008_Bus bus_rpt = new FAC_008_Bus();
            List<FAC_008_Info> lst_rpt = bus_rpt.get_list(IdEmpresa,IdSucursal, IdBodega, IdNota);
            this.DataSource = lst_rpt;


            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var empresa = bus_empresa.get_info(IdEmpresa);
            lbl_empresa.Text = empresa.em_nombre;
            lbl_direccion.Text = empresa.em_direccion;
            lbl_dir.Text = empresa.em_direccion;
            lbl_correo.Text = empresa.ContribuyenteEspecial;
            lbl_ruc.Text = empresa.em_ruc;

            if (empresa != null && empresa.em_logo != null)
            {
                ImageConverter obj = new ImageConverter();
                lbl_imagen.Image = (Image)obj.ConvertFrom(empresa.em_logo);
            }
        }
    }
}
