using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FacturaBLL
    {
        private FacturaDAL facturaDAL = new FacturaDAL();

        public void GuardarFactura(Factura factura)
        {
            if (factura == null)
                throw new ArgumentNullException(nameof(factura), "La factura no puede ser nula.");

            if (factura.Cliente == null || string.IsNullOrWhiteSpace(factura.Cliente.Cedula))
                throw new ArgumentException("La factura debe incluir un cliente válido con cédula.");

            if (factura.Productos == null || factura.Productos.Count == 0)
                throw new ArgumentException("La factura debe incluir al menos un producto.");

            if (factura.Total <= 0)
                throw new ArgumentException("El total de la factura debe ser mayor que cero.");

            try
            {
                // Llamar a la DAL para guardar la factura
                facturaDAL.GuardarFactura(factura);

                // Aquí, factura.Id debería haberse actualizado con el número generado en la DAL
                if (factura.Id <= 0)
                {
                    throw new Exception("No se pudo obtener un número de factura válido después de guardar.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la capa de negocio al guardar la factura: {ex.Message}", ex);
            }
        }

        public List<Factura> ObtenerFacturas()
        {
            return facturaDAL.ObtenerFacturas();
        }

        public Factura ObtenerDetallesFactura(int numeroFactura)
        {
            Factura factura = facturaDAL.ObtenerDetallesFactura(numeroFactura);
            if (factura != null)
            {
                if (factura.Cliente == null || factura.Productos == null)
                {
                    throw new Exception("Datos incompletos para la factura.");
                }
            }
            return factura;
        }

        public double ObtenerTotalFacturas()
        {
            return facturaDAL.ObtenerTotalFacturas();
        }

        public Dictionary<string, int> ObtenerFacturasPorPeriodo(string periodo)
        {
            return facturaDAL.ObtenerFacturasPorPeriodo(periodo);
        }
    }
}
