using BLL;
using ENTITY;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Pages
{
    public partial class Ventas : Page
    {
        private ProductoBLL productoBLL = new ProductoBLL();
        private int contadorProductos = 0;
        private double subtotal = 0;
        private double cantidadAnterior = 0;

        public Ventas()
        {
            InitializeComponent();
            EstablecerClientePredeterminado();
        }

        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuscarProducto(txtCodigo, txtNombre, txtCantidad, txtPrecioUnitario);

                if (string.IsNullOrEmpty(txtCantidad.Text) || !double.TryParse(txtCantidad.Text, out _))
                {
                    txtCantidad.Text = "1";
                }

                ActualizarCantidad(txtCantidad, txtPrecioUnitario, txtTotal);
            }
        }

        private void EfectivoRecibido_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(txtEfectivoRecibido.Text, out double efectivoRecibido) &&
                double.TryParse(txtTotalFactura.Text.Replace("$ ", ""), out double totalFactura))
            {
                double cambio = efectivoRecibido - totalFactura;

                txtTotalCambio.Text = $"$ {cambio:F2}";
            }
            else
            {
                txtTotalCambio.Text = "$ 0.00";
            }
        }

        private void BuscarProducto(TextBox txtCodigo, TextBox txtNombre, TextBox txtCantidad, TextBox txtPrecioUnitario)
        {
            if (int.TryParse(txtCodigo.Text, out int codigo))
            {
                Producto producto = productoBLL.ObtenerProductoPorCodigo(codigo);
                if (producto != null)
                {
                    if (contadorProductos == 0) 
                    {
                        txtNombre.Text = producto.Nombre;
                        txtPrecioUnitario.Text = producto.PrecioUnitario.ToString("F2");

                        if (double.TryParse(txtCantidad.Text, out double cantidad) && cantidad > 0)
                        {
                            double totalProducto = cantidad * producto.PrecioUnitario;
                            subtotal += totalProducto;

                            txtSubtotal.Text = $"$ {subtotal:F2}";
                            txtTotalFactura.Text = $"$ {subtotal:F2}";
                            txtTotal.Text = totalProducto.ToString("F2");
                        }

                        contadorProductos++;
                        lblProductosIngresados.Content = $"Productos ingresados: {contadorProductos}";

                        AgregarCamposDeEntrada();
                    }
                    else
                    {
                        bool productoLlenado = LlenarPrimeraFilaVacia(producto);

                        if (productoLlenado)
                        {
                            AgregarCamposDeEntrada();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                    LimpiarCampos(txtNombre, txtCantidad, txtPrecioUnitario);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un código válido.");
            }
        }



        private void AgregarNuevoProductoInput(Producto producto)
        {
            StackPanel productoPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 10, 0, 0)
            };

            TextBox nuevoTxtCodigo = new TextBox
            {
                Text = producto.Id.ToString(),
                Width = 29,
                Margin = new Thickness(0, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#cba8cd"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsEnabled = false
            };

            TextBox nuevoTxtNombre = new TextBox
            {
                Text = producto.Nombre,
                Width = 349,
                Margin = new Thickness(1, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#cba8cd"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsEnabled = false
            };

            TextBox nuevoTxtCantidad = new TextBox
            {
                Text = "1", 
                Width = 99,
                Margin = new Thickness(1, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#cba8cd"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };

            TextBox nuevoTxtPrecioUnitario = new TextBox
            {
                Text = producto.PrecioUnitario.ToString("F2"),
                Width = 179,
                Margin = new Thickness(1, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#cba8cd"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsEnabled = false
            };

            TextBox nuevoTxtTotal = new TextBox
            {
                Text = (producto.PrecioUnitario * 1).ToString("F2"),
                Width = 195,
                Margin = new Thickness(1, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#cba8cd"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsEnabled = false
            };

            productoPanel.Children.Add(nuevoTxtCodigo);
            productoPanel.Children.Add(nuevoTxtNombre);
            productoPanel.Children.Add(nuevoTxtCantidad);
            productoPanel.Children.Add(nuevoTxtPrecioUnitario);
            productoPanel.Children.Add(nuevoTxtTotal);

            stackPanelProductos.Children.Add(productoPanel);

            subtotal += producto.PrecioUnitario; 
            txtSubtotal.Text = $"$ {subtotal:F2}";
            txtTotalFactura.Text = $"$ {subtotal:F2}";

            contadorProductos++;
            lblProductosIngresados.Content = $"Productos ingresados: {contadorProductos}";
        }


        private void txtCantidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox txtCantidad = sender as TextBox;
                StackPanel fila = txtCantidad.Parent as StackPanel;

                TextBox txtPrecioUnitario = fila.Children[3] as TextBox;
                TextBox txtTotal = fila.Children[4] as TextBox;

                ActualizarCantidad(txtCantidad, txtPrecioUnitario, txtTotal);
            }
        }

        private void ActualizarCantidad(TextBox txtCantidad, TextBox txtPrecioUnitario, TextBox txtTotal)
        {
            if (double.TryParse(txtCantidad.Text, out double nuevaCantidad) &&
                double.TryParse(txtPrecioUnitario.Text, out double precioUnitario))
            {
                double nuevoTotalProducto = nuevaCantidad * precioUnitario;
                txtTotal.Text = nuevoTotalProducto.ToString("F2");

                subtotal = 0;
                foreach (StackPanel fila in stackPanelProductos.Children)
                {
                    TextBox totalBox = fila.Children[4] as TextBox;
                    if (double.TryParse(totalBox.Text, out double totalProducto))
                    {
                        subtotal += totalProducto;
                    }
                }

                txtSubtotal.Text = $"$ {subtotal:F2}";
                txtTotalFactura.Text = $"$ {subtotal:F2}";
            }
            else
            {
                MessageBox.Show("Por favor, ingrese valores válidos en cantidad y precio.");
            }
        }



        public void BuscarProductos_Click(object sender, RoutedEventArgs e)
        {
            ListarProductos proWindow = new ListarProductos();

            if (proWindow.ShowDialog() == true) 
            {
                Producto productoSeleccionado = proWindow.ProductoSeleccionado;

                bool productoLlenado = LlenarPrimeraFilaVacia(productoSeleccionado);

                if (!productoLlenado)
                {
                    AgregarNuevoProductoInput(productoSeleccionado);
                }

                AgregarCamposDeEntrada();

            }
        }

        private bool LlenarPrimeraFilaVacia(Producto producto)
        {
            foreach (StackPanel fila in stackPanelProductos.Children)
            {
                var txtCodigo = fila.Children[0] as TextBox;
                var txtNombre = fila.Children[1] as TextBox;
                var txtCantidad = fila.Children[2] as TextBox;
                var txtPrecioUnitario = fila.Children[3] as TextBox;
                var txtTotal = fila.Children[4] as TextBox;

                if (txtNombre != null && string.IsNullOrEmpty(txtNombre.Text))
                {
                    txtCodigo.Text = producto.Id.ToString();
                    txtNombre.Text = producto.Nombre;
                    txtPrecioUnitario.Text = producto.PrecioUnitario.ToString("F2");

                    txtCantidad.Text = "1";
                    double cantidad = 1;

                    double totalProducto = cantidad * producto.PrecioUnitario;
                    txtTotal.Text = totalProducto.ToString("F2");

                    subtotal += totalProducto;
                    txtSubtotal.Text = $"$ {subtotal:F2}";
                    txtTotalFactura.Text = $"$ {subtotal:F2}";

                    contadorProductos++;
                    lblProductosIngresados.Content = $"Productos ingresados: {contadorProductos}";

                    return true;
                }
            }

            return false; 
        }

        private void EstablecerClientePredeterminado()
        {
            txtNombreCliente.Text = "CONSUMIDOR FINAL";
            txtNIT.Text = "222222222";
            txtTelefono.Text = "NA";
            txtDireccion.Text = "PUNTO DE VENTA";
            txtCorreo.Text = "no-reply@empresa.com";
        }

        private void AgregarCamposDeEntrada()
        {
            StackPanel entradaProductoPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 10, 0, 0)
            };

            TextBox nuevoTxtCodigo = new TextBox
            {
                Width = 29,
                Margin = new Thickness(0, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#cba8cd"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };

            TextBox nuevoTxtNombre = new TextBox
            {
                Width = 349,
                Margin = new Thickness(1, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#cba8cd"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsEnabled = false
            };

            TextBox nuevoTxtCantidad = new TextBox
            {
                Width = 99,
                Margin = new Thickness(1, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#cba8cd"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };

            nuevoTxtCantidad.KeyDown += txtCantidad_KeyDown;

            TextBox nuevoTxtPrecioUnitario = new TextBox
            {
                Width = 179,
                Margin = new Thickness(1, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#cba8cd"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsEnabled = false
            };

            TextBox nuevoTxtTotal = new TextBox
            {
                Width = 145,
                Margin = new Thickness(1, 10, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#cba8cd"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsEnabled = false
            };

            Button btnEliminar = new Button
            {
                Content = "X",
                Width = 30,
                Height = 30,
                Margin = new Thickness(5, 10, 0, 0),
                Background = Brushes.Red,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Cursor = Cursors.Hand
            };

            // Evento de click para eliminar la fila
            btnEliminar.Click += (s, e) => EliminarFila(entradaProductoPanel);

            nuevoTxtCodigo.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    BuscarProducto(nuevoTxtCodigo, nuevoTxtNombre, nuevoTxtCantidad, nuevoTxtPrecioUnitario);
                }
            };

            entradaProductoPanel.Children.Add(nuevoTxtCodigo);
            entradaProductoPanel.Children.Add(nuevoTxtNombre);
            entradaProductoPanel.Children.Add(nuevoTxtCantidad);
            entradaProductoPanel.Children.Add(nuevoTxtPrecioUnitario);
            entradaProductoPanel.Children.Add(nuevoTxtTotal);
            entradaProductoPanel.Children.Add(btnEliminar);


            stackPanelProductos.Children.Add(entradaProductoPanel);

            nuevoTxtCodigo.Focus();
        }

        private void EliminarFila(StackPanel fila)
        {
            // Obtener el TextBox de total en la fila para ajustar el subtotal
            TextBox txtTotal = fila.Children[4] as TextBox;
            if (double.TryParse(txtTotal.Text, out double totalProducto))
            {
                subtotal -= totalProducto; // Restar el total de este producto del subtotal
            }

            // Actualizar los campos de subtotal y total de factura
            txtSubtotal.Text = $"$ {subtotal:F2}";
            txtTotalFactura.Text = $"$ {subtotal:F2}";

            // Eliminar la fila del StackPanel principal
            stackPanelProductos.Children.Remove(fila);

            // Actualizar el contador de productos
            contadorProductos--;
            lblProductosIngresados.Content = $"Productos ingresados: {contadorProductos}";
        }

        private void AgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            Window clientesWindow = Window.GetWindow(this);
            AgregarCWindow cliWindow = new AgregarCWindow();

            cliWindow.Owner = clientesWindow;
            cliWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cliWindow.ShowDialog();
        }

        private void LimpiarCampos(TextBox txtNombre, TextBox txtCantidad, TextBox txtPrecioUnitario)
        {
            txtNombre.Text = string.Empty;
            txtCantidad.Text = string.Empty;
            txtPrecioUnitario.Text = string.Empty;
            txtTotal.Text = string.Empty;
        }
    }
}
