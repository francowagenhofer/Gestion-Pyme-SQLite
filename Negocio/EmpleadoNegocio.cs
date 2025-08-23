using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.ReglasDelNegocio;
using negocio;
using Microsoft.Data.Sqlite;


using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;

namespace Negocio
{
    public class EmpleadoNegocio
    {
        // LISTAR EMPLEADOS
        public List<Empleado> ListarEmpleados()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Empleado> lista = new List<Empleado>();

            try
            {
                datos.setearConsulta(@"
                    SELECT 
                        E.Id,
                        E.Nombre,
                        E.Apellido,
                        E.FechaNacimiento,
                        E.DNI,
                        E.Imagen,
                        E.FechaIngreso,
                        E.IsActive,
                        C.Nombre AS NombreCategoria,   
                        S.Monto AS MontoSalario        
                    FROM Empleados E
                    LEFT JOIN Categorias C ON E.IdCategoria = C.Id   
                    LEFT JOIN Salarios S ON E.IdSalario = S.Id
                ");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Empleado aux = new Empleado();

                    aux.Id = Convert.ToInt32(datos.Lector["Id"]);

                    aux.Nombre = datos.Lector["Nombre"]?.ToString();
                    aux.Apellido = datos.Lector["Apellido"]?.ToString();

                    // SQLite guarda fechas como texto, se parsea si no es NULL
                    aux.FechaNacimiento = datos.Lector["FechaNacimiento"] != DBNull.Value
                        ? DateTime.Parse(datos.Lector["FechaNacimiento"].ToString())
                        : (DateTime?)null;

                    aux.DNI = datos.Lector["DNI"] != DBNull.Value ? datos.Lector["DNI"].ToString() : null;
                    aux.Imagen = datos.Lector["Imagen"] != DBNull.Value ? datos.Lector["Imagen"].ToString() : null;

                    aux.FechaIngreso = datos.Lector["FechaIngreso"] != DBNull.Value
                        ? DateTime.Parse(datos.Lector["FechaIngreso"].ToString())
                        : DateTime.MinValue;

                    aux.IsActive = datos.Lector["IsActive"] != DBNull.Value && Convert.ToInt32(datos.Lector["IsActive"]) == 1;

                    aux.NombreCategoria = datos.Lector["NombreCategoria"] != DBNull.Value ? datos.Lector["NombreCategoria"].ToString() : null;

                    aux.MontoSalario = datos.Lector["MontoSalario"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["MontoSalario"]) : (decimal?)null;

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar empleados", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // AGREGAR EMPLEADO
        public void AgregarEmpleado(Empleado agregarEmpleado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    INSERT INTO Empleados
                    (Nombre, Apellido, FechaNacimiento, DNI, Imagen, FechaIngreso, IdCategoria, IdSalario, IsActive)
                    VALUES
                    (@Nombre, @Apellido, @FechaNacimiento, @DNI, @Imagen, @FechaIngreso, @IdCategoria, @IdSalario, @IsActive)
                ");

                datos.setearParametro("@Nombre", agregarEmpleado.Nombre ?? (object)DBNull.Value);
                datos.setearParametro("@Apellido", agregarEmpleado.Apellido ?? (object)DBNull.Value);
                datos.setearParametro("@FechaNacimiento", agregarEmpleado.FechaNacimiento.HasValue ? agregarEmpleado.FechaNacimiento.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                datos.setearParametro("@DNI", agregarEmpleado.DNI ?? (object)DBNull.Value);
                datos.setearParametro("@Imagen", agregarEmpleado.Imagen ?? (object)DBNull.Value);
                datos.setearParametro("@FechaIngreso", agregarEmpleado.FechaIngreso.HasValue ? agregarEmpleado.FechaIngreso.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);

                datos.setearParametro("@IdCategoria", agregarEmpleado.Categoria);
                datos.setearParametro("@IdSalario", agregarEmpleado.Salario);
                datos.setearParametro("@IsActive", agregarEmpleado.IsActive ? 1 : 0);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar empleado", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // VALIDAR DNI
        public bool ValidarDNI(string dni)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Empleados WHERE DNI = @DNI");
                datos.setearParametro("@DNI", dni);

                long conteo = (long)datos.ejecutarEscalar();
                return conteo > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar el DNI", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // MODIFICAR EMPLEADO
        public void ModificarEmpleado(Empleado modificarEmpleado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    UPDATE Empleados SET 
                        Nombre = @Nombre,
                        Apellido = @Apellido,
                        FechaNacimiento = @FechaNacimiento,
                        DNI = @DNI,
                        Imagen = @Imagen,
                        FechaIngreso = @FechaIngreso,
                        IdCategoria = @IdCategoria,
                        IdSalario = @IdSalario,
                        IsActive = @IsActive
                    WHERE Id = @Id
                ");

                datos.setearParametro("@Nombre", modificarEmpleado.Nombre ?? (object)DBNull.Value);
                datos.setearParametro("@Apellido", modificarEmpleado.Apellido ?? (object)DBNull.Value);
                datos.setearParametro("@FechaNacimiento", modificarEmpleado.FechaNacimiento.HasValue ? modificarEmpleado.FechaNacimiento.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                datos.setearParametro("@DNI", modificarEmpleado.DNI ?? (object)DBNull.Value);
                datos.setearParametro("@Imagen", modificarEmpleado.Imagen ?? (object)DBNull.Value);
                datos.setearParametro("@FechaIngreso", modificarEmpleado.FechaIngreso.HasValue ? modificarEmpleado.FechaIngreso.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                datos.setearParametro("@IdCategoria", modificarEmpleado.Categoria);
                datos.setearParametro("@IdSalario", modificarEmpleado.Salario);
                datos.setearParametro("@IsActive", modificarEmpleado.IsActive ? 1 : 0);
                datos.setearParametro("@Id", modificarEmpleado.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar empleado", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // ELIMINAR EMPLEADO
        public void EliminarEmpleado(int id)
        {
            if (TieneDependencias(id))
                throw new InvalidOperationException("El empleado tiene roles, proyectos, tareas o bonos vinculados. Debe desvincularlos antes de eliminar.");

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    DELETE FROM Empleados
                    WHERE Id = @Id
                ");

                datos.setearParametro("@Id", Convert.ToInt64(id)); 

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

        // Lista empleados asignados a un proyecto
        public List<Empleado> ListarEmpleadosAsignados(int idProyecto)
        {
            List<Empleado> empleadosAsignados = new List<Empleado>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT 
                        e.Id,
                        e.Nombre,
                        e.Apellido,
                        e.DNI,
                        e.FechaIngreso,
                        e.IsActive,
                        c.Nombre AS NombreCategoria
                    FROM AsignacionProyectos ap
                    INNER JOIN Empleados e ON ap.IdEmpleado = e.Id
                    INNER JOIN Categorias c ON e.IdCategoria = c.Id
                    WHERE ap.IdProyecto = @IdProyecto
                      AND e.IsActive = 1
                ");

                datos.setearParametro("@IdProyecto", idProyecto);

                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Empleado empleado = new Empleado();
                    empleado.Id = Convert.ToInt32(datos.Lector["Id"]);
                    empleado.Nombre = datos.Lector["Nombre"].ToString();
                    empleado.Apellido = datos.Lector["Apellido"].ToString();
                    empleado.IsActive = Convert.ToInt32(datos.Lector["IsActive"]) == 1;
                    empleado.NombreCategoria = datos.Lector["NombreCategoria"] != DBNull.Value ? datos.Lector["NombreCategoria"].ToString() : null;
                    empleadosAsignados.Add(empleado);
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

            return empleadosAsignados;
        }

        // Lista empleados disponibles para asignar a proyecto
        public List<Empleado> ListarEmpleadosDisponiblesParaAsignarAProyecto(int idProyecto)
        {
            List<Empleado> empleadosDisponibles = new List<Empleado>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT 
                        e.Id,
                        e.Nombre,
                        e.Apellido,
                        e.DNI,
                        e.FechaIngreso,
                        e.IsActive,
                        c.Nombre AS NombreCategoria
                    FROM Empleados e
                    INNER JOIN Categorias c ON e.IdCategoria = c.Id
                    WHERE e.IsActive = 1
                      AND e.Id NOT IN (
                          SELECT IdEmpleado 
                          FROM AsignacionProyectos 
                          WHERE IdProyecto = @IdProyecto
                      )
                ");
                datos.setearParametro("@IdProyecto", idProyecto);

                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Empleado empleado = new Empleado();
                    empleado.Id = Convert.ToInt32(datos.Lector["Id"]);
                    empleado.Nombre = datos.Lector["Nombre"].ToString();
                    empleado.Apellido = datos.Lector["Apellido"].ToString();
                    empleado.IsActive = Convert.ToInt32(datos.Lector["IsActive"]) == 1;
                    empleado.NombreCategoria = datos.Lector["NombreCategoria"] != DBNull.Value ? datos.Lector["NombreCategoria"].ToString() : null;
                    empleadosDisponibles.Add(empleado);
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

            return empleadosDisponibles;
        }

        public bool TieneDependencias(int empleadoId)
        {
            return ObtenerCantidadRoles(empleadoId) > 0 ||
                   ObtenerCantidadProyectos(empleadoId) > 0 ||
                   ObtenerCantidadTareas(empleadoId) > 0 ||
                   ObtenerCantidadBonos(empleadoId) > 0;
        }

        private int ObtenerCantidadRoles(int empleadoId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM AsignacionRolesProyecto WHERE IdEmpleado = @IdEmpleado");
                datos.setearParametro("@IdEmpleado", empleadoId);
                return Convert.ToInt32(datos.ejecutarEscalar());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private int ObtenerCantidadProyectos(int empleadoId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM AsignacionProyectos WHERE IdEmpleado = @IdEmpleado");
                datos.setearParametro("@IdEmpleado", empleadoId);
                return Convert.ToInt32(datos.ejecutarEscalar());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private int ObtenerCantidadTareas(int empleadoId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM AsignacionTareas WHERE IdEmpleado = @IdEmpleado");
                datos.setearParametro("@IdEmpleado", empleadoId);
                return Convert.ToInt32(datos.ejecutarEscalar());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private int ObtenerCantidadBonos(int empleadoId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM AsignacionBonos WHERE IdEmpleado = @IdEmpleado");
                datos.setearParametro("@IdEmpleado", empleadoId);
                return Convert.ToInt32(datos.ejecutarEscalar());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
