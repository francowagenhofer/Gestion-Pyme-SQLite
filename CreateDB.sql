-- Habilitar claves foráneas en SQLite
PRAGMA foreign_keys = ON;

-- TABLAS

CREATE TABLE Categorias (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT,
    Descripcion TEXT
);

CREATE TABLE Salarios (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdCategoria INTEGER NOT NULL,
    Monto REAL NOT NULL,
    FOREIGN KEY (IdCategoria) REFERENCES Categorias(Id)
);

CREATE TABLE Empleados (
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

CREATE TABLE TipoBonos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT,
    Monto REAL DEFAULT 0
);

CREATE TABLE AsignacionBonos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdEmpleado INTEGER NOT NULL,
    IdTipoBono INTEGER NOT NULL,
    FechaAsignacion TEXT,
    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(Id),
    FOREIGN KEY (IdTipoBono) REFERENCES TipoBonos(Id),
    UNIQUE (IdEmpleado, IdTipoBono)
);

CREATE TABLE Proyectos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT,
    Descripcion TEXT,
    FechaInicio TEXT,
    FechaFin TEXT,
    Presupuesto REAL,
    EstadoProyecto TEXT,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE AsignacionProyectos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdEmpleado INTEGER NOT NULL,
    IdProyecto INTEGER NOT NULL,
    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(Id),
    FOREIGN KEY (IdProyecto) REFERENCES Proyectos(Id)
);

CREATE TABLE Tareas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT,
    Descripcion TEXT,
    FechaInicio TEXT,
    FechaFin TEXT,
    Estado TEXT
);

CREATE TABLE TareasProyectos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdTarea INTEGER NOT NULL,
    IdProyecto INTEGER NOT NULL,
    FOREIGN KEY (IdTarea) REFERENCES Tareas(Id),
    FOREIGN KEY (IdProyecto) REFERENCES Proyectos(Id),
    UNIQUE (IdTarea, IdProyecto)
);

CREATE TABLE AsignacionTareas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdTareaProyecto INTEGER NOT NULL,
    IdEmpleado INTEGER NOT NULL,
    FechaAsignacion TEXT,
    FOREIGN KEY (IdTareaProyecto) REFERENCES TareasProyectos(Id),
    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(Id),
    UNIQUE (IdTareaProyecto, IdEmpleado)
);

CREATE TABLE Roles (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT,
    Descripcion TEXT
);

CREATE TABLE AsignacionRolesProyecto (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdEmpleado INTEGER NOT NULL,
    IdRol INTEGER NOT NULL,
    IdProyecto INTEGER NOT NULL,
    FechaAsignacion TEXT,
    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(Id),
    FOREIGN KEY (IdRol) REFERENCES Roles(Id),
    FOREIGN KEY (IdProyecto) REFERENCES Proyectos(Id),
    UNIQUE (IdEmpleado, IdProyecto)
);

CREATE TABLE ReportesFinanzas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    TotalSalarios REAL,
    TotalBonos REAL,
    TotalPresupuestosProyectos REAL,
    TotalGastos REAL,
    FechaGeneracion TEXT
);

CREATE TABLE ReportesProyectos (
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

CREATE TABLE ReportesEmpleados (
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

CREATE TABLE Usuarios (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Email TEXT UNIQUE NOT NULL,
    PasswordHash TEXT NOT NULL,
    Nombre TEXT,
    Apellido TEXT,
    ImagenPerfil TEXT,
    Admin INTEGER DEFAULT 0
);

CREATE TABLE Clientes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT,
    Email TEXT UNIQUE,
    Telefono TEXT,
    Direccion TEXT
);

CREATE TABLE AsignacionClientesProyectos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdCliente INTEGER NOT NULL,
    IdProyecto INTEGER NOT NULL,
    FechaAsignacion TEXT,
    FOREIGN KEY (IdCliente) REFERENCES Clientes(Id),
    FOREIGN KEY (IdProyecto) REFERENCES Proyectos(Id)
);
