﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Core.Info.Reportes.Academico;
using System.Collections.Generic;
using Core.Bus.Reportes.Academico;
using Core.Bus.Academico;
using Core.Bus.General;
using DevExpress.XtraPrinting;

namespace Core.Web.Reportes.Academico
{
    public partial class ACA_007_Rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public string usuario { get; set; }
        public string empresa { get; set; }

        ACA_007_Bus bus_rpt = new ACA_007_Bus();
        public ACA_007_Rpt()
        {
            InitializeComponent();
        }
        private void ACA_007_Rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {

                lbl_fecha.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                lbl_usuario.Text = usuario;

                int IdEmpresa = string.IsNullOrEmpty(p_IdEmpresa.Value.ToString()) ? 0 : Convert.ToInt32(p_IdEmpresa.Value);
                int IdSede = string.IsNullOrEmpty(p_IdSede.Value.ToString()) ? 0 : Convert.ToInt32(p_IdSede.Value);
                int IdJornada = string.IsNullOrEmpty(p_IdJornada.Value.ToString()) ? 0 : Convert.ToInt32(p_IdJornada.Value);
                int IdNivel = string.IsNullOrEmpty(p_IdNivel.Value.ToString()) ? 0 : Convert.ToInt32(p_IdNivel.Value);
                int IdCurso = string.IsNullOrEmpty(p_IdCurso.Value.ToString()) ? 0 : Convert.ToInt32(p_IdCurso.Value);
                int IdParalelo = string.IsNullOrEmpty(p_IdParalelo.Value.ToString()) ? 0 : Convert.ToInt32(p_IdParalelo.Value);
                int IdAnio = string.IsNullOrEmpty(p_IdAnio.Value.ToString()) ? 0 : Convert.ToInt32(p_IdAnio.Value);
                DateTime fecha_ini = string.IsNullOrEmpty(p_fecha_ini.Value.ToString()) ? DateTime.Now.Date : Convert.ToDateTime(p_fecha_ini.Value);
                DateTime fecha_fin = string.IsNullOrEmpty(p_fecha_fin.Value.ToString()) ? DateTime.Now.Date : Convert.ToDateTime(p_fecha_fin.Value);
                bool MostrarAlumnosRetirados = Convert.ToBoolean(p_MostarAlumnosRetirados.Value);
                aca_Sede_Bus bus_sede = new aca_Sede_Bus();

                List<ACA_007_Info> Lista = bus_rpt.GetList(IdEmpresa, IdSede, IdAnio, IdJornada, IdNivel, IdCurso, IdParalelo, fecha_ini, fecha_fin, MostrarAlumnosRetirados);
                this.DataSource = Lista;

                tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
                var emp = bus_empresa.get_info(IdEmpresa);
                if (emp != null)
                {
                    if (emp.em_logo != null)
                    {
                        ImageConverter obj = new ImageConverter();
                        lbl_imagen.Image = (Image)obj.ConvertFrom(emp.em_logo);
                    }
                }

                var info_sede = bus_sede.GetInfo(IdEmpresa, IdSede);
                var NomSede = "";
                if (info_sede != null)
                {
                    NomSede = info_sede.NomSede;

                }

                lbl_sede.Text = NomSede;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void xrPivotGrid1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrPivotGrid1_PrintFieldValue(object sender, DevExpress.XtraReports.UI.PivotGrid.CustomExportFieldValueEventArgs e)
        {
            try
            {
                if (e.Field != null && (e.Field.FieldName == "NomPlantillaTipo") && e.Field.Area == DevExpress.XtraPivotGrid.PivotArea.ColumnArea && e.ValueType != DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal && e.ValueType != DevExpress.XtraPivotGrid.PivotGridValueType.Total)
                {
                    LabelBrick lb = new DevExpress.XtraPrinting.LabelBrick();
                    lb.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 2, GraphicsUnit.Pixel);
                    lb.Angle = 90;
                    lb.Text = e.Field.FieldName == "Cantidad" ? Convert.ToString(e.Text.Replace("Total", "")) : e.Text;
                    lb.Rect = DevExpress.XtraPrinting.GraphicsUnitConverter.DocToPixel(e.Brick.Rect);
                    e.Brick = lb;
                }else
                    if (e.Field != null && (e.Field.FieldName == "Descripcion" || e.Field.FieldName == "NomJornada" || e.Field.FieldName == "NomNivel") && e.Field.Area == DevExpress.XtraPivotGrid.PivotArea.RowArea && e.ValueType != DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal && e.ValueType != DevExpress.XtraPivotGrid.PivotGridValueType.Total)
                {
                    LabelBrick lb = new DevExpress.XtraPrinting.LabelBrick();
                    lb.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 2, GraphicsUnit.Pixel);
                    lb.Angle = 90;
                    lb.Text = e.Field.FieldName == "Cantidad" ? Convert.ToString(e.Text.Replace("Total", "")) : e.Text;
                    lb.Rect = DevExpress.XtraPrinting.GraphicsUnitConverter.DocToPixel(e.Brick.Rect);
                    e.Brick = lb;
                }
                else
                    if (e.Field != null && e.Field.FieldName == "Cantidad" && e.Field.Area == DevExpress.XtraPivotGrid.PivotArea.RowArea && (e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal || e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.Total))
                {
                    LabelBrick lb = new DevExpress.XtraPrinting.LabelBrick();
                    lb.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 2, GraphicsUnit.Pixel);
                    lb.Text = e.Field.FieldName == "Cantidad" ? Convert.ToString(e.Text.Replace("Total", "")) : e.Text;
                    lb.Rect = DevExpress.XtraPrinting.GraphicsUnitConverter.DocToPixel(e.Brick.Rect);
                    e.Brick = lb;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void xrPivotGrid1_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrPivotGrid2_PrintFieldValue(object sender, DevExpress.XtraReports.UI.PivotGrid.CustomExportFieldValueEventArgs e)
        {
            try
            {
                if (e.Field != null && (e.Field.FieldName == "NomPlantillaTipo") && e.Field.Area == DevExpress.XtraPivotGrid.PivotArea.ColumnArea && e.ValueType != DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal && e.ValueType != DevExpress.XtraPivotGrid.PivotGridValueType.Total)
                {
                    LabelBrick lb = new DevExpress.XtraPrinting.LabelBrick();
                    lb.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 2, GraphicsUnit.Pixel);
                    lb.Angle = 90;
                    lb.Text = e.Field.FieldName == "Cantidad" ? Convert.ToString(e.Text.Replace("Total", "")) : e.Text;
                    lb.Rect = DevExpress.XtraPrinting.GraphicsUnitConverter.DocToPixel(e.Brick.Rect);
                    e.Brick = lb;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
