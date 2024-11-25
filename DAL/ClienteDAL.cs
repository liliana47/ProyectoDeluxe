using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ClienteDAL
    {
        public void agregarCliente(Cliente cliente)
        {
            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO clientes (numerodocumento, nombre, apellido, direccion, telefono, correo)" +
                               "VALUES (:numerodocumento, :nombre, :apellido, :direccion, :telefono, :correo)";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(":numerodocumento", cliente.Cedula);
                    command.Parameters.Add(":nombre", cliente.Nombre);
                    command.Parameters.Add(":apellido", cliente.Apellido);
                    command.Parameters.Add(":direccion", cliente.Direccion);
                    command.Parameters.Add(":telefono", cliente.Telefono);
                    command.Parameters.Add(":correo", cliente.CorreoElectronico);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool CedulaExiste(string cedula)
        {
            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM clientes WHERE cedula = :numerodocumento";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(":numerodocumento", cedula);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}
