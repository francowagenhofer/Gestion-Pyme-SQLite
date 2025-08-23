using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.ReglasDelNegocio;
using negocio;
using Dominio.Entidades.Dominio.Entidades;
using Microsoft.Data.Sqlite;

namespace Negocio
{
    public class SalariosNegocio
    {
        public List<Salarios> ListarSalarios()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Salarios> lista = new List<Salarios>();

            try
            {
                datos.setearConsulta(
                    "SELECT S.Id, S.IdCategoria, S.Monto, C.Nombre AS NombreCategoria " +
                    "FROM Salarios S " +
                    "JOIN Categorias C ON S.IdCategoria = C.Id"
                );

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Salarios salario = new Salarios
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        IdCategoria = Convert.ToInt32(datos.Lector["IdCategoria"]),
                        Monto = Convert.ToDecimal(datos.Lector["Monto"]),
                        NombreCategoria = datos.Lector["NombreCategoria"].ToString()
                    };

                    lista.Add(salario);
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

        public void ModificarSalario(Salarios salarioModificado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "UPDATE Salarios " +
                    "SET IdCategoria = @IdCategoria, Monto = @Monto " +
                    "WHERE Id = @Id"
                );

                datos.setearParametro("@Id", salarioModificado.Id);
                datos.setearParametro("@IdCategoria", salarioModificado.IdCategoria);
                datos.setearParametro("@Monto", salarioModificado.Monto);

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

        public void AgregarSalario(Salarios nuevoSalario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "INSERT INTO Salarios (IdCategoria, Monto) " +
                    "VALUES (@IdCategoria, @Monto)"
                );

                datos.setearParametro("@IdCategoria", nuevoSalario.IdCategoria);
                datos.setearParametro("@Monto", nuevoSalario.Monto);

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

        public void EliminarSalario(int idSalario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM Salarios WHERE Id = @Id");
                datos.setearParametro("@Id", idSalario);
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

    }
}