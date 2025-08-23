using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace Negocio
{
    public static class GestorPymeDB
    {
        private static string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GestorPyme.db");
        public static string connectionString => $"Data Source={dbFile}";
        public static string RutaDB => dbFile;

        public static void InicializarBaseDeDatos()
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string script = @"
                -- Tablas principales
                CREATE TABLE IF NOT EXISTS Categorias (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT,
                    Descripcion TEXT
                );

                CREATE TABLE IF NOT EXISTS Salarios (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdCategoria INTEGER NOT NULL,
                    Monto REAL NOT NULL,
                    FOREIGN KEY (IdCategoria) REFERENCES Categorias(Id)
                );

                CREATE TABLE IF NOT EXISTS Empleados (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT,
                    Apellido TEXT,
                    FechaNacimiento TEXT,
                    DNI TEXT UNIQUE,
                    Imagen TEXT,
                    FechaIngreso TEXT,
                    IdCategoria INTEGER NOT NULL,
                    IdSalario INTEGER,
                    IsActive INTEGER DEFAULT 1,
                    FOREIGN KEY (IdCategoria) REFERENCES Categorias(Id),
                    FOREIGN KEY (IdSalario) REFERENCES Salarios(Id)
                );

                CREATE TABLE IF NOT EXISTS TipoBonos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT,
                    Monto REAL DEFAULT 0
                );

                CREATE TABLE IF NOT EXISTS AsignacionBonos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdEmpleado INTEGER NOT NULL,
                    IdTipoBono INTEGER NOT NULL,
                    FechaAsignacion TEXT,
                    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(Id),
                    FOREIGN KEY (IdTipoBono) REFERENCES TipoBonos(Id),
                    UNIQUE(IdEmpleado, IdTipoBono)
                );

                CREATE TABLE IF NOT EXISTS Proyectos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT,
                    Descripcion TEXT,
                    FechaInicio TEXT,
                    FechaFin TEXT,
                    Presupuesto REAL,
                    EstadoProyecto TEXT,
                    IsActive INTEGER DEFAULT 1
                );

                CREATE TABLE IF NOT EXISTS AsignacionProyectos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdEmpleado INTEGER NOT NULL,
                    IdProyecto INTEGER NOT NULL,
                    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(Id),
                    FOREIGN KEY (IdProyecto) REFERENCES Proyectos(Id)
                );

                CREATE TABLE IF NOT EXISTS Tareas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT,
                    Descripcion TEXT,
                    FechaInicio TEXT,
                    FechaFin TEXT,
                    Estado TEXT
                );

                CREATE TABLE IF NOT EXISTS TareasProyectos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdTarea INTEGER NOT NULL,
                    IdProyecto INTEGER NOT NULL,
                    FOREIGN KEY (IdTarea) REFERENCES Tareas(Id),
                    FOREIGN KEY (IdProyecto) REFERENCES Proyectos(Id),
                    UNIQUE(IdTarea, IdProyecto)
                );

                CREATE TABLE IF NOT EXISTS AsignacionTareas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdTareaProyecto INTEGER NOT NULL,
                    IdEmpleado INTEGER NOT NULL,
                    FechaAsignacion TEXT,
                    FOREIGN KEY (IdTareaProyecto) REFERENCES TareasProyectos(Id),
                    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(Id),
                    UNIQUE(IdTareaProyecto, IdEmpleado)
                );

                CREATE TABLE IF NOT EXISTS Roles (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT,
                    Descripcion TEXT
                );

                CREATE TABLE IF NOT EXISTS AsignacionRolesProyecto (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdEmpleado INTEGER, 
                    IdRol INTEGER NOT NULL,
                    IdProyecto INTEGER NOT NULL,
                    FechaAsignacion TEXT,
                    FOREIGN KEY(IdEmpleado) REFERENCES Empleados(Id),
                    FOREIGN KEY(IdProyecto) REFERENCES Proyectos(Id),
                    FOREIGN KEY(IdRol) REFERENCES Roles(Id),
                    UNIQUE(IdEmpleado, IdProyecto)
                );

                CREATE TABLE IF NOT EXISTS ReportesFinanzas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TotalSalarios REAL,
                    TotalBonos REAL,
                    TotalPresupuestosProyectos REAL,
                    TotalGastos REAL,
                    FechaGeneracion TEXT
                );

                CREATE TABLE IF NOT EXISTS ReportesProyectos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdProyecto INTEGER NOT NULL,
                    NombreProyecto TEXT,
                    Presupuesto REAL,
                    EstadoProyecto TEXT,
                    AsignacionesEmpleados TEXT,
                    TareasAsignadas TEXT,
                    RolesAsignados TEXT,
                    TiempoEstimado REAL,
                    FechaGeneracion TEXT,
                    FOREIGN KEY (IdProyecto) REFERENCES Proyectos(Id)
                );

                CREATE TABLE IF NOT EXISTS ReportesEmpleados (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdEmpleado INTEGER NOT NULL,
                    NombreEmpleado TEXT,
                    Categoria TEXT,
                    SalarioActual REAL,
                    TotalBonos REAL,
                    ProyectosAsignados TEXT,
                    RolesAsignados TEXT,
                    TareasAsignadas TEXT,
                    FechaGeneracion TEXT,
                    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(Id)
                );

                CREATE TABLE IF NOT EXISTS Usuarios (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Email TEXT UNIQUE NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    Nombre TEXT,
                    Apellido TEXT,
                    ImagenPerfil TEXT,
                    Admin INTEGER DEFAULT 0
                );

                CREATE TABLE IF NOT EXISTS Clientes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT,
                    Email TEXT UNIQUE,
                    Telefono TEXT,
                    Direccion TEXT
                );

                CREATE TABLE IF NOT EXISTS AsignacionClientesProyectos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdCliente INTEGER NOT NULL,
                    IdProyecto INTEGER NOT NULL,
                    FechaAsignacion TEXT,
                    FOREIGN KEY (IdCliente) REFERENCES Clientes(Id),
                    FOREIGN KEY (IdProyecto) REFERENCES Proyectos(Id)
                );
                ";

                    using (var command = new SqliteCommand(script, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al inicializar la base de datos: {ex.Message}");
                throw;
            }
        }

    }
}
