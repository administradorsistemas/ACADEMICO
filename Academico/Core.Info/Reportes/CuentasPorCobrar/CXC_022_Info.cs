using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Reportes.CuentasPorCobrar
{
    public class CXC_022_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public decimal IdCobro { get; set; }
        public DateTime cr_fecha { get; set; }
        public string tc_descripcion { get; set; }
        public string NombreTarjeta { get; set; }
        public string cr_tarjeta { get; set; }
        public string Codigo { get; set; }
        public string Alumno { get; set; }
        public string pe_cedulaRuc { get; set; }
        public string Cliente { get; set; }
        public string cr_observacion { get; set; }
        public string IdUsuario { get; set; }
        public double cr_TotalCobro { get; set; }
    }
}
