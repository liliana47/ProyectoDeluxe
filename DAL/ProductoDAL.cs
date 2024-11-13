using Oracle.ManagedDataAccess.Client;
using System;
using ENTITY;

namespace DAL
{
    public class ProductoDAL
    {
        public void AgregarProducto(Producto producto)
        {
            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO productos (Nombre, Descripcion, Cantidad, Precio_Unitario, Impuesto)VALUES(:nombre, :descripcion, :cantidad, :preciounitario, :impuesto)";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(":nombre", producto.Nombre_Producto);
                    command.Parameters.Add(":descrípcion", producto.Descripcion);
                    command.Parameters.Add(":cantidad", producto.Cantidad);
                    command.Parameters.Add(":preciounitario", producto.PrecioUnitario);
                    command.Parameters.Add(":impuesto", producto.Impuesto);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
