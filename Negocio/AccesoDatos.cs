using System;
using Microsoft.Data.Sqlite;
using Negocio;
using static System.Net.Mime.MediaTypeNames;


namespace negocio
{
    public class AccesoDatos
    {
        private SqliteConnection conexion;
        private SqliteCommand comando;
        private SqliteDataReader lector;

        public SqliteDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            //  Dos formas de manejar la base de datos: 
            // 1 - Clase GestionarPymeDB
            // 2 - Archivo CreateDB.db

            // Clase GestorPymeDB
            //GestorPymeDB.InicializarBaseDeDatos();
            //conexion = new SqliteConnection(GestorPymeDB.connectionString);
            //comando = new SqliteCommand();

            // Archivo CreateDB.db
            string rutaDB = @"C:\Users\franc\Proyectos\Gestion Pyme - SQL_Lite\AppEscritorio_GestionDeEmpleados\CreateDB.db";
            conexion = new SqliteConnection($"Data Source={rutaDB}");
            comando = new SqliteCommand();
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ejecutarAccionScalar()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                return Convert.ToInt32(comando.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
        }

        public void cerrarConexion()
        {
            if (lector != null && !lector.IsClosed)
                lector.Close();
            conexion.Close();
        }

        public object ejecutarEscalar()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                return comando.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cerrarConexion();
            }
        }

        public void LimpiarParametros()
        {
            comando.Parameters.Clear();
        }
    }
}
