using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Lógica de interacción para ListarProductos.xaml
    /// </summary>
    public partial class ListarProductos : Window
    {
        private ProductoBLL productoBLL = new ProductoBLL();
        private List<Producto> productosOriginales; // Lista completa de productos
        public Producto ProductoSeleccionado { get; private set; }

        public ListarProductos()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void CargarProductos()
        {
            // Obtiene la lista completa de productos
            productosOriginales = productoBLL.ObtenerProductos();
            ProductosDataGrid.ItemsSource = productosOriginales;
        }

        private void dataGridProductos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProductosDataGrid.SelectedItem is Producto producto)
            {
                ProductoSeleccionado = producto;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void txtBuscarProducto_KeyUp(object sender, KeyEventArgs e)
        {
            string filtro = txtBuscarProducto.Text.ToLower();
            FiltrarProductosPorNombre(filtro);
        }

        private void FiltrarProductosPorNombre(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
            {
                // Si el filtro está vacío, mostrar todos los productos
                ProductosDataGrid.ItemsSource = productosOriginales;
            }
            else
            {
                // Filtrar los productos cuyo nombre contenga el texto ingresado
                var productosFiltrados = productosOriginales
                    .Where(p => p.Nombre.ToLower().Contains(filtro))
                    .ToList();

                ProductosDataGrid.ItemsSource = productosFiltrados;
            }
        }
    }
}
