using Core.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.CuentasPorCobrar
{
    public class CXC_022_Data
    {
        public List<CXC_022_Info> GetList(int IdEmpresa, DateTime FechaCorte)
        {
            List<CXC_022_Info> Lista = new List<CXC_022_Info>();
            using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "declare @IdEmpresa int = "+IdEmpresa.ToString()+", @FechaCorte date = datefromparts("+FechaCorte.Year.ToString()+","+FechaCorte.Month.ToString()+","+FechaCorte.Day.ToString()+")"
                                    + " select a.IdEmpresa, a.IdSucursal, a.IdCobro, a.cr_fecha, d.tc_descripcion, g.NombreTarjeta, a.cr_Tarjeta, b.Codigo, c.pe_nombreCompleto as Alumno, f.pe_cedulaRuc, f.pe_nombreCompleto as Cliente,a.cr_observacion, a.IdUsuario,a.cr_TotalCobro"
                                    + " from cxc_cobro as a with(nolock) left join"
                                    + " aca_Alumno as b with(nolock) on a.IdEmpresa = b.IdEmpresa and a.IdAlumno = b.IdAlumno left join"
                                    + " tb_persona as c with(nolock) on b.IdPersona = c.IdPersona join"
                                    + " cxc_cobro_tipo as d with(nolock) on a.IdCobro_tipo = d.IdCobro_tipo left join"
                                    + " fa_cliente as e with(nolock) on a.IdEmpresa = e.IdEmpresa and a.IdCliente = e.IdCliente left join"
                                    + " tb_persona as f with(nolock) on e.IdPersona = f.IdPersona left join"
                                    + " tb_TarjetaCredito as g on a.IdEmpresa = g.IdEmpresa and a.IdTarjeta = g.IdTarjeta"
                                    + " where a.IdEmpresa = @IdEmpresa and a.cr_fecha <= @FechaCorte and a.cr_estado = 'A' and d.EsTarjetaCredito = 1 and not exists("
                                        + " select x.IdEmpresa"
                                        + " from cxc_LiquidacionTarjeta as x with(nolock) join"
                                        + " cxc_LiquidacionTarjeta_x_cxc_cobro as y with(nolock) on x.IdEmpresa = y.IdEmpresa and x.IdLiquidacion = y.IdLiquidacion"
                                        + " where x.IdEmpresa = @IdEmpresa and x.Estado = 1 and x.Fecha <= @FechaCorte"
                                        + " and a.IdEmpresa = y.IdEmpresa and a.IdSucursal = y.IdSucursal and a.IdCobro = y.IdCobro"
                                    + " )";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Lista.Add(new CXC_022_Info
                    {
                        IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                        IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                        IdCobro = Convert.ToDecimal(reader["IdCobro"]),
                        cr_fecha = Convert.ToDateTime(reader["cr_fecha"]),
                        tc_descripcion = Convert.ToString(reader["tc_descripcion"]),
                        NombreTarjeta = Convert.ToString(reader["NombreTarjeta"]),
                        cr_tarjeta = Convert.ToString(reader["cr_tarjeta"]),
                        Codigo = Convert.ToString(reader["Codigo"]),
                        Alumno = Convert.ToString(reader["Alumno"]),
                        pe_cedulaRuc = Convert.ToString(reader["pe_cedulaRuc"]),
                        Cliente = Convert.ToString(reader["Cliente"]),
                        cr_observacion = Convert.ToString(reader["cr_observacion"]),
                        IdUsuario = Convert.ToString(reader["IdUsuario"]),
                        cr_TotalCobro = Convert.ToDouble(reader["cr_TotalCobro"])
                    });                    
                }
                reader.Close();
            }
            return Lista;
        }
    }
}
