using Dominio.Entidades;
using Dominio.Entidades.Dominio.Entidades;
using Dominio.ReglasDelNegocio;
using Microsoft.VisualBasic;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Data.Sqlite;


namespace Negocio
{
    public class ProyectosNegocio
    {
        public List<Proyectos> ListarProyectos()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Proyectos> lista = new List<Proyectos>();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Descripcion, FechaInicio, FechaFin, Presupuesto, EstadoProyecto, IsActive FROM Proyectos");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Proyectos proyecto = new Proyectos();

                    proyecto.Id = Convert.ToInt32(datos.Lector["Id"]);
                    proyecto.Nombre = datos.Lector["Nombre"]?.ToString();
                    proyecto.Descripcion = datos.Lector["Descripcion"]?.ToString();

                    proyecto.FechaInicio = datos.Lector["FechaInicio"] != DBNull.Value
                        ? DateTime.Parse(datos.Lector["FechaInicio"].ToString())
                        : (DateTime?)null;

                    proyecto.FechaFin = datos.Lector["FechaFin"] != DBNull.Value
                        ? DateTime.Parse(datos.Lector["FechaFin"].ToString())
                        : (DateTime?)null;

                    proyecto.Presupuesto = datos.Lector["Presupuesto"] != DBNull.Value
                        ? Convert.ToDecimal(datos.Lector["Presupuesto"])
                        : 0;

                    proyecto.EstadoProyecto = datos.Lector["EstadoProyecto"]?.ToString();

                    proyecto.IsActive = datos.Lector["IsActive"] != DBNull.Value &&
                                        Convert.ToInt32(datos.Lector["IsActive"]) == 1;

                    lista.Add(proyecto);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar proyectos", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void AgregarProyecto(Proyectos nuevoProyecto)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
            INSERT INTO Proyectos 
            (Nombre, Descripcion, FechaInicio, FechaFin, Presupuesto, EstadoProyecto, IsActive) 
            VALUES 
            (@Nombre, @Descripcion, @FechaInicio, @FechaFin, @Presupuesto, @EstadoProyecto, 1)");

                datos.setearParametro("@Nombre", nuevoProyecto.Nombre ?? (object)DBNull.Value);
                datos.setearParametro("@Descripcion", nuevoProyecto.Descripcion ?? (object)DBNull.Value);
                datos.setearParametro("@FechaInicio", nuevoProyecto.FechaInicio.HasValue ? nuevoProyecto.FechaInicio.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                datos.setearParametro("@FechaFin", nuevoProyecto.FechaFin.HasValue ? nuevoProyecto.FechaFin.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                datos.setearParametro("@Presupuesto", nuevoProyecto.Presupuesto);
                datos.setearParametro("@EstadoProyecto", nuevoProyecto.EstadoProyecto ?? (object)DBNull.Value);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar proyecto", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void ModificarProyecto(Proyectos proyectoModificado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
            UPDATE Proyectos SET 
                Nombre = @Nombre,
                Descripcion = @Descripcion,
                FechaInicio = @FechaInicio,
                FechaFin = @FechaFin,
                Presupuesto = @Presupuesto,
                EstadoProyecto = @EstadoProyecto,
                IsActive = @IsActive
            WHERE Id = @Id");

                datos.setearParametro("@Id", Convert.ToInt64(proyectoModificado.Id)); 
                datos.setearParametro("@Nombre", proyectoModificado.Nombre ?? (object)DBNull.Value);
                datos.setearParametro("@Descripcion", proyectoModificado.Descripcion ?? (object)DBNull.Value);
                datos.setearParametro("@FechaInicio", proyectoModificado.FechaInicio.HasValue ? proyectoModificado.FechaInicio.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                datos.setearParametro("@FechaFin", proyectoModificado.FechaFin.HasValue ? proyectoModificado.FechaFin.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                datos.setearParametro("@Presupuesto", proyectoModificado.Presupuesto);
                datos.setearParametro("@EstadoProyecto", proyectoModificado.EstadoProyecto ?? (object)DBNull.Value);
                datos.setearParametro("@IsActive", proyectoModificado.IsActive ? 1 : 0);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar proyecto", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void EliminarProyecto(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM Proyectos WHERE Id = @Id");
                datos.setearParametro("@Id", Convert.ToInt64(id));

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar proyecto", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void AsignarEmpleadoAProyecto(int idProyecto, int idEmpleado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "INSERT INTO AsignacionProyectos (IdEmpleado, IdProyecto) VALUES (@IdEmpleado, @IdProyecto)"
                );

                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdProyecto", idProyecto);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void DesasignarEmpleadoDeProyecto(int idProyecto, int idEmpleado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "DELETE FROM AsignacionProyectos WHERE IdEmpleado = @IdEmpleado AND IdProyecto = @IdProyecto"
                );

                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdProyecto", idProyecto);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        //public void DesasignarEmpleadoDeTodosLosProyectos(int idEmpleado)
        //{
        //    AccesoDatos datos = new AccesoDatos();
        //    try
        //    {
        //        datos.setearProcedimiento("DesasignarEmpleadoDeTodosLosProyectos");
        //        datos.setearParametro("@IdEmpleado", idEmpleado);

        //        datos.ejecutarAccion();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        datos.cerrarConexion();
        //    }
        //}

        public List<Proyectos> ListarProyectosAsignadosEmpleado(int idEmpleado)
        {
            List<Proyectos> proyectosAsignados = new List<Proyectos>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT 
                p.Id AS ProyectoId, 
                p.Nombre,
                p.Descripcion,
                p.FechaInicio,
                p.FechaFin,
                p.Presupuesto,
                p.EstadoProyecto,
                p.IsActive
            FROM AsignacionProyectos ap
            INNER JOIN Proyectos p ON ap.IdProyecto = p.Id
            WHERE ap.IdEmpleado = @IdEmpleado");

                datos.setearParametro("@IdEmpleado", idEmpleado);

                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Proyectos proyecto = new Proyectos();
                    proyecto.Id = Convert.ToInt32(datos.Lector["ProyectoId"]);
                    proyecto.Nombre = datos.Lector["Nombre"]?.ToString();
                    proyecto.Descripcion = datos.Lector["Descripcion"]?.ToString();
                    proyecto.EstadoProyecto = datos.Lector["EstadoProyecto"]?.ToString();
                    proyecto.IsActive = datos.Lector["IsActive"] != DBNull.Value &&
                                        Convert.ToInt32(datos.Lector["IsActive"]) == 1;

                    proyectosAsignados.Add(proyecto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar proyectos asignados a empleado", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return proyectosAsignados;
        }


        public List<Proyectos> ListarProyectosDisponiblesEmpleado(int idEmpleado)
        {
            List<Proyectos> proyectosDisponibles = new List<Proyectos>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT 
                p.Id,
                p.Nombre,
                p.Descripcion,
                p.EstadoProyecto,
                p.IsActive
            FROM Proyectos p
            WHERE p.IsActive = 1
              AND p.Id NOT IN (
                  SELECT ap.IdProyecto
                  FROM AsignacionProyectos ap
                  WHERE ap.IdEmpleado = @IdEmpleado
              )");

                datos.setearParametro("@IdEmpleado", idEmpleado);

                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Proyectos proyecto = new Proyectos();
                    proyecto.Id = Convert.ToInt32(datos.Lector["Id"]);
                    proyecto.Nombre = datos.Lector["Nombre"]?.ToString();
                    proyecto.Descripcion = datos.Lector["Descripcion"]?.ToString();
                    proyecto.EstadoProyecto = datos.Lector["EstadoProyecto"]?.ToString();
                    proyecto.IsActive = datos.Lector["IsActive"] != DBNull.Value &&
                                        Convert.ToInt32(datos.Lector["IsActive"]) == 1;

                    proyectosDisponibles.Add(proyecto);
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return proyectosDisponibles;
        }


    }
}
