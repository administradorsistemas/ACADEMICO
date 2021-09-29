using Core.Data.Base;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Academico
{
    public class aca_Reporte_Data
    {
        public List<aca_Reporte_Info> get_list()
        {
            try
            {
                List<aca_Reporte_Info> Lista;
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    Lista = (from q in Context.aca_Reporte
                             select new aca_Reporte_Info
                             {

                                 CodModulo = q.CodModulo,
                                 CodReporte = q.CodReporte,
                                 observacion = q.observacion,
                                 orden = q.orden,
                                 nom_reporte = q.nom_reporte,
                                 estado = q.estado,
                                 mvc_accion = q.mvc_accion,
                                 mvc_controlador = q.mvc_controlador,
                                 mvc_area = q.mvc_area
                             }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public aca_Reporte_Info get_info(string CodReporte)
        {
            try
            {
                aca_Reporte_Info info = new aca_Reporte_Info();
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Reporte Entity = Context.aca_Reporte.FirstOrDefault(q => q.CodReporte == CodReporte);
                    if (Entity == null) return null;
                    info = new aca_Reporte_Info
                    {
                        CodReporte = Entity.CodReporte,
                        observacion = Entity.observacion,
                        orden = Entity.orden,
                        estado = Entity.estado,
                        mvc_accion = Entity.mvc_accion,
                        mvc_area = Entity.mvc_area,
                        mvc_controlador = Entity.mvc_controlador,
                        nom_reporte = Entity.nom_reporte,
                        rpt_clase_data = Entity.rpt_clase_data,
                        rpt_clase_info = Entity.rpt_clase_info,
                        rpt_clase_bus = Entity.rpt_clase_bus,
                        rpt_clase_rpt = Entity.rpt_clase_rpt,
                        rpt_store_procedure = Entity.rpt_store_procedure,
                        rpt_usa_store_procedure = Entity.rpt_usa_store_procedure,
                        rpt_vista = Entity.rpt_vista,
                        se_muestra_administrador_reportes = Entity.se_muestra_administrador_reportes,
                        //rpt_muestra_disenador_reporte = Entity.se_muestra_administrador_reportes,
                        CodModulo = Entity.CodModulo,

                        //Nom_Carpeta = Entity.tb_modulo.Nom_Carpeta
                    };
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool validar_existe_CodReporte(string CodReporte)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var lst = from q in Context.aca_Reporte
                              where CodReporte == q.CodReporte
                              select q;

                    if (lst.Count() > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string get_id(string CodModulo)
        {
            string num = "";
            using (EntitiesAcademico Context = new EntitiesAcademico())
            {
                try
                {
                    var c = Context.Database.SqlQuery<int>("select max(substring(CodReporte,LEN(CodReporte)-2,3) )+1 from web.aca_Reporte where CodModulo='" + CodModulo + "' ");

                    var c2 = c.FirstOrDefault();
                    if (c2 == 0)
                        num = "001";
                    else if (Convert.ToDecimal(c2) < 10)
                        num = "00" + Convert.ToString(c2);
                    else if (Convert.ToDecimal(c2) < 99)
                        num = "0" + Convert.ToString(c2);
                    else if (Convert.ToDecimal(c2) < 999)
                        num = Convert.ToString(c2);
                    else
                        num = null;
                }
                catch (Exception)
                {
                    return "001";
                }
                return num;
            }
        }

        public bool guardarDB(aca_Reporte_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Reporte Entity = new aca_Reporte
                    {
                        CodModulo = info.CodModulo,
                        CodReporte = info.CodReporte,
                        observacion = info.observacion,
                        orden = info.orden,
                        estado = info.estado == true,
                        mvc_accion = info.mvc_accion,
                        mvc_area = info.mvc_area,
                        mvc_controlador = info.mvc_controlador,
                        nom_reporte = info.nom_reporte,
                        rpt_clase_data = info.rpt_clase_data,
                        rpt_clase_info = info.rpt_clase_info,
                        rpt_clase_bus = info.rpt_clase_bus,
                        rpt_clase_rpt = info.rpt_clase_rpt,
                        rpt_store_procedure = info.rpt_store_procedure,
                        rpt_usa_store_procedure = info.rpt_usa_store_procedure,
                        rpt_vista = info.rpt_vista,
                        se_muestra_administrador_reportes = info.se_muestra_administrador_reportes,
                        Se_Mustra_Disenador_rpt = info.rpt_muestra_disenador_reporte,
                    };
                    Context.aca_Reporte.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(aca_Reporte_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Reporte Entity = Context.aca_Reporte.FirstOrDefault(q => q.CodReporte == info.CodReporte);
                    if (Entity == null) return false;


                    Entity.observacion = info.observacion;
                    Entity.orden = info.orden;
                    Entity.estado = info.estado == true;
                    Entity.mvc_accion = info.mvc_accion;
                    Entity.mvc_area = info.mvc_area;
                    Entity.mvc_controlador = info.mvc_controlador;
                    Entity.nom_reporte = info.nom_reporte;
                    Entity.rpt_clase_data = info.rpt_clase_data;
                    Entity.rpt_clase_info = info.rpt_clase_info;
                    Entity.rpt_clase_bus = info.rpt_clase_bus;
                    Entity.rpt_clase_rpt = info.rpt_clase_rpt;
                    Entity.rpt_store_procedure = info.rpt_store_procedure;
                    Entity.rpt_usa_store_procedure = info.rpt_usa_store_procedure;
                    Entity.rpt_vista = info.rpt_vista;
                    Entity.se_muestra_administrador_reportes = info.se_muestra_administrador_reportes;
                    Entity.Se_Mustra_Disenador_rpt = info.se_muestra_administrador_reportes;

                    Context.SaveChanges();
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
