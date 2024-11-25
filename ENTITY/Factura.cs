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
        public Cliente Cliente { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime HoraEmision { get; set; }
        public string LugarEmision { get; set; }
        public string NITEmpresa { get; set; }
        public string CodigoInterno { get; set; }
        public double Descuento { get; set; }
        public string Estado { get; set; }
        public List<Producto> Productos { get; set; }
        public double Total { get; private set; }
        public string FormaDePago { get; set; }

        public Factura(int numeroFactura, Cliente cliente, string lugarEmision, string nitEmpresa, string codigoInterno, string formaDePago)
        {
            NumeroFactura = numeroFactura;
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
    }
}
