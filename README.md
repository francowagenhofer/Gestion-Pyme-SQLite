# Gestión Pyme - App de Escritorio (C# + SQLite)

Aplicación de escritorio en **WinForms (.NET)** para la gestión de empleados, proyectos y tareas de una PyME.  
La app utiliza **SQLite** como base de datos local.

---

## 🚀 Características principales
- Gestión de empleados (altas, bajas, modificaciones, activación/inactivación).
- Gestión de proyectos y asignación de empleados.
- Gestión de tareas.
- Interfaz amigable en WinForms.
- Base de datos **portátil** (archivo `.db`).

---

## 📂 Estructura del proyecto
- **AppEscritorio_GestionDeEmpleados/** → Formularios WinForms (UI).
- **Negocio/** → Lógica de negocio.
- **Dominio/** → Entidades (clases).
- **AccesoDatos/** → Acceso a la base de datos.

---

## 🗄️ Base de datos
La app puede trabajar de dos formas:

1. **Archivo SQLite** (`CreateDB.db`)  
   - Ruta por defecto:  
     ```
     AppEscritorio_GestionDeEmpleados/CreateDB.db
     ```
   - Conexión configurada en `AccesoDatos.cs`.

2. **Clase GestorPymeDB**  
   - Genera y gestiona la base de datos desde código C#.  
   - Inicialización:
     ```csharp
     GestorPymeDB.InicializarBaseDeDatos();
     conexion = new SqliteConnection(GestorPymeDB.connectionString);
     ```

Por defecto está habilitado el **modo archivo `.db`**, pero puede cambiarse fácilmente en `AccesoDatos.cs`.

---

## ⚙️ Requisitos
- Windows 10/11  
- [.NET Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (si no lo tenés ya instalado)

---

## ▶️ Ejecución
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/francowagenhofer/Gestion-Pyme-SQLite.git
