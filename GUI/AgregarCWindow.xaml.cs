using BLL;
using ENTITY;
using GUI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para AgregarCWindow.xaml
    /// </summary>
    public partial class AgregarCWindow : Window
    {
        public Cliente ClienteRegistrado { get; private set; }

        public AgregarCWindow()
        {
            InitializeComponent();
        }

        private void RegistrarCliente_Click(object sender, RoutedEventArgs e)
        {
            string cedula = txtCedula.Text;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string direccion = txtDireccion.Text;
            string telefono = txtTelefono.Text;
            string correo = txtCorreo.Text;

            ClienteBLL clienteBLL = new ClienteBLL();

            // Validaciones
            if (string.IsNullOrWhiteSpace(cedula) || !EsNumerico(cedula))
            {
                MessageBox.Show("La cédula debe contener solo caracteres numéricos.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(telefono) && !EsNumerico(telefono))
            {
                MessageBox.Show("El número de teléfono debe contener solo caracteres numéricos.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Intentar agregar el cliente
            try
            {
                clienteBLL.AgregarCliente(cedula, nombre, apellido, direccion, telefono, correo);
                MessageBox.Show("Cliente registrado exitosamente.", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);

                // Establecer el cliente registrado para retornarlo al dueño
                ClienteRegistrado = new Cliente
                {
                    Cedula = cedula,
                    Nombre = nombre,
                    Apellido = apellido,
                    Direccion = direccion,
                    Telefono = telefono,
                    CorreoElectronico = correo
                };

                this.DialogResult = true; // Indicar que se completó correctamente
                this.Close(); // Cerrar la ventana
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al registrar el cliente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private bool EsNumerico(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        private void InputTextBoxCedula_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string input = textBox.Text;
                if (!Regex.IsMatch(input, @"^\d*$"))
                {
                    ErrorCedula.Visibility = Visibility.Visible;
                }
                else
                {
                    ErrorCedula.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void InputTextBoxTelefono_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string input = textBox.Text;
                if (!Regex.IsMatch(input, @"^\d*$"))
                {
                    ErrorTelefono.Visibility = Visibility.Visible;
                }
                else
                {
                    ErrorTelefono.Visibility = Visibility.Collapsed;
                }
            }
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
