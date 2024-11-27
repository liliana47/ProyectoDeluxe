using BLL;
using ENTITY;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Pages
{
    public partial class Ventas : Page
    {
        private ProductoBLL productoBLL = new ProductoBLL();
        private FacturaBLL facturaBLL = new FacturaBLL();
        private ClienteBLL clienteBLL = new ClienteBLL();
        private List<Cliente> Clientes { get; set; } = new List<Cliente>();
        private int contadorProductos = 0;
        private double subtotal = 0;
        private double cantidadAnterior = 0;

        public Ventas()
        {
            InitializeComponent();
            CargarClientes();
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
            AgregarCWindow cliWindow = new AgregarCWindow
            {
                Owner = clientesWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            if (cliWindow.ShowDialog() == true)
            {
                CargarClientes();
            }
        }

        private void LimpiarCampos(TextBox txtNombre, TextBox txtCantidad, TextBox txtPrecioUnitario)
        {
            txtNombre.Text = string.Empty;
            txtCantidad.Text = string.Empty;
            txtPrecioUnitario.Text = string.Empty;
            txtTotal.Text = string.Empty;
        }

        private Factura ObtenerFacturaDesdeInterfaz()
        {
            // Crear una instancia del cliente
            Cliente clienteSeleccionado = cmbCedula.SelectedItem as Cliente;
            if (clienteSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un cliente antes de continuar.");
                return null;
            }

            // Crear la factura utilizando el constructor que requiere Cliente y LugarEmision
            Factura factura = new Factura(clienteSeleccionado)
            {
                FechaEmision = DateTime.Now,
                HoraEmision = DateTime.Now,
                Productos = new List<Producto>() // Inicializa la lista de productos
            };

            // Recorrer y agregar productos
            double totalFactura = 0;

            foreach (var panel in stackPanelProductos.Children)
            {
                if (panel is StackPanel productoPanel)
                {
                    var txtCodigo = productoPanel.Children[0] as TextBox;
                    var txtNombre = productoPanel.Children[1] as TextBox;
                    var txtCantidad = productoPanel.Children[2] as TextBox;
                    var txtPrecio = productoPanel.Children[3] as TextBox;

                    int codigo;
                    int cantidad;
                    double precio;

                    if (!int.TryParse(txtCodigo.Text, out codigo))
                    {
                        MessageBox.Show("El código del producto debe ser un número válido.");
                        return null;
                    }

                    if (!int.TryParse(txtCantidad.Text, out cantidad))
                    {
                        MessageBox.Show("La cantidad debe ser un número entero válido.");
                        return null;
                    }

                    if (!double.TryParse(txtPrecio.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out precio))
                    {
                        MessageBox.Show("El precio debe ser un número válido.");
                        return null;
                    }

                    Producto producto = new Producto
                    {
                        Id = codigo,
                        Nombre = txtNombre.Text,
                        Cantidad = cantidad,
                        PrecioUnitario = precio,
                        Total = cantidad * precio
                    };

                    factura.Productos.Add(producto);
                    totalFactura += producto.Total; // Sumar al total de la factura
                }
            }

            factura.Total = totalFactura;

            if (factura.Total <= 0)
            {
                MessageBox.Show("El total de la factura debe ser mayor a 0.");
                return null;
            }

            return factura;
        }

        private void btnGuardarFactura_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEfectivoRecibido.Text))
            {
                MessageBox.Show("El campo 'Efectivo Recibido' no puede estar vacío.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            try
            {
                Factura factura = ObtenerFacturaDesdeInterfaz();
                facturaBLL.GuardarFactura(factura);
                MessageBox.Show($"Factura guardada exitosamente con el número {factura.Id}.");
                GenerarFacturaPDF(factura);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void GenerarFacturaPDF(Factura factura)
        {
            string fileName = $"Factura_{factura.Id}.pdf";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            Document doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            BaseColor customColor = new BaseColor(38, 101, 147);
            BaseColor grayBackground = new BaseColor(211, 211, 211);
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK);
            Font bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);
            Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.GRAY);
            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);

            iTextSharp.text.Paragraph header = new iTextSharp.text.Paragraph("Factura\n", titleFont);
            header.Alignment = Element.ALIGN_RIGHT;
            header.SpacingAfter = 10;
            doc.Add(header);

            DateTime fechaFactura = DateTime.Now;
            iTextSharp.text.Paragraph facturaInfo = new iTextSharp.text.Paragraph($"Número de factura: {factura.Id}\nFecha de la factura: {fechaFactura.ToString("dd MMMM yyyy", new CultureInfo("es-CO"))}\n\n", bodyFont);
            facturaInfo.Alignment = Element.ALIGN_RIGHT;
            doc.Add(facturaInfo);

            PdfPTable infoTable = new PdfPTable(2);
            infoTable.WidthPercentage = 100;
            infoTable.SetWidths(new float[] { 50f, 50f });

            PdfPCell facturaHeaderCell = new PdfPCell(new iTextSharp.text.Paragraph("De", boldFont));
            facturaHeaderCell.BackgroundColor = grayBackground;
            facturaHeaderCell.Border = PdfPCell.NO_BORDER;
            infoTable.AddCell(facturaHeaderCell);

            PdfPCell clienteHeaderCell = new PdfPCell(new iTextSharp.text.Paragraph("Facturar a", boldFont));
            clienteHeaderCell.BackgroundColor = grayBackground;
            clienteHeaderCell.Border = PdfPCell.NO_BORDER;
            infoTable.AddCell(clienteHeaderCell);

            PdfPCell facturaInfoCell = new PdfPCell();
            facturaInfoCell.Border = PdfPCell.NO_BORDER;
            facturaInfoCell.AddElement(new iTextSharp.text.Paragraph("Mi Empresa", bodyFont));
            facturaInfoCell.AddElement(new iTextSharp.text.Paragraph("Teléfono: 123456789", bodyFont));
            facturaInfoCell.AddElement(new iTextSharp.text.Paragraph("Correo: empresa@email.com", bodyFont));
            facturaInfoCell.AddElement(new iTextSharp.text.Paragraph("Ciudad - País", bodyFont));
            infoTable.AddCell(facturaInfoCell);

            PdfPCell clienteInfoCell = new PdfPCell();
            clienteInfoCell.Border = PdfPCell.NO_BORDER;
            clienteInfoCell.AddElement(new iTextSharp.text.Paragraph($"Cédula: {factura.Cliente.Cedula}", bodyFont));
            clienteInfoCell.AddElement(new iTextSharp.text.Paragraph($"Nombre: {factura.Cliente.Nombre}", bodyFont));
            clienteInfoCell.AddElement(new iTextSharp.text.Paragraph($"Correo: {factura.Cliente.CorreoElectronico}", bodyFont));
            clienteInfoCell.AddElement(new iTextSharp.text.Paragraph($"Teléfono: {factura.Cliente.Telefono}", bodyFont));
            infoTable.AddCell(clienteInfoCell);

            doc.Add(infoTable);

            iTextSharp.text.Paragraph detallesHeader = new iTextSharp.text.Paragraph("Detalles de la Factura", titleFont);
            detallesHeader.Alignment = Element.ALIGN_CENTER;
            detallesHeader.SpacingBefore = 20;
            detallesHeader.SpacingAfter = 10;
            doc.Add(detallesHeader);

            PdfPTable productoTable = new PdfPTable(5);
            productoTable.WidthPercentage = 100;
            productoTable.SetWidths(new float[] { 15f, 40f, 15f, 15f, 15f });

            productoTable.AddCell(CrearCeldaEncabezado("Código Producto"));
            productoTable.AddCell(CrearCeldaEncabezado("Nombre Producto"));
            productoTable.AddCell(CrearCeldaEncabezado("Cantidad"));
            productoTable.AddCell(CrearCeldaEncabezado("Precio Unitario"));
            productoTable.AddCell(CrearCeldaEncabezado("Total"));

            foreach (var producto in factura.Productos)
            {
                productoTable.AddCell(CrearCeldaValor(producto.Id.ToString()));
                productoTable.AddCell(CrearCeldaValor(producto.Nombre));
                productoTable.AddCell(CrearCeldaValor(producto.Cantidad.ToString()));
                productoTable.AddCell(CrearCeldaValor(producto.PrecioUnitario.ToString("C", new CultureInfo("es-CO"))));
                productoTable.AddCell(CrearCeldaValor(producto.Total.ToString("C", new CultureInfo("es-CO"))));
            }

            doc.Add(productoTable);

            iTextSharp.text.Paragraph totalFactura = new iTextSharp.text.Paragraph($"Total Factura: {factura.Total.ToString("C", new CultureInfo("es-CO"))}", titleFont);
            totalFactura.Alignment = Element.ALIGN_RIGHT;
            totalFactura.SpacingBefore = 20;
            doc.Add(totalFactura);

            iTextSharp.text.Paragraph agradecimiento = new iTextSharp.text.Paragraph("Gracias por su compra.", smallFont) { Leading = 20 };
            agradecimiento.SpacingBefore = 40;
            agradecimiento.Alignment = Element.ALIGN_CENTER;
            doc.Add(agradecimiento);

            doc.Close();

            MessageBox.Show($"PDF generado y guardado como: {fileName}");
        }

        private PdfPCell CrearCeldaEncabezado(string texto)
        {
            BaseColor grayBackground = new BaseColor(211, 211, 211);
            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);
            PdfPCell cell = new PdfPCell(new iTextSharp.text.Paragraph(texto, headerFont))
            {
                BackgroundColor = grayBackground,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            };
            return cell;
        }

        private PdfPCell CrearCeldaValor(string texto)
        {
            Font bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);
            PdfPCell cell = new PdfPCell(new iTextSharp.text.Paragraph(texto, bodyFont))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            };
            return cell;
        }


        private void CargarClientes()
        {
            try
            {
                Clientes = clienteBLL.ObtenerClientes(); // Obtener clientes desde la base de datos
                cmbCedula.ItemsSource = Clientes; // Asignar al ComboBox
                cmbCedula.DisplayMemberPath = "Nombre"; // Mostrar el nombre del cliente
                cmbCedula.SelectedValuePath = "Cedula"; // Identificador único
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los clientes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ActualizarTextBlocks(Cliente cliente)
        {
            txtCliente.Text = cliente.Nombre + " " + cliente.Apellido;
            txtTelefono.Text = cliente.Telefono;
            txtDireccion.Text = cliente.Direccion;
            txtCorreo.Text = cliente.CorreoElectronico;
        }

        private void cmbCedula_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCedula.SelectedItem is Cliente selectedCliente)
            {
                txtCliente.Text = selectedCliente.Nombre + " " + selectedCliente.Apellido;
                txtNombreCliente.Text = selectedCliente.Nombre + " " + selectedCliente.Apellido;
                txtNIT.Text = selectedCliente.Cedula.ToString();
                txtTelefono.Text = selectedCliente.Telefono;
                txtDireccion.Text = selectedCliente.Direccion;
                txtCorreo.Text = selectedCliente.CorreoElectronico;
            }
            else
            {
                // Limpiar los TextBlock si no hay selección
                txtCliente.Text = string.Empty;
                txtNIT.Text = string.Empty;
                txtTelefono.Text = string.Empty;
                txtDireccion.Text = string.Empty;
                txtCorreo.Text = string.Empty;
            }
        }

    }
}

