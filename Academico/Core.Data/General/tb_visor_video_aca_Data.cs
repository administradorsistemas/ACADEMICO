﻿using Core.Data.Base;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.General
{
   public class tb_visor_video_aca_Data
    {
        public List<tb_visor_video_aca_Info> get_list(bool mostrar_anulados)
        {
            try
            {
                List<tb_visor_video_aca_Info> Lista;
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                        Lista = (from q in Context.tb_visor_video_aca
                                 select new tb_visor_video_aca_Info
                                 {
                                     Cod_video = q.Cod_video,
                                     Nombre_video = q.Nombre_video,
                                     Estado=q.Estado
                                 }).ToList();
                    
                }
                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

      
        public bool guardarDB(tb_visor_video_aca_Info info)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_visor_video_aca Entity = new tb_visor_video_aca
                    {
                        Cod_video = info.Cod_video,
                        Nombre_video = info.Nombre_video,
                        IdUsuario = info.IdUsuario,
                        Estado=true
                    };
                    Context.tb_visor_video_aca.Add(Entity);
                    Context.SaveChanges();

                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tb_visor_video_aca_Info get_info(string Cod_video)
        {
            try
            {
                tb_visor_video_aca_Info info = new tb_visor_video_aca_Info();
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_visor_video_aca Entity = Context.tb_visor_video_aca.FirstOrDefault(q => q.Cod_video == Cod_video);
                    if (Entity == null) return null;

                    info = new tb_visor_video_aca_Info
                    {
                        Cod_video = Entity.Cod_video,
                        Nombre_video = Entity.Nombre_video
                    };
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool si_existe(string Cod_video)
        {
            try
            {
                tb_visor_video_aca_Info info = new tb_visor_video_aca_Info();
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_visor_video_aca Entity = Context.tb_visor_video_aca.FirstOrDefault(q => q.Cod_video == Cod_video);
                    if (Entity == null)
                        return false;
                    else
                        return true;

                   
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(tb_visor_video_aca_Info info)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_visor_video_aca Entity = Context.tb_visor_video_aca.FirstOrDefault(q => q.Cod_video == info.Cod_video);
                    if (Entity == null)
                        return false;
                    Entity.Nombre_video = info.Nombre_video;
                    Entity.IdUsuarioModifica = info.IdUsuarioModifica;
                    Context.SaveChanges();

                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(tb_visor_video_aca_Info info)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_visor_video_aca Entity = Context.tb_visor_video_aca.FirstOrDefault(q => q.Cod_video == info.Cod_video);
                    if (Entity == null)
                        return false;
                    Entity.Estado = false;
                    Entity.IdUsuarioAnulacion = info.IdUsuarioAnulacion;
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
