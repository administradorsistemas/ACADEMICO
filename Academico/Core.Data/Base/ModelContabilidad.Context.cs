﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Data.Base
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class EntitiesContabilidad : DbContext
    {
        public EntitiesContabilidad()
            : base("name=EntitiesContabilidad")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public void SetCommandTimeOut(int TimeOut)
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = TimeOut;
        }



        public virtual DbSet<ct_cbtecble> ct_cbtecble { get; set; }
        public virtual DbSet<ct_cbtecble_det> ct_cbtecble_det { get; set; }
        public virtual DbSet<ct_periodo> ct_periodo { get; set; }
        public virtual DbSet<vwct_cbtecble_det> vwct_cbtecble_det { get; set; }
        public virtual DbSet<vwct_cbtecble> vwct_cbtecble { get; set; }
        public virtual DbSet<ct_cbtecble_Reversado> ct_cbtecble_Reversado { get; set; }
        public virtual DbSet<ct_cbtecble_tipo> ct_cbtecble_tipo { get; set; }
        public virtual DbSet<vwct_periodo> vwct_periodo { get; set; }
        public virtual DbSet<ct_CierrePorModuloPorSucursal> ct_CierrePorModuloPorSucursal { get; set; }
        public virtual DbSet<ct_anio_fiscal> ct_anio_fiscal { get; set; }
        public virtual DbSet<ct_anio_fiscal_x_cuenta_utilidad> ct_anio_fiscal_x_cuenta_utilidad { get; set; }
        public virtual DbSet<ct_anio_fiscal_x_tb_sucursal> ct_anio_fiscal_x_tb_sucursal { get; set; }
        public virtual DbSet<vwct_anio_fiscal_x_cuenta_utilidad> vwct_anio_fiscal_x_cuenta_utilidad { get; set; }
        public virtual DbSet<vwct_anio_fiscal_x_tb_sucursal> vwct_anio_fiscal_x_tb_sucursal { get; set; }
        public virtual DbSet<vwct_anio_fiscal_x_tb_sucursal_SinCierre> vwct_anio_fiscal_x_tb_sucursal_SinCierre { get; set; }
        public virtual DbSet<ct_plancta> ct_plancta { get; set; }
        public virtual DbSet<ct_plancta_nivel> ct_plancta_nivel { get; set; }
        public virtual DbSet<ct_grupocble> ct_grupocble { get; set; }
        public virtual DbSet<vwct_cbtecble_con_ctacble_acreedora> vwct_cbtecble_con_ctacble_acreedora { get; set; }
        public virtual DbSet<ct_periodo_x_tb_modulo> ct_periodo_x_tb_modulo { get; set; }
        public virtual DbSet<vwct_CierrePorModuloPorSucursal> vwct_CierrePorModuloPorSucursal { get; set; }
        public virtual DbSet<ct_parametro> ct_parametro { get; set; }
    
        public virtual ObjectResult<SPACA_ContabilizacionCobros_Result> SPACA_ContabilizacionCobros(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaIni, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaIniParameter = fechaIni.HasValue ?
                new ObjectParameter("FechaIni", fechaIni) :
                new ObjectParameter("FechaIni", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SPACA_ContabilizacionCobros_Result>("SPACA_ContabilizacionCobros", idEmpresaParameter, fechaIniParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SPACA_ContabilizacionFacturas_Result> SPACA_ContabilizacionFacturas(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaIni, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaIniParameter = fechaIni.HasValue ?
                new ObjectParameter("FechaIni", fechaIni) :
                new ObjectParameter("FechaIni", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SPACA_ContabilizacionFacturas_Result>("SPACA_ContabilizacionFacturas", idEmpresaParameter, fechaIniParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SPACA_ContabilizacionNotas_Result> SPACA_ContabilizacionNotas(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaIni, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaIniParameter = fechaIni.HasValue ?
                new ObjectParameter("FechaIni", fechaIni) :
                new ObjectParameter("FechaIni", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SPACA_ContabilizacionNotas_Result>("SPACA_ContabilizacionNotas", idEmpresaParameter, fechaIniParameter, fechaFinParameter);
        }
    }
}
