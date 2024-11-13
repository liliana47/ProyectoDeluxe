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

        public List<Producto> ObtenerProductos()
        {
            return productoDAL.ObtenerProductos();
        }
    }
}
