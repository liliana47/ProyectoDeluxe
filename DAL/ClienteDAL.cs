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
                string query = "INSERT INTO clientes (cedula, nombre, apellido, direccion, telefono, correo)" +
                               "VALUES (:cedula, :nombre, :apellido, :direccion, :telefono, :correo)";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(":cedula", cliente.Cedula);
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
                string query = "SELECT COUNT(1) FROM clientes WHERE cedula = :cedula";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(":cedula", cedula);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public List<Cliente> ObtenerClientes()
        {
            List<Cliente> clientes = new List<Cliente>();

            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM clientes";

                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cliente cliente = new Cliente
                            {
                                Cedula = reader["Cedula"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                CorreoElectronico = reader["Correo"].ToString()
                            };

                            clientes.Add(cliente);
                        }
                    }
                }
                connection.Close();
            }
            return clientes;
        }

        public Cliente ObtenerClientePorCedula(string cedula)
        {
            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM clientes WHERE cedula = :cedula";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(":cedula", cedula);

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            return new Cliente
                            {
                                Cedula = reader["cedula"].ToString(),
                                Nombre = reader["nombres"].ToString(),
                                Apellido = reader["apellidos"].ToString(),
                                CorreoElectronico = reader["correo"].ToString(),
                                Direccion = reader["direccion"].ToString(),
                                Telefono = reader["telefono"].ToString()
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
