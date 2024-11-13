using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para Reservas.xaml
    /// </summary>
    public partial class Productos : Page
    {
        public List<Producto> producto = null;
        ProductoBLL servicio = new ProductoBLL();
        private ProductoBLL productoBLL = new ProductoBLL();

        public Productos()
        {
            InitializeComponent();
            DataContext = this;
            producto = servicio.ObtenerProductos();
            ProductosDataGrid.ItemsSource = producto;
            CargarProductos();
        }

        private void AgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            Window productosWindow = Window.GetWindow(this);
            AgregarPWindow proWindow = new AgregarPWindow();

            proWindow.Owner = productosWindow;
            proWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            proWindow.ShowDialog();

            Productos updateProducto = new Productos();
            this.NavigationService.Navigate(updateProducto);
        }

        private void CargarProductos()
        {
            List<Producto> productos = productoBLL.ObtenerProductos();
            ProductosDataGrid.ItemsSource = productos;
        }
    }
}
