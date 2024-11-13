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
                string query = "INSERT INTO clientes (nombre, apellido, tipodocumento, numerodocumento, telefono, correo)" +
                               "VALUES (:nombre, :apellido, :tipodocumento, :numerodocumento, :telefono, :correo)";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(":nombre", cliente.Nombre);
                    command.Parameters.Add(":apellido", cliente.Apellido);
                    command.Parameters.Add(":tipodocumento", cliente.TipoDocumento);
                    command.Parameters.Add(":numerodocumento", cliente.NumeroDocumento);
                    command.Parameters.Add(":telefono", cliente.Telefono);
                    command.Parameters.Add(":correo", cliente.CorreoElectronico);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
