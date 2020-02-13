//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class aca_CondicionalMatricula
    {
        public int IdEmpresa { get; set; }
        public decimal IdCondicional { get; set; }
        public decimal IdAlumno { get; set; }
        public int IdAnio { get; set; }
        public int IdCatalogoCONDIC { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public Nullable<bool> Estado { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string IdUsuarioAnulacion { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        public string MotivoAnulacion { get; set; }
    
        public virtual aca_Alumno aca_Alumno { get; set; }
        public virtual aca_AnioLectivo aca_AnioLectivo { get; set; }
        public virtual aca_Catalogo aca_Catalogo { get; set; }
    }
}