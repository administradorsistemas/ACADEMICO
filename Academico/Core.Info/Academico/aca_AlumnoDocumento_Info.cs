using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Academico
{
    public class aca_AlumnoDocumento_Info
    {
        public int IdEmpresa { get; set; }
        public int Secuencia { get; set; }
        public decimal IdAlumno { get; set; }
        public int IdDocumento { get; set; }
        public bool EnArchivo { get; set; }

        #region Campos que no existen en la tabla
        public string IdStringDoc { get; set; }
        public string NomDocumento { get; set; }
        public string Ruta { get; set; }
        public bool seleccionado { get; set; }
        public string urlDoc { get; set; }
        public decimal IdAdmision { get; set; }
        #endregion
    }
}
