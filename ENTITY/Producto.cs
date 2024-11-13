using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Producto
    {
        public int Id_Producto { get; set; }
        public string Nombre_Producto { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public double Impuesto { get; set; }

        public Producto() { }

        public Producto(string nombreProducto, string descripcion, int cantidad, double precioUnitario)
        {
            Nombre_Producto = nombreProducto;
            Descripcion = descripcion;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            Impuesto = 0.0;
        }

        public double CalcularValorTotal()
        {
            return (PrecioUnitario * Cantidad) + Impuesto;
        }
        public void AsignarImpuesto(double porcentajeImpuesto)
        {
            Impuesto = (PrecioUnitario * Cantidad) * (porcentajeImpuesto / 100);

        }
       
        public bool ValidarProducto()
        {
            return !string.IsNullOrEmpty(Nombre_Producto) && Cantidad > 0 && PrecioUnitario > 0;
        }
    }
}
