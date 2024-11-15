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
using System.Windows.Shapes;
using BLL;

namespace GUI
{
    /// <summary>
    /// Lógica de interacción para AgregarPWindow.xaml
    /// </summary>
    public partial class AgregarPWindow : Window
    {
        private ProductoBLL productoBLL = new ProductoBLL(); 

        public AgregarPWindow()
        {
            InitializeComponent();
        }

        public void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombre.Text;
            string descripcion = txtDescripcion.Text;
            int cantidad = Convert.ToInt16(txtCantidad.Text);
            double precio = Convert.ToDouble(txtPrecioUnitario.Text);
            double impuesto = Convert.ToDouble(txtImpuesto.Text);

            productoBLL.AgregarProducto(nombre, descripcion,cantidad, precio);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
