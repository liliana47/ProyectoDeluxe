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
    public partial class Facturar : Page
    {
        public Facturar()
        {
            InitializeComponent();
        }

        public void AgregarVenta_Click(object sender, RoutedEventArgs e)
        {
            Window ventasWindow = Window.GetWindow(this);
            AgregarVWindow venWindow = new AgregarVWindow();

            venWindow.Owner = ventasWindow;
            venWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            venWindow.ShowDialog();

            Facturar updateFactura = new Facturar();
            this.NavigationService.Navigate(updateFactura);
        }
    }
}
