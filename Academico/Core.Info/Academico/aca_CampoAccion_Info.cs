﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Academico
{
    public class aca_CampoAccion_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCampoAccion { get; set; }
        [StringLength(200, MinimumLength = 1, ErrorMessage = "el campo nombre debe tener mínimo 1 caracter y máximo 200")]
        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        public string NombreCampoAccion { get; set; }
        [Required(ErrorMessage = "El campo orden es obligatorio")]
        public int OrdenCampoAccion { get; set; }
        public bool Estado { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string IdUsuarioAnulacion { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        [Required(ErrorMessage = "El campo motivo de anulación es obligatorio")]
        public string MotivoAnulacion { get; set; }
    }
}
