using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITY;
using DAL;

namespace BLL
{
    public class ProductoBLL
    {
        private ProductoDAL productoDAL = new ProductoDAL();

        public void AgregarProducto(string nombre, string descripcion, int cantidad, double preciounitario)
        {
            Producto producto = new Producto()
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Cantidad = cantidad,
                PrecioUnitario = preciounitario,
            };

            productoDAL.AgregarProducto(producto);
        }

        public List<Producto> ObtenerProductos()
        {
            return productoDAL.ObtenerProductos();
        }

        public Producto ObtenerProductoPorCodigo(int id)
        {
            return productoDAL.ObtenerProductoPorCodigo(id);
        }

        public List<(string NombreProducto, int CantidadVendida)> ObtenerProductosMasVendidos()
        {
            return productoDAL.ObtenerProductosMasVendidos();
        }
    }
}
