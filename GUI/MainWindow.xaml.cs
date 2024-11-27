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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frameContent.Content = new Estadisticas();
        }

        private void rdEstadisticas_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new Estadisticas());
        }

        private void rdEstadisticas_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rdMovimientos_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new Facturas());
        }

        private void rdMovimientos_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rdProductos_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new Productos());
        }

        private void rdProductos_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rdVentas_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new Ventas());
        }

        private void rdVentas_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rdClientes_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new Clientes());
        }

        private void rdClientes_Checked(object sender, RoutedEventArgs e)
        {

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
