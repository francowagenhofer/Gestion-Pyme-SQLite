using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.ReglasDelNegocio;
using Microsoft.Data.Sqlite;
using negocio;

namespace Negocio
{
    public class BonosNegocio
    {
        public List<Bonos> ListarBonos()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Bonos> lista = new List<Bonos>();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Monto FROM TipoBonos");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Bonos bono = new Bonos
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Monto = Convert.ToDecimal(datos.Lector["Monto"])
                    };
                    lista.Add(bono);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void AgregarBono(Bonos nuevoBono)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO TipoBonos (Nombre, Monto) VALUES (@Nombre, @Monto)");
                datos.setearParametro("@Nombre", nuevoBono.Nombre);
                datos.setearParametro("@Monto", nuevoBono.Monto);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void ModificarBono(Bonos bonoModificado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE TipoBonos SET Monto = @Monto WHERE Id = @Id");
                datos.setearParametro("@Id", bonoModificado.Id);
                datos.setearParametro("@Monto", bonoModificado.Monto);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void EliminarBono(int idBono)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM TipoBonos WHERE Id = @Id");
                datos.setearParametro("@Id", idBono);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void AsignarBonoAEmpleado(int idEmpleado, int idTipoBono, DateTime fechaAsignacion)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO AsignacionBonos (IdEmpleado, IdTipoBono, FechaAsignacion) VALUES (@IdEmpleado, @IdTipoBono, @FechaAsignacion)");
                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdTipoBono", idTipoBono);
                datos.setearParametro("@FechaAsignacion", fechaAsignacion);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void DesasignarBonoDeEmpleado(int idEmpleado, int idTipoBono)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM AsignacionBonos WHERE IdEmpleado = @IdEmpleado AND IdTipoBono = @IdTipoBono");
                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdTipoBono", idTipoBono);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Bonos> ListarBonosAsignadosEmpleado(int idEmpleado)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Bonos> bonosAsignados = new List<Bonos>();

            try
            {
                datos.setearConsulta(@"
                    SELECT 
                        ab.Id AS AsignacionId,
                        tb.Id AS BonoId,
                        tb.Nombre,
                        tb.Monto,
                        ab.FechaAsignacion
                    FROM AsignacionBonos ab
                    INNER JOIN TipoBonos tb ON ab.IdTipoBono = tb.Id
                    INNER JOIN Empleados e ON ab.IdEmpleado = e.Id
                    WHERE ab.IdEmpleado = @IdEmpleado
                    ORDER BY ab.FechaAsignacion DESC
                ");
                datos.setearParametro("@IdEmpleado", idEmpleado);

                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Bonos bono = new Bonos
                    {
                        Id = Convert.ToInt32(datos.Lector["BonoId"]),
                        Nombre = datos.Lector["Nombre"] != DBNull.Value ? datos.Lector["Nombre"].ToString() : "Nombre no disponible",
                        Monto = datos.Lector["Monto"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["Monto"]) : 0,
                        FechaAsignacion = datos.Lector["FechaAsignacion"] != DBNull.Value ? Convert.ToDateTime(datos.Lector["FechaAsignacion"]) : DateTime.MinValue
                    };
                    bonosAsignados.Add(bono);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer bonos asignados: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
            finally
            {
                datos.cerrarConexion();
            }

            return bonosAsignados;
        }

        public List<Bonos> ListarBonosDisponiblesEmpleado(int idEmpleado)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Bonos> bonosDisponibles = new List<Bonos>();

            try
            {
                datos.setearConsulta(@"
                    SELECT 
                        tb.Id AS BonoId,
                        tb.Nombre,
                        tb.Monto
                    FROM TipoBonos tb
                    WHERE tb.Id NOT IN (
                        SELECT ab.IdTipoBono
                        FROM AsignacionBonos ab
                        WHERE ab.IdEmpleado = @IdEmpleado
                    )
                    ORDER BY tb.Nombre
                ");
                datos.setearParametro("@IdEmpleado", idEmpleado);

                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Bonos bono = new Bonos
                    {
                        Id = Convert.ToInt32(datos.Lector["BonoId"]),
                        Nombre = datos.Lector["Nombre"] != DBNull.Value ? datos.Lector["Nombre"].ToString() : "Nombre no disponible",
                        Monto = datos.Lector["Monto"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["Monto"]) : 0
                    };
                    bonosDisponibles.Add(bono);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer bonos disponibles: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
            finally
            {
                datos.cerrarConexion();
            }

            return bonosDisponibles;
        }
    }
}