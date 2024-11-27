using BLL;
using GUI.Pages;
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

namespace GUI
{
    /// <summary>
    /// Lógica de interacción para DetallesFactura.xaml
    /// </summary>
    public partial class DetallesFactura : Window
    {
        private Factura _factura;
        private FacturaBLL facturaBLL = new FacturaBLL();

        public DetallesFactura(int numeroFactura)
        {
            InitializeComponent();
            _factura = facturaBLL.ObtenerDetallesFactura(numeroFactura);

            if (_factura == null)
            {
                MessageBox.Show("No se encontró la factura especificada.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }
            DataContext = _factura;
        }
    }
}
