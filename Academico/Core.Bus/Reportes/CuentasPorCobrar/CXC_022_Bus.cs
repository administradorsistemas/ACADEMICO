using Core.Data.Reportes.CuentasPorCobrar;
using Core.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Reportes.CuentasPorCobrar
{
    public class CXC_022_Bus
    {
        CXC_022_Data odata = new CXC_022_Data();
        public List<CXC_022_Info> GetList(int IdEmpresa, DateTime FechaCorte)
        {
            return odata.GetList(IdEmpresa, FechaCorte);
        }
    }
}
