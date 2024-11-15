using Oracle.ManagedDataAccess.Client;
using System;
using ENTITY;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class ProductoDAL
    {
        public void AgregarProducto(Producto producto)
        {
            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO productos (Nombre, Descripcion, Cantidad, Precio_Unitario)VALUES(:nombre, :descripcion, :cantidad, :preciounitario)";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(":nombre", producto.Nombre);
                    command.Parameters.Add(":descrípcion", producto.Descripcion);
                    command.Parameters.Add(":cantidad", producto.Cantidad);
                    command.Parameters.Add(":preciounitario", producto.PrecioUnitario);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();

            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM productos";

                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto producto = new Producto
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Cantidad = Convert.ToInt16(reader["Cantidad"]),
                                PrecioUnitario = Convert.ToDouble(reader["Precio_Unitario"]),
                                Impuesto = Convert.ToInt32(reader["Cantidad"])
                            };

                            productos.Add(producto);
                        }
                    }
                }
                connection.Close();
            }
            return productos;
        }
    }
}
