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
    /// Lógica de interacción para Facturar.xaml
    /// </summary>
    public partial class Facturas : Page
    {
        private FacturaBLL facturaBLL = new FacturaBLL();
        public List<Factura> factura = null;

        public Facturas()
        {
            InitializeComponent();
            DataContext = this;
            factura = facturaBLL.ObtenerFacturas();
            FacturasDataGrid.ItemsSource = factura;
            CargarFacturas();
        }

        private void CargarFacturas()
        {
            List<Factura> facturas = facturaBLL.ObtenerFacturas();
            FacturasDataGrid.ItemsSource = facturas;
        }

        private void VerDetalles_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int numeroFactura = int.Parse(button.Tag.ToString());
            DetallesFactura detallesFactura = new DetallesFactura(numeroFactura);
            detallesFactura.ShowDialog();
        }
    }
}
