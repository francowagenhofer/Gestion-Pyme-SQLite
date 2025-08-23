using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.ReglasDelNegocio;
using Dominio.Entidades.Dominio.Entidades;
using negocio;
using Microsoft.Data.Sqlite;

namespace Negocio
{
    public class ReportesNegocio
    {
        // METODOS DE REPORTE DE EMPLEADOS
        public List<ReporteEmpleado> ObtenerReportesEmpleados()
        {
            AccesoDatos datos = new AccesoDatos();
            List<ReporteEmpleado> lista = new List<ReporteEmpleado>();

            try
            {
                datos.setearConsulta(@"
                    SELECT 
                        Id,
                        IdEmpleado,
                        NombreEmpleado,
                        Categoria,
                        SalarioActual,
                        TotalBonos,
                        ProyectosAsignados,
                        RolesAsignados,
                        TareasAsignadas,
                        FechaGeneracion
                    FROM ReportesEmpleados;
                ");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    ReporteEmpleado aux = new ReporteEmpleado();

                    aux.Id = Convert.ToInt32(datos.Lector["Id"]);
                    aux.IdEmpleado = Convert.ToInt32(datos.Lector["IdEmpleado"]);
                    aux.NombreEmpleado = datos.Lector["NombreEmpleado"].ToString();
                    aux.Categoria = datos.Lector["Categoria"].ToString();

                    aux.SalarioActual = datos.Lector["SalarioActual"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["SalarioActual"]) : 0m;
                    aux.TotalBonos = datos.Lector["TotalBonos"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["TotalBonos"]) : 0m;

                    aux.ProyectosAsignados = datos.Lector["ProyectosAsignados"] != DBNull.Value ? datos.Lector["ProyectosAsignados"].ToString() : "";
                    aux.RolesAsignados = datos.Lector["RolesAsignados"] != DBNull.Value ? datos.Lector["RolesAsignados"].ToString() : "";
                    aux.TareasAsignadas = datos.Lector["TareasAsignadas"] != DBNull.Value ? datos.Lector["TareasAsignadas"].ToString() : "";

                    aux.FechaGeneracion = Convert.ToDateTime(datos.Lector["FechaGeneracion"]);

                    lista.Add(aux);
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

        public void GenerarReporteEmpleados(ReporteEmpleado nuevoReporte)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {

                datos.setearConsulta(@"
                                       INSERT INTO ReportesEmpleados 
                                           (IdEmpleado, NombreEmpleado, Categoria, SalarioActual, TotalBonos, ProyectosAsignados, RolesAsignados, TareasAsignadas, FechaGeneracion)
                                       SELECT 
                                           e.Id AS IdEmpleado,
                                           e.Nombre || ' ' || e.Apellido AS NombreEmpleado,
                                           c.Nombre AS Categoria,
                                           s.Monto AS SalarioActual,
                                           IFNULL(SUM(tb.Monto), 0) AS TotalBonos,
                                           (SELECT GROUP_CONCAT(p.Nombre, ', ') 
                                            FROM AsignacionProyectos ap 
                                            JOIN Proyectos p ON ap.IdProyecto = p.Id 
                                            WHERE ap.IdEmpleado = e.Id) AS ProyectosAsignados,
                                           (SELECT GROUP_CONCAT(r.Nombre, ', ') 
                                            FROM AsignacionRolesProyecto arp 
                                            JOIN Roles r ON arp.IdRol = r.Id 
                                            WHERE arp.IdEmpleado = e.Id) AS RolesAsignados,
                                           (SELECT GROUP_CONCAT(tarea, ', ')
                                            FROM (
                                                SELECT DISTINCT t.Nombre || ' (' || pr.Nombre || ')' AS tarea
                                                FROM AsignacionTareas at
                                                JOIN TareasProyectos tp ON at.IdTareaProyecto = tp.Id
                                                JOIN Tareas t ON tp.IdTarea = t.Id
                                                JOIN Proyectos pr ON tp.IdProyecto = pr.Id
                                                WHERE at.IdEmpleado = e.Id
                                            )) AS TareasAsignadas,
                                           datetime('now') AS FechaGeneracion
                                       FROM Empleados e
                                       LEFT JOIN Categorias c ON e.IdCategoria = c.Id
                                       LEFT JOIN Salarios s ON e.IdSalario = s.Id
                                       LEFT JOIN AsignacionBonos ab ON ab.IdEmpleado = e.Id
                                       LEFT JOIN TipoBonos tb ON ab.IdTipoBono = tb.Id
                                       WHERE e.Id = @IdEmpleado
                                       GROUP BY e.Id, e.Nombre, e.Apellido, c.Nombre, s.Monto;
                                     ");

                datos.setearParametro("@IdEmpleado", nuevoReporte.IdEmpleado);
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

        public void EliminarReporteEmpleados(int idReporte)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"DELETE FROM ReportesEmpleados WHERE Id = @Id;");
                datos.setearParametro("@Id", idReporte);
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


        // ------------------------------------------------------------------------------------------------------------- // 
        // METODOS DE REPORTE DE PROYECTOS

        public List<ReporteProyecto> ObtenerReportesProyectos()
        {
            AccesoDatos datos = new AccesoDatos();
            List<ReporteProyecto> lista = new List<ReporteProyecto>();

            try
            {
                datos.setearConsulta(@"
                    SELECT 
                        Id,
                        IdProyecto,
                        NombreProyecto,
                        Presupuesto,
                        EstadoProyecto,
                        AsignacionesEmpleados,
                        TareasAsignadas,
                        RolesAsignados,
                        TiempoEstimado,
                        FechaGeneracion
                    FROM ReportesProyectos;
                ");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    ReporteProyecto aux = new ReporteProyecto();

                    aux.Id = Convert.ToInt32(datos.Lector["Id"]);
                    aux.IdProyecto = Convert.ToInt32(datos.Lector["IdProyecto"]);
                    aux.NombreProyecto = datos.Lector["NombreProyecto"].ToString();

                    aux.Presupuesto = datos.Lector["Presupuesto"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["Presupuesto"]) : 0m;
                    aux.TiempoEstimado = datos.Lector["TiempoEstimado"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["TiempoEstimado"]) : 0m;

                    aux.EstadoProyecto = datos.Lector["EstadoProyecto"] != DBNull.Value ? datos.Lector["EstadoProyecto"].ToString() : "";
                    aux.AsignacionesEmpleados = datos.Lector["AsignacionesEmpleados"] != DBNull.Value ? datos.Lector["AsignacionesEmpleados"].ToString() : "";
                    aux.TareasAsignadas = datos.Lector["TareasAsignadas"] != DBNull.Value ? datos.Lector["TareasAsignadas"].ToString() : "";
                    aux.RolesAsignados = datos.Lector["RolesAsignados"] != DBNull.Value ? datos.Lector["RolesAsignados"].ToString() : "";

                    aux.FechaGeneracion = datos.Lector["FechaGeneracion"] != DBNull.Value ? Convert.ToDateTime(datos.Lector["FechaGeneracion"]) : DateTime.MinValue;

                    lista.Add(aux);
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

        public void GenerarReporteProyectos(ReporteProyecto nuevoReporte)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                                        INSERT INTO ReportesProyectos 
                                            (IdProyecto, NombreProyecto, Presupuesto, EstadoProyecto, TiempoEstimado, AsignacionesEmpleados, RolesAsignados, TareasAsignadas, FechaGeneracion)
                                        SELECT 
                                            p.Id AS IdProyecto,
                                            p.Nombre AS NombreProyecto,
                                            p.Presupuesto,
                                            p.EstadoProyecto,
                                            IFNULL(SUM(s.Monto), 0) AS TiempoEstimado,
                                            (SELECT GROUP_CONCAT(e.Nombre || ' ' || e.Apellido, ', ')
                                             FROM AsignacionProyectos ap
                                             JOIN Empleados e ON ap.IdEmpleado = e.Id
                                             WHERE ap.IdProyecto = p.Id) AS AsignacionesEmpleados,
                                            (SELECT GROUP_CONCAT(r.Nombre, ', ')
                                             FROM AsignacionRolesProyecto arp
                                             JOIN Roles r ON arp.IdRol = r.Id
                                             WHERE arp.IdProyecto = p.Id) AS RolesAsignados,
                                            (SELECT GROUP_CONCAT(t.Nombre || ' (' || e.Nombre || ' ' || e.Apellido || ')', ', ')
                                             FROM AsignacionTareas at
                                             JOIN TareasProyectos tp ON at.IdTareaProyecto = tp.Id
                                             JOIN Tareas t ON tp.IdTarea = t.Id
                                             JOIN Empleados e ON at.IdEmpleado = e.Id
                                             WHERE tp.IdProyecto = p.Id
                                            ) AS TareasAsignadas,
                                            datetime('now') AS FechaGeneracion
                                        FROM Proyectos p
                                        LEFT JOIN AsignacionProyectos ap ON p.Id = ap.IdProyecto
                                        LEFT JOIN Empleados e ON ap.IdEmpleado = e.Id
                                        LEFT JOIN Salarios s ON e.IdSalario = s.Id
                                        WHERE p.Id = @IdProyecto
                                        GROUP BY p.Id, p.Nombre, p.Presupuesto, p.EstadoProyecto;
                                     ");

                datos.setearParametro("@IdProyecto", nuevoReporte.IdProyecto);
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

        public void EliminarReporteProyectos(int idReporte)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"DELETE FROM ReportesProyectos WHERE Id = @Id;");
                datos.setearParametro("@Id", idReporte);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        // ------------------------------------------------------------------------------------------------------------- // 
        // METODOS DE REPORTES FINANCIEROS

        public List<ReporteFinanzas> ObtenerReportesFinanzas()
        {
            AccesoDatos datos = new AccesoDatos();
            List<ReporteFinanzas> lista = new List<ReporteFinanzas>();

            try
            {
                datos.setearConsulta(@"
                    SELECT 
                        Id,
                        TotalSalarios,
                        TotalBonos,
                        TotalPresupuestosProyectos,
                        TotalGastos,
                        FechaGeneracion
                    FROM ReportesFinanzas;
                ");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    ReporteFinanzas aux = new ReporteFinanzas();

                    aux.Id = Convert.ToInt32(datos.Lector["Id"]);
                    aux.TotalSalarios = datos.Lector["TotalSalarios"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["TotalSalarios"]) : 0m;
                    aux.TotalBonos = datos.Lector["TotalBonos"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["TotalBonos"]) : 0m;
                    aux.TotalPresupuestosProyectos = datos.Lector["TotalPresupuestosProyectos"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["TotalPresupuestosProyectos"]) : 0m;
                    aux.TotalGastos = datos.Lector["TotalGastos"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["TotalGastos"]) : 0m;
                    aux.FechaGeneracion = Convert.ToDateTime(datos.Lector["FechaGeneracion"]);

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void GenerarReporteFinanzas(ReporteFinanzas nuevoReporte)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    INSERT INTO ReportesFinanzas 
                        (TotalSalarios, TotalBonos, TotalPresupuestosProyectos, TotalGastos, FechaGeneracion)
                    SELECT 
                        IFNULL(SUM(s.Monto), 0) AS TotalSalarios,
                        IFNULL(SUM(tb.Monto), 0) AS TotalBonos,
                        IFNULL(SUM(p.Presupuesto), 0) AS TotalPresupuestosProyectos,
                        (IFNULL(SUM(s.Monto), 0) + IFNULL(SUM(tb.Monto), 0) + IFNULL(SUM(p.Presupuesto), 0)) AS TotalGastos,
                        datetime('now') AS FechaGeneracion
                    FROM Empleados e
                    LEFT JOIN Salarios s ON e.IdSalario = s.Id
                    LEFT JOIN AsignacionBonos ab ON e.Id = ab.IdEmpleado
                    LEFT JOIN TipoBonos tb ON ab.IdTipoBono = tb.Id
                    LEFT JOIN Proyectos p ON p.Id = e.Id;
                ");

                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void EliminarReporteFinanzas(int idReporte)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"DELETE FROM ReportesFinanzas WHERE Id = @Id;");
                datos.setearParametro("@Id", idReporte);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
