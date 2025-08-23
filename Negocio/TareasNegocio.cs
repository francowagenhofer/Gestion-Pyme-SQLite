using Dominio.ReglasDelNegocio;
using negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dominio.Entidades.Dominio.Entidades;
using Dominio.Entidades;
using Microsoft.Data.Sqlite;

namespace Negocio
{
    public class TareasNegocio
    {
        public List<Tareas> ListarTodasLasTareas()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Tareas> tareas = new List<Tareas>();

            try
            {
                datos.setearConsulta("SELECT * FROM Tareas");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Tareas tarea = new Tareas
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        Nombre = Convert.ToString(datos.Lector["Nombre"]),
                        Descripcion = Convert.ToString(datos.Lector["Descripcion"]),
                        FechaInicio = datos.Lector["FechaInicio"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(datos.Lector["FechaInicio"]),
                        FechaFin = datos.Lector["FechaFin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(datos.Lector["FechaFin"]),
                        Estado = Convert.ToString(datos.Lector["Estado"])
                    };
                    tareas.Add(tarea);
                }

                return tareas;
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

        //public Tareas ObtenerInformacionTarea(int idTarea)
        //{
        //    AccesoDatos datos = new AccesoDatos();
        //    Tareas tarea = null;

        //    try
        //    {
        //        datos.setearProcedimiento("ObtenerInformacionTarea");
        //        datos.setearParametro("@IdTarea", idTarea);
        //        datos.ejecutarLectura();

        //        List<Empleado> empleadosAsignados = new List<Empleado>();
        //        HashSet<Proyectos> proyectosAsignados = new HashSet<Proyectos>(new ProyectosComparer());

        //        while (datos.Lector.Read())
        //        {
        //            if (tarea == null)
        //            {
        //                tarea = new Tareas
        //                {
        //                    Id = (int)datos.Lector["TareaId"],
        //                    Nombre = (string)datos.Lector["TareaNombre"],
        //                    Descripcion = (string)datos.Lector["TareaDescripcion"]
        //                };
        //            }

        //            if (datos.Lector["ProyectoId"] != DBNull.Value)
        //            {
        //                Proyectos proyecto = new Proyectos
        //                {
        //                    Id = (int)datos.Lector["ProyectoId"],
        //                    Nombre = (string)datos.Lector["ProyectoNombre"]
        //                };
        //                proyectosAsignados.Add(proyecto); // Usa HashSet para evitar duplicados
        //            }

        //            if (datos.Lector["EmpleadoId"] != DBNull.Value)
        //            {
        //                Empleado empleado = new Empleado
        //                {
        //                    Id = (int)datos.Lector["EmpleadoId"],
        //                    Nombre = (string)datos.Lector["EmpleadoNombre"],
        //                    Apellido = (string)datos.Lector["EmpleadoApellido"]
        //                };
        //                empleadosAsignados.Add(empleado);
        //            }
        //        }

        //        if (tarea != null)
        //        {
        //            tarea.EmpleadosAsignados = empleadosAsignados;
        //            tarea.ProyectosAsignados = proyectosAsignados.ToList(); // Convertir de HashSet a List
        //        }

        //        return tarea;
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

        //public int ObtenerIdTarea(string nombreTarea)
        //{
        //    var datos = new AccesoDatos();
        //    try
        //    {
        //        datos.setearProcedimiento("ObtenerIdTarea");
        //        datos.setearParametro("@NombreTarea", nombreTarea);
        //        datos.ejecutarLectura();

        //        if (datos.Lector.Read())
        //            return (int)datos.Lector["Id"];

        //        return -1; // No encontrada
        //    }
        //    finally
        //    {
        //        datos.cerrarConexion();
        //    }
        //}

        public void AgregarTarea(Tareas nuevaTarea)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "INSERT INTO Tareas (Nombre, Descripcion, FechaInicio, FechaFin, Estado) VALUES (@Nombre, @Descripcion, @FechaInicio, @FechaFin, @Estado)");

                datos.setearParametro("@Nombre", nuevaTarea.Nombre);
                datos.setearParametro("@Descripcion", nuevaTarea.Descripcion);
                datos.setearParametro("@FechaInicio", nuevaTarea.FechaInicio.HasValue ? nuevaTarea.FechaInicio.Value : DBNull.Value);
                datos.setearParametro("@FechaFin", nuevaTarea.FechaFin.HasValue ? nuevaTarea.FechaFin.Value : DBNull.Value);
                datos.setearParametro("@Estado", nuevaTarea.Estado);

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

        public void ModificarTarea(Tareas tarea)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "UPDATE Tareas SET Nombre = @Nombre, Descripcion = @Descripcion, FechaInicio = @FechaInicio, FechaFin = @FechaFin, Estado = @Estado WHERE Id = @IdTarea");

                datos.setearParametro("@IdTarea", tarea.Id);
                datos.setearParametro("@Nombre", tarea.Nombre);
                datos.setearParametro("@Descripcion", tarea.Descripcion);
                datos.setearParametro("@FechaInicio", tarea.FechaInicio.HasValue ? tarea.FechaInicio.Value : DBNull.Value);
                datos.setearParametro("@FechaFin", tarea.FechaFin.HasValue ? tarea.FechaFin.Value : DBNull.Value);
                datos.setearParametro("@Estado", tarea.Estado);

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

        public void EliminarTarea(int idTarea)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "DELETE FROM AsignacionTareas WHERE IdTareaProyecto = @IdTarea; DELETE FROM Tareas WHERE Id = @IdTarea");
                datos.setearParametro("@IdTarea", idTarea);
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

        // ASIGNACION EMPLEADO - TAREA
        public List<Tareas> ListarTareasAsignadasPorEmpleadoYProyecto(int idEmpleado, int idProyecto)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Tareas> lista = new List<Tareas>();

            try
            {
                datos.setearConsulta(@"
                    SELECT tp.Id AS IdTareaProyecto, t.Nombre, t.Descripcion, at.FechaAsignacion
                    FROM AsignacionTareas at
                    INNER JOIN TareasProyectos tp ON at.IdTareaProyecto = tp.Id
                    INNER JOIN Tareas t ON tp.IdTarea = t.Id
                    WHERE at.IdEmpleado = @IdEmpleado AND tp.IdProyecto = @IdProyecto
                ");

                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdProyecto", idProyecto);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Tareas t = new Tareas
                    {
                        IdTareaProyecto = Convert.ToInt32(datos.Lector["IdTareaProyecto"]),
                        Nombre = Convert.ToString(datos.Lector["Nombre"]),
                        Descripcion = datos.Lector["Descripcion"] as string,
                        FechaAsignacion = datos.Lector["FechaAsignacion"] != DBNull.Value ? Convert.ToDateTime(datos.Lector["FechaAsignacion"]) : (DateTime?)null
                    };
                    lista.Add(t);
                }
                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Tareas> ListarTareasDisponiblesParaAsignarPorEmpleadoYProyecto(int idEmpleado, int idProyecto)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Tareas> lista = new List<Tareas>();

            try
            {
                datos.setearConsulta(@"
                    SELECT tp.Id AS IdTareaProyecto, t.Nombre, t.Descripcion
                    FROM TareasProyectos tp
                    INNER JOIN Tareas t ON tp.IdTarea = t.Id
                    WHERE tp.IdProyecto = @IdProyecto
                      AND tp.Id NOT IN (
                          SELECT IdTareaProyecto FROM AsignacionTareas WHERE IdEmpleado = @IdEmpleado
                      )
                ");

                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdProyecto", idProyecto);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Tareas t = new Tareas
                    {
                        IdTareaProyecto = Convert.ToInt32(datos.Lector["IdTareaProyecto"]),
                        Nombre = Convert.ToString(datos.Lector["Nombre"]),
                        Descripcion = datos.Lector["Descripcion"] as string
                    };
                    lista.Add(t);
                }
                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void AsignarTareaAEmpleadoEnProyecto(int idTareaProyecto, int idEmpleado, int idProyecto, DateTime fechaAsignacion)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    INSERT INTO AsignacionTareas (IdTareaProyecto, IdEmpleado, IdProyecto, FechaAsignacion)
                    VALUES (@IdTareaProyecto, @IdEmpleado, @IdProyecto, @FechaAsignacion);
                ");

                datos.setearParametro("@IdTareaProyecto", idTareaProyecto);
                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdProyecto", idProyecto);
                datos.setearParametro("@FechaAsignacion", fechaAsignacion);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void DesasignarTareaDeEmpleadoEnProyecto(int idTareaProyecto, int idEmpleado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    DELETE FROM AsignacionTareas
                    WHERE IdTareaProyecto = @IdTareaProyecto AND IdEmpleado = @IdEmpleado;
                ");
                datos.setearParametro("@IdTareaProyecto", idTareaProyecto);
                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        // ASIGNACION PROYECTO - TAREA
        public void AsignarTareaAProyecto(int idTarea, int idProyecto)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    INSERT INTO TareasProyectos (IdTarea, IdProyecto)
                    VALUES (@IdTarea, @IdProyecto);
                ");
                datos.setearParametro("@IdTarea", idTarea);
                datos.setearParametro("@IdProyecto", idProyecto);
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

        public void DesignarTareaAProyecto(int idTarea, int idProyecto)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // 1. Obtener el Id de la relación tarea-proyecto
                datos.setearConsulta(@"
                                        SELECT Id FROM TareasProyectos
                                        WHERE IdTarea = @IdTarea AND IdProyecto = @IdProyecto;
                                    ");
                datos.setearParametro("@IdTarea", idTarea);
                datos.setearParametro("@IdProyecto", idProyecto);
                int idTareaProyecto = Convert.ToInt32(datos.ejecutarAccionScalar());

                // 2. Limpiar parámetros antes de la siguiente consulta
                datos.LimpiarParametros(); 

                // 3. Borrar asignaciones de empleados
                datos.setearConsulta(@"
                                        DELETE FROM AsignacionTareas
                                        WHERE IdTareaProyecto = @IdTareaProyecto;
                                    ");
                datos.setearParametro("@IdTareaProyecto", idTareaProyecto);
                datos.ejecutarAccion();

                // 4. Limpiar parámetros antes de la siguiente consulta
                datos.LimpiarParametros();

                // 5. Borrar relación tarea-proyecto
                datos.setearConsulta(@"
                                        DELETE FROM TareasProyectos
                                        WHERE Id = @IdTareaProyecto;
                                    ");
                datos.setearParametro("@IdTareaProyecto", idTareaProyecto);
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

        public List<Tareas> ListarTareasAsignadasAProyecto(int idProyecto)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Tareas> tareas = new List<Tareas>();

            try
            {
                datos.setearConsulta(@"
                    SELECT T.Id AS IdTarea, T.Nombre, T.Descripcion, T.FechaInicio, T.FechaFin, T.Estado
                    FROM Tareas T
                    INNER JOIN TareasProyectos TP ON T.Id = TP.IdTarea
                    WHERE TP.IdProyecto = @IdProyecto
                ");

                datos.setearParametro("@IdProyecto", idProyecto);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Tareas tarea = new Tareas
                    {
                        Id = Convert.ToInt32(datos.Lector["IdTarea"]),
                        Nombre = Convert.ToString(datos.Lector["Nombre"]),
                        Descripcion = Convert.ToString(datos.Lector["Descripcion"]),
                        FechaInicio = datos.Lector["FechaInicio"] != DBNull.Value ? Convert.ToDateTime(datos.Lector["FechaInicio"]) : (DateTime?)null,
                        FechaFin = datos.Lector["FechaFin"] != DBNull.Value ? Convert.ToDateTime(datos.Lector["FechaFin"]) : (DateTime?)null,
                        Estado = Convert.ToString(datos.Lector["Estado"])
                    };
                    tareas.Add(tarea);
                }

                return tareas;
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

        public List<Tareas> ListarTareasDisponiblesParaAsignarAProyecto(int idProyecto)
        {
            List<Tareas> tareas = new List<Tareas>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT T.Id, T.Nombre, T.Descripcion, T.FechaInicio, T.FechaFin, T.Estado
                    FROM Tareas T
                    WHERE T.Id NOT IN (
                        SELECT IdTarea 
                        FROM TareasProyectos 
                        WHERE IdProyecto = @IdProyecto
                    )
                ");

                datos.setearParametro("@IdProyecto", idProyecto);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Tareas tarea = new Tareas
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        Nombre = Convert.ToString(datos.Lector["Nombre"]),
                        Descripcion = Convert.ToString(datos.Lector["Descripcion"]),
                        FechaInicio = datos.Lector["FechaInicio"] != DBNull.Value ? Convert.ToDateTime(datos.Lector["FechaInicio"]) : (DateTime?)null,
                        FechaFin = datos.Lector["FechaFin"] != DBNull.Value ? Convert.ToDateTime(datos.Lector["FechaFin"]) : (DateTime?)null,
                        Estado = Convert.ToString(datos.Lector["Estado"])
                    };
                    tareas.Add(tarea);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return tareas;
        }

        //·······································································································//
        //    public List<Empleado> ListarEmpleadosAsignadosATarea(int idTarea, int idProyecto)
        //    {
        //        AccesoDatos datos = new AccesoDatos();
        //        List<Empleado> empleados = new List<Empleado>();

        //        try
        //        {
        //            datos.setearProcedimiento("ListarEmpleadosAsignadosATarea");
        //            datos.setearParametro("@IdTarea", idTarea);
        //            datos.setearParametro("@IdProyecto", idProyecto);
        //            datos.ejecutarLectura();

        //            while (datos.Lector.Read())
        //            {
        //                Empleado empleado = new Empleado();

        //                empleado.Id = (int)datos.Lector["Id"];
        //                empleado.Nombre = (string)datos.Lector["Nombre"];
        //                empleado.Apellido = (string)datos.Lector["Apellido"];

        //                empleados.Add(empleado);
        //            }

        //            return empleados;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            datos.cerrarConexion();
        //        }
        //    }

        //}

        public class ProyectosComparer : IEqualityComparer<Proyectos>
        {
            public bool Equals(Proyectos x, Proyectos y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(Proyectos obj)
            {
                return obj.Id.GetHashCode();
            }
        }

    }
}
