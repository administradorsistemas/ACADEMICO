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
    
    public partial class vwaca_AnioLectivoConductaEquivalencia
    {
        public int IdEmpresa { get; set; }
        public int IdAnio { get; set; }
        public int Secuencia { get; set; }
        public string Letra { get; set; }
        public decimal Calificacion { get; set; }
        public string Descripcion { get; set; }
        public string Equivalencia { get; set; }
        public string DescripcionEquivalencia { get; set; }
        public Nullable<bool> IngresaMotivo { get; set; }
        public Nullable<bool> IngresaProfesor { get; set; }
        public Nullable<bool> IngresaInspector { get; set; }
    }
}
