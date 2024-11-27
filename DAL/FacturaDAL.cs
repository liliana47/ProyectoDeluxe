using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITY;
using Oracle.ManagedDataAccess.Types;
using System.Data.SqlClient;

namespace DAL
{
    public class FacturaDAL
    {
        public void GuardarFactura(Factura factura)
        {
            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                OracleTransaction transaction = connection.BeginTransaction(); // Iniciar la transacción

                try
                {
                    // Comando para llamar al procedimiento almacenado
                    using (OracleCommand command = new OracleCommand("SP_GUARDAR_FACTURA", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;

                        // Parámetros de entrada
                        command.Parameters.Add("p_cedula_cliente", OracleDbType.Varchar2).Value = factura.Cliente.Cedula;
                        command.Parameters.Add("p_total", OracleDbType.Decimal).Value = factura.Total;

                        // Parámetro de salida
                        OracleParameter outputParam = new OracleParameter("p_numero_factura", OracleDbType.Decimal)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        // Ejecutar el procedimiento almacenado
                        command.ExecuteNonQuery();

                        // Asignar el valor retornado a la factura
                        factura.Id = Convert.ToInt32(((OracleDecimal)outputParam.Value).Value);
                    }

                    // Insertar los productos relacionados con la factura
                    foreach (var producto in factura.Productos)
                    {
                        string queryProducto = @"INSERT INTO productos_factura (id_producto_factura, numero_factura, codigo_producto, nombre_producto, cantidad, precio_unitario, total)
                 VALUES (SEQ_PRODUCTO_FACTURA.NEXTVAL, :numero_factura, :codigo_producto, :nombre_producto, :cantidad, :precio_unitario, :total)";

                        using (OracleCommand command = new OracleCommand(queryProducto, connection))
                        {
                            command.Transaction = transaction;

                            // Agregar parámetros
                            command.Parameters.Add(":numero_factura", factura.Id);
                            command.Parameters.Add(":codigo_producto", producto.Id);
                            command.Parameters.Add(":nombre_producto", producto.Nombre);
                            command.Parameters.Add(":cantidad", producto.Cantidad);
                            command.Parameters.Add(":precio_unitario", producto.PrecioUnitario);
                            command.Parameters.Add(":total", producto.Total);

                            // Ejecutar la consulta
                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit(); // Confirmar la transacción
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Revertir la transacción en caso de error
                    throw new Exception($"Error al guardar la factura: {ex.Message}");
                }
            }
        }



        public List<Factura> ObtenerFacturas()
        {
            List<Factura> facturas = new List<Factura>();

            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                string queryFacturas = "SELECT f.numero_factura, f.total, f.fecha_registro, f.cedula_cliente, c.nombre AS cliente_nombre, c.apellido AS cliente_apellido, c.direccion AS cliente_direccion, c.telefono AS cliente_telefono, c.correo AS cliente_correo FROM facturas f LEFT JOIN clientes c ON f.cedula_cliente = c.cedula";

                using (OracleCommand command = new OracleCommand(queryFacturas, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Crear instancia de Cliente
                            Cliente cliente = new Cliente
                            {
                                Cedula = reader["cedula_cliente"].ToString(),
                                Nombre = reader["cliente_nombre"].ToString(),
                                Apellido = reader["cliente_apellido"].ToString(),
                                Direccion = reader["cliente_direccion"].ToString(),
                                Telefono = reader["cliente_telefono"].ToString(),
                                CorreoElectronico = reader["cliente_correo"].ToString()
                            };

                            // Crear instancia de Factura
                            Factura factura = new Factura(cliente)
                            {
                                Id = Convert.ToInt32(reader["numero_factura"]),
                                Total = Convert.ToDouble(reader["total"]),
                                FechaEmision = Convert.ToDateTime(reader["fecha_registro"]),
                                Productos = new List<Producto>() // Inicializamos la lista vacía
                            };

                            facturas.Add(factura);
                        }
                    }
                }
            }

            return facturas;
        }

        public Factura ObtenerDetallesFactura(int numeroFactura)
        {
            Factura factura = null;
            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                string query = @"SELECT f.numero_factura, f.total, f.fecha_registro, 
                               c.cedula AS cliente_cedula, c.nombre AS cliente_nombre,
                               p.codigo_producto, p.nombre_producto, p.cantidad, p.precio_unitario, p.total AS producto_total
                        FROM facturas f
                        JOIN clientes c ON f.cedula_cliente = c.cedula
                        JOIN productos_factura p ON p.numero_factura = f.numero_factura
                        WHERE f.numero_factura = :numeroFactura";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(":numeroFactura", numeroFactura);


                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (factura == null)
                            {
                                factura = new Factura
                                {
                                    Id = Convert.ToInt32(reader["numero_factura"]),
                                    Total = Convert.ToInt32(reader["total"]),
                                    FechaEmision = Convert.ToDateTime(reader["fecha_registro"]),
                                    Cliente = new Cliente
                                    {
                                        Cedula = reader["cliente_cedula"].ToString(),
                                        Nombre = reader["cliente_nombre"].ToString()
                                    },
                                    Productos = new List<Producto>()
                                };
                            }

                            Producto producto = new Producto
                            {
                                Id = Convert.ToInt32(reader["codigo_producto"]),
                                Nombre = reader["nombre_producto"].ToString(),
                                Cantidad = Convert.ToInt32(reader["cantidad"]),
                                PrecioUnitario = Convert.ToInt32(reader["precio_unitario"]),
                                Total = Convert.ToInt32(reader["producto_total"])
                            };

                            factura.Productos.Add(producto);
                        }
                    }
                }
            }

            if (factura == null)
            {
                Console.WriteLine($"No se encontró ninguna factura con ID {numeroFactura}");
            }
            else
            {
                Console.WriteLine($"Factura encontrada: {factura.Id}, Total: {factura.Total}");
            }

            return factura;
        }

        public double ObtenerTotalFacturas()
        {
            double totalFacturas = 0.0;

            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                string query = "SELECT SUM(total) as TotalFacturas FROM facturas";

                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalFacturas = Convert.ToDouble(result);
                    }
                }
                connection.Close();
            }

            return totalFacturas;
        }

        public Dictionary<string, int> ObtenerFacturasPorPeriodo(string periodo)
        {
            string groupByColumn;
            switch (periodo.ToLower())
            {
                case "día":
                    groupByColumn = "TO_CHAR(fecha_registro, 'DD-MM-YYYY')";
                    break;
                case "mes":
                    groupByColumn = "TO_CHAR(fecha_registro, 'MM-YYYY')";
                    break;
                case "año":
                    groupByColumn = "TO_CHAR(fecha_registro, 'YYYY')";
                    break;
                default:
                    throw new ArgumentException("Periodo no válido. Use 'Día', 'Mes' o 'Año'.");
            }

            string query = $@"
                SELECT {groupByColumn} AS Periodo, COUNT(*) AS TotalFacturas
                FROM facturas
                GROUP BY {groupByColumn}
                ORDER BY MIN(fecha_registro)";

            Dictionary<string, int> facturasPorPeriodo = new Dictionary<string, int>();

            using (OracleConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string periodoStr = reader["Periodo"].ToString();
                            int totalFacturas = Convert.ToInt32(reader["TotalFacturas"]);

                            facturasPorPeriodo.Add(periodoStr, totalFacturas);
                        }
                    }
                }
            }

            return facturasPorPeriodo;
        }
    }
}
