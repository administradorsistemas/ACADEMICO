using Core.Data.Base;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.General
{
    public class tb_sis_reporte_x_tb_empresa_Data
    {
        public tb_sis_reporte_x_tb_empresa_Info GetInfo(int IdEmpresa, string CodReporte)
        {
            try
            {
                tb_sis_reporte_x_tb_empresa_Info info;

                using (EntitiesAcademico db = new EntitiesAcademico())
                {
                    var Entity = db.aca_Reporte_x_tb_empresa.Where(q => q.IdEmpresa == IdEmpresa && q.CodReporte == CodReporte).FirstOrDefault();
                    if (Entity == null) return null;
                    else
                        info = new tb_sis_reporte_x_tb_empresa_Info
                        {
                            IdEmpresa = Entity.IdEmpresa,
                            CodReporte = Entity.CodReporte,
                            ReporteDisenio = Entity.ReporteDisenio,
                            //Nom_Carpeta = Entity.tb_sis_reporte.tb_modulo.Nom_Carpeta,
                            //Reporte = Entity.tb_sis_reporte.nom_reporte
                        };
                }

                return info;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarDB(tb_sis_reporte_x_tb_empresa_Info info)
        {
            try
            {
                using (EntitiesAcademico db = new EntitiesAcademico())
                {
                    var Entity = db.aca_Reporte_x_tb_empresa.Where(q => q.IdEmpresa == info.IdEmpresa && q.CodReporte == info.CodReporte).FirstOrDefault();
                    if (Entity == null)
                        db.aca_Reporte_x_tb_empresa.Add(new aca_Reporte_x_tb_empresa
                        {
                            IdEmpresa = info.IdEmpresa,
                            CodReporte = info.CodReporte,
                            ReporteDisenio = info.ReporteDisenio
                        });
                    else
                        Entity.ReporteDisenio = info.ReporteDisenio;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
