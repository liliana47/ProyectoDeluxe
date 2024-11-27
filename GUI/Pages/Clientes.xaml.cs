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
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Clientes : Page
    {
        public List<Cliente> cliente = null;

        ClienteBLL clienteBLL = new ClienteBLL();

        public Clientes()
        {
            InitializeComponent();
            DataContext = this;
            cliente = clienteBLL.ObtenerClientes();
            ClientesDataGrid.ItemsSource = cliente;
        }

        private void RegistrarCliente_Click(object sender, EventArgs e)
        {
            Window clientesWindow = Window.GetWindow(this);
            AgregarCWindow cliWindow = new AgregarCWindow();

            cliWindow.Owner = clientesWindow;
            cliWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cliWindow.ShowDialog();

            Clientes updateCliente = new Clientes();
            this.NavigationService.Navigate(updateCliente);
        }
    }
}
