using Dominio.Entidades.Dominio.Entidades;
using Dominio.Entidades;
using Dominio.ReglasDelNegocio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Negocio
{
    public class RolNegocio
    {
        public List<Rol> ListarRoles()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Rol> roles = new List<Rol>();
            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Descripcion FROM Roles");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Rol rol = new Rol
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Descripcion = datos.Lector["Descripcion"]?.ToString()
                    };
                    roles.Add(rol);
                }

                return roles;
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

        //public Rol ObtenerInformacionRol(int idRol)
        //{
        //    AccesoDatos datos = new AccesoDatos();
        //    Rol rol = null;

        //    try
        //    {
        //        datos.setearProcedimiento("ObtenerInformacionRol");
        //        datos.setearParametro("@IdRol", idRol);
        //        datos.ejecutarLectura();

        //        List<Empleado> empleadosAsignados = new List<Empleado>();
        //        HashSet<Proyectos> proyectosAsignados = new HashSet<Proyectos>(new ProyectosComparer());

        //        while (datos.Lector.Read())
        //        {
        //            if (rol == null)
        //            {
        //                rol = new Rol
        //                {
        //                    Id = (int)datos.Lector["RolId"],
        //                    Nombre = (string)datos.Lector["RolNombre"],
        //                    Descripcion = (string)datos.Lector["RolDescripcion"]
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

        //        if (rol != null)
        //        {
        //            rol.EmpleadosAsignados = empleadosAsignados;
        //            rol.ProyectosAsignados = proyectosAsignados.ToList(); // Convertir de HashSet a List
        //        }

        //        return rol;
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

        public void CrearRol(Rol rol)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO Roles (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)");
                datos.setearParametro("@Nombre", rol.Nombre);
                datos.setearParametro("@Descripcion", rol.Descripcion);
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


        public void ModificarRol(Rol rol)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Roles SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id");
                datos.setearParametro("@Id", rol.Id);
                datos.setearParametro("@Nombre", rol.Nombre);
                datos.setearParametro("@Descripcion", rol.Descripcion);
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

        public void EliminarRol(int idRol)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // 1. Primero eliminar las asignaciones
                datos.setearConsulta("DELETE FROM AsignacionRolesProyecto WHERE IdRol = @IdRol");
                datos.setearParametro("@IdRol", idRol);
                datos.ejecutarAccion();

                datos.LimpiarParametros();

                // 2. Luego eliminar el rol
                datos.setearConsulta("DELETE FROM Roles WHERE Id = @IdRol");
                datos.setearParametro("@IdRol", idRol);
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


        // Roles asignados a empleados en proyectos
        public void AsignarRolAEmpleadoEnProyecto(int idEmpleado, int idRol, int idProyecto)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO AsignacionRolesProyecto (IdEmpleado, IdRol, IdProyecto, FechaAsignacion) " +
                                     "VALUES (@IdEmpleado, @IdRol, @IdProyecto, @FechaAsignacion)");

                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdRol", idRol);
                datos.setearParametro("@IdProyecto", idProyecto);
                datos.setearParametro("@FechaAsignacion", DateTime.Now);

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

        public void DesasignarRolDeEmpleadoEnProyecto(int idEmpleado, int idRol, int idProyecto)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM AsignacionRolesProyecto " +
                                     "WHERE IdEmpleado = @IdEmpleado AND IdRol = @IdRol AND IdProyecto = @IdProyecto");

                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdRol", idRol);
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

        public List<Rol> ListarRolesAsignadosPorEmpleadoYProyecto(int idEmpleado, int idProyecto)
        {
            List<Rol> lista = new List<Rol>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT r.Id AS IdRol, r.Nombre AS NombreRol " +
                                     "FROM AsignacionRolesProyecto arp " +
                                     "INNER JOIN Roles r ON arp.IdRol = r.Id " +
                                     "WHERE arp.IdEmpleado = @IdEmpleado AND arp.IdProyecto = @IdProyecto;");

                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdProyecto", idProyecto);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Rol
                    {
                        Id = Convert.ToInt32(datos.Lector["IdRol"]),
                        Nombre = datos.Lector["NombreRol"].ToString()
                    });
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }

        // Roles asignados a proyectos
        public List<Rol> ListarRolesAsignadosAProyecto(int idProyecto)
        {
            List<Rol> lista = new List<Rol>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT r.Id, r.Nombre, r.Descripcion " +
                                     "FROM AsignacionRolesProyecto arp " +
                                     "INNER JOIN Roles r ON arp.IdRol = r.Id " +
                                     "WHERE arp.IdProyecto = @IdProyecto AND arp.IdEmpleado IS NULL");

                datos.setearParametro("@IdProyecto", idProyecto);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Rol
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Descripcion = datos.Lector["Descripcion"]?.ToString()
                    });
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }

        public List<Rol> ListarRolesDisponiblesParaAsignarPorEmpleadoYProyecto(int idEmpleado, int idProyecto)
        {
            List<Rol> lista = new List<Rol>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT r.Id AS IdRol, r.Nombre AS NombreRol " +
                                     "FROM Roles r " +
                                     "WHERE NOT EXISTS ( " +
                                     "    SELECT 1 FROM AsignacionRolesProyecto arp " +
                                     "    WHERE arp.IdEmpleado = @IdEmpleado " +
                                     "      AND arp.IdProyecto = @IdProyecto " +
                                     "      AND arp.IdRol = r.Id);");

                datos.setearParametro("@IdEmpleado", idEmpleado);
                datos.setearParametro("@IdProyecto", idProyecto);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Rol
                    {
                        Id = Convert.ToInt32(datos.Lector["IdRol"]),
                        Nombre = datos.Lector["NombreRol"].ToString()
                    });
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }

        public List<Rol> ListarRolesDisponiblesParaAsignarAProyecto(int idProyecto)
        {
            List<Rol> lista = new List<Rol>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT r.Id, r.Nombre, r.Descripcion " +
                                     "FROM Roles r " +
                                     "WHERE r.Id NOT IN ( " +
                                     "    SELECT IdRol " +
                                     "    FROM AsignacionRolesProyecto " +
                                     "    WHERE IdProyecto = @IdProyecto AND IdEmpleado IS NULL);");

                datos.setearParametro("@IdProyecto", idProyecto);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Rol
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Descripcion = datos.Lector["Descripcion"]?.ToString()
                    });
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }

        //public void AsignarRolAProyecto(int idProyecto, int idRol)
        //{
        //    AccesoDatos datos = new AccesoDatos();
        //    try
        //    {
        //        datos.setearConsulta("INSERT INTO AsignacionRolesProyecto (IdProyecto, IdRol, FechaAsignacion, IdEmpleado) " +
        //                             "VALUES (@IdProyecto, @IdRol, @FechaAsignacion, NULL)");

        //        datos.setearParametro("@IdProyecto", idProyecto);
        //        datos.setearParametro("@IdRol", idRol);
        //        datos.setearParametro("@FechaAsignacion", DateTime.Now);

        //        datos.ejecutarAccion();
        //    }
        //    finally
        //    {
        //        datos.cerrarConexion();
        //    }
        //}

        public void AsignarRolAProyecto(int idProyecto, int idRol, int? idEmpleado = null)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "INSERT INTO AsignacionRolesProyecto (IdProyecto, IdRol, FechaAsignacion, IdEmpleado) " +
                    "VALUES (@IdProyecto, @IdRol, @FechaAsignacion, @IdEmpleado)");

                datos.setearParametro("@IdProyecto", idProyecto);
                datos.setearParametro("@IdRol", idRol);
                datos.setearParametro("@FechaAsignacion", DateTime.Now);
               
                if (idEmpleado.HasValue)
                    datos.setearParametro("@IdEmpleado", idEmpleado.Value);
                else
                    datos.setearParametro("@IdEmpleado", DBNull.Value);


                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void DesasignarRolDeProyecto(int idProyecto, int idRol)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM AsignacionRolesProyecto " +
                                     "WHERE IdProyecto = @IdProyecto AND IdRol = @IdRol AND IdEmpleado IS NULL");

                datos.setearParametro("@IdProyecto", idProyecto);
                datos.setearParametro("@IdRol", idRol);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}