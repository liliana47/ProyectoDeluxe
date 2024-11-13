using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Factura
    {
        public int NumeroFactura { get; set; }
        public Cajero Cajero { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime HoraEmision { get; set; }
        public string LugarEmision { get; set; }
        public string NITEmpresa { get; set; }
        public string CodigoInterno { get; set; }
        public Impuesto Impuesto { get; set; }
        public double Descuento { get; set; }
        public string Estado { get; set; }
        public List<Producto> Productos { get; set; }
        public double Total { get; private set; }
        public string FormaDePago { get; set; }

        public Factura(int numeroFactura, Cajero cajero, Cliente cliente, string lugarEmision, string nitEmpresa, string codigoInterno, string formaDePago)
        {
            NumeroFactura = numeroFactura;
            Cajero = cajero;
            Cliente = cliente;
            FechaEmision = DateTime.Now;
            HoraEmision = DateTime.Now;
            LugarEmision = lugarEmision;
            NITEmpresa = nitEmpresa;
            CodigoInterno = codigoInterno;
            FormaDePago = formaDePago;
            Productos = new List<Producto>();
            Estado = "Pendiente";
            Total = 0.0;
        }
        public void GenerarXML()
        {

        }
        public void FirmarFactura()
        {
           
        }

        public void CalcularTotal()
        {
            Total = 0.0;
            foreach (var producto in Productos)
            {
                Total += producto.PrecioUnitario * producto.Cantidad;
            }

            if (Impuesto != null)
            {
                Impuesto.CalcularImpuesto(Total);
                Total += Impuesto.Valor;
            }

            AplicarDescuento();
        }

        public void AplicarDescuento()
        {
            if (Descuento > 0)
            {
                Total -= Total * (Descuento / 100);
            }
        }
    }
}
