# 💼 Gestión Pyme - App de Escritorio (C# + SQLite)

Aplicación de escritorio en **WinForms (.NET)** para la gestión de empleados, proyectos, salarios y más, de una PyME.  
La app utiliza **SQLite** como base de datos local, por lo que no requiere instalación de servidores externos.  

---

## 🚀 Características principales
- Gestión de empleados: agregar, modificar, ver detalles, eliminar, y asignar a proyectos, roles, tareas y bonos.
- Gestión de proyectos: agregar, modificar, ver detalles, eliminar, y asignar empleados, roles y tareas.
- Gestión de roles: agregar, modificar, ver detalles, eliminar.
- Gestión de tareas: agregar, modificar, ver detalles, eliminar.
- Gestión de puestos y salarios: agregar, modificar, ver detalles, eliminar.
- Gestión de bonos: agregar, modificar, ver detalles, eliminar.
- Reportes: ver reportes, crear reportes y eliminar reportes.
- Todos los formularios incluyen listados para facilitar la navegación y gestión de los datos.
- Interfaz amigable en WinForms.
- Base de datos **portátil** (archivo `.db`).

---

## 📂 Estructura del proyecto
- **AppEscritorio_GestionDeEmpleados/** → Formularios WinForms (UI).  
- **Negocio/**  
  - Lógica de negocio.  
  - Acceso a la base de datos (`AccesoDatos/`).  
- **Dominio/** → Entidades (clases).

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
   - Ejemplo de inicialización:  
     ```csharp
     GestorPymeDB.InicializarBaseDeDatos();
     conexion = new SqliteConnection(GestorPymeDB.connectionString);
     ```

> 🔹 Por defecto está habilitado el **modo archivo `.db`**, pero puede cambiarse fácilmente en `AccesoDatos.cs`.

---

## ⚙️ Requisitos
- Windows 10/11  
- [.NET Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (si no lo tenés ya instalado).  

---

## ▶️ Formas de ejecución

### 🔹 Opción 1: Descargar la versión portable (recomendada)
No es necesario compilar ni configurar nada.  
1. [⬇️ Descargar App Portable](https://github.com/francowagenhofer/Gestion-Pyme-SQLite/releases/download/v1.0.0/AppGestionPyme_Portable.zip)  
2. Extraer el ZIP en tu computadora.  
3. Ejecutar `AppGestionPyme_Portable.exe`.

---

### 🔹 Opción 2: Clonar el repositorio y compilar
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/francowagenhofer/Gestion-Pyme-SQLite.git

2. Abrir el proyecto en Visual Studio.

3. Compilar la solución (Ctrl + Shift + B).

4. Ejecutar desde el IDE o desde la carpeta /bin/Debug/net8.0-windows/.
